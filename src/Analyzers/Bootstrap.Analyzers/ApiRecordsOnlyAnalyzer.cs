using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Bootstrap.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ApiRecordsOnlyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MOD004";
    private const string Category = "Design";

    private static readonly LocalizableString Title = "Consider using record instead of class for contracts";
    private static readonly LocalizableString MessageFormat = "Consider using 'record' instead of 'class' for immutable contract '{0}'";
    private static readonly LocalizableString Description = "In contract and API projects, prefer records over classes for immutable data transfer objects.";

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
            var assemblyName = compilation.AssemblyName ?? string.Empty;

            // Only analyze projects with "Contracts" or "Api" in their name
            if (!assemblyName.Contains("Contracts", System.StringComparison.OrdinalIgnoreCase) &&
                !assemblyName.Contains("Api", System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            compilationContext.RegisterSyntaxNodeAction(syntaxContext =>
            {
                var classDeclaration = (ClassDeclarationSyntax)syntaxContext.Node;
                
                // Skip static classes
                if (classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
                {
                    return;
                }

                // Get the semantic model
                var semanticModel = syntaxContext.SemanticModel;
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

                if (classSymbol == null)
                {
                    return;
                }

                // Skip controllers (classes that end with "Controller")
                if (classSymbol.Name.EndsWith("Controller", System.StringComparison.Ordinal))
                {
                    return;
                }

                // Skip DbContext classes
                var baseType = classSymbol.BaseType;
                while (baseType != null)
                {
                    if (baseType.Name.Contains("DbContext", System.StringComparison.Ordinal))
                    {
                        return;
                    }
                    baseType = baseType.BaseType;
                }

                // Skip exception classes
                if (classSymbol.Name.EndsWith("Exception", System.StringComparison.Ordinal))
                {
                    return;
                }

                // Skip attribute classes
                if (classSymbol.Name.EndsWith("Attribute", System.StringComparison.Ordinal))
                {
                    return;
                }

                // Skip classes with mutable state (having settable properties without init)
                var hasNonInitProperties = classSymbol.GetMembers()
                    .OfType<IPropertySymbol>()
                    .Any(p => p.SetMethod != null && !p.SetMethod.IsInitOnly);

                // If it has mutable properties, it might be intentional
                if (hasNonInitProperties)
                {
                    return;
                }

                var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classSymbol.Name);
                syntaxContext.ReportDiagnostic(diagnostic);
            }, SyntaxKind.ClassDeclaration);
        });
    }
}
