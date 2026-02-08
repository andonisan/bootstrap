using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Bootstrap.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class InternalModuleAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MOD001";
    private const string Category = "Architecture";

    private static readonly LocalizableString Title = "Type should be internal in module projects";
    private static readonly LocalizableString MessageFormat = "The type '{0}' in module project must be internal. Only API/Contract projects can have public types.";
    private static readonly LocalizableString Description = "Types within module projects should be internal unless they are in API or Contract projects.";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Error,
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
            var assemblyName = compilation.AssemblyName ?? string.Empty;
            
            // Skip if this is an API or Contracts project
            if (assemblyName.EndsWith(".Api", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.EndsWith(".Contracts", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Contains("Contracts", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Equals("Api", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Contains("BuildingBlocks", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Contains("ServiceDefaults", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Contains("AspireHost", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Contains("Test", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Contains("Seeder", System.StringComparison.OrdinalIgnoreCase) ||
                assemblyName.Contains("DbService", System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Find interfaces to check for exemptions
            var featureModuleInterface = compilation.GetTypeByMetadataName("BuildingBlocks.Features.IFeatureModule");
            var migrationBaseClass = compilation.GetTypeByMetadataName("Microsoft.EntityFrameworkCore.Migrations.Migration");

            compilationContext.RegisterSymbolAction(symbolContext =>
            {
                var symbol = symbolContext.Symbol;
                
                // Only analyze named types (classes, structs, interfaces, enums, etc.)
                if (symbol is not INamedTypeSymbol namedType)
                {
                    return;
                }

                // Skip compiler-generated types
                if (namedType.IsImplicitlyDeclared)
                {
                    return;
                }

                // Check if the type is public
                if (namedType.DeclaredAccessibility != Accessibility.Public)
                {
                    return;
                }

                // Exemptions:
                
                // 1. Skip IFeatureModule implementations - these need to be public for API registration
                if (featureModuleInterface != null && 
                    namedType.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, featureModuleInterface)))
                {
                    return;
                }

                // 1b. Skip nested types within IFeatureModule implementations
                if (featureModuleInterface != null && namedType.ContainingType != null)
                {
                    var containingType = namedType.ContainingType;
                    if (containingType.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, featureModuleInterface)))
                    {
                        return;
                    }
                }

                // 2. Skip EF Core Migration classes
                if (migrationBaseClass != null)
                {
                    var currentBaseType = namedType.BaseType;
                    while (currentBaseType != null)
                    {
                        if (SymbolEqualityComparer.Default.Equals(currentBaseType, migrationBaseClass))
                        {
                            return;
                        }
                        currentBaseType = currentBaseType.BaseType;
                    }
                }

                // 3. Skip module registration classes (classes ending with "Module")
                if (namedType.Name.EndsWith("Module", System.StringComparison.Ordinal))
                {
                    return;
                }

                // 4. Skip DbContext classes
                if (namedType.Name.EndsWith("DbContext", System.StringComparison.Ordinal))
                {
                    return;
                }

                // 5. Skip SignalR Hub classes and interfaces
                if (namedType.Name.EndsWith("Hub", System.StringComparison.Ordinal) ||
                    (namedType.Name.StartsWith("I", System.StringComparison.Ordinal) && 
                     namedType.Name.EndsWith("Hub", System.StringComparison.Ordinal)))
                {
                    return;
                }

                // 6. Skip CAP consumers (event consumers)
                if (namedType.Name.EndsWith("Consumer", System.StringComparison.Ordinal))
                {
                    return;
                }

                var diagnostic = Diagnostic.Create(Rule, namedType.Locations[0], namedType.Name);
                symbolContext.ReportDiagnostic(diagnostic);
            }, SymbolKind.NamedType);
        });
    }
}
