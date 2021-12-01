using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Day12
{
    class Program
    {
        private static int GetTotal(JsonElement element, bool ignoreWithRed)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Undefined:
                case JsonValueKind.Null:
                case JsonValueKind.String:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return 0;
                case JsonValueKind.Number:
                    return element.GetInt32();
                case JsonValueKind.Array:
                    return element.EnumerateArray().Sum(e => GetTotal(e, ignoreWithRed));
                case JsonValueKind.Object:
                    if (ignoreWithRed && element.EnumerateObject()
                        .Any(e => e.Value.ValueKind == JsonValueKind.String && e.Value.GetString() == "red"))
                    {
                        return 0;
                    }
                    return element.EnumerateObject().Sum(e => GetTotal(e.Value, ignoreWithRed));
                default:
                    throw new ArgumentOutOfRangeException(nameof(element));
            }
        }

        private static int Part1(JsonDocument document)
        {
            return GetTotal(document.RootElement, false);
        }

        private static int Part2(JsonDocument document)
        {
            return GetTotal(document.RootElement, true);
        }

        public static void Main()
        {
            using var document = JsonDocument.Parse(File.ReadAllText("input.txt"));
            Console.WriteLine(Part1(document));
            Console.WriteLine(Part2(document));
        }
    }
}
