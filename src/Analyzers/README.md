# Bootstrap Architecture Analyzers

This project contains custom Roslyn analyzers that enforce architectural rules for the Bootstrap modular monolith at compile time, preventing architectural drift and ensuring code quality.

## Analyzers Overview

### MOD001: InternalModuleAnalyzer
**Severity:** Error  
**Purpose:** Ensures types within module projects are `internal` by default.

#### What it does
- Detects `public` types in projects that are NOT:
  - API projects (ending in `.Api`)
  - Contract projects (ending in `.Contracts` or containing "Contracts")
  - BuildingBlocks, ServiceDefaults, or test projects

#### Why it matters
In a modular monolith, modules should be self-contained with clear boundaries. Making types internal by default prevents tight coupling between modules and forces developers to explicitly expose only what's needed through contracts.

#### Examples

❌ **Incorrect:**
```csharp
namespace Todos.Domain;

public class Todo // Error: MOD001
{
    public Guid Id { get; set; }
}
```

✅ **Correct:**
```csharp
namespace Todos.Domain;

internal class Todo
{
    public Guid Id { get; set; }
}
```

✅ **Correct (in Contracts project):**
```csharp
namespace Contracts;

public sealed record TodoCreatedEvent(Guid TodoId) : IDomainEvent;
```

#### How to disable temporarily
```csharp
#pragma warning disable MOD001
public class MyPublicClass { }
#pragma warning restore MOD001
```

---

### MOD002: NoCommandChainingAnalyzer
**Severity:** Error  
**Purpose:** Prevents command handlers from invoking other commands to avoid coupling.

#### What it does
- Detects when an `ICommandHandler<>` calls `ISender.Send()` with another command
- Allows queries to be called from command handlers
- Identifies commands by checking if they implement `IBaseCommand`

#### Why it matters
Command chaining creates tight coupling between use cases and can lead to:
- Difficult-to-trace execution flows
- Transaction boundary issues
- Circular dependencies
- Hard-to-test code

Instead, use domain logic directly or call queries if you need to read data.

#### Examples

❌ **Incorrect:**
```csharp
internal sealed class CreateOrderHandler(ISender sender) : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // Error: MOD002 - Command handler calling another command
        await sender.Send(new ValidateInventoryCommand(request.ProductId), ct);
        
        // ... rest of logic
        return Result.Success();
    }
}
```

✅ **Correct - Use domain logic:**
```csharp
internal sealed class CreateOrderHandler(
    OrderService orderService,
    ISender sender) : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // Use domain service instead
        var validationResult = await orderService.ValidateInventory(request.ProductId);
        
        // ... rest of logic
        return Result.Success();
    }
}
```

✅ **Correct - Query is allowed:**
```csharp
internal sealed class CreateOrderHandler(ISender sender) : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // OK: Queries can be called from commands
        var product = await sender.Send(new GetProductQuery(request.ProductId), ct);
        
        // ... rest of logic
        return Result.Success();
    }
}
```

#### How to disable temporarily
```csharp
#pragma warning disable MOD002
await sender.Send(new MyCommand(), ct);
#pragma warning restore MOD002
```

---

### MOD003: DomainEventsLocationAnalyzer
**Severity:** Warning  
**Purpose:** Ensures domain events are located in appropriate namespaces.

#### What it does
- Detects classes that implement `IDomainEvent`
- Validates that their namespace contains `.Domain.Events` or ends with `.Domain`

#### Why it matters
Consistent organization of domain events makes them easier to find and understand. It creates a clear convention across all modules.

#### Examples

❌ **Incorrect:**
```csharp
namespace Todos.Application; // Warning: MOD003

public sealed record TodoCompletedEvent(Guid TodoId) : IDomainEvent;
```

✅ **Correct:**
```csharp
namespace Todos.Domain.Events;

public sealed record TodoCompletedEvent(Guid TodoId) : IDomainEvent;
```

✅ **Also correct:**
```csharp
namespace Todos.Domain;

public sealed record TodoCompletedEvent(Guid TodoId) : IDomainEvent;
```

#### How to disable temporarily
```csharp
#pragma warning disable MOD003
public sealed record MyEvent(Guid Id) : IDomainEvent;
#pragma warning restore MOD003
```

---

