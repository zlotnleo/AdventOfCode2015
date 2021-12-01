using System;
using System.Collections.Generic;

namespace Day22
{
    class Program
    {
        private static readonly IReadOnlyList<Spell> Spells = new Spell[]
        {
            new()
            {
                ManaCost = 53,
                Cast = game => game.BossHp -= 4
            },
            new()
            {
                ManaCost = 73,
                Cast = game =>
                {
                    game.BossHp -= 2;
                    game.PlayerHp += 2;
                }
            },
            new()
            {
                ManaCost = 113,
                Cast = game => game.PlayerArmour += 7,
                Effect = new Effect
                {
                    Name = "Shield",
                    EffectDuration = 6,
                    EffectEnd = game => game.PlayerArmour -= 7
                }
            },
            new()
            {
                ManaCost = 173,
                Effect = new Effect
                {
                    Name = "Poison",
                    EffectDuration = 6,
                    EffectTick = game => game.BossHp -= 3
                }
            },
            new()
            {
                ManaCost = 229,
                Effect = new Effect {
                    Name = "Recharge",
                    EffectDuration = 5,
                    EffectTick = game => game.PlayerMana += 101
                }
            }
        };

        public static int Solve(bool hardMode)
        {
            var games = new Stack<Game>();
            games.Push(new Game());

            var minManaSpent = int.MaxValue;

            while (games.Count != 0)
            {
                var game = games.Pop();

                if (game.ManaSpent >= minManaSpent)
                {
                    continue;
                }

                foreach (var spell in Spells)
                {
                    var newGame = game.Clone();
                    var outcome = newGame.PlayerTurn(spell, hardMode);
                    if (outcome == Game.Outcome.Ongoing)
                    {
                        outcome = newGame.BossTurn();
                    }

                    switch (outcome)
                    {
                        case Game.Outcome.Ongoing:
                            games.Push(newGame);
                            break;
                        case Game.Outcome.PlayerWin:
                            minManaSpent = Math.Min(minManaSpent, newGame.ManaSpent);
                            break;
                    }
                }
            }

            return minManaSpent;
        }

        public static void Main()
        {
            Console.WriteLine(Solve(false));
            Console.WriteLine(Solve(true));
        }
    }
}
