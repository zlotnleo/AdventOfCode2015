using System;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        private static int Part1(string brackets) =>
            brackets.Sum(bracket => bracket switch
            {
                '(' => 1,
                ')' => -1,
                _ => 0
            });

        private static int Part2(string brackets)
        {
            var floor = 0;
            for (var i = 0; i < brackets.Length; i++)
            {
                floor += brackets[i] switch
                {
                    '(' => 1,
                    ')' => -1,
                    _ => 0
                };
                if (floor == -1)
                {
                    return i + 1;
                }
            }

            return -1;
        }

        public static void Main()
        {
            var brackets = File.ReadAllText("input.txt");
            Console.WriteLine(Part1(brackets));
            Console.WriteLine(Part2(brackets));
        }
    }
}
