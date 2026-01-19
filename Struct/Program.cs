using System;

namespace StructDemo
{
    // =====================================================
    // 1. BASIC STRUCT (VALUE TYPE SEMANTICS)
    // =====================================================
    // Structs are value types.
    // Assigning a struct copies the entire value, not a reference.
    struct Point
    {
        public int X;
        public int Y;
    }

    // =====================================================
    // 2. VALUE TYPE COPYING BEHAVIOR
    // =====================================================
    class ValueTypeCopyDemo
    {
        public static void Run()
        {
            Point p1 = new Point { X = 1, Y = 2 };
            Point p2 = p1;        // COPY happens here

            p2.X = 99;

            // p1 is NOT affected because structs use value semantics
            Console.WriteLine($"p1: ({p1.X}, {p1.Y})"); // (1, 2)
            Console.WriteLine($"p2: ({p2.X}, {p2.Y})"); // (99, 2)
        }
    }

    // =====================================================
    // 3. DEFAULT CONSTRUCTOR & default KEYWORD
    // =====================================================
    struct DefaultPoint
    {
        int x = 1;
        int y;

        // Explicit parameterless constructor (C# 11+)
        public DefaultPoint()
        {
            y = 1;
        }

        public override string ToString() => $"({x}, {y})";
    }

    class DefaultConstructorDemo
    {
        public static void Run()
        {
            DefaultPoint p1 = new DefaultPoint(); // explicit constructor
            DefaultPoint p2 = default;            // implicit default constructor

            Console.WriteLine($"p1 = {p1}"); // (1, 1)
            Console.WriteLine($"p2 = {p2}"); // (0, 0)
        }
    }

    // =====================================================
    // 4. STRUCTS CANNOT PARTICIPATE IN INHERITANCE
    // =====================================================
    // ❌ INVALID:
    // struct Child : Parent { }
    //
    // Structs implicitly inherit from System.ValueType → object
    // Therefore:
    // - no virtual
    // - no abstract
    // - no protected
    // - no finalizers

    // =====================================================
    // 5. STRUCT AS FIELD IN A CLASS (HEAP STORAGE)
    // =====================================================
    class Container
    {
        // This struct lives on the heap
        public Point Location;
    }

    // =====================================================
    // 6. NULLABILITY OF STRUCTS
    // =====================================================
    class NullabilityDemo
    {
        public static void Run()
        {
            Point p = new Point();     // cannot be null
            // p = null;               // ❌ Compile-time error

            Point? nullablePoint = null; // Nullable struct
            Console.WriteLine(nullablePoint.HasValue); // false
        }
    }

    // =====================================================
    // 7. READONLY STRUCT (IMMUTABILITY)
    // =====================================================
    // readonly struct guarantees immutability
    readonly struct ReadOnlyPoint
    {
        public readonly int X;
        public readonly int Y;

        public ReadOnlyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Safe: does not modify state
        public int Sum() => X + Y;
    }

    // =====================================================
    // 8. READONLY FUNCTIONS (C# 8+)
    // =====================================================
    struct MutablePoint
    {
        public int X;
        public int Y;

        // readonly method promises not to modify the struct
        public readonly int GetX() => X;

        // ❌ Compile-time error if uncommented
        // public readonly void ResetX() => X = 0;
    }

    // =====================================================
    // 9. BOXING & UNBOXING WITH STRUCTS
    // =====================================================
    class BoxingDemo
    {
        public static void Run()
        {
            Point p = new Point { X = 3, Y = 4 };

            // Boxing: struct → object (heap allocation)
            object boxed = p;

            // Unboxing: object → struct (explicit cast)
            Point unboxed = (Point)boxed;

            Console.WriteLine($"Unboxed: ({unboxed.X}, {unboxed.Y})");
        }
    }

    // =====================================================
    // 10. TOSTRING OVERRIDE IN STRUCT
    // =====================================================
    struct Person
    {
        public string Name;
        public int Age;

        // Override ToString() from System.ValueType → object
        public override string ToString()
        {
            return $"{Name}, Age {Age}";
        }
    }

    // =====================================================
    // 11. REF STRUCT (STACK-ONLY STRUCT)
    // =====================================================
    // ref struct enforces stack-only allocation
    ref struct StackOnlyPoint
    {
        public int X;
        public int Y;
    }

    class RefStructDemo
    {
        public static void Run()
        {
            StackOnlyPoint p;
            p.X = 10;
            p.Y = 20;

            Console.WriteLine($"Ref struct point: ({p.X}, {p.Y})");

            // ❌ INVALID USAGE:
            // object o = p;                    // boxing → heap
            // var arr = new StackOnlyPoint[5]; // arrays live on heap
        }
    }

    // =====================================================
    // 12. WHY REF STRUCT EXISTS (PERFORMANCE NOTE)
    // =====================================================
    // Ref structs are used by Span<T> and ReadOnlySpan<T>
    // to safely wrap stack memory without heap allocation.

    // =====================================================
    // 13. MAIN PROGRAM
    // =====================================================
    class Program
    {
        static void Main()
        {
            ValueTypeCopyDemo.Run();
            DefaultConstructorDemo.Run();
            NullabilityDemo.Run();
            BoxingDemo.Run();
            RefStructDemo.Run();

            Person p = new Person { Name = "Joan", Age = 22 };
            Console.WriteLine(p); // Calls overridden ToString()
        }
    }
}
