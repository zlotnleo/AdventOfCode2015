using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day19
{
    class Program
    {
        private static IEnumerable<(string, string)> ParseReplacements(IEnumerable<string> replacements)
        {
            var replacementRegex = new Regex("(\\w+) => (\\w+)", RegexOptions.Compiled);
            return replacements.Select(replacement => replacementRegex.Match(replacement))
                .Where(match => match.Success)
                .Select(match => (match.Groups[1].Value, match.Groups[2].Value));
        }

        private static int Part1(string molecule, IEnumerable<(string, string)> replacements)
        {
            var newMolecules = new HashSet<string>();
            foreach (var (from, to) in replacements)
            {
                for (var index = molecule.IndexOf(from, StringComparison.Ordinal);
                    index != -1;
                    index = molecule.IndexOf(from, index + 1, StringComparison.Ordinal))
                {
                    newMolecules.Add(molecule[..index] + to + molecule[(index + @from.Length)..]);
                }
            }

            return newMolecules.Count;
        }

        private static int CountSubstring(string s, string sub)
        {
            var count = 0;
            for (var index = s.IndexOf(sub, StringComparison.Ordinal);
                index != -1;
                index = s.IndexOf(sub, index + 1, StringComparison.Ordinal))
            {
                count++;
            }

            return count;
        }

        private static int Part2(string molecule) =>
            molecule.Count(c => c is >= 'A' and <= 'Z')
            - CountSubstring(molecule, "Rn")
            - CountSubstring(molecule, "Ar")
            - 2 * CountSubstring(molecule, "Y")
            - 1;

        public static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            var replacements = ParseReplacements(lines.Take(lines.Length - 2)).ToList();
            var medicineMolecule = lines[^1];
            Console.WriteLine(Part1(medicineMolecule, replacements));
            Console.WriteLine(Part2(medicineMolecule));
        }
    }
}
