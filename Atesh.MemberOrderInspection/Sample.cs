#pragma warning disable 649
#pragma warning disable 67
#pragma warning disable 169
// ReSharper disable UnusedType.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable EventNeverSubscribedTo.Local
// ReSharper disable ValueParameterNotUsed
// ReSharper disable EmptyDestructor
// ReSharper disable CheckNamespace
// ReSharper disable UnusedParameter.Local
// ReSharper disable BadDeclarationBracesLineBreaks
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global

using System;

class Sample
{
    //static int Bbbbb;

    public bool PB => true;
    internal bool IB => true;
    protected internal bool PiB => true;
    protected bool PpB => true;
    protected private bool PppB => true;
    private bool PpppB => true;

    bool this[int I]
    {
        get => false;
        set { }
    }

    public event Action Event;
    event Action Event2;

    public bool PF;
    internal bool IF;
    protected internal bool PiF;
    protected bool PpF;
    protected private bool PppF;
    private bool PpppF;

    public Sample() { }
    Sample(int A) { }

    ~Sample() { }

    void Method() { }

    interface Interface { }

    struct Struct { }

    class Class { }

    public const int Const2 = 0;
    const int Const = 0;

    delegate void Delegate();

    enum Enum
    {
        None
    }

    public static bool SPB => true;
    static bool Aaaa => true;

    static bool Bbbb;
}