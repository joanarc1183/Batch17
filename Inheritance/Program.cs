using System;

namespace InheritanceDemo
{
    // =====================================================
    // 1. BASE CLASS
    // =====================================================
    // Base class that will be inherited by other classes
    public abstract class Asset
    {
        // Required member (C# 11+)
        // Must be set via object initializer or constructor
        public required string Name;

        // Virtual property
        // Can be overridden by derived classes
        public virtual decimal Liability => 0;  // Virtual property with a default implementation


        // Abstract property
        // Must be implemented by derived classes
        public abstract decimal NetValue { get; }

        // Virtual method with covariant return support
        public virtual Asset Clone()
        {
            return new SimpleAsset { Name = Name };
        }
    }

    // Helper class for cloning
    public class SimpleAsset : Asset
    {
        public override decimal NetValue => 0;
    }

    // =====================================================
    // 2. DERIVED CLASS: STOCK
    // =====================================================
    public class Stock : Asset
    {
        public long SharesOwned;
        public decimal CurrentPrice;

        // Implements abstract property
        public override decimal NetValue => SharesOwned * CurrentPrice;

        // Uses base implementation of Liability (0)
    }

    // =====================================================
    // 3. DERIVED CLASS: HOUSE
    // =====================================================
    public class House : Asset
    {
        public decimal Mortgage;

        // Override virtual property
        public override decimal Liability => Mortgage;  // House overrides Liability

        // Implement abstract property
        public override decimal NetValue => -Mortgage;

        // Covariant return type (C# 9+)
        // Returns House instead of Asset
        public override House Clone()
        {
            return new House { Name = Name, Mortgage = Mortgage };
        }

        public override string ToString()
        {
            return $"House(Name={Name}, Mortgage={Mortgage})";
        }
    }

    // =====================================================
    // 4. POLYMORPHISM
    // =====================================================
    class PolymorphismDemo
    {
        // Accepts base type
        public static void Display(Asset asset)
        {
            Console.WriteLine($"Asset name: {asset.Name}");
            Console.WriteLine($"Net value: {asset.NetValue}");
        }
    }

    // =====================================================
    // 5. HIDING vs OVERRIDING
    // =====================================================
    public class CounterBase
    {
        public int Counter = 1;

        public virtual void Show()
        {
            Console.WriteLine("CounterBase.Show()");
        }
    }

    public class CounterHider : CounterBase
    {
        // Hides base member (NOT polymorphic)
        public new int Counter = 2;

        public new void Show()
        {
            Console.WriteLine("CounterHider.Show()");
        }
    }

    public class CounterOverrider : CounterBase
    {
        // Overrides virtual method (polymorphic)
        public override void Show()
        {
            Console.WriteLine("CounterOverrider.Show()");
        }
    }

    // =====================================================
    // 6. SEALED CLASS AND SEALED METHOD
    // =====================================================
    public sealed class FinalAsset
    {
        // This class cannot be inherited
        public string Name = "Final";
    }

    public class SecureHouse : House
    {
        // Sealed override
        // Prevents further overriding
        public sealed override decimal Liability => Mortgage;
    }

    // =====================================================
    // 7. CONSTRUCTORS AND INHERITANCE
    // =====================================================
    public class BaseClass
    {
        public int X;

        public BaseClass()
        {
            X = 1;
            Console.WriteLine("BaseClass parameterless constructor");
        }

        public BaseClass(int x)
        {
            X = x;
            Console.WriteLine("BaseClass(int) constructor");
        }
    }

    public class SubClass : BaseClass
    {
        // Explicitly calls base constructor
        public SubClass(int x) : base(x)
        {
            Console.WriteLine("SubClass constructor");
        }
    }

    // =====================================================
    // 8. INHERITANCE WITH PRIMARY CONSTRUCTORS (C# 12+)
    // =====================================================
    public class Vehicle(int speed)
    {
        public int Speed => speed;
    }

    public class Car(int speed, int doors) : Vehicle(speed)
    {
        public int Doors => doors;
    }

    // =====================================================
    // 9. OVERLOADING & RESOLUTION
    // =====================================================
    class OverloadDemo
    {
        public static void Foo(Asset a)
        {
            Console.WriteLine("Foo(Asset)");
        }

        public static void Foo(House h)
        {
            Console.WriteLine("Foo(House)");
        }
    }

    // =====================================================
    // STACK USING object (TYPE UNIFICATION)
    // =====================================================
    public class Stack
    {
        int position;
        object[] data = new object[10]; // Can hold any type because it stores objects

        public void Push(object obj) { data[position++] = obj; } // Pushes any object
        public object Pop() { return data[--position]; } // Pops an object
    }

