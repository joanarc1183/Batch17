using System;
using System.Runtime.InteropServices.Marshalling;

namespace OperatorOverloading
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Basic rules of operarot overloading
                // - Must be declard as public and static
                // - Method name uses the 'operator' keyword followed by the symbol
                // - At least one operand must be of the containing type
                // - You cannot overload operators for built-in types only (e.g., int + int)


            // 2. Example: Overloading Operator +
            Note B = new Note(2);       // B is 2 semitones from A
            Note CSharp = B+2;          // CSharp is now a Note representing (2 + 2) = 4 semitones from A
            CSharp += 2;        // Equivalent to CSharp = CSharp + 2
            System.Console.WriteLine(CSharp);
            
            Note other = checked(B + int.MaxValue);     // This will call the 'checked +' operator
            // and likely throw an OverflowException
            System.Console.WriteLine(other);

            // 3. Overloadable Operators
                // Unary   : +, -, !, ~, ++, --
                // Binary  : +, -, *, /, %, &, |, ^, <<, >>,
                //           ==, !=, <, >, <=, >=
                //
                // Automatically supported:
                // - Compound assignments (+=, -=, etc.) if base operators are overloaded
                // - && and || if &, | and true/false are overloaded


            // 4. Equality and comparison operators
                // == must be paired with !=
                // < must be paired with >
                // <= must be paired with >=
                // If == / != are overloaded → override Equals() & GetHashCode()
                // If < > <= >= are overloaded → implement IComparable<T>


            // 5. Implicit and Explicit conversions
            
            NoteWithConversion n = (NoteWithConversion)544.37;  // Explicit conversion from double to Note
            double freq = n;      // Implicit conversion from double to Note
            System.Console.WriteLine(freq);
            // Note:
            // - Custom conversions are NOT used by `is` or `as`


            // 6. true and false operators (Rare case)
            SqlBoolean a = SqlBoolean.Null;
            if (a)              // Uses the overloaded operator true
            {
                System.Console.WriteLine("True");
            } 
            else if (!a)        // Uses the overloaded operator false and operator !
            {
                System.Console.WriteLine("False");
            }
            else
            {
                System.Console.WriteLine("Null");   // Output: Null
            }

            // The Key is...
            // - Operator overloading improves readability and expressiveness
            // - Use it sparingly and only when it makes semantic sense
            // - Always follow pairing rules and ensure consistent equality/comparison behavior
        }
        
        
        // 2. Example: Overloading Operator +
        public struct Note
        {
            private int value;      // Semitones from a base 'A' note
            // Semitones from A (e.g., A=0, A#=1, B=2...)

            public Note(int semitonesFromA) { value = semitonesFromA; }

            // Overloading the '+' operator
            // This allows us to add an integer (semitones) to a Note
            public static Note operator +(Note x, int semitones)
            {
                return new Note(x.value + semitones);       // Unchecked version
            }

            // Checked version (C# 11+)
            public static Note operator checked +(Note x, int semitones)
            {
                return checked(new Note(x.value + semitones));      
            }

            public override string ToString() => $"Note({value})";
        }

        // 5. Implicit and Explicit conversions
        public struct NoteWithConversion
        {
            private int value;

            public NoteWithConversion(int semitonesFromA)
            {
                value = semitonesFromA;
            }

            // Implicit conversion from Note to double (frequency in Hertz)
            // No information loss, always succeeds.
            public static implicit operator double(NoteWithConversion x)
                => 440 * Math.Pow(2, (double)x.value / 12);

            // Explicit conversion from double (frequency) to Note
            // Information might be lost (rounding to nearest semitone), so explicit is required.
            public static explicit operator NoteWithConversion(double x)
                => new NoteWithConversion((int)(0.5 + 12 * (Math.Log(x/440) / Math.Log(2))));
        }


        // 6. true and false operators (Rare case)
        // Reimplements parts of SqlBoolean for demonstration
        public struct SqlBoolean
        {
            public static readonly SqlBoolean Null = new SqlBoolean(0);
            public static readonly SqlBoolean False = new SqlBoolean(1);
            public static readonly SqlBoolean True = new SqlBoolean(2);
            private byte m_value;

            private SqlBoolean(byte value)
            {
                m_value = value;
            }

            public static bool operator true(SqlBoolean x)
                => x.m_value == True.m_value;       // Returns true if the internal value matches 'True'
            
            public static bool operator false(SqlBoolean x)
                => x.m_value == False.m_value;      // Returns true if the internal value matches 'False'

            public static SqlBoolean operator !(SqlBoolean x)
            {
                if (x.m_value == Null.m_value) return Null;
                if(x.m_value == False.m_value) return True;
                return False;
            }
        }
    
    }
}