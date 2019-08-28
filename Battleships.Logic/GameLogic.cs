using System;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Grid;
using Battleships.Logic.Public;
using Battleships.Logic.Settings;

namespace Battleships.Logic
{
    public interface IGameLogic
    {
        ResponseEnvelope<GridSize> StartNewGame();
        ResponseEnvelope<ShotResult> MakeNewMove(string shotCoordinate);
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

        public ResponseEnvelope<GridSize> StartNewGame()
        {
            try
            {
                var validationResult = _settingsChecker.Check();
                if (validationResult?.ErrorMessage != null)
                    return new ResponseEnvelope<GridSize> {Success = false, ErrorDescription = validationResult.ErrorMessage};

                _grid = _gridBuilder.Build();

                return new ResponseEnvelope<GridSize> {Success = true, Content = _grid.Size};
            }
            catch (Exception e)
            {
                return new ResponseEnvelope<GridSize> {Success = false, ErrorDescription = e.Message};
            }
        }

        public ResponseEnvelope<ShotResult> MakeNewMove(string shotCoordinate)
        {
            try
            {
                var validationResult = _coordinateParser.TryParse(shotCoordinate, out var coordinate);
                if (validationResult?.ErrorMessage != null)
                    return new ResponseEnvelope<ShotResult>
                        {Success = true, Content = new ShotResult {Kind = ShotResult.Kinds.WrongCoordinates, Description = validationResult.ErrorMessage}};

                var shotResult = _grid.Shot(coordinate);

                return new ResponseEnvelope<ShotResult> {Success = true, Content = shotResult};
            }
            catch (Exception e)
            {
                return new ResponseEnvelope<ShotResult> {Success = false, ErrorDescription = e.Message};
            }
        }
    }
}