    // =====================================================
    // 10. MAIN PROGRAM
    // =====================================================
    class Program
    {
        static void Main()
        {
            // ----- Polymorphism -----
            Asset stock = new Stock
            {
                Name = "MSFT",
                SharesOwned = 100,
                CurrentPrice = 300
            };

            Asset house = new House
            {
                Name = "Villa",
                Mortgage = 250000
            };

            PolymorphismDemo.Display(stock);
            PolymorphismDemo.Display(house);

            // ----- Upcasting & Downcasting -----
            // Upcasting occurs when a derived class object is referenced by a base class variable.
            Asset a = new Stock { Name = "AAPL", SharesOwned = 10, CurrentPrice = 200 };

            // Downcasting is converting a base class reference back to a derived type.
            // Safe downcasting using "is" with pattern variable
            if (a is Stock s)
            {
                Console.WriteLine($"Shares owned: {s.SharesOwned}");
            }

            // Safe downcasting using "as"
            Stock? s2 = a as Stock;
            if (s2 != null)
            {
                Console.WriteLine("Downcast using 'as' succeeded");
            }

            // ----- new vs override -----
            CounterBase b1 = new CounterHider();
            CounterBase b2 = new CounterOverrider();

            b1.Show(); // BaseClass.Show()
            b2.Show(); // CounterOverrider.Show()

            // ----- Constructors -----
            SubClass sc = new SubClass(10);
            Console.WriteLine($"X = {sc.X}");

            // ----- Primary constructor inheritance -----
            Car car = new Car(120, 4);
            Console.WriteLine($"Car speed: {car.Speed}, doors: {car.Doors}");

            // ----- Overload resolution -----
            Asset assetHouse = new House { Name = "Mansion", Mortgage = 500000 };
            OverloadDemo.Foo(assetHouse);          // Foo(Asset)
            OverloadDemo.Foo((House)assetHouse);   // Foo(House)
            OverloadDemo.Foo((dynamic)assetHouse); // Foo(House)

            // =====================================================================================
            // ----- Object Type -----
            // Every type in C# ultimately derives from System.Object

            object o1 = "hello";     // string → object (upcasting)
            object o2 = 42;          // int → object (boxing)
            object o3 = new House { Name = "Villa", Mortgage = 100_000 };

            Console.WriteLine(o1);   // calls string.ToString()
            Console.WriteLine(o2);   // calls int.ToString()
            Console.WriteLine(o3);   // calls overridden ToString() if available
            
            // ----- Stack -----
            Stack stack = new Stack();

            stack.Push("sausage");                      // string
            stack.Push(3);                              // int (boxed)
            stack.Push(new Stock { Name = "MSFT" });    // custom object

            Stock objStock = (Stock)stack.Pop();    // Pop Stock
            int num = (int)stack.Pop();             // Pop 3
            string word = (string)stack.Pop();      // Pop "sausage"

            Console.WriteLine(objStock.Name);
            Console.WriteLine(num);
            Console.WriteLine(word);

            // ----- Boxing & Unboxing -----
            int x = 9;
            // Boxing: value type → object (heap allocation)
            object boxed = x;
            // Unboxing: object → value type (explicit cast required)
            int y = (int)boxed;
            Console.WriteLine($"x = {x}, boxed = {boxed}, y = {y}");
            // Copy semantics
            x = 20;
            Console.WriteLine($"After change: x = {x}, boxed = {boxed}");

            // ----- Type Check -----
            // Static type checking (compile-time)
            // int x = "5";  // ❌ Compile-time error
            object o = "5";
            // Runtime type checking
            // int y = (int)o; // ❌ InvalidCastException
            if (o is string str)
            {
                Console.WriteLine($"Runtime check succeeded: {str}");
            }
            
            // ----- GetType() vs typeof -----
            House h = new House { Name = "Mansion", Mortgage = 500_000 };
            // Runtime type
            Console.WriteLine(h.GetType().Name);
            Console.WriteLine(h.GetType().FullName);
            // Compile-time type
            Console.WriteLine(typeof(House).Name);
            // Comparison
            Console.WriteLine(h.GetType() == typeof(House)); // true
            // Value type runtime type
            int w = 10;
            Console.WriteLine(w.GetType().Name); // Int32

            // ----- Overriding ToString() -----
            House rumah = new House { Name = "Villa", Mortgage = 250_000 };
            Console.WriteLine(rumah);   // Calls overridden ToString()

            // ----- ToString AND BOXING -----
            int x = 1;
            // No boxing
            string s1 = x.ToString();
            // Boxing occurs
            object box = x;
            string s2 = box.ToString();
            Console.WriteLine($"{s1}, {s2}");

            // ----- Object Base Members -----
            House h1 = new House { Name = "Villa", Mortgage = 100_000 };
            House h2 = h1;

            Console.WriteLine(h1.Equals(h2));                 // true
            Console.WriteLine(object.ReferenceEquals(h1, h2)); // true
            Console.WriteLine(h1.GetHashCode());
            Console.WriteLine(h1.GetType().BaseType);          // Asset
        }
    }
}
