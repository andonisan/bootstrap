using System.CommandLine;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using YamlDotNet.Serialization;

var rootCommand = new RootCommand("Event Documentation Generator for CAP and MediatR events");

var sourcePathOption = new Option<string>(
    name: "--source-path",
    description: "Path to the directory containing assemblies to scan",
    getDefaultValue: () => "./bin/Debug/net9.0");

var outputPathOption = new Option<string>(
    name: "--output-path",
    description: "Path to output directory for generated documentation",
    getDefaultValue: () => "./docs-output");

rootCommand.AddOption(sourcePathOption);
rootCommand.AddOption(outputPathOption);

rootCommand.SetHandler(async (sourcePath, outputPath) =>
{
    await GenerateDocumentation(sourcePath, outputPath);
}, sourcePathOption, outputPathOption);

return await rootCommand.InvokeAsync(args);

static async Task GenerateDocumentation(string sourcePath, string outputPath)
{
    Console.WriteLine($"Scanning assemblies in: {sourcePath}");
    Console.WriteLine($"Output directory: {outputPath}");

    Directory.CreateDirectory(outputPath);

    // Ensure source path is absolute
    if (!Path.IsPathRooted(sourcePath))
    {
        sourcePath = Path.GetFullPath(sourcePath);
    }

    var assemblyFiles = Directory.GetFiles(sourcePath, "*.dll", SearchOption.TopDirectoryOnly)
        .Where(f => !Path.GetFileName(f).StartsWith("Microsoft.") &&
                    !Path.GetFileName(f).StartsWith("System.") &&
                    !Path.GetFileName(f).StartsWith("netstandard") &&
                    !Path.GetFileName(f).Contains("test", StringComparison.OrdinalIgnoreCase))
        .ToList();

    Console.WriteLine($"Found {assemblyFiles.Count} assemblies to scan");

    var capEvents = new List<CapEventInfo>();
    var domainEvents = new List<DomainEventInfo>();

    foreach (var assemblyFile in assemblyFiles)
    {
        try
        {
            Console.WriteLine($"Scanning: {Path.GetFileName(assemblyFile)}");
            ScanAssembly(assemblyFile, capEvents, domainEvents);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Warning: Could not scan {Path.GetFileName(assemblyFile)}: {ex.Message}");
        }
    }

    Console.WriteLine($"\nFound {capEvents.Count} CAP subscribers");
    Console.WriteLine($"Found {domainEvents.Count} domain events");

    // Generate AsyncAPI specification
    await GenerateAsyncApiSpec(capEvents, Path.Combine(outputPath, "asyncapi.yaml"));

    // Generate domain events documentation
    await GenerateDomainEventsDoc(domainEvents, Path.Combine(outputPath, "domain-events.md"));

    // Generate CAP events documentation
    await GenerateCapEventsDoc(capEvents, Path.Combine(outputPath, "integration-events.md"));

    Console.WriteLine("\nDocumentation generation completed!");
}

