using System;
using System.Drawing;
using System.Text;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Nullable
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("1..............................................................");
            // 1. Nullable value Types (T?)

            string s = null;    // OK, 's' refers to nothing

            // int i = null;       // ❌ Compile error: value type cannot be null
        
            int? i = null;      // OK, 'i' can now be null
            System.Console.WriteLine(i == null);    // true


            System.Console.WriteLine("2..............................................................");
            // 2. System.Nullable<T>

            // T? is syntactic sugar for Nullable<T>
            Nullable<int> i1 = new Nullable<int>();  // 'i.HasValue' will be false, 'i.Value' will be default(int)
            System.Console.WriteLine(i1.HasValue);  // False
            // System.Console.WriteLine(i1.Value);  // ❌ Throws InvalidOperationException

            // safe ways to read the value
            System.Console.WriteLine(i1.GetValueOrDefault());   // 0
            System.Console.WriteLine(i1.GetValueOrDefault(99)); // 99


            System.Console.WriteLine("3..............................................................");
            // 3. Nullable COnversions

            // T -> T? (implicit and safe)
            int x = 5;
            int? nullableX = x;     // Implicit conversion from int to int?
            System.Console.WriteLine(nullableX);

            // T? -> T (explicit, may throw if null)
            int? p = 5; 
            int q = (int)p;         // Explicit conversion from int? to int, same as p.Value
            System.Console.WriteLine(q);

            // int? p = null;
            // int q = (int)p;      // ❌ InvalidOperationException


            System.Console.WriteLine("4..............................................................");
            // 4. Boxing and Unboing Nullable Values
            int? nb = 10;
            object o1 = nb;             // HasValue=true → boxes the int value
            object o2 = (int?) null;    // HasValue=false → boxes as null

            object o3 = "string";
            int? result = o3 as int?;     // 'o' is not an int, so 'x' becomes null
            System.Console.WriteLine(result.HasValue);   // false


            System.Console.WriteLine("5..............................................................");
            // 5. Operator lifting
            int? j = 5;
            int? k = 10;
            bool l = j < k;         // This compiles and works correctly

            bool l1 = (j.HasValue && k.HasValue) ? (j.Value < k.Value) : false;
            
            // Equality Operators (== and !=)
            System.Console.WriteLine( (int?)null == (int?)null ); // True
            System.Console.WriteLine( j == k );    // False (5 == 10)
            System.Console.WriteLine( j == null ); // False (5 == null)
            System.Console.WriteLine( k == null ); // True (null == null)
            System.Console.WriteLine( k != 5 );    // True (null != 5)
        
            // Relational Operators (<, <=, >=, >)
            int? m = 5;
            int? n = null;
            System.Console.WriteLine( m < 6 ); // True (5 < 6)
            System.Console.WriteLine( n < 6 ); // False (null < 6)
            System.Console.WriteLine( n > 6 ); // False (null > 6)
        
            // All Other Operators (+, -, *, /, %, &, |, ^, <<, >>, !, ~, ++, --)
            int? u = 5;
            int? v = null;
            System.Console.WriteLine(u + 5);  // 10 (5 + (int?)5)
            System.Console.WriteLine(u + v);  // null (any null operand makes result null)
            // the translation u + v would be ..
            int? w = (u.HasValue && v.HasValue) ? (int?)(u.Value + v.Value) : null;
        

            System.Console.WriteLine("6..............................................................");
            // 6. Mixing Nullable and Non-Nullable Types
            int? a = null;
            int b = 2;
            int? c = a + b; // 'b' is implicitly converted to 'int?', then the lifted addition rule applies. 'c' will be null.
            System.Console.WriteLine(c);    // null


            System.Console.WriteLine("7..............................................................");
            // 7. Null-Aware Operators (?? and ?.)
            
            // Null-Coalescing Operator (??)
            int? e = null;
            int f = e ?? 5; // If e is null, f becomes 5. Otherwise, f becomes e.Value.
            System.Console.WriteLine(f);

            // Null-Conditional Operator (?.)
            StringBuilder sb = null;
            int? length = sb?.ToString().Length; // 'sb' is null, so 'length' becomes null
            System.Console.WriteLine(length);

            int length1 = sb?.ToString().Length ?? 0; // If 'sb' is null, 'length' becomes 0 instead of null
            System.Console.WriteLine(length1);


            System.Console.WriteLine("8..............................................................");
            // 8. Common Use Cases

            // Database or optional data
            Customer cust = new Customer();
            cust.AccountBalance = null;
            // Output to verify nullable database field
            Console.WriteLine(cust.AccountBalance == null
                ? "AccountBalance is NULL"
                : $"AccountBalance: {cust.AccountBalance}");

            // Ambient property example
            Grid grid = new Grid { Color = ConsoleColor.Blue };
            Row row = new Row(grid);
            row.Color = ConsoleColor.Blue;      // Stored as null (inherits parent value)
            Console.WriteLine($"Row.Color: {row.Color}"); // Inherited from Grid


            System.Console.WriteLine("9..............................................................");
            // 9. Alternatives to Nullable Value Types (Historical Context)
            int color = "Pink".IndexOf('b');        // Returns -1 if 'b' is not found
            System.Console.WriteLine(color);
        }

        // Represents a nullable value type (T?)

        public struct Nullable<T> where T : struct      // 'where T : struct' constraint
        {
            // public T Value { get; }
            private readonly T _value;

            // public bool HasValue { get; }
            private readonly bool _hasValue;

            public Nullable(T value)
            {
                _value = value;
                _hasValue = true;
            }

            public T value
            {
                get
                {
                    if (!_hasValue)
                    {
                        throw new InvalidOperationException("No value present");
                    }
                    return _value;
                }
            }

            public bool HasValue => _hasValue;

            // // Methods for convenience:
            // public T GetValueDefault();
            public T GetValueOrDefault()
            {
                return _hasValue ? _value : default;
            }

            // public T GetValueDefault(T defaultValue);
            public T GetValueOrDefault(T defaultValue)
            {
                return _hasValue ? _value : defaultValue;
            }

            // // ... (Constructors, operators)
        }

        public class Customer
        {
            public decimal? AccountBalance; // Can be null if the database column allows null
        }

        public class Grid
        {
            public ConsoleColor Color;
        }

        public class Row
        {
            Grid parent;    // Assuming parent provides a default color
            ConsoleColor? color;   // Backing field for the ambient property

            public Row(Grid parent)
            {
                this.parent = parent;
            }

            // Ambient property: 
            // Uses parent value when local value is null
            public ConsoleColor Color
            {
                get { return color ?? parent.Color; }
                // If 'color' is null, use parent's color

                set { color = (value == parent.Color) ? (ConsoleColor?) null : value; }
                // Store null if same as parent's, otherwise store value
            }
        }
    }
}