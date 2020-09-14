using System.Linq;
using Atesh.MemberOrderInspection;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(Highlighting.SeverityId, null, HighlightingGroupIds.CodeStyleIssues, Highlighting.Description, Highlighting.Description, Severity.ERROR)]

namespace Atesh.MemberOrderInspection
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.ERROR, OverloadResolvePriority = 0, ToolTipFormatString = Message)]
    public class Highlighting : IHighlighting
    {
        public string ToolTip => string.Format(Message, Declaration.DeclaredName);
        public string ErrorStripeToolTip => ToolTip;

        readonly ICSharpDeclaration Declaration;

        public Highlighting(ICSharpDeclaration Declaration) => this.Declaration = Declaration;

        public bool IsValid() => Declaration.IsValid();

        public DocumentRange CalculateRange()
        {
            if (Declaration.NameIdentifier == null)
            {
                switch (Declaration)
                {
                case IConstructorDeclaration _:
                case IDestructorDeclaration _: return Declaration.Children().First(X => X is IIdentifier).GetHighlightingRange();
                case IIndexerDeclaration _: return Declaration.Children().First(X => X is ITokenNode && X.GetText() == "this").GetHighlightingRange();
                default: return DocumentRange.InvalidRange;
                }
            }

            return Declaration.NameIdentifier.GetHighlightingRange();
        }

        internal const string SeverityId = nameof(Highlighting);
        internal const string Message = "Declaration order of '{0}' is incorrect.";
        internal const string Description = "Declaration order is incorrect.";
    }
}