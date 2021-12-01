using System;
using System.Text;

namespace Day10
{
    class Program
    {
        private static string Iterate(string s)
        {
            var outputBuilder = new StringBuilder();
            var lastDigit = s[0];
            var count = 1;
            for (var i = 1; i < s.Length; i++)
            {
                if (s[i] != lastDigit)
                {
                    outputBuilder.Append(count);
                    outputBuilder.Append(lastDigit);
                    lastDigit = s[i];
                    count = 1;
                }
                else
                {
                    count++;
                }
            }

            outputBuilder.Append(count);
            outputBuilder.Append(lastDigit);

            return outputBuilder.ToString();
        }

        private static int IterateRepeated(string s, int times)
        {
            for (var i = 0; i < times; i++)
            {
                s = Iterate(s);
            }

            return s.Length;
        }

        public static void Main()
        {
            const string input = "1113222113";
            Console.WriteLine(IterateRepeated(input, 40));
            Console.WriteLine(IterateRepeated(input, 50));
        }
    }
}
