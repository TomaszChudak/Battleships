using Battleships.Logic.Public;
using System;

namespace Battleships.Console.Output
{
    internal interface IOutputFacade
    {
        void PaintNewGrid(GridSize gridSize);
        void MarkAndDisplayResult(ShotResult result);
        void DisplayException(Exception e);
    }

    internal class OutputFacade : IOutputFacade
    {
        private readonly ICursorHelper _cursorHelper;
        private readonly IGridPainter _gridPainter;
        private readonly IGridResultPainter _gridResultPainter;
        private readonly ISoundPlayer _soundPlayer;
        private readonly ITextResultDisplayer _textResultDisplayer;
        private readonly IOutputWriter _outputWriter;

        public OutputFacade(ICursorHelper cursorHelper, IGridPainter gridPainter, IGridResultPainter gridResultPainter, ITextResultDisplayer textResultDisplayer,
            ISoundPlayer soundPlayer, IOutputWriter outputWriter)
        {
            _cursorHelper = cursorHelper;
            _gridPainter = gridPainter;
            _gridResultPainter = gridResultPainter;
            _textResultDisplayer = textResultDisplayer;
            _soundPlayer = soundPlayer;
            _outputWriter = outputWriter;
        }

        public void PaintNewGrid(GridSize gridSize)
        {
            _cursorHelper.SetGridSize(gridSize);
            _gridPainter.PaintNewGrid(gridSize);
            _textResultDisplayer.DisplayResult(null);
        }

        public void MarkAndDisplayResult(ShotResult result)
        {
            if (result.Kind != ShotResult.Kinds.WrongCoordinates
                && result.Kind != ShotResult.Kinds.TheSameCoordinatesAgain
                && result.Kind != ShotResult.Kinds.Exception)
            {
                _soundPlayer.PlayResult(result);
                _gridResultPainter.PaintResult(result);
            }

            _textResultDisplayer.DisplayResult(result);
        }

        public void DisplayException(Exception e)
        {
            _outputWriter.SetCursorPosition(0, 0);
            _outputWriter.Write(e.Message);
        }
    }
}