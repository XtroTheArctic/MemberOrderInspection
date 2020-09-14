namespace Atesh.MemberOrderInspection
{
    enum MemberOrder
    {
        None,

        PublicProperty,
        InternalProperty,
        ProtectedInternalProperty,
        ProtectedProperty,
        PrivateProtectedProperty,
        PrivateProperty,

        Indexer,

        PublicEvent,
        InternalEvent,
        ProtectedInternalEvent,
        ProtectedEvent,
        PrivateProtectedEvent,
        PrivateEvent,

        PublicField,
        InternalField,
        ProtectedInternalField,
        ProtectedField,
        PrivateProtectedField,
        PrivateField,

        PublicConstructor,
        InternalConstructor,
        ProtectedInternalConstructor,
        ProtectedConstructor,
        PrivateProtectedConstructor,
        PrivateConstructor,

        Destructor,

        Method,

        Interface,

        Struct,

        Class,

        PublicConstant,
        InternalConstant,
        ProtectedInternalConstant,
        ProtectedConstant,
        PrivateProtectedConstant,
        PrivateConstant,

        Delegate,
        Enum
    }
}