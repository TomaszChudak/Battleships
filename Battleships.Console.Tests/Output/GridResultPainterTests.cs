﻿using System.Collections.Generic;
using Battleships.Console.Output;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.Tests.Output
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
        }

        [Fact]
        public void DisplayResult_WrongCoordinates_CoordinatesAreNotDisplayed()
        {
            var shotResult = new ShotResult {Coordinate = null, Description = "Wrong coordinates.", Kind = ShotResult.Kinds.Exception};

            _sut.PaintResult(shotResult);
        }
    }
}