using System.Collections.Generic;
using Battleships.Console.Output;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.Tests.Output
{
    public class TextResultDisplayerIntegrationTests
    {
        public TextResultDisplayerIntegrationTests()
        {
            _cursorHelper = new CursorHelper();
            _outputWriterMock = new Mock<IOutputWriter>(MockBehavior.Strict);
            _sut = new TextResultDisplayer(_outputWriterMock.Object, _cursorHelper);
        }

        private readonly ICursorHelper _cursorHelper;
        private readonly Mock<IOutputWriter> _outputWriterMock;
        private readonly ITextResultDisplayer _sut;

        [Fact]
        public void DisplayResult_GoodCoordinatesWithHit_CoordinatesAreDisplayed()
        {
            _cursorHelper.SetGridSize(new GridSize(10, 10));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Kind = ShotResult.Kinds.Hit, Description = "Shit has been hit."};

            _sut.DisplayResult(shotResult);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithLastSink_CoordinatesAreDisplayed()
        {
            _cursorHelper.SetGridSize(new GridSize(10, 10));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = new Coordinate(4, 4), Kind = ShotResult.Kinds.GameEnd, Description = "Shit has been sink. You have win."};

            _sut.DisplayResult(shotResult);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithSink_CoordinatesAreDisplayed()
        {
            _cursorHelper.SetGridSize(new GridSize(10, 10));
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
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithWater_CoordinatesAreDisplayed()
        {
            _cursorHelper.SetGridSize(new GridSize(10, 10));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = new Coordinate(5, 5), Description = "Water.", Kind = ShotResult.Kinds.Water};

            _sut.DisplayResult(shotResult);
        }

        [Fact]
        public void DisplayResult_WrongCoordinates_CoordinatesAreNotDisplayed()
        {
            _cursorHelper.SetGridSize(new GridSize(10, 10));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWriterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            var shotResult = new ShotResult {Coordinate = null, Description = "Wrong coordinates.", Kind = ShotResult.Kinds.Exception};

            _sut.DisplayResult(shotResult);
        }
    }
}