using System;
using System.Runtime.CompilerServices;

// =====================================================
// FRIEND ASSEMBLY (COMMENT ONLY)
// =====================================================
// This attribute allows another assembly to access internal members.
// [assembly: InternalsVisibleTo("FriendAssembly")]

// =====================================================
// FILE-SCOPED TYPE (C# 11+)
// =====================================================
// Accessible ONLY within this source file
file class FileHelper
{
    public static void Help()
    {
        Console.WriteLine("FileHelper accessed inside the same file.");
    }
}

// =====================================================
// TYPE ACCESSIBILITY
// =====================================================

// internal is the default for non-nested types
// This class is accessible only within the same assembly
class InternalClass
{
    public void SayHello()
    {
        Console.WriteLine("Hello from InternalClass");
    }
}

// public class can be accessed from any assembly
public class PublicClass
{
    public void SayHello()
    {
        Console.WriteLine("Hello from PublicClass");
    }
}

// =====================================================
// MEMBER ACCESS MODIFIERS
// =====================================================
public class AccessDemo
{
    // private is the default for members
    // Accessible only within this class
    private int privateField = 1;

    // internal: accessible within the same assembly
    internal int internalField = 2;

    // protected: accessible in this class and derived classes
    protected int protectedField = 3;

    // protected internal: union of protected OR internal
    protected internal int protectedInternalField = 4;

    // private protected: intersection of protected AND internal
    private protected int privateProtectedField = 5;

    public void ShowFields()
    {
        Console.WriteLine(privateField);
        Console.WriteLine(internalField);
        Console.WriteLine(protectedField);
        Console.WriteLine(protectedInternalField);
        Console.WriteLine(privateProtectedField);
    }
}

// =====================================================
// INHERITANCE + ACCESS MODIFIERS
// =====================================================
public class DerivedDemo : AccessDemo
{
    public void TestAccess()
    {
        // privateField ❌ NOT accessible
        Console.WriteLine(internalField);            // ✔ same assembly
        Console.WriteLine(protectedField);           // ✔ derived class
        Console.WriteLine(protectedInternalField);   // ✔ protected OR internal
        Console.WriteLine(privateProtectedField);    // ✔ same assembly + derived
    }
}

// =====================================================
// ACCESSIBILITY CAPPING
// =====================================================

// This class is internal (default)
class CappedClass
{
    // Declared public, but effectively INTERNAL
    // because the containing type is internal
    public void Foo()
    {
        Console.WriteLine("Foo from internal class");
    }
}

// =====================================================
// OVERRIDING ACCESSIBILITY RULES
// =====================================================
public class BaseClass
{
    // Protected virtual method
    protected virtual void DoWork()
    {
        Console.WriteLine("BaseClass.DoWork");
    }
}

public class DerivedClass : BaseClass
{
    // Accessibility must be IDENTICAL
    protected override void DoWork()
    {
        Console.WriteLine("DerivedClass.DoWork");
    }

    // ❌ NOT allowed:
    // public override void DoWork() { }
}

// =====================================================
// SUBCLASS ACCESSIBILITY RULE
// =====================================================

// internal base class
internal class InternalBase { }

// ❌ NOT allowed:
// public class InvalidDerived : InternalBase { }

// ✔ Allowed: equal or less accessible
internal class ValidDerived : InternalBase { }

// =====================================================
// DEMO PROGRAM
// =====================================================
class Program
{
    static void Main()
    {
        // File-scoped type (same file)
        FileHelper.Help();

        // Internal vs public types
        InternalClass ic = new InternalClass();
        ic.SayHello();

        PublicClass pc = new PublicClass();
        pc.SayHello();

        // Member access
        AccessDemo demo = new AccessDemo();
        demo.ShowFields();

        DerivedDemo derived = new DerivedDemo();
        derived.TestAccess();

        // Accessibility capping
        CappedClass capped = new CappedClass();
        capped.Foo(); // Accessible here (same assembly)

        // Polymorphism with protected members
        BaseClass obj = new DerivedClass();
        // obj.DoWork(); ❌ Not accessible (protected)
    }
}