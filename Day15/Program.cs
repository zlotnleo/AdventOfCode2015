using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day15
{
    class Program
    {
        private struct Ingredient
        {
            public int capacity;
            public int durability;
            public int flavor;
            public int texture;
            public int calories;
        }

        private const int MaxIngredients = 100;
        private const int TotalCalories = 500;

        private static List<Ingredient> ParseIngredients(IEnumerable<string> lines)
        {
            var ingredientRegex = new Regex("\\w+: capacity (-?\\d+), durability (-?\\d+), flavor (-?\\d+), texture (-?\\d+), calories (-?\\d+)", RegexOptions.Compiled);
            return lines.Select(line => ingredientRegex.Match(line))
                .Where(match => match.Success)
                .Select(match => new Ingredient
                {
                    capacity = int.Parse(match.Groups[1].Value),
                    durability = int.Parse(match.Groups[2].Value),
                    flavor = int.Parse(match.Groups[3].Value),
                    texture = int.Parse(match.Groups[4].Value),
                    calories = int.Parse(match.Groups[5].Value)
                })
                .ToList();
        }

        private static int ScoreCookie(List<Ingredient> ingredients, int[] counts)
        {
            var capacity = ingredients.Select((ingredient, i) => ingredient.capacity * counts[i]).Sum();
            var durability = ingredients.Select((ingredient, i) => ingredient.durability * counts[i]).Sum();
            var flavor = ingredients.Select((ingredient, i) => ingredient.flavor * counts[i]).Sum();
            var texture = ingredients.Select((ingredient, i) => ingredient.texture * counts[i]).Sum();

            return Math.Max(capacity, 0)
                   * Math.Max(durability, 0)
                   * Math.Max(flavor, 0)
                   * Math.Max(texture, 0);
        }

        private static int GetCalories(List<Ingredient> ingredients, int[] counts) =>
            ingredients.Select((ingredient, i) => ingredient.calories * counts[i]).Sum();

        private static IEnumerable<IEnumerable<int>> GetCountsAddingUpToTotal(int n, int total)
        {
            if (n == 1)
            {
                return new[] {new[] {total}};
            }

            return Enumerable.Range(0, total + 1)
                .SelectMany(i => GetCountsAddingUpToTotal(n - 1, total - i).Select(l => l.Prepend(i)));
        }

        private static int Part1(List<Ingredient> ingredients, IEnumerable<int[]> validCounts) =>
            validCounts.Select(count => ScoreCookie(ingredients, count.ToArray())).Max();

        private static int Part2(List<Ingredient> ingredients, IEnumerable<int[]> validCounts) =>
            validCounts
                .Where(count => GetCalories(ingredients, count.ToArray()) == 500)
                .Select(count => ScoreCookie(ingredients, count.ToArray()))
                .Max();

        public static void Main()
        {
            var ingredients = ParseIngredients(File.ReadAllLines("input.txt"));
            var validCounts = GetCountsAddingUpToTotal(ingredients.Count, MaxIngredients)
                .Select(count => count.ToArray())
                .ToArray();
            Console.WriteLine(Part1(ingredients, validCounts));
            Console.WriteLine(Part2(ingredients, validCounts));
        }
    }
}
