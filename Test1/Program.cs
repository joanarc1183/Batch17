// See https://aka.ms/new-console-template for more information
using System;

namespace NamaProyek
{
 class Program
   {
       static void Main(string[] args)
       {
            int n = 15;
            for (int i=1; i<=n; i++)
            {
                if (i % 3 == 0)
                {
                    Console.WriteLine("foo");
                } else if (i%5 == 0)
                {
                    Console.WriteLine("bar");
                }
            }
            
       }
   }
}
