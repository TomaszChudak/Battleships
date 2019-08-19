using System;

namespace Battleships.Logic.Helpers
{
    internal interface IRandomWrapper
    {
        int Next(int maxValue);
    }

    internal class RandomWrapper : IRandomWrapper
    {
        private readonly Random _random;

        public RandomWrapper()
        {
            _random = new Random();
        }

        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}