using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.Linq;

namespace Bootstrap.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NoCommandChainingAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MOD002";
    private const string Category = "Architecture";

    private static readonly LocalizableString Title = "Command handlers should not send other commands";
    private static readonly LocalizableString MessageFormat = "CommandHandler '{0}' cannot send other Commands. Use domain logic or queries instead.";
    private static readonly LocalizableString Description = "Command handlers should not invoke other commands to avoid coupling. Use domain logic or queries instead.";

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
            
            // Find ICommandHandler<> interface
            var commandHandlerInterface = compilation.GetTypeByMetadataName("BuildingBlocks.CQRS.ICommandHandler`1");
            var commandHandlerInterface2 = compilation.GetTypeByMetadataName("BuildingBlocks.CQRS.ICommandHandler`2");
            
            // Find IBaseCommand interface to identify commands
            var baseCommandInterface = compilation.GetTypeByMetadataName("BuildingBlocks.CQRS.IBaseCommand");
            
            // Find ISender interface
            var senderInterface = compilation.GetTypeByMetadataName("MediatR.ISender");

            if (baseCommandInterface == null || senderInterface == null)
            {
                return;
            }

            compilationContext.RegisterOperationAction(operationContext =>
            {
                var invocation = (IInvocationOperation)operationContext.Operation;
                var containingType = invocation.SemanticModel?.GetEnclosingSymbol(invocation.Syntax.SpanStart)?.ContainingType;
                
                if (containingType == null)
                {
                    return;
                }

                // Check if the containing type is a command handler
                var isCommandHandler = containingType.AllInterfaces.Any(i =>
                    SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, commandHandlerInterface) ||
                    SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, commandHandlerInterface2));

                if (!isCommandHandler)
                {
                    return;
                }

                // Check if this is a call to ISender.Send()
                if (invocation.TargetMethod.Name != "Send" && invocation.TargetMethod.Name != "SendAsync")
                {
                    return;
                }

                var receiverType = invocation.TargetMethod.ContainingType;
                if (receiverType == null)
                {
                    return;
                }

                // Check if the receiver implements ISender
                var implementsSender = receiverType.AllInterfaces.Any(i =>
                    SymbolEqualityComparer.Default.Equals(i, senderInterface));

                if (!implementsSender && !SymbolEqualityComparer.Default.Equals(receiverType, senderInterface))
                {
                    return;
                }

                // Get the argument being sent
                if (invocation.Arguments.Length == 0)
                {
                    return;
                }

                var firstArgument = invocation.Arguments[0];
                var argumentType = firstArgument.Value?.Type;

                if (argumentType == null)
                {
                    return;
                }

                // Check if the argument type implements IBaseCommand (is a command)
                var isCommand = argumentType.AllInterfaces.Any(i =>
                    SymbolEqualityComparer.Default.Equals(i, baseCommandInterface));

                if (isCommand)
                {
                    var diagnostic = Diagnostic.Create(Rule, invocation.Syntax.GetLocation(), containingType.Name);
                    operationContext.ReportDiagnostic(diagnostic);
                }
            }, OperationKind.Invocation);
        });
    }
}
