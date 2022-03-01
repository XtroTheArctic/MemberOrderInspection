using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Atesh.MemberOrderInspection
{
    [RegisterConfigurableSeverity(SeverityId, null, HighlightingGroupIds.CodeStyleIssues, Description, Description, Severity.ERROR)]
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
            if (Declaration.NameIdentifier != null) return Declaration.NameIdentifier.GetHighlightingRange();

            switch (Declaration)
            {
            case IConstructorDeclaration ConstructorDeclaration: return ConstructorDeclaration.TypeName.GetHighlightingRange();
            case IDestructorDeclaration DestructorDeclaration: return DestructorDeclaration.TypeName.GetHighlightingRange();
            case IIndexerDeclaration IndexerDeclaration: return IndexerDeclaration.ThisKeyword.GetHighlightingRange();
            default: return DocumentRange.InvalidRange;
            }
        }

        const string SeverityId = nameof(Highlighting);
        const string Description = "Declaration order is incorrect.";
        const string Message = "Declaration order of '{0}' is incorrect.";
    }
}