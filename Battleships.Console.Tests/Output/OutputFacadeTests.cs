using System;
using Battleships.Console.Output;
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
            var exception = new ApplicationException("Some exception");
            _outputWriter.Setup(x => x.SetCursorPosition(0, 0));
            _outputWriter.Setup(x => x.Write("Some exception"));

            _sut.DisplayException(exception);

            _cursorHelperMock.Verify(x => x.SetGridSize(It.IsAny<GridSize>()), Times.Never);
            _gridPainterMock.Verify(x => x.PaintNewGrid(It.IsAny<GridSize>()), Times.Never);
            _gridResultPainterMock.Verify(x => x.PaintResult(It.IsAny<ShotResult>()), Times.Never);
            _textResultDisplayerMock.Verify(x => x.DisplayResult(It.IsAny<ShotResult>()), Times.Never);
            _soundPlayerMock.Verify(x => x.PlayResult(It.IsAny<ShotResult>()), Times.Never);
            _outputWriter.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _outputWriter.Verify(x => x.SetCursorPosition(0, 0), Times.Once);
            _outputWriter.Verify(x => x.Write(It.IsAny<string>()), Times.Once);
            _outputWriter.Verify(x => x.Write("Some exception"), Times.Once);
        }

        [Fact]
        public void MarkAndDisplayResult_SimpleRun_ResultIsPaintedAndDescribed()
        {
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Description = "Water.", Kind = ShotResult.Kinds.Water};
            _gridResultPainterMock.Setup(x => x.PaintResult(shotResult));
            _textResultDisplayerMock.Setup(x => x.DisplayResult(shotResult));
            _soundPlayerMock.Setup(x => x.PlayResult(shotResult));

            _sut.MarkAndDisplayResult(shotResult);

            _cursorHelperMock.Verify(x => x.SetGridSize(It.IsAny<GridSize>()), Times.Never);
            _gridPainterMock.Verify(x => x.PaintNewGrid(It.IsAny<GridSize>()), Times.Never);
            _gridResultPainterMock.Verify(x => x.PaintResult(It.IsAny<ShotResult>()), Times.Once);
            _gridResultPainterMock.Verify(x => x.PaintResult(shotResult), Times.Once);
            _textResultDisplayerMock.Verify(x => x.DisplayResult(It.IsAny<ShotResult>()), Times.Once);
            _textResultDisplayerMock.Verify(x => x.DisplayResult(shotResult), Times.Once);
            _soundPlayerMock.Verify(x => x.PlayResult(It.IsAny<ShotResult>()), Times.Once);
            _soundPlayerMock.Verify(x => x.PlayResult(shotResult), Times.Once);
        }

        [Fact]
        public void MarkAndDisplayResult_TheSameCoordinatesAgain_OnlyTextResultWillBeChanged()
        {
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Description = "The Same Coordinates Again", Kind = ShotResult.Kinds.TheSameCoordinatesAgain};
            _textResultDisplayerMock.Setup(x => x.DisplayResult(shotResult));

            _sut.MarkAndDisplayResult(shotResult);

            _cursorHelperMock.Verify(x => x.SetGridSize(It.IsAny<GridSize>()), Times.Never);
            _gridPainterMock.Verify(x => x.PaintNewGrid(It.IsAny<GridSize>()), Times.Never);
            _gridResultPainterMock.Verify(x => x.PaintResult(It.IsAny<ShotResult>()), Times.Never);
            _textResultDisplayerMock.Verify(x => x.DisplayResult(It.IsAny<ShotResult>()), Times.Once);
            _textResultDisplayerMock.Verify(x => x.DisplayResult(shotResult), Times.Once);
            _soundPlayerMock.Verify(x => x.PlayResult(It.IsAny<ShotResult>()), Times.Never);
        }

        [Fact]
        public void PaintNewGrid_SimpleRun_GridIsPainted()
        {
            var gridSize = new GridSize(5, 8);
            _cursorHelperMock.Setup(x => x.SetGridSize(gridSize));
            _gridPainterMock.Setup(x => x.PaintNewGrid(gridSize));
            _textResultDisplayerMock.Setup(x => x.DisplayResult(null));

            _sut.PaintNewGrid(gridSize);

            _cursorHelperMock.Verify(x => x.SetGridSize(It.IsAny<GridSize>()), Times.Once);
            _cursorHelperMock.Verify(x => x.SetGridSize(gridSize), Times.Once);
            _gridPainterMock.Verify(x => x.PaintNewGrid(It.IsAny<GridSize>()), Times.Once);
            _gridPainterMock.Verify(x => x.PaintNewGrid(gridSize), Times.Once);
            _gridResultPainterMock.Verify(x => x.PaintResult(It.IsAny<ShotResult>()), Times.Never);
            _textResultDisplayerMock.Verify(x => x.DisplayResult(It.IsAny<ShotResult>()), Times.Once);
            _textResultDisplayerMock.Verify(x => x.DisplayResult(null), Times.Once);
            _soundPlayerMock.Verify(x => x.PlayResult(It.IsAny<ShotResult>()), Times.Never);
        }
    }
}