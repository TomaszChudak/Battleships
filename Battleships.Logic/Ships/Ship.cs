using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Features;
using Battleships.Logic.Public;

namespace Battleships.Logic.Ships
{
    internal interface IShip
    {
        string Name { get; }
        IReadOnlyList<Coordinate> Coordinates { get; }
        IReadOnlyList<Coordinate> MarginCoordinates { get; }
        int Size { get; }
        ShotResult Shot(Coordinate coordinate);
        bool Sink { get; }
    }

    internal class Ship : IShip
    {
        private readonly IDictionary<Coordinate, bool> _hits;

        public Ship(string name, Coordinate topLeftCoordinate, bool isHorizontal, int size)
        {
            Name = name;
            Size = size;

            Coordinates = GetCoordinates(topLeftCoordinate, isHorizontal);
            MarginCoordinates = GetMarginCoordinates(topLeftCoordinate, isHorizontal);

            _hits = Coordinates.ToDictionary(x => x, x => false);
        }

        public int Size { get; }
        public string Name { get; }
        public IReadOnlyList<Coordinate> Coordinates { get; }
        public IReadOnlyList<Coordinate> MarginCoordinates { get; }

        public ShotResult Shot(Coordinate coordinate)
        {
            if (!_hits.ContainsKey(coordinate))
                throw new KeyNotFoundException("Wrong coordinates.");

            _hits[coordinate] = true;

            return Sink
                ? new ShotResult {Coordinate = coordinate, Description = $"{Name} has been sink.", Kind = ShotResult.Kinds.Sink, SinkShip = Coordinates}
                : new ShotResult {Coordinate = coordinate, Description = $"{Name} has been hit.", Kind = ShotResult.Kinds.Hit, SinkShip = null};
        }

        public bool Sink => _hits.All(x => x.Value);

        private List<Coordinate> GetCoordinates(Coordinate topLeftCoordinate, bool isHorizontal)
        {
            return Enumerable.Range(0, Size)
                .Select(x => new Coordinate(topLeftCoordinate.Column + (isHorizontal ? x : 0), topLeftCoordinate.Row + (isHorizontal ? 0 : x)))
                .OrderBy(x => x.Column)
                .ToList();
        }

        private List<Coordinate> GetMarginCoordinates(Coordinate topLeftCoordinate, bool isHorizontal)
        {
            var columns = Enumerable.Range(topLeftCoordinate.Column - 1, (isHorizontal ? Size : 1) + 2);
            var rows = Enumerable.Range(topLeftCoordinate.Row - 1, (isHorizontal ? 1 : Size) + 2);
            var marginPlusShip = from c in columns
                from r in rows
                select new Coordinate(c, r);
            return marginPlusShip
                .Except(GetCoordinates(topLeftCoordinate, isHorizontal))
                .OrderBy(x => x.Row)
                .ThenBy(x => x.Column)
                .ToList();
        }
    }
}