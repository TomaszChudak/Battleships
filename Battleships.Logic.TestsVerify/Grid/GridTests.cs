using System;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Grid;
using Battleships.Logic.Public;
using Battleships.Logic.Ships;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.TestsVerify.Grid
{
    public class GridTests
    {
        public GridTests()
        {
            _sut = new Logic.Grid.Grid();
        }

        private readonly IGrid _sut;

        [Fact]
        public void Build_RightData_SizeAndShipsAreInitialized()
        {
            _sut.Build(10, 5);

            _sut.Size.ColumnCount.Should().Be(10);
            _sut.Size.RowCount.Should().Be(5);
            _sut.Ships.Should().NotBeNull();
            _sut.Ships.Should().BeEmpty();
        }

        [Fact]
        public void Shot_LastShipIsSink_InfoAboutSink()
        {
            _sut.Build(10, 5);
            var shipToSink = new Ship("Destroyer", new Coordinate(0, 3), true, 4);
            _sut.TryPlaceShip(shipToSink);

            _sut.Shot(new Coordinate(1, 3));
            _sut.Shot(new Coordinate(3, 3));
            _sut.Shot(new Coordinate(2, 3));
            var result = _sut.Shot(new Coordinate(0, 3));

            result.Coordinate.Should().Be(new Coordinate(0, 3));
            result.Description.Should().Be("Destroyer has been sink. You have WIN !!!");
            result.Kind.Should().Be(ShotResult.Kinds.GameEnd);
            result.SinkShip.Should().BeEquivalentTo(shipToSink.Coordinates);
        }

        [Fact]
        public void Shot_ShipCoordinatesAreEnteredAgain_InfoAboutHit()
        {
            _sut.Build(10, 5);
            _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 3), true, 4));
            _sut.Shot(new Coordinate(2, 3));

            var result = _sut.Shot(new Coordinate(2, 3));

            result.Coordinate.Should().Be(new Coordinate(2, 3));
            result.Description.Should().Be("You have entered the same coordinates again");
            result.Kind.Should().Be(ShotResult.Kinds.TheSameCoordinatesAgain);
            result.SinkShip.Should().BeNull();
        }

        [Fact]
        public void Shot_ShipIsHit_InfoAboutHit()
        {
            _sut.Build(10, 5);
            _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 3), true, 4));

            var result = _sut.Shot(new Coordinate(2, 3));

            result.Coordinate.Should().Be(new Coordinate(2, 3));
            result.Description.Should().Be("Destroyer has been hit.");
            result.Kind.Should().Be(ShotResult.Kinds.Hit);
            result.SinkShip.Should().BeNull();
        }

        [Fact]
        public void Shot_ShipIsSink_InfoAboutSink()
        {
            _sut.Build(10, 5);
            var shipToSink = new Ship("Destroyer", new Coordinate(0, 3), true, 4);
            _sut.TryPlaceShip(shipToSink);
            _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 1), true, 4));

            _sut.Shot(new Coordinate(1, 3));
            _sut.Shot(new Coordinate(3, 3));
            _sut.Shot(new Coordinate(2, 3));
            var result = _sut.Shot(new Coordinate(0, 3));

            result.Coordinate.Should().Be(new Coordinate(0, 3));
            result.Description.Should().Be("Destroyer has been sink.");
            result.Kind.Should().Be(ShotResult.Kinds.Sink);
            result.SinkShip.Should().BeEquivalentTo(shipToSink.Coordinates);
        }

        [Fact]
        public void Shot_Water_InfoAboutWater()
        {
            _sut.Build(10, 5);

            var result = _sut.Shot(new Coordinate(2, 3));

            result.Coordinate.Should().Be(new Coordinate(2, 3));
            result.Description.Should().Be("Water");
            result.Kind.Should().Be(ShotResult.Kinds.Water);
            result.SinkShip.Should().BeNull();
        }

        [Fact]
        public void Shot_WaterCoordinatesAreEnteredAgain_InfoAboutHit()
        {
            _sut.Build(10, 5);
            _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 3), true, 4));
            _sut.Shot(new Coordinate(5, 3));

            var result = _sut.Shot(new Coordinate(5, 3));

            result.Coordinate.Should().Be(new Coordinate(5, 3));
            result.Description.Should().Be("You have entered the same coordinates again");
            result.Kind.Should().Be(ShotResult.Kinds.TheSameCoordinatesAgain);
            result.SinkShip.Should().BeNull();
        }

        [Fact]
        public void Shot_WrongCoordinates_ThrowException()
        {
            _sut.Build(10, 5);

            Action act = () => _sut.Shot(new Coordinate(10, 10));

            act.Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void TryPlaceShip_FirstShip_Success()
        {
            _sut.Build(10, 5);
            var result = _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 3), true, 4));

            result.Should().BeTrue();
        }

        [Fact]
        public void TryPlaceShip_SecondShipInFreePlace_Success()
        {
            _sut.Build(10, 5);
            _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 2), true, 4));

            var result = _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 4), true, 4));

            result.Should().BeTrue();
        }

        [Fact]
        public void TryPlaceShip_SecondShipInNextToIt_Fail()
        {
            _sut.Build(10, 5);
            _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 3), true, 4));

            var result = _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 4), false, 4));

            result.Should().BeFalse();
        }

        [Fact]
        public void TryPlaceShip_SecondShipOverlapping_Fail()
        {
            _sut.Build(10, 5);
            _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(0, 3), true, 4));

            var result = _sut.TryPlaceShip(new Ship("Destroyer", new Coordinate(3, 0), false, 4));

            result.Should().BeFalse();
        }
    }
}