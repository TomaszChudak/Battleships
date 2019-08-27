using System;
using System.Linq;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using Microsoft.Extensions.Options;

namespace Battleships.Logic.Grid
{
    internal interface IGridBuilder
    {
        IGrid Build();
    }

    internal class GridBuilder : IGridBuilder
    {
        private readonly IOptions<AppSettings> _config;
        private readonly IGrid _grid;
        private readonly IShipFactory _shipFactory;

        public GridBuilder(IOptions<AppSettings> config, IShipFactory shipFactory, IGrid grid)
        {
            _config = config;
            _shipFactory = shipFactory;
            _grid = grid;
        }

        public IGrid Build()
        {
            _grid.Build(_config.Value.Grid.ColumnCount.Value, _config.Value.Grid.RowCount.Value);

            foreach (var shipType in _config.Value.ShipTypes.OrderByDescending(x => x.Size))
                for (var i = 0; i < shipType.Count; i++)
                for (var x = 0; x < 1000; x++)
                {
                    var ship = _shipFactory.BuildShip(shipType.Name);
                    if (_grid.TryPlaceShip(ship))
                        break;
                    if (x == 99)
                        throw new ApplicationException("Can't find any place for new ship.");
                }

            return _grid;
        }
    }
}