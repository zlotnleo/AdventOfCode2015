using System;
using System.Collections.Generic;
using System.Linq;

namespace Day22
{
    public class Game
    {
        public enum Outcome
        {
            Ongoing,
            PlayerWin,
            BossWin
        }

        public int PlayerHp = 50;
        public int PlayerMana = 500;
        public int PlayerArmour = 0;
        public int BossHp = 71;
        public int BossDamage = 10;

        public int ManaSpent = 0;

        public List<Effect> currentEffects = new();

        private void ApplyEffects()
        {
            for (var i = currentEffects.Count - 1; i >= 0; i--)
            {
                var effect = currentEffects[i];
                effect.EffectTick?.Invoke(this);
                if (--effect.EffectDuration == 0)
                {
                    effect.EffectEnd?.Invoke(this);
                    currentEffects.RemoveAt(i);
                }
            }
        }

        private bool CheckGameFinished(int? manaRequired, out Outcome outcome)
        {
            if (PlayerHp <= 0 || manaRequired > PlayerMana)
            {
                outcome = Outcome.BossWin;
                return true;
            }

            if (BossHp <= 0)
            {
                outcome = Outcome.PlayerWin;
                return true;
            }

            outcome = Outcome.Ongoing;
            return false;
        }

        public Outcome PlayerTurn(Spell spell, bool hardMode)
        {
            if (hardMode)
            {
                PlayerHp--;
            }

            ApplyEffects();

            if (CheckGameFinished(spell.ManaCost, out var outcome))
            {
                return outcome;
            }

            ManaSpent += spell.ManaCost;
            PlayerMana -= spell.ManaCost;
            spell.Cast?.Invoke(this);

            if (spell.Effect != null)
            {
                if (currentEffects.Any(e => e.Name == spell.Effect.Name))
                {
                    return Outcome.BossWin;
                }

                currentEffects.Add(spell.Effect.Clone());
            }

            CheckGameFinished(null, out outcome);
            return outcome;
        }

        public Outcome BossTurn()
        {
            ApplyEffects();

            if (CheckGameFinished(null, out var outcome))
            {
                return outcome;
            }

            PlayerHp -= Math.Max(1, BossDamage - PlayerArmour);

            CheckGameFinished(null, out outcome);
            return outcome;
        }

        public Game Clone() => new()
        {
            PlayerHp = PlayerHp,
            PlayerMana = PlayerMana,
            PlayerArmour = PlayerArmour,
            BossHp = BossHp,
            BossDamage = BossDamage,
            ManaSpent = ManaSpent,
            currentEffects = currentEffects.Select(e => e.Clone()).ToList()
        };
    }
}
