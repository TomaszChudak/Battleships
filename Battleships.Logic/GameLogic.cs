using System;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Grid;
using Battleships.Logic.Public;
using Battleships.Logic.Settings;

namespace Battleships.Logic
{
    public interface IGameLogic
    {
        GridSize StartNewGame();
        ShotResult MakeNewMove(string shotCoordinate);
    }

    internal class GameLogic : IGameLogic
    {
        private readonly ICoordinateParser _coordinateParser;
        private readonly IGridBuilder _gridBuilder;
        private readonly ISettingsChecker _settingsChecker;
        private IGrid _grid;

        public GameLogic(ISettingsChecker settingsChecker, IGridBuilder gridBuilder, ICoordinateParser coordinateParser)
        {
            _settingsChecker = settingsChecker;
            _gridBuilder = gridBuilder;
            _coordinateParser = coordinateParser;
        }

        public GridSize StartNewGame()
        {
            var validationResult = _settingsChecker.Check();
            if (validationResult?.ErrorMessage != null)
                throw new ApplicationException(validationResult.ErrorMessage);

            _grid = _gridBuilder.Build();

            return _grid.Size;
        }

        public ShotResult MakeNewMove(string shotCoordinate)
        {
            var validationResult = _coordinateParser.TryParse(shotCoordinate, out var coordinate);
            if (validationResult?.ErrorMessage != null)
                return new ShotResult {Kind = ShotResult.Kinds.WrongCoordinates, Description = validationResult.ErrorMessage};

            return _grid.Shot(coordinate);
        }
    }
}