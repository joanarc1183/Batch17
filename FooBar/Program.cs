// See https://aka.ms/new-console-template for more information
using System;

namespace NamaProyek
{
 class Program
   {
       static void Main(string[] args)
       {
            int n = 105;
            for (int i=1; i<=n; i++)
            {
                string result = "";

                if (i%3 == 0)
                {
                    result += "foo";
                } 
                if (i%5 == 0)
                {
                    result += "bar";
                }
                if (i % 7 == 0)
                {
                    result += "jazz";
                } 

                if (result=="")
                {
                    System.Console.WriteLine(i);
                }
                else
                {
                    System.Console.WriteLine(result);
                }
            }
            
       }
   }
}
