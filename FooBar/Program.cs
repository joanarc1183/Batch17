using System;

namespace NamaProyek
{
    class Program
    {
       static void Main(string[] args)
       {
            NumberToWordGenerator generator = new NumberToWordGenerator();

            generator.AddRule(3, "foo");
            generator.AddRule(4, "baz");
            generator.AddRule(5, "bar");
            generator.AddRule(7, "jazz");
            generator.AddRule(9, "huzz");

            int n = 105;
            for (int i=1; i<=n; i++)
            {
                System.Console.Write(generator.Generate(i));
                if (i < n)
                {
                    System.Console.Write(", ");
                }
            }
        }
    }

    class NumberToWordGenerator{
        private readonly Dictionary<int, string> _rules;

        public NumberToWordGenerator()
        {
            _rules = new();
        }
        public void AddRule(int input, string output)
        {
            if (input > 0)
            {
                _rules[input] = output;
            }
        }

        public string Generate(int number)
        {
            string result = "";
            foreach(var rule in _rules)
            {
                if (number%rule.Key == 0)
                {
                    result += rule.Value;
                }
            }

            if (result.Length == 0)
            {
                result = number.ToString();
            }
            return result;
        }
   }
}
