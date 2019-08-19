using System.Collections.Generic;
using Battleships.Console.Output;
using Battleships.Logic.Features;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.Tests.Output
{
    public class TextResultDisplayerTests
    {
        public TextResultDisplayerTests()
        {
            _cursorHelperMock = new Mock<ICursorHelper>(MockBehavior.Strict);
            _outputWriterMock = new Mock<IOutputWriter>(MockBehavior.Strict);
            _sut = new TextResultDisplayer(_outputWriterMock.Object, _cursorHelperMock.Object);
        }

        private readonly Mock<IOutputWriter> _outputWriterMock;
        private readonly Mock<ICursorHelper> _cursorHelperMock;
        private readonly ITextResultDisplayer _sut;

        [Fact]
        public void DisplayResult_GoodCoordinatesWithHit_CoordinatesAreDisplayed()
        {
            _cursorHelperMock.Setup(x => x.TextResultTop)
                .Returns(22);
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Kind = ShotResult.Kinds.Hit, Description = "Shit has been hit."};

            _sut.DisplayResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.TextResultTop, Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 22), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 23), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(2, 23), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(4));
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("E5"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("->"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("Shit has been hit."))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("End"))), Times.Never);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithLastSink_CoordinatesAreDisplayed()
        {
            _cursorHelperMock.Setup(x => x.TextResultTop)
                .Returns(22);
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Kind = ShotResult.Kinds.GameEnd, Description = "Shit has been sink. You have win."};

            _sut.DisplayResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.TextResultTop, Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 22), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 23), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(2, 23), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(5));
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("E5"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("->"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("Shit has been sink. You have win."))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("End"))), Times.Once);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithSink_CoordinatesAreDisplayed()
        {
            _cursorHelperMock.Setup(x => x.TextResultTop)
                .Returns(22);
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult
            {
                Coordinate = new Coordinate(4, 4), Kind = ShotResult.Kinds.Sink,
                SinkShip = new List<Coordinate> {new Coordinate(3, 4), new Coordinate(4, 4)},
                Description = "Shit has been sink."
            };

            _sut.DisplayResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.TextResultTop, Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 22), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 23), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(2, 23), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(4));
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("E5"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("->"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("Shit has been sink."))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("End"))), Times.Never);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithWater_CoordinatesAreDisplayed()
        {
            _cursorHelperMock.Setup(x => x.TextResultTop)
                .Returns(22);
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = new Coordinate(5, 5), Description = "Water.", Kind = ShotResult.Kinds.Water};

            _sut.DisplayResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.TextResultTop, Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 22), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 23), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(2, 23), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(4));
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("F6"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("->"))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("Water."))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("End"))), Times.Never);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }

        [Fact]
        public void DisplayResult_WrongCoordinates_CoordinatesAreNotDisplayed()
        {
            _cursorHelperMock.Setup(x => x.TextResultTop)
                .Returns(22);
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = null, Description = "Wrong coordinates.", Kind = ShotResult.Kinds.Exception};

            _sut.DisplayResult(shotResult);

            _cursorHelperMock.Verify(x => x.GetLeftForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.GetTopForCoordinate(It.IsAny<Coordinate>()), Times.Never);
            _cursorHelperMock.Verify(x => x.TextResultTop, Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 22), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(0, 23), Times.Once);
            _outputWriterMock.Verify(x => x.SetCursorPosition(2, 23), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(4));
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("->"))), Times.Never);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("Wrong coordinates."))), Times.Once);
            _outputWriterMock.Verify(x => x.Write(It.Is<string>(y => y.Contains("End"))), Times.Never);
            _outputWriterMock.Verify(x => x.WriteNewLine(), Times.Never);
        }
    }
}