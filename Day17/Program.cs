using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
{
    class Program
    {
        private static IEnumerable<int> CountsOfContainers(ICollection<int> containers, int total)
        {
            switch (total)
            {
                case < 0:
                    return Array.Empty<int>();
                case 0:
                    return new[] {0};
            }

            if (!containers.Any())
            {
                return Array.Empty<int>();
            }

            var thisContainer = containers.First();
            var otherContainers = containers.Skip(1).ToList();
            return CountsOfContainers(otherContainers, total - thisContainer)
                .Select(count => count + 1)
                .Concat(CountsOfContainers(otherContainers, total));
        }

        private static int Part1(ICollection<int> countsOfContainers) =>
            countsOfContainers.Count;

        private static int Part2(ICollection<int> countsOfContainers)
        {
            var min = countsOfContainers.Min();
            return countsOfContainers.Count(c => c == min);
        }

        public static void Main()
        {
            var containers = File.ReadAllLines("input.txt").Select(int.Parse).ToList();
            var countsOfContainers = CountsOfContainers(containers, 150).ToArray();
            Console.WriteLine(Part1(countsOfContainers));
            Console.WriteLine(Part2(countsOfContainers));
        }
    }
}
