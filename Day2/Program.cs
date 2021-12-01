using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Program
    {
        private static readonly Regex RowRegex = new("(\\d+)x(\\d+)x(\\d+)", RegexOptions.Compiled);

        private static (int, int, int) GetDimensions(string dimensions)
        {
            var matches = RowRegex.Match(dimensions);
            return (
                int.Parse(matches.Groups[1].Value),
                int.Parse(matches.Groups[2].Value),
                int.Parse(matches.Groups[3].Value)
            );
        }

        private static int Part1(IEnumerable<(int, int, int)> dimensions) =>
            dimensions.Sum(dim =>
            {
                var (l1, l2, l3) = dim;
                var a1 = l1 * l2;
                var a2 = l2 * l3;
                var a3 = l3 * l1;
                return 2 * (a1 + a2 + a3) + Math.Min(a1, Math.Min(a2, a3));
            });

        private static int Part2(IEnumerable<(int, int, int)> dimensions) =>
            dimensions.Sum(dim =>
            {
                var (l1, l2, l3) = dim;
                return 2 * (l1 + l2 + l3 - Math.Max(l1, Math.Max(l2, l3))) + l1 * l2 * l3;
            });

        public static void Main()
        {
            var dimensions = File.ReadLines("input.txt").Select(GetDimensions).ToList();
            Console.WriteLine(Part1(dimensions));
            Console.WriteLine(Part2(dimensions));
        }
    }
}