static void ScanAssembly(string assemblyPath, List<CapEventInfo> capEvents, List<DomainEventInfo> domainEvents)
{
    Assembly assembly;
    try
    {
        assembly = Assembly.LoadFrom(assemblyPath);
    }
    catch
    {
        // Skip if assembly cannot be loaded
        return;
    }

    var types = assembly.GetTypes();

    // Scan for CAP subscribers (methods with [CapSubscribe] attribute)
    foreach (var type in types)
    {
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        
        foreach (var method in methods)
        {
            var capSubscribeAttr = method.GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType().Name == "CapSubscribeAttribute");

            if (capSubscribeAttr != null)
            {
                // Extract topic name from attribute
                var topicNameProp = capSubscribeAttr.GetType().GetProperty("Name");
                var topicName = topicNameProp?.GetValue(capSubscribeAttr)?.ToString() ?? "Unknown";

                // Get parameter type
                var parameters = method.GetParameters();
                var messageType = parameters.Length > 0 ? parameters[0].ParameterType.Name : "unknown";

                capEvents.Add(new CapEventInfo
                {
                    TopicName = topicName,
                    SubscriberType = type.FullName ?? type.Name,
                    SubscriberMethod = method.Name,
                    MessageType = messageType,
                    IsPublisher = false
                });
            }
        }
    }

    // Scan for domain events (types implementing IDomainEvent)
    foreach (var type in types)
    {
        if (type.IsClass || type.IsValueType)
        {
            var interfaces = type.GetInterfaces();
            if (interfaces.Any(i => i.Name == "IDomainEvent" || i.Name == "INotification"))
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => new PropertyInfo
                    {
                        Name = p.Name,
                        Type = GetFriendlyTypeName(p.PropertyType)
                    })
                    .ToList();

                domainEvents.Add(new DomainEventInfo
                {
                    EventType = type.Name,
                    Namespace = type.Namespace ?? "",
                    FullName = type.FullName ?? type.Name,
                    Properties = properties,
                    Handlers = new List<string>()
                });
            }
        }
    }

    // Find handlers for domain events
    foreach (var type in types)
    {
        var interfaces = type.GetInterfaces();
        foreach (var iface in interfaces)
        {
            if (iface.IsGenericType && iface.GetGenericTypeDefinition().Name == "INotificationHandler`1")
            {
                var eventType = iface.GetGenericArguments()[0];
                var domainEvent = domainEvents.FirstOrDefault(e => e.FullName == eventType.FullName);
                if (domainEvent != null)
                {
                    domainEvent.Handlers.Add(type.FullName ?? type.Name);
                }
            }
        }
    }
}

static string GetFriendlyTypeName(Type type)
{
    if (!type.IsGenericType)
        return type.Name;

    var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
    var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetFriendlyTypeName));
    return $"{baseName}<{genericArgs}>";
}

static async Task GenerateAsyncApiSpec(List<CapEventInfo> capEvents, string outputPath)
{
    var channels = new Dictionary<string, object>();

    foreach (var capEvent in capEvents.DistinctBy(e => e.TopicName))
    {
        var subscribers = capEvents.Where(e => e.TopicName == capEvent.TopicName && !e.IsPublisher).ToList();

        var channelObj = new Dictionary<string, object>();

        if (subscribers.Any())
        {
            channelObj["address"] = capEvent.TopicName;
            channelObj["messages"] = new Dictionary<string, object>
            {
                [capEvent.TopicName] = new Dictionary<string, object>
                {
                    ["payload"] = new Dictionary<string, object>
                    {
                        ["type"] = "object",
                        ["properties"] = new Dictionary<string, object>
                        {
                            ["messageType"] = new { type = "string", description = capEvent.MessageType }
                        }
                    },
                    ["description"] = $"Consumed by: {string.Join(", ", subscribers.Select(s => s.SubscriberType))}"
                }
            };
        }

        channels[capEvent.TopicName] = channelObj;
    }

    var operations = new Dictionary<string, object>();
    
    foreach (var capEvent in capEvents.DistinctBy(e => e.TopicName))
    {
        operations[$"receive{capEvent.TopicName}"] = new Dictionary<string, object>
        {
            ["action"] = "receive",
            ["channel"] = new Dictionary<string, object>
            {
                ["$ref"] = $"#/channels/{capEvent.TopicName}"
            },
            ["messages"] = new List<object>
            {
                new Dictionary<string, object>
                {
                    ["$ref"] = $"#/channels/{capEvent.TopicName}/messages/{capEvent.TopicName}"
                }
            }
        };
    }

    var asyncApiSpec = new Dictionary<string, object>
    {
        ["asyncapi"] = "3.0.0",
        ["info"] = new Dictionary<string, object>
        {
            ["title"] = "Bootstrap Integration Events API",
            ["version"] = "1.0.0",
            ["description"] = "Auto-generated AsyncAPI specification for CAP integration events"
        },
        ["channels"] = channels,
        ["operations"] = operations
    };

    var serializer = new SerializerBuilder()
        .WithIndentedSequences()
        .Build();

    var yaml = serializer.Serialize(asyncApiSpec);
    await File.WriteAllTextAsync(outputPath, yaml);
    Console.WriteLine($"AsyncAPI spec written to: {outputPath}");
}

