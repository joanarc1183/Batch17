using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace TryCatch
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Exception Handling (Basics)
            // - An exception is a runtime error condition (e.g. divide by zero, file not found).
            // - Exception handling prevents the program from crashing.
            // - Main keywords:
                // - try : code that may throw an exception
                // - catch : handles specific exceptions
                // - finally : always executes (cleanup logic)
            

            // 2. Handling Division by Zero

            int Calc(int x) => 10 / x;

            // ...
            // int y = Calc(0); // This would cause a DivideByZeroException and crash the program
            // Console.WriteLine(y);

            try
            {
                int y = Calc(0);
                System.Console.WriteLine(y);
            }
            catch (DivideByZeroException ex)
            {
                System.Console.WriteLine("Error: x cannot be zero.");
            }
            System.Console.WriteLine("Program completed.");


            // 3. How Exception Handling Works
            // - When an exception occurs inside a try block:
            // 1) The CLR searches for a matching catch block (top to bottom).
            // 2) If found → execute catch → execute finally → resume program flow.
            // 3) If not found → search up the call stack.
            // 4) If no handler exists → application terminates.


            // 4. The catch Clauses

            // - All exceptions derive from System.Exception.
            // - Always catch MORE SPECIFIC exceptions first, then general ones.
            // Correct order example:
            try
            {
                byte b = byte.Parse(args[0]);   // args[0] might not exist, or not be a number, or be too large
                System.Console.WriteLine(b);
            }
            catch (IndexOutOfRangeException)    // You can catch without a variable
            {
                System.Console.WriteLine("Please provide at least one argument.");
            }
            catch (FormatException)
            {
                System.Console.WriteLine("That is not a number!");
            }
            catch (OverflowException)
            {
                System.Console.WriteLine("You are given me more than a byte!");
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"An unexpected error occured: {ex.Message}");
            }
            // - You can catch all exceptions:
            // catch { /* ... */ }


            // 5. Exception Filters (when)
            // - The `when` keyword allows conditional exception handling.
            // - If the condition is false, the catch block is skipped.
            try
            {
                // Simulation of web operations that can timeout
                throw new WebException("Request timeout", null, WebExceptionStatus.Timeout, null);
            }
            catch(WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                System.Console.WriteLine("Web request time out.");
            }
            catch(WebException ex)
            {
                System.Console.WriteLine($"Another WebException occured: {ex.Status}");
            }
        

            // 6. finally Block
            // - The finally block ALWAYS executes, even if:
            // - an exception is thrown
            // - a return statement is executed
            // - no matching catch exists
            // - Typical usage:
                // - closing files
                // - disposing resources
                // - releasing unmanaged memory

            void ReadFile()
            {
                StreamReader reader = null;     // Decalre outside try for finnaly block access
                try
                {
                    reader = File.OpenText("file.txt");
                    if(reader.EndOfStream) return;      // Returns early, but finally still executes
                    System.Console.WriteLine(reader.ReadToEnd());
                }
                finally
                {
                    if(reader != null)
                    {
                        reader.Dispose();       // Ensures the file stream is closed
                    }
                }
            }


            // 7. using Statement & IDisposable
            // - Classes that manage unmanaged resources usually implement IDisposable.
            // - The using statement guarantees Dispose() is called.
            using (StreamReader reader = File.OpenText("file.txt"))    // Reader is automatically disposed
            {
                // use reader here
                System.Console.WriteLine(reader.ReadToEnd());
            }   // At the end of this block, reader.Dispose() is implicitly called
        

            // 8. using Declaration (C# 8+)
            // - Disposes the resource automatically when leaving the scope.
            if (File.Exists("file.txt"))
            {
                using var reader = File.OpenText("file.txt");   // reader will be disposed when leaving the 'if' block
                System.Console.WriteLine(reader.ReadLine());
                // ... more code using reader
            }   // reader.Dispose() is called here
        

            // 9. Throwing Exceptions
            // - Exceptions can be thrown manually using `throw`.
            // - .NET 6+ provides helpers like ArgumentNullException.ThrowIfNull().
            void Display(string name)
            {
                // This
                if(name == null)
                {
                    throw new ArgumentNullException(nameof(name), "Name cannot be null.");
                }
                // Or this 
                // ArgumentNullException.ThrowIfNull(name);    // Throws ArgmentNullException if name is null

                // Throw an exception
                System.Console.WriteLine(name);
            }

            try 
            { 
                Display(null); 
            }
            catch (ArgumentNullException ex)
            {
                System.Console.WriteLine($"Caught the exception: {ex.Message}");
            }


            // 10. throw as an Expression (C# 7+)
            // - `throw` can be used inside expressions and expression-bodied members.
            string Foo() => throw new NotImplementedException();

            string ProperCase(string value) => 
                value == null ? throw new ArgumentException("Value cannot be null.") :  // Throw as an expression
                value == "" ? "" :
                char.ToUpper(value[0]) + value.Substring(1);


            // 11. Rethrowing Exceptions
            // - Use `throw;` to preserve the original stack trace.
            // - DO NOT use `throw ex;` because it resets the stack trace.
            try
            {
                // some risky operation
            }
            catch (Exception ex)
            {
                // Log the error: ex.Message, ex.StackTrace, etc.
                System.Console.WriteLine($"Logged error: {ex.Message}");
                throw;      // Rethrows the original exception, preserving its stack trace
            }


            // 12. Wrapping Exceptions
            // - You can wrap an exception to add context.
            // - The original exception is stored in InnerException.
            try
            {
                // Parse a DateTime from XML element data
            }
            catch (FormatException ex)
            {
                throw new XmlException("Invalid DateTime format in XML.", ex);
            }


            // 13. Important System.Exception Properties
            // - Message : error description
            // - StackTrace : call history leading to the error
            // - InnerException: original exception when wrapped


            // 14. Common Exception Types
                // ArgumentException        → Invalid argument
                // ArgumentNullException    → Argument is null
                // ArgumentOutOfRangeException → Value out of range
                // InvalidOperationException → Object state is invalid
                // NotSupportedException   → Operation not supported
                // NotImplementedException → Not implemented yet
                // ObjectDisposedException → Object already disposed
                // NullReferenceException  → Accessing null object (bug)


            // 15. TryXXX Method Pattern
            // - Used when failure is EXPECTED and part of normal flow.
            // - Avoids try-catch for common validation logic.
            // Example:
                // - int.Parse() → throws exception on failure
                // - int.TryParse() → returns true/false

            // Throws an exception if parsing fails
            int number1 = int.Parse("123");
            // int number2 = int.Parse("abc");  // Thows FormatException

            // Returns true/false and provides result via 'out' parameter
            if (int.TryParse("123", out int result1)){
                System.Console.WriteLine($"Parsed: {result1}");
            }
            if (!int.TryParse("abc", out int result2)){
                System.Console.WriteLine("Failed to parse 'abc'."); 
            }


            // 16. Best Practices
            // - Use exceptions for unexpected situations.
            // - Validate input with conditions when possible.
            // - Do not use exceptions for normal control flow.
            // - Always order catch blocks from specific to general.


        }
    }
}