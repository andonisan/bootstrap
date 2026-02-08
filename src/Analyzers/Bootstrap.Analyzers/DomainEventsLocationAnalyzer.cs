using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Bootstrap.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DomainEventsLocationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MOD003";
    private const string Category = "Architecture";

    private static readonly LocalizableString Title = "Domain events should be in correct namespace";
    private static readonly LocalizableString MessageFormat = "DomainEvent '{0}' should be in namespace '*.Domain.Events' or '*.Domain'";
    private static readonly LocalizableString Description = "Domain events should be located in namespaces ending with .Domain.Events or .Domain for consistency.";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(compilationContext =>
        {
            var compilation = compilationContext.Compilation;
            
            // Find IDomainEvent interface
            var domainEventInterface = compilation.GetTypeByMetadataName("BuildingBlocks.DomainEvents.IDomainEvent");

            if (domainEventInterface == null)
            {
                return;
            }

            compilationContext.RegisterSymbolAction(symbolContext =>
            {
                var namedType = (INamedTypeSymbol)symbolContext.Symbol;

                // Skip compiler-generated types
                if (namedType.IsImplicitlyDeclared)
                {
                    return;
                }

                // Check if this type implements IDomainEvent
                var implementsDomainEvent = namedType.AllInterfaces.Any(i =>
                    SymbolEqualityComparer.Default.Equals(i, domainEventInterface));

                if (!implementsDomainEvent)
                {
                    return;
                }

                // Get the namespace
                var namespaceName = namedType.ContainingNamespace?.ToDisplayString();
                
                if (string.IsNullOrEmpty(namespaceName))
                {
                    return;
                }

                // Check if namespace contains .Domain.Events or ends with .Domain
                var isValidNamespace = namespaceName.Contains(".Domain.Events", System.StringComparison.Ordinal) ||
                                     namespaceName.EndsWith(".Domain", System.StringComparison.Ordinal) ||
                                     namespaceName == "Domain";

                if (!isValidNamespace)
                {
                    var diagnostic = Diagnostic.Create(Rule, namedType.Locations[0], namedType.Name);
                    symbolContext.ReportDiagnostic(diagnostic);
                }
            }, SymbolKind.NamedType);
        });
    }
}