static async Task GenerateDomainEventsDoc(List<DomainEventInfo> domainEvents, string outputPath)
{
    var sb = new StringBuilder();
    sb.AppendLine("# Domain Events Documentation");
    sb.AppendLine();
    sb.AppendLine("This document lists all domain events (MediatR) in the system and their handlers.");
    sb.AppendLine();
    sb.AppendLine("## Events");
    sb.AppendLine();

    foreach (var evt in domainEvents.OrderBy(e => e.EventType))
    {
        sb.AppendLine($"### {evt.EventType}");
        sb.AppendLine();
        sb.AppendLine($"**Namespace:** `{evt.Namespace}`");
        sb.AppendLine();

        if (evt.Properties.Any())
        {
            sb.AppendLine("**Properties:**");
            sb.AppendLine();
            foreach (var prop in evt.Properties)
            {
                sb.AppendLine($"- `{prop.Name}`: {prop.Type}");
            }
            sb.AppendLine();
        }

        if (evt.Handlers.Any())
        {
            sb.AppendLine("**Handlers:**");
            sb.AppendLine();
            foreach (var handler in evt.Handlers)
            {
                sb.AppendLine($"- `{handler}`");
            }
            sb.AppendLine();
        }

        // Generate Mermaid diagram
        if (evt.Handlers.Any())
        {
            sb.AppendLine("**Event Flow:**");
            sb.AppendLine();
            sb.AppendLine("```mermaid");
            sb.AppendLine("graph LR");
            sb.AppendLine($"    Event[{evt.EventType}]");
            foreach (var handler in evt.Handlers)
            {
                var handlerName = handler.Split('.').Last();
                sb.AppendLine($"    Event --> {handlerName}[{handlerName}]");
            }
            sb.AppendLine("```");
            sb.AppendLine();
        }

        sb.AppendLine("---");
        sb.AppendLine();
    }

    await File.WriteAllTextAsync(outputPath, sb.ToString());
    Console.WriteLine($"Domain events documentation written to: {outputPath}");
}

static async Task GenerateCapEventsDoc(List<CapEventInfo> capEvents, string outputPath)
{
    var sb = new StringBuilder();
    sb.AppendLine("# Integration Events Documentation");
    sb.AppendLine();
    sb.AppendLine("This document lists all integration events (CAP) in the system.");
    sb.AppendLine();
    sb.AppendLine("## Topics");
    sb.AppendLine();

    var groupedByTopic = capEvents.GroupBy(e => e.TopicName);

    foreach (var group in groupedByTopic.OrderBy(g => g.Key))
    {
        sb.AppendLine($"### {group.Key}");
        sb.AppendLine();

        var subscribers = group.Where(e => !e.IsPublisher).ToList();

        if (subscribers.Any())
        {
            sb.AppendLine("**Subscribers:**");
            sb.AppendLine();
            foreach (var sub in subscribers)
            {
                sb.AppendLine($"- `{sub.SubscriberType}.{sub.SubscriberMethod}` (Message: `{sub.MessageType}`)");
            }
            sb.AppendLine();
        }

        // Generate Mermaid diagram
        sb.AppendLine("**Message Flow:**");
        sb.AppendLine();
        sb.AppendLine("```mermaid");
        sb.AppendLine("graph LR");
        sb.AppendLine($"    Topic[{group.Key}]");
        
        foreach (var sub in subscribers)
        {
            var subName = sub.SubscriberType.Split('.').Last();
            sb.AppendLine($"    Topic --> {subName}[{subName}]");
        }
        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("---");
        sb.AppendLine();
    }

    await File.WriteAllTextAsync(outputPath, sb.ToString());
    Console.WriteLine($"Integration events documentation written to: {outputPath}");
}

// Helper classes
class CapEventInfo
{
    public string TopicName { get; set; } = "";
    public string SubscriberType { get; set; } = "";
    public string SubscriberMethod { get; set; } = "";
    public string MessageType { get; set; } = "";
    public bool IsPublisher { get; set; }
}

class DomainEventInfo
{
    public string EventType { get; set; } = "";
    public string Namespace { get; set; } = "";
    public string FullName { get; set; } = "";
    public List<PropertyInfo> Properties { get; set; } = new();
    public List<string> Handlers { get; set; } = new();
}

class PropertyInfo
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
}
