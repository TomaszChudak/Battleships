using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Public;
using Battleships.Logic.Ships;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.Tests.Ships
{
    public class ShipTests
    {
        [Theory]
        [InlineData("Battleship", 0, 0, 5)]
        [InlineData("Battleship", 2, 6, 5)]
        [InlineData("Battleship", 4, 4, 5)]
        [InlineData("Destroyer", 6, 6, 4)]
        public void Construct_ExampleHorizontalShip_RightObject(string name, int column, int row, int size)
        {
            var result = new Ship(name, new Coordinate(column, row), true, size);

            result.Size.Should().Be(size);
            result.Name.Should().Be(name);
            result.Coordinates.Should().HaveCount(size);
            result.Coordinates.Should().OnlyContain(r => r.Row == row);
            for (var c = 0; c < size; c++)
                result.Coordinates[c].Column.Should().Be(column + c);
            result.MarginCoordinates.Should().HaveCount(size * 2 + 6);
            result.MarginCoordinates.Should().OnlyContain(r => r.Row == row - 1 || r.Row == row || r.Row == row + 1);
            for (var c = -1; c <= size; c++)
            {
                // top margin (with corners)
                result.MarginCoordinates[c + 1].Column.Should().Be(column + c);
                result.MarginCoordinates[c + 1].Row.Should().Be(row - 1);
                // bottom margin (with corners)
                result.MarginCoordinates[size + 4 + c + 1].Column.Should().Be(column + c);
                result.MarginCoordinates[size + 4 + c + 1].Row.Should().Be(row + 1);
            }

            // left margin (one square)
            result.MarginCoordinates[size + 2].Column.Should().Be(column - 1);
            result.MarginCoordinates[size + 2].Row.Should().Be(row);
            // right margin (one square)
            result.MarginCoordinates[size + 3].Column.Should().Be(column + size);
            result.MarginCoordinates[size + 3].Row.Should().Be(row);

            result.Coordinates.Intersect(result.MarginCoordinates).Should().BeEmpty();
        }

        [Theory]
        [InlineData("Battleship", 0, 0, 5)]
        [InlineData("Battleship", 2, 6, 5)]
        [InlineData("Battleship", 4, 4, 5)]
        [InlineData("Destroyer", 6, 6, 4)]
        public void Construct_ExampleVerticalShip_RightObject(string name, int column, int row, int size)
        {
            var result = new Ship(name, new Coordinate(column, row), false, size);

            result.Size.Should().Be(size);
            result.Name.Should().Be(name);
            result.Coordinates.Should().HaveCount(size);
            result.Coordinates.Should().OnlyContain(x => x.Column == column);
            for (var r = 0; r < size; r++)
                result.Coordinates[r].Row.Should().Be(row + r);
            result.MarginCoordinates.Should().HaveCount(size * 2 + 6);
            result.MarginCoordinates.Should().OnlyContain(r => r.Column == column - 1 || r.Column == column || r.Column == column + 1);
            for (var r = 0; r < size; r++)
            {
                // left margin (without corners)
                result.MarginCoordinates[2 * r + 3].Column.Should().Be(column - 1);
                result.MarginCoordinates[2 * r + 3].Row.Should().Be(row + r);
                // right margin (without corners)
                result.MarginCoordinates[2 * r + 4].Column.Should().Be(column + 1);
                result.MarginCoordinates[2 * r + 4].Row.Should().Be(row + r);
            }

            for (var c = -1; c <= 1; c++)
            {
                // top margin (with corners)
                result.MarginCoordinates[c + 1].Column.Should().Be(column + c);
                result.MarginCoordinates[c + 1].Row.Should().Be(row - 1);
                // bottom margin (with corners)
                result.MarginCoordinates[2 * size + 4 + c].Column.Should().Be(column + c);
                result.MarginCoordinates[2 * size + 4 + c].Row.Should().Be(size + row);
            }

            result.Coordinates.Intersect(result.MarginCoordinates).Should().BeEmpty();
        }

        [Fact]
        public void Shot_FirstShot_HitInfo()
        {
            var ship = new Ship("Destroyer", new Coordinate(5, 5), true, 4);

            var result = ship.Shot(new Coordinate(6, 5));

            result.Coordinate.Should().Be(new Coordinate(6, 5));
            result.Description.Should().Be("Destroyer has been hit.");
            result.Kind.Should().Be(ShotResult.Kinds.Hit);
            result.SinkShip.Should().BeNull();
        }

        [Fact]
        public void Shot_LastShot_SinkInfo()
        {
            var ship = new Ship("Destroyer", new Coordinate(5, 5), true, 4);

            ship.Shot(new Coordinate(5, 5));
            ship.Shot(new Coordinate(6, 5));
            ship.Shot(new Coordinate(7, 5));
            var result = ship.Shot(new Coordinate(8, 5));

            result.Coordinate.Should().Be(new Coordinate(8, 5));
            result.Description.Should().Be("Destroyer has been sink.");
            result.Kind.Should().Be(ShotResult.Kinds.Sink);
            result.SinkShip.Should().BeSameAs(ship.Coordinates);
        }

        [Fact]
        public void Shot_CoordinateDontBelongToShip_ShouldThrowException()
        {
            var ship = new Ship("Destroyer", new Coordinate(5, 5), true, 4);

            Action act = () => ship.Shot(new Coordinate(3, 3));

            act.Should().Throw<KeyNotFoundException>().WithMessage("Wrong coordinates.");
        }
    }
}