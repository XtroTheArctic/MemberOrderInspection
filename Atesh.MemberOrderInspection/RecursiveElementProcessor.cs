using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace Atesh.MemberOrderInspection
{
    class RecursiveElementProcessor : IRecursiveElementProcessor
    {
        public bool ProcessingIsFinished => false;

        readonly IHighlightingConsumer Consumer;
        readonly Key<Data> DataKey = new Key<Data>("Atesh.MemberOrderInspection.Data");

        public RecursiveElementProcessor(IHighlightingConsumer Consumer) => this.Consumer = Consumer;

        public bool InteriorShouldBeProcessed(ITreeNode Element) => Element is INamespaceDeclarationHolder || Element is INamespaceBody || Element is IClassBody || Element is IInterfaceDeclaration || Element is IStructDeclaration || Element is IClassDeclaration;

        public void ProcessBeforeInterior(ITreeNode Element)
        {
            if (!IsContainer(Element)) return;

            var Data = Element.UserData.GetOrCreateDataNoLock(DataKey, () => new Data());

            var ChildStack = new Stack<ITreeNode>();
            var Child = Element.FirstChild;

            do
            {
                while (Child != null)
                {
                    if (Child is IMultipleEventDeclaration || Child is IMultipleFieldDeclaration || Child is IMultipleConstantDeclaration)
                    {
                        ChildStack.Push(Child);
                        Child = Child.FirstChild;
                    }

                    if (Child is ICSharpDeclaration Declaration)
                    {
                        var (DeclarationMemberOrder, DeclarationIsStatic) = GetMemberType(Declaration);

                        void CompareMemberOrder()
                        {
                            if (Data.CurrentMaximumMemberOrder < DeclarationMemberOrder) Data.CurrentMaximumMemberOrder = DeclarationMemberOrder;
                            else if (Data.CurrentMaximumMemberOrder > DeclarationMemberOrder) AddHighlighting(Declaration);
                        }

                        if (DeclarationMemberOrder != MemberOrder.None)
                        {
                            if (Data.CurrentMaximumMemberOrderIsStatic)
                            {
                                if (DeclarationIsStatic) CompareMemberOrder();
                                else AddHighlighting(Declaration);
                            }
                            else
                            {
                                if (DeclarationIsStatic)
                                {
                                    Data.CurrentMaximumMemberOrderIsStatic = true;
                                    Data.CurrentMaximumMemberOrder = DeclarationMemberOrder;
                                }
                                else CompareMemberOrder();
                            }
                        }
                    }

                    Child = Child.NextSibling;
                }

                Child = ChildStack.TryPop()?.NextSibling;
            } while (Child != null);
        }

        public void ProcessAfterInterior(ITreeNode Element)
        {
            if (IsContainer(Element)) Element.UserData.RemoveKey(DataKey);
        }

        void AddHighlighting(ICSharpDeclaration Declaration) => Consumer.AddHighlighting(new Highlighting(Declaration));

        class Data
        {
            public MemberOrder CurrentMaximumMemberOrder;
            public bool CurrentMaximumMemberOrderIsStatic;
        }

        static (MemberOrder MemberOrder, bool IsStatic) GetMemberType(ICSharpDeclaration Declaration)
        {
            switch (Declaration)
            {
            case IPropertyDeclaration PropertyDeclaration: return (GetMemberOrder(PropertyDeclaration.GetAccessRights(), MemberOrder.PublicProperty), PropertyDeclaration.IsStatic);
            case IIndexerDeclaration IndexerDeclaration: return (MemberOrder.Indexer, IndexerDeclaration.IsStatic);
            case IEventDeclaration EventDeclaration: return (GetMemberOrder(EventDeclaration.GetAccessRights(), MemberOrder.PublicEvent), EventDeclaration.IsStatic);
            case IFieldDeclaration FieldDeclaration: return (GetMemberOrder(FieldDeclaration.GetAccessRights(), MemberOrder.PublicField), FieldDeclaration.IsStatic);
            case IConstructorDeclaration ConstructorDeclaration: return (GetMemberOrder(ConstructorDeclaration.GetAccessRights(), MemberOrder.PublicConstructor), ConstructorDeclaration.IsStatic);
            case IDestructorDeclaration DestructorDeclaration: return (MemberOrder.Destructor, DestructorDeclaration.IsStatic);
            case IMethodDeclaration MethodDeclaration: return (MemberOrder.Method, MethodDeclaration.IsStatic);
            case IInterfaceDeclaration _: return (MemberOrder.Interface, false);
            case IStructDeclaration _: return (MemberOrder.Struct, false);
            case IClassDeclaration ClassDeclaration: return (MemberOrder.Class, ClassDeclaration.IsStatic);
            case IConstantDeclaration ConstantDeclaration: return (GetMemberOrder(ConstantDeclaration.GetAccessRights(), MemberOrder.PublicConstant), false);
            case IDelegateDeclaration _: return (MemberOrder.Delegate, false);
            case IEnumDeclaration _: return (MemberOrder.Enum, false);
            default: return (MemberOrder.None, false);
            }
        }

        static MemberOrder GetMemberOrder(AccessRights AccessRights, MemberOrder PublicMemberOrder)
        {
            switch (AccessRights)
            {
            case AccessRights.PUBLIC: return PublicMemberOrder;
            case AccessRights.INTERNAL: return PublicMemberOrder + 1;
            case AccessRights.PROTECTED_OR_INTERNAL: return PublicMemberOrder + 2;
            case AccessRights.PROTECTED: return PublicMemberOrder + 3;
            case AccessRights.PROTECTED_AND_INTERNAL: return PublicMemberOrder + 4;
            default: return PublicMemberOrder + 5;
            }
        }

        static bool IsContainer(ITreeNode Element) => Element is ICSharpFile || Element is INamespaceBody || Element is IInterfaceDeclaration || Element is IStructDeclaration || Element is IClassBody;
    }
}