using System;
using Battleships.Console.Input;
using Battleships.Console.Output;
using Battleships.Logic;
using Battleships.Logic.Public;

namespace Battleships.Console
{
    internal interface IDispatcher
    {
        void PlayGame();
    }

    internal class Dispatcher : IDispatcher
    {
        private readonly IGameLogic _gameLogic;
        private readonly IInputReader _inputReader;
        private readonly IOutputFacade _outputFacade;

        public Dispatcher(IGameLogic gameLogic, IInputReader inputReader, IOutputFacade outputFacade)
        {
            _gameLogic = gameLogic;
            _inputReader = inputReader;
            _outputFacade = outputFacade;
        }

        public void PlayGame()
        {
            try
            {
                StartAndPlayGame();
            }
            catch (Exception e)
            {
                _outputFacade.DisplayException(e.Message);
            }

            _inputReader.ReadUserKey();
        }

        private void StartAndPlayGame()
        {
            var gridSizeEnvelope = _gameLogic.StartNewGame();

            if (!gridSizeEnvelope.Success)
            {
                _outputFacade.DisplayException(gridSizeEnvelope.ErrorDescription);
                return;
            }

            _outputFacade.PaintNewGrid(gridSizeEnvelope.Content);

            var result = new ShotResult();

            while (result.Kind != ShotResult.Kinds.GameEnd)
            {
                var userCommand = _inputReader.ReadUserCommand();

                result = GetNextMoveResult(userCommand);

                _outputFacade.MarkAndDisplayResult(result);
            }
        }

        private ShotResult GetNextMoveResult(string userCommand)
        {
            var shotResultEnvelope = _gameLogic.MakeNewMove(userCommand);

            if (!shotResultEnvelope.Success)
                return new ShotResult {Description = shotResultEnvelope.ErrorDescription, Kind = ShotResult.Kinds.Exception};

            return shotResultEnvelope.Content;
        }
    }
}