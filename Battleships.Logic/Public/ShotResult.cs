using System.Collections.Generic;
using Battleships.Logic.Coordinates;

namespace Battleships.Logic.Public
{
    public class ShotResult
    {
        public enum Kinds
        {
            Water = 1,
            Hit = 2,
            Sink = 3,
            GameEnd = 4,
            Exception = 11,
            TheSameCoordinatesAgain = 12,
        }

        public Coordinate Coordinate;
        public string Description;
        public Kinds Kind;
        public IEnumerable<Coordinate> SinkShip;
    }
}