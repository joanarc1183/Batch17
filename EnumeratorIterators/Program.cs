using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace enumiterator
{
    // Enumeration : the process of stepping through a sequence of elements
    // Iterator : a language feature that makes creating such sequences remarkably easy

    // Enumerator (the cursor)
    // - Responsible for iteration logic
    // - Knows the current position in the sequence
    // - Moves forward only (no backward iteration)
    class MyEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _items;
        private int _index = -1;

        public MyEnumerator(T[] items)
        {
            _items = items;
        }

        // Generic current
        // public T Current { get { /* ... returns current element ... */ }}
        public T Current => _items[_index];

        // Non-generic current 
        // - Required by IEnumerator
        // - Explicit interface implementation
        object IEnumerator.Current => Current;

        // Move Next 
        // - Advances the cursor 
        // - Return false when the end of the sequence is reached
        public bool MoveNext() 
        { 
            /* ... moves to next, returns true/false ... */ 
            _index++;
            return _index < _items.Length;
        }

        // Other IEnumerator members like Reset(), Dispose()

        // Reset
        // - Resets the enumerator to the initial position
        // - Often not supported in modern implementations
        public void Reset()
        {
            throw new NotSupportedException();
        }

        // Dispose
        // - Used to clean up unmanaged resources
        // - Often empty if no resources are used
        public void Dispose()
        {
            // No resources to clean up
        }
    }

    // Enumerable (the sequence)
    // - Represents a collection or sequence
    // - Does not store iteration state
    // - Produces a new enumerator each time GetEnumerator() is called
    class MyEnumerable<T> : IEnumerable<T>
    {
        private readonly T[] _items;

        public MyEnumerable(T[] items)
        {
            _items = items;
        }

        // Required return type
        public IEnumerator<T> GetEnumerator()
        {
            /* ... returns new MyEnumerator<T>() ... */
            return new MyEnumerator<T>(_items);
        }

        // Non-generic GetEnumerator() for IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1. Enumeration (foreach)
            // Enumeration  : iterating over a sequence

            // High-level: using foreach
            // foreach      : high-level syntax + automatic Dispose
            foreach (char c in "beer")  // "beer" is an enumerable object (string implements IEnumerable<char>)
            {
                System.Console.WriteLine(c);
            }

            // Low-level: manual enumeration
            using (var enumerator = "beer".GetEnumerator())     // Get an enumerator from the enumerable object
            {
                while (enumerator.MoveNext())               // Move to the next element
                {
                    var element = enumerator.Current;       // Get the current element
                    System.Console.WriteLine(element);
                }
            }


            // 2. Collection Initializer
            // - Creates and populates a collection in one expression
            // - Requires IEnumerable + accessible Add() method

            var list = new List<int> { 1, 2, 3 };       // Creates a List<int> and adds 1, 2, 3
            
            // The compiler translates to...
            List<int> list1 = new List<int>();
            list1.Add(1);
            list1.Add(2);
            list1.Add(3);

            // Dictionary initializer
            var dict = new Dictionary<int, string>()
            {
                {5, "five"},        // Key-value pair syntax
                {10, "ten"}
            };

            // Indexer-based syntax
            var dict1 = new Dictionary<int, string>()
            {
                [3] = "three",
                [10] = "ten"
            };


            // 3. Collection Expression (C# 12+)
            // - Uses [] instead of {}
            // - Target-typed: type inferred from context

            List<int> list2 = [1,2,3];   // More concise way to create and populate a List<int>

            int[] array = [1,2,3];      // Target type is int[]
            Span<int> span = [1,2,3];   // Targer type is Span<int>

            Foo([1,2,3]);     // Compuler infers List<int> based on Foo's parameter type


            // 4. Iterator (yield)
            // Iterator     : sequence producer using yield
            // - Produce sequences lazily
            // - No need to manually implement IEnumerator/IEnumerable
            foreach (int fib in Fibs(6))    // Consumes the sequence produced by Fibs
            {
                System.Console.WriteLine(fib+ " ");
            }


            // 5. Composing iterators
            // - Iterators can be chained (LINQ foundation)
            // - Each element flows through the pipeline lazily
            foreach (int fib in EvenNumbersOnly(Fibs(10)))      // Chains Fibs with EvenNumbersOnly
            {
                System.Console.WriteLine(fib+" ");
            }

        }
        static void Foo(List<int> numbers)
        {
            // Used to demonstrate target-typed collection expressions
        }

        // Iterator: Fibonacci
            // - Generates sequences lazily
            // Lazy Eval    : elements created on demand
            // - Compiler creates IEnumerator & IEnumerable automatically
            // Enumerator   : cursor (MoveNext + Current)
            // Enumerable   : creates enumerators (GetEnumerator) 
        static IEnumerable<int> Fibs(int fibCount)     // Returns IEnumerable<int>
        {
            for (int i = 0, prevFib = 1, curFib = 1; i < fibCount; i++)
            {
                yield return prevFib;           // Yields the current Fibonacci number
                int newFib = prevFib+curFib;
                prevFib = curFib;
                curFib = newFib;
            }
        }

        // Iterator Composition
        // New iterator that filters for even numbers
            // - Filter other sequences
        static IEnumerable<int> EvenNumbersOnly(IEnumerable<int> sequence)
        {
            foreach (int x in sequence)     // Iterates over the input sequence
            {
                if ((x%2)==0)
                {
                    yield return x;         // Yields only even numbers
                }
            }
        }
    }

}