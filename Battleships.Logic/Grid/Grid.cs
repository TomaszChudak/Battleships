using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Features;
using Battleships.Logic.Public;
using Battleships.Logic.Ships;

namespace Battleships.Logic.Grid
{
    internal interface IGrid
    {
        ICollection<Ship> Ships { get; }
        GridSize Size { get; }

        ShotResult Shot(Coordinate coordinate);
        void Build(int columnCount, int rowCount);
        bool TryPlaceShip(Ship ship);
    }

    internal class Grid : IGrid
    {
        private object[,] _grid;
        public GridSize Size { get; private set; }
        public ICollection<Ship> Ships { get; private set; }

        public void Build(int columnCount, int rowCount)
        {
            Size = new GridSize(columnCount, rowCount);
            Ships = new List<Ship>();
            _grid = new object[rowCount, columnCount];
        }
        
        public bool TryPlaceShip(Ship ship)
        {
            if (IsShipOverlappingOtherOne(ship)
                || IsShipNextToOtherOne(ship))
                return false;

            PlaceShip(ship);

            return true;
        }

        public ShotResult Shot(Coordinate coordinate)
        {
            var shot = _grid[coordinate.Row, coordinate.Column];

            _grid[coordinate.Row, coordinate.Column] = true;

            if (shot == null)
                return new ShotResult {Coordinate = coordinate, Description = "Water", Kind = ShotResult.Kinds.Water};

            if(shot is bool)
                return new ShotResult {Coordinate = coordinate, Description = "You have entered the same coordinates again", Kind = ShotResult.Kinds.TheSameCoordinatesAgain};

            var hit = ((Ship) shot).Shot(coordinate);

            if (Ships.All(x => x.Sink))
            {
                hit.Description += " You have WIN !!!";
                hit.Kind = ShotResult.Kinds.GameEnd;
            }

            return hit;
        }

        private bool IsShipOverlappingOtherOne(Ship ship)
        {
            return Ships.SelectMany(x => x.Coordinates).Intersect(ship.Coordinates).Any();
        }

        private bool IsShipNextToOtherOne(Ship ship)
        {
            return Ships.SelectMany(x => x.MarginCoordinates).Intersect(ship.Coordinates).Any();
        }

        private void PlaceShip(Ship ship)
        {
            Ships.Add(ship);

            ship.Coordinates.ToList().ForEach(x => _grid[x.Row, x.Column] = ship);
        }
    }
}