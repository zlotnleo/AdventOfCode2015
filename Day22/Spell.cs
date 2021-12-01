using System;

namespace Day22
{
    public class Spell
    {
        public int ManaCost;
        public Action<Game> Cast;
        public Effect Effect;
    }
}
