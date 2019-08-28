using System.Collections.Generic;
using Battleships.Console.Output;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.Tests.Output
{
    public class GridResultPainterIntegrationTests
    {
        public GridResultPainterIntegrationTests()
        {
            var cursorHelper = new CursorHelper();
            _outputWriterMock = new Mock<IOutputWriter>(MockBehavior.Strict);
            _sut = new GridResultPainter(cursorHelper, _outputWriterMock.Object);
        }

        private readonly Mock<IOutputWriter> _outputWriterMock;
        private readonly IGridResultPainter _sut;

        [Fact]
        public void DisplayResult_GoodCoordinatesWithHit_CoordinatesAreDisplayed()
        {
            _outputWriterMock.Setup(x => x.SetCursorPosition(9, 14));
            _outputWriterMock.Setup(x => x.Write("X"));
            var shotResult = new ShotResult {Coordinate = new Coordinate('B', "7"), Kind = ShotResult.Kinds.Hit, Description = "Shit has been hit."};

            _sut.PaintResult(shotResult);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithSink_CoordinatesAreDisplayed()
        {
            _outputWriterMock.Setup(x => x.SetCursorPosition(29, 14));
            _outputWriterMock.Setup(x => x.SetCursorPosition(29, 16));
            _outputWriterMock.Setup(x => x.SetCursorPosition(29, 18));
            _outputWriterMock.Setup(x => x.Write("#"));
            var shotResult = new ShotResult
            {
                Coordinate = new Coordinate('G', "8"), Kind = ShotResult.Kinds.Sink,
                SinkShip = new List<Coordinate> {new Coordinate('G', "7"), new Coordinate('G', "8"), new Coordinate('G', "9")},
                Description = "Shit has been sink."
            };

            _sut.PaintResult(shotResult);
        }

        [Fact]
        public void DisplayResult_GoodCoordinatesWithWater_CoordinatesAreDisplayed()
        {
            _outputWriterMock.Setup(x => x.SetCursorPosition(25, 12));
            _outputWriterMock.Setup(x => x.Write("."));
            var shotResult = new ShotResult {Coordinate = new Coordinate('F', "6"), Description = "Water.", Kind = ShotResult.Kinds.Water};

            _sut.PaintResult(shotResult);
        }

        [Fact]
        public void DisplayResult_WrongCoordinates_CoordinatesAreNotDisplayed()
        {
            var shotResult = new ShotResult {Coordinate = null, Description = "Wrong coordinates.", Kind = ShotResult.Kinds.Exception};

            _sut.PaintResult(shotResult);
        }
    }
}