### MOD004: ApiRecordsOnlyAnalyzer
**Severity:** Suggestion/Warning  
**Purpose:** Encourages the use of records over classes in contract and API projects.

#### What it does
- Analyzes projects containing "Contracts" or "Api" in their name
- Suggests using `record` instead of `class` for immutable data
- Excludes:
  - Controllers (names ending with "Controller")
  - DbContext classes
  - Static classes
  - Exception classes
  - Attribute classes
  - Classes with mutable properties (non-init setters)

#### Why it matters
Records provide:
- Value-based equality by default
- Immutability with `init` accessors
- Built-in deconstruction
- Cleaner syntax for DTOs and contracts

#### Examples

❌ **Not ideal:**
```csharp
namespace Contracts;

public class TodoDto // Warning: MOD004
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
}
```

✅ **Better:**
```csharp
namespace Contracts;

public record TodoDto(Guid Id, string Title);
```

✅ **Mutable class is OK (no warning):**
```csharp
namespace Contracts;

public class TodoUpdateRequest
{
    public string Title { get; set; } = string.Empty; // Mutable, so no warning
}
```

#### How to disable temporarily
```csharp
#pragma warning disable MOD004
public class MyContract { }
#pragma warning restore MOD004
```

---

## Disabling Analyzers

### For a specific file
Add at the top of the file:
```csharp
#pragma warning disable MOD001, MOD002, MOD003, MOD004
// Your code here
#pragma warning restore MOD001, MOD002, MOD003, MOD004
```

### For a project
In the `.csproj` file:
```xml
<PropertyGroup>
  <NoWarn>MOD001;MOD002;MOD003;MOD004</NoWarn>
</PropertyGroup>
```

### Via .editorconfig
Update the `.editorconfig` file:
```ini
# Disable a specific analyzer
dotnet_diagnostic.MOD001.severity = none

# Or change severity
dotnet_diagnostic.MOD002.severity = warning
```

---

## Extending with New Rules

To add a new analyzer:

1. **Create a new analyzer class** in this project:
```csharp
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Bootstrap.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MyNewAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MOD005";
    private const string Category = "Architecture";

    private static readonly LocalizableString Title = "My rule title";
    private static readonly LocalizableString MessageFormat = "My message for '{0}'";
    private static readonly LocalizableString Description = "Detailed description";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics 
        => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        // Register your analysis logic here
        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        // Your analysis logic
    }
}
```

2. **Add to .editorconfig**:
```ini
dotnet_diagnostic.MOD005.severity = warning
```

3. **Update this README** with the new rule documentation.

4. **Rebuild the solution** - the analyzer will be automatically applied to all projects.

---

## Troubleshooting

### Analyzers not showing up in IDE
1. Close and reopen Visual Studio / Rider
2. Clean and rebuild the solution
3. Delete `bin/` and `obj/` folders
4. Check that `Directory.Build.props` includes the analyzer reference

### False positives
If an analyzer is reporting false positives:
1. Use `#pragma warning disable` for specific cases
2. File an issue to improve the analyzer logic
3. Consider if the architectural rule needs adjustment

### Performance issues
If analyzers are slowing down builds:
1. Check if they're running on generated code (they shouldn't be)
2. Optimize the analysis logic
3. Consider making the rule less strict or optional

---

## Technical Details

- **Target Framework:** netstandard2.0 (required for Roslyn analyzers)
- **Roslyn Version:** Microsoft.CodeAnalysis.CSharp 4.8.0
- **Analyzer SDK:** Microsoft.CodeAnalysis.Analyzers 3.3.4

### How Analyzers Are Loaded
Analyzers are automatically loaded in all projects via `Directory.Build.props`:
```xml
<ItemGroup Condition="!$(MSBuildProjectName.Contains('.Analyzers'))">
  <ProjectReference Include="$(MSBuildThisFileDirectory)src\Analyzers\Bootstrap.Analyzers\Bootstrap.Analyzers.csproj"
                    OutputItemType="Analyzer"
                    ReferenceOutputAssembly="false" />
</ItemGroup>
```

The condition `!$(MSBuildProjectName.Contains('.Analyzers'))` ensures the analyzer project doesn't reference itself.

---

## Resources

- [Roslyn Analyzers Documentation](https://github.com/dotnet/roslyn-analyzers)
- [Writing a Roslyn Analyzer](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix)
- [Analyzer Configuration](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files)
