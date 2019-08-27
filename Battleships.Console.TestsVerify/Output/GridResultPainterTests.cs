using System.Collections.Generic;
using Battleships.Console.Output;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.TestsVerify.Output
{
    public class GridResultPainterTests
    {
        public GridResultPainterTests()
        {
            _cursorHelperMock = new Mock<ICursorHelper>(MockBehavior.Strict);
            _outputWriterMock = new Mock<IOutputWriter>(MockBehavior.Strict);
            _sut = new GridResultPainter(_cursorHelperMock.Object, _outputWriterMock.Object);
        }

        private readonly Mock<ICursorHelper> _cursorHelperMock;
        private readonly Mock<IOutputWriter> _outputWriterMock;
        private readonly IGridResultPainter _sut;

        [Fact]
        public void DisplayResult_GoodCoordinatesWithHit_CoordinatesAreDisplayed()
        {
            _cursorHelperMock.Setup(x => x.GetLeftForCoordinate(new Coordinate('B', "7")))
                .Returns(13);
            _cursorHelperMock.Setup(x => x.GetTopForCoordinate(new Coordinate('B', "7")))
                .Returns(16);
            _outputWriterMock.Setup(x => x.SetCursorPosition(13, 16));
            _outputWriterMock.Setup(x => x.Write("X"));
            var shotResult = new ShotResult {Coordinate = new Coordinate('B', "7"), Kind = ShotResult.Kinds.Hit, Description = "Shit has been hit."};

            _sut.PaintResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Once);
            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(new Coordinate('B', "7")), Times.Once);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Once);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(new Coordinate('B', "7")), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(13, 16), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Once);
            _outputWriterMock.Verify(x => x.Write("X"), Times.Once);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithSink_CoordinatesAreDisplayed()
        {
            _cursorHelperMock.Setup(x => x.GetLeftForCoordinate(new Coordinate('G', "7")))
                .Returns(33);
            _cursorHelperMock.Setup(x => x.GetLeftForCoordinate(new Coordinate('G', "8")))
                .Returns(33);
            _cursorHelperMock.Setup(x => x.GetLeftForCoordinate(new Coordinate('G', "9")))
                .Returns(33);
            _cursorHelperMock.Setup(x => x.GetTopForCoordinate(new Coordinate('G', "7")))
                .Returns(19);
            _cursorHelperMock.Setup(x => x.GetTopForCoordinate(new Coordinate('G', "8")))
                .Returns(20);
            _cursorHelperMock.Setup(x => x.GetTopForCoordinate(new Coordinate('G', "9")))
                .Returns(21);
            _outputWriterMock.Setup(x => x.SetCursorPosition(33, 19));
            _outputWriterMock.Setup(x => x.SetCursorPosition(33, 20));
            _outputWriterMock.Setup(x => x.SetCursorPosition(33, 21));
            _outputWriterMock.Setup(x => x.Write("#"));
            var shotResult = new ShotResult
            {
                Coordinate = new Coordinate('G', "8"), Kind = ShotResult.Kinds.Sink,
                SinkShip = new List<Coordinate> {new Coordinate('G', "7"), new Coordinate('G', "8"), new Coordinate('G', "9")},
                Description = "Shit has been sink."
            };

            _sut.PaintResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Exactly(3));
            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(new Coordinate('G', "7")), Times.Once);
            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(new Coordinate('G', "8")), Times.Once);
            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(new Coordinate('G', "9")), Times.Once);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Exactly(3));
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(new Coordinate('G', "7")), Times.Once);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(new Coordinate('G', "8")), Times.Once);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(new Coordinate('G', "9")), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(33, 19), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(33, 20), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(33, 21), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.Write("#"), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithWater_CoordinatesAreDisplayed()
        {
            _cursorHelperMock.Setup(x => x.GetLeftForCoordinate(new Coordinate('F', "6")))
                .Returns(29);
            _cursorHelperMock.Setup(x => x.GetTopForCoordinate(new Coordinate('F', "6")))
                .Returns(12);
            _outputWriterMock.Setup(x => x.SetCursorPosition(29, 12));
            _outputWriterMock.Setup(x => x.Write("."));
            var shotResult = new ShotResult {Coordinate = new Coordinate('F', "6"), Description = "Water.", Kind = ShotResult.Kinds.Water};

            _sut.PaintResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Once);
            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(new Coordinate('F', "6")), Times.Once);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Once);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(new Coordinate('F', "6")), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(29, 12), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Once);
            _outputWriterMock.Verify(x => x.Write("."), Times.Once);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }

        [Fact]
        public void DisplayResult_WrongCoordinates_CoordinatesAreNotDisplayed()
        {
            var shotResult = new ShotResult {Coordinate = null, Description = "Wrong coordinates.", Kind = ShotResult.Kinds.Exception};

            _sut.PaintResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Never);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }
    }
}