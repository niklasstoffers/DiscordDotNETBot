using System;

namespace Hainz.Models 
{
    public class Dice 
    {
        public const int DEFAULT_SIDES = 6;

        private int _sides;
        private Random _random;

        public Dice(int sides) 
        {
            if (sides < 1)
                throw new ArgumentException();

            _sides = sides;
            _random = new Random();
        }

        public int RollOnce() => _random.Next(1, _sides + 1);
    }
}