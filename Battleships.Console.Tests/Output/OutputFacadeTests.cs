﻿using Battleships.Console.Output;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.Tests.Output
{
    public class OutputFacadeTests
    {
        public OutputFacadeTests()
        {
            _cursorHelperMock = new Mock<ICursorHelper>(MockBehavior.Strict);
            _gridPainterMock = new Mock<IGridPainter>(MockBehavior.Strict);
            _gridResultPainterMock = new Mock<IGridResultPainter>(MockBehavior.Strict);
            _textResultDisplayerMock = new Mock<ITextResultDisplayer>(MockBehavior.Strict);
            _soundPlayerMock = new Mock<ISoundPlayer>(MockBehavior.Strict);
            _outputWriter = new Mock<IOutputWriter>(MockBehavior.Strict);

            _sut = new OutputFacade(_cursorHelperMock.Object, _gridPainterMock.Object, _gridResultPainterMock.Object, _textResultDisplayerMock.Object, _soundPlayerMock.Object,
                _outputWriter.Object);
        }

        private readonly Mock<ICursorHelper> _cursorHelperMock;
        private readonly Mock<IGridPainter> _gridPainterMock;
        private readonly Mock<IGridResultPainter> _gridResultPainterMock;
        private readonly Mock<ITextResultDisplayer> _textResultDisplayerMock;
        private readonly Mock<ISoundPlayer> _soundPlayerMock;
        private readonly Mock<IOutputWriter> _outputWriter;

        private readonly IOutputFacade _sut;

        [Fact]
        public void DisplayException_SimpleRun_ExceptionIsDisplayed()
        {
            var exception = "Some exception";
            _outputWriter.Setup(x => x.SetCursorPosition(0, 0));
            _outputWriter.Setup(x => x.Write("Some exception"));

            _sut.DisplayException(exception);
        }

        [Fact]
        public void MarkAndDisplayResult_SimpleRun_ResultIsPaintedAndDescribed()
        {
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Description = "Water.", Kind = ShotResult.Kinds.Water};
            _gridResultPainterMock.Setup(x => x.PaintResult(shotResult));
            _textResultDisplayerMock.Setup(x => x.DisplayResult(shotResult));
            _soundPlayerMock.Setup(x => x.PlayResult(shotResult));

            _sut.MarkAndDisplayResult(shotResult);
        }

        [Fact]
        public void MarkAndDisplayResult_TheSameCoordinatesAgain_OnlyTextResultWillBeChanged()
        {
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Description = "The Same Coordinates Again", Kind = ShotResult.Kinds.TheSameCoordinatesAgain};
            _textResultDisplayerMock.Setup(x => x.DisplayResult(shotResult));

            _sut.MarkAndDisplayResult(shotResult);
        }

        [Fact]
        public void PaintNewGrid_SimpleRun_GridIsPainted()
        {
            var gridSize = new GridSize(5, 8);
            _cursorHelperMock.Setup(x => x.SetGridSize(gridSize));
            _gridPainterMock.Setup(x => x.PaintNewGrid(gridSize));
            _textResultDisplayerMock.Setup(x => x.DisplayResult(null));

            _sut.PaintNewGrid(gridSize);
        }
    }
}