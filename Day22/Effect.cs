using System;

namespace Day22
{
    public class Effect
    {
        public string Name;
        public int EffectDuration;
        public Action<Game> EffectEnd;
        public Action<Game> EffectTick;

        public Effect Clone() => new()
        {
            Name = Name,
            EffectDuration = EffectDuration,
            EffectEnd = EffectEnd,
            EffectTick = EffectTick
        };
    }
}
