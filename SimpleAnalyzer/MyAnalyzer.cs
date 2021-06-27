using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace SimpleAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MyAnalyzer : DiagnosticAnalyzer
    {
        // Metadata of the analyzer
        public const string DiagnosticId = "PHIL1229";

        // You could use LocalizedString but it's a little more complicated for this sample
        private const string Title = "Phil is wrong";
        private const string MessageFormat = "Methods name Phil aren't allowed";
        private const string Description = "They are the worst";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(AnalyzeNode, OperationKind.Invocation);
        }

        private static void AnalyzeNode(OperationAnalysisContext operationAnalysisContext)
        {
            var invocationOperation = (IInvocationOperation)operationAnalysisContext.Operation;

            if (invocationOperation.TargetMethod.Name != "Phil")
                return;

            var diagnostic = Diagnostic.Create(Rule, invocationOperation.Syntax.GetLocation());
            operationAnalysisContext.ReportDiagnostic(diagnostic);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}