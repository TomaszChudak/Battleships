using System.Linq;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using Microsoft.Extensions.Options;

namespace Battleships.Logic.Ships
{
    internal interface IShipFactory
    {
        Ship BuildShip(string name);
    }

    internal class ShipFactory : IShipFactory
    {
        private readonly IRandomWrapper _randomWrapper;
        private readonly IOptions<AppSettings> _config;

        public ShipFactory(IOptions<AppSettings> config, IRandomWrapper randomWrapper)
        {
            _config = config;
            _randomWrapper = randomWrapper;
        }

        public Ship BuildShip(string name)
        {
            var size = _config.Value.ShipTypes.Single(x => x.Name == name).Size.Value;
            var horizontal = _randomWrapper.Next(2);
            var row = _randomWrapper.Next(_config.Value.Grid.RowCount.Value - (horizontal == 0 ? size : 0));
            var column = _randomWrapper.Next(_config.Value.Grid.ColumnCount.Value - (horizontal == 1 ? size : 0));
            var topLeftCoordinates = new Coordinate(column, row);
            return new Ship(name, topLeftCoordinates, horizontal == 1, size);
        }
    }
}