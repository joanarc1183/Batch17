using System;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Delegate 
{
    delegate int Transformer(int x);    // Delegate type declaration
    
    class Program {
        static void Main(string[] args)
        {
            // 1. Defining and Using a Delegate

            int Square(int x) { return x*x; }
            // Or more concisely with an expression-bodied method:
            // int Square(int x) => x * x;

            int Cube(int x) => x*x*x;
            
            Transformer t = Square;         // Create delegate instance

            int answer = t(3);              // Invoke delegate (calls Square(3))
            
            Console.WriteLine(answer);


            // 2. Writing Plug-in Methods with Delegates

            void Transform(int[] values, Transformer t)     // 't' is a delegate parameter
            {
                for (int i=0; i<values.Length; i++)
                {
                    values[i] = t(values[i]);       // Invoke the plug-in method
                }
            }

            int[] values = { 1, 2, 3 };

            Transform(values, Square);      // Use Square method as the plug-in

            foreach (int i in values)
            {
                System.Console.WriteLine(i+" ");
            }
            // Change the plug-in behavior easily:
            // Transform(values, Cube); // Would use Cube method instead
        
        
            // 3. Instance and Static Method Targets
            Transformer t1 = Test.Square;        // Referencing a static method
            System.Console.WriteLine(t1(10));


            // 4. Multicast Delegates

            // Example 1;
            void SomeMethod1() { System.Console.WriteLine("Method 1"); }
            void SomeMethod2() { System.Console.WriteLine("Method 2"); }

            SomeDelegate d = SomeMethod1;       // d points to SomeMethod1
            d += SomeMethod2;                   // d now points to both Method1 and Method2
            // Invoking 'd' will call Method1, then Method2
            d();        // call delegate

            // Example 2:
            void WriteProgressToConsole(int percentComplete) => System.Console.WriteLine($"Console: {percentComplete}%");
            void WriteProgressToFile(int percentComplete) => File.WriteAllText("progress.txt", percentComplete.ToString());

            ProgressReporter p = WriteProgressToConsole;        // Start with console reporting
            p += WriteProgressToFile;                           // Add file reporting

            HardWork(p);        // Both methods will be called as progress is made.


            // 5. Generic Delegate Types
            Transform(values, Square);
            foreach (int i in values)
            {
                System.Console.WriteLine(i+" ");
            }


            // 7. Delegates Versus Interfaces
            // Many problems solvable with delegates can also be solved with interfaces.
            TransformAll(values, new Squarer());
            foreach (int i in values)
            {
                System.Console.WriteLine(i+" ");
            }
            // When to prefer a delegate:
                // 1. Single-method logic
                    // Use a delegate if only one method is needed.
                // 2. Multicast support
                    // Delegates support multicasting; interfaces do not.
                // 3. Multiple behaviors in one class
                    // A class can expose multiple methods (e.g., Square, Cube)
                    // using delegates, but not with interfaces.


            // 8. Delegate Compatibility (Variance)
            
            void Method1() 
            {
                System.Console.WriteLine("Hello");
            }

            D1 d1 = Method1;
            /// ❌ Compile-time error:
            // Even though D1 and D2 have identical signatures,
            // they are different delegate types.
            // D2 d2 = d1;
            D2 d2 = new D2(d1);     // ✅ OK: Can construct a new D2 from an existing D1 delegate instance
            d2();
            // Delegate instances are considered equal if:
                // - They reference the same target method(s)
                // - In the same invocation order (for multicast delegates)


            // 8.1 Parameter Compatibility (Contravariance)
            
            void ActOnObject(object o)
            {
                System.Console.WriteLine(o);
            }

            // ✅ Legal due to contravariance:
            // string can be safely passed as object
            StringAction action = new StringAction(ActOnObject);

            // "hello" is passed as string,
            // then implicitly upcast to object
            action("hello");


            // 8.2 Return Type Compatibility (Covariance)

            // Method returns a more specific type
            string RetrieveString()
            {
                return "hello!";
            }

            // ✅ Legal due to covariance:
            // string can be returned as object
            ObjectRetriever retriever = new ObjectRetriever(RetrieveString);

            // The returned string is implicitly upcast to object
            object result = retriever();
            System.Console.WriteLine(result);


            // 8.3 Generic Delegate Type Parameter Variance 
            
            // ----- Contravaiance (in) -----
            // Method accepts a ore general type (object)
            void ConsumeObject(object o)
            {
                System.Console.WriteLine(o);
            }
            // Consumer<string> can point to COnsumeObject
            // because string can be passed as object
            Consumer<string> stringConsumer = ConsumeObject;
            stringConsumer("Hello Variance");
            
            // ----- Convaiance (out) -----
            // Method returns a more specific type (string)
            string ProduceString()
            {
                return "Hello Covariance";
            }
            // Producer<object> can reference ProduceString
            // because string can be returned as object
            Producer<object> objectProducer = ProduceString;
            object res = objectProducer;
            System.Console.WriteLine(res);
            
            // Notes:
                // - Use 'in'  when the generic type is only used as a parameter
                // - Use 'out' when the generic type is only used as a return value
                // - This enables safe and flexible delegate assignments
                // - Func and Action are designed using this variance pattern
        }


        // 4. Multicast Delegatess
        delegate void SomeDelegate();

        public delegate void ProgressReporter(int percentComplete);

        public static void HardWork(ProgressReporter p)
        {
            for (int i=0; i<10; i++)
            {
                p(i*10);        // Invoke the delegate to report progress
                System.Threading.Thread.Sleep(100);     // Simulate work
            }
        }


        // 5. Generic Delegate Types
        public delegate TResult Transformer<TArg, TResult> (TArg arg);
        // Example: Transformer<int, int> can refer to Square
        // Transformer<string, int> could refer to a method that counts characters in a string

        public static void Transform<T>(T[] values, Transformer<T, T> t)
        {
            for(int i=0; i<values.Length; i++)
            {
                values[i] = t(values[i]);
            }
        }

        // 6. The Func and Action Delegates
        // C# provides built-in generic delegates in the System namespace:
        // Func   → represents a method that RETURNS a value
        // Action → represents a method that returns void

        // Func<TResult>           → no parameters, returns TResult
        // Func<TArg, TResult>     → 1 parameter, returns TResult
        // Func<T1, T2, TResult>   → 2 parameters, returns TResult
        // (up to 16 input parameters)

        // Action                  → no parameters, returns void
        // Action<TArg>            → 1 parameter, returns void
        // Action<T1, T2>          → 2 parameters, returns void
        // (up to 16 input parameters)
        // Our Transformer<T, T> delegate above can be replaced directly with Func<T, T>
        public static void Transform<T>(T[] values, Func<T, T> transformer)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = transformer(values[i]);
        }

        // 7. Delegates Versus Interfaces
        public static void TransformAll(int[] values, ITransformer t)
        {
            for (int i=0; i< values.Length; i++)
            {
                values[i] = t.Transform(values[i]);
            }
        }
    }

    // 3. Instance and Static Method Targets
    class Test { public static int Square(int x) => x*x; }


    // 7. Delegates Versus Interfaces
    public interface ITransformer
    {
        int Transform(int x);
    }

    class Squarer : ITransformer
    {
        public int Transform(int x) => x*x;
    }

    // 8. Delegate Compatibility (Variance)
    delegate void D1();
    delegate void D2();

    // 8.1 Parameter Compatibility (Contravariance)
    delegate void StringAction(string s);

    // 8.2 Return Type Compatibility (Covariance)
    delegate object ObjectRetriever();
    
    // 8.3 Generic Delegate Type Parameter Variance 
    // 'in'  → contravariant (used only for input parameters)
    // 'out' → covariant (used only for return values)

    // Contravariant generik delegate
    // T is used only as a parameter
    delegate void Consumer<in T>(T value);

    // Covariant generic delegate
    // T is used only as a return type
    delegate T Producer<out T>();

}