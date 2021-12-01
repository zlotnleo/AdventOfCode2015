using System;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        private static bool Part1IsNice(string s)
        {
            var bannedStrings = new[] {"ab", "cd", "pq", "xy"};
            if (bannedStrings.Any(bannedString => s.IndexOf(bannedString, StringComparison.Ordinal) != -1))
            {
                return false;
            }

            var vowels = new[] {'a', 'e', 'i', 'o', 'u'};
            var vowelCount = s.Count(vowels.Contains);
            if (vowelCount < 3)
            {
                return false;
            }

            for (var i = 0; i < s.Length - 1; i++)
            {
                if (s[i] == s[i + 1])
                {
                    return true;
                }
            }

            return false;
        }

        private static bool Part2IsNice(string s)
        {
            var foundRepeatingPair = false;
            for (var i = 0; i < s.Length - 1 && !foundRepeatingPair; i++)
            {
                foundRepeatingPair = s.IndexOf($"{s[i]}{s[i + 1]}", i + 2, StringComparison.Ordinal) != -1;
            }

            if (!foundRepeatingPair)
            {
                return false;
            }

            for (var i = 0; i < s.Length - 2; i++)
            {
                if (s[i] == s[i + 2])
                {
                    return true;
                }
            }

            return false;
        }

        public static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(lines.Count(Part1IsNice));
            Console.WriteLine(lines.Count(Part2IsNice));
        }
    }
}
