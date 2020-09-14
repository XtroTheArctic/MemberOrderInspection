using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Atesh.MemberOrderInspection
{
    // Types mentioned in this attribute are used for performance optimizations.
    [ElementProblemAnalyzer(typeof(ICSharpFile), HighlightingTypes = new[] { typeof(Highlighting) })]
    public class ProblemAnalyzer : ElementProblemAnalyzer<ICSharpFile>
    {
        protected override void Run(ICSharpFile Element, ElementProblemAnalyzerData Data, IHighlightingConsumer Consumer)
        {
            var RecursiveElementProcessor = new RecursiveElementProcessor(Consumer);
            RecursiveElementProcessor.ProcessBeforeInterior(Element);
            Element.ProcessDescendantsForResolve(RecursiveElementProcessor);
            RecursiveElementProcessor.ProcessAfterInterior(Element);
        }
    }
}