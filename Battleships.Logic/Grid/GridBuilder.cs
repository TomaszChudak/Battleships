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
            for (var i = 0; i < 100; i++)
            {
                _grid.Build(_config.Value.Grid.ColumnCount.Value, _config.Value.Grid.RowCount.Value);

                if (TrySetShipsOnGrid())
                    return _grid;
            }

            throw new ApplicationException("Can't find any place for new ship.");
        }

        private bool TrySetShipsOnGrid()
        {
            foreach (var shipType in _config.Value.ShipTypes.OrderByDescending(x => x.Size))
                for (var i = 0; i < shipType.Count; i++)
                    if (!TryPlaceNextShip(shipType.Name))
                        return false;

            return true;
        }

        private bool TryPlaceNextShip(string shipTypeName)
        {
            for (var x = 0; x < 1000; x++)
            {
                var ship = _shipFactory.BuildShip(shipTypeName);
                if (_grid.TryPlaceShip(ship))
                    return true;
            }

            return false;
        }
    }
}