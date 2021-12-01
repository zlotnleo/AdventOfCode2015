using System;
using System.Collections.Generic;
using System.Linq;

namespace Day21
{
    class Program
    {
        private class Item
        {
            public int cost;
            public int damage;
            public int armour;

            public Item(int cost, int damage, int armour)
            {
                this.cost = cost;
                this.damage = damage;
                this.armour = armour;
            }
        }

        private class Participant
        {
            public string name;
            public int hp;
            public int damage;
            public int armour;
        }

        private static readonly IReadOnlyList<Item> Weapons = new[]
        {
            new Item(8, 4, 0),
            new Item(10, 5, 0),
            new Item(25, 6, 0),
            new Item(40, 7, 0),
            new Item(74, 8, 0)
        };

        private static readonly IReadOnlyList<Item> Armour = new[]
        {
            new Item(13, 0, 1),
            new Item(31, 0, 2),
            new Item(53, 0, 3),
            new Item(75, 0, 4),
            new Item(102, 0, 5)
        };

        private static readonly IReadOnlyList<Item> Rings = new[]
        {
            new Item(25, 1, 0),
            new Item(50, 2, 0),
            new Item(100, 3, 0),
            new Item(20, 0, 1),
            new Item(40, 0, 2),
            new Item(80, 0, 3)
        };

        private static Participant MakeBoss() => new()
        {
            name = "Boss",
            hp = 100,
            damage = 8,
            armour = 2
        };

        private static Participant MakePlayer(string name, ICollection<Item> items) => new()
        {
            name = name,
            hp = 100,
            damage = items.Where(i => i != null).Sum(i => i.damage),
            armour = items.Where(i => i != null).Sum(i => i.armour)
        };

        private static string GetWinner(Participant attacker, Participant defender)
        {
            while (true)
            {
                defender.hp -= Math.Max(1, attacker.damage - defender.armour);
                if (defender.hp <= 0)
                {
                    return attacker.name;
                }

                (attacker, defender) = (defender, attacker);
            }
        }

        private static (int, int) Solve()
        {
            const string playerName = "Little Henry Case";

            var ringCombos = Rings.SelectMany((ring1, i) =>
                    Enumerable.Range(i + 1, Rings.Count - i - 1)
                        .Select(j => (ring1, ring2: Rings[j]))
                        .Append((ring1, null))
                )
                .Prepend((null, null))
                .ToList();

            var minCostWin = int.MaxValue;
            var maxCostLose = 0;

            foreach (var weapon in Weapons)
            {
                foreach (var armour in Armour.Prepend(null))
                {
                    foreach (var (ring1, ring2) in ringCombos)
                    {
                        var items = new[] {weapon, armour, ring1, ring2};
                        var player = MakePlayer(playerName, items);
                        var boss = MakeBoss();

                        var cost = items.Where(i => i != null).Sum(i => i.cost);
                        if (GetWinner(player, boss) == playerName)
                        {
                            if (cost < minCostWin)
                            {
                                minCostWin = cost;
                            }
                        }
                        else
                        {
                            if (cost > maxCostLose)
                            {
                                maxCostLose = cost;
                            }
                        }
                    }
                }
            }

            return (minCostWin, maxCostLose);
        }

        public static void Main()
        {
            var (minCoinsWin, maxCoinsLose) = Solve();
            Console.WriteLine(minCoinsWin);
            Console.WriteLine(maxCoinsLose);
        }
    }
}
