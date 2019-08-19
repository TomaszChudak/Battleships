using Battleships.Logic.Grid;
using Battleships.Logic.Helpers;
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
            _settingsChecker.Check();

            _grid = _gridBuilder.Build();

            return _grid.Size;
        }

        public ShotResult MakeNewMove(string shotCoordinate)
        {
            var coordinate = _coordinateParser.Parse(shotCoordinate);

            return _grid.Shot(coordinate);
        }
    }
}