using Battleships.Logic.Coordinates;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.Tests.Features
{
    public class CoordinateTests
    {
        [Theory]
        [InlineData('A', "1", 0, 0)]
        [InlineData('A', "2", 0, 1)]
        [InlineData('B', "5", 1, 4)]
        [InlineData('E', "5", 4, 4)]
        [InlineData('K', "6", 10, 5)]
        [InlineData('H', "5", 7, 4)]
        [InlineData('Z', "6", 25, 5)]
        public void Construct_SensibleCoordinate_ConstructRightObject(char columnChar, string rowString, int expectedColumn, int expectedRow)
        {
            var result = new Coordinate(columnChar, rowString);

            result.Column.Should().Be(expectedColumn);
            result.Row.Should().Be(expectedRow);
            result.ColumnChar.Should().Be(columnChar);
            result.RowString.Should().Be(rowString);
        }

        [Theory]
        [InlineData(0, 0, 'A', "1")]
        [InlineData(0, 6, 'A', "7")]
        [InlineData(1, 4, 'B', "5")]
        [InlineData(4, 8, 'E', "9")]
        [InlineData(10, 4, 'K', "5")]
        [InlineData(7, 2, 'H', "3")]
        [InlineData(25, 2, 'Z', "3")]
        public void Construct_IntegerColumn_ConstructRightObject(int column, int row, char expectedColumnChar, string expectedRowString)
        {
            var result = new Coordinate(column, row);

            result.Column.Should().Be(column);
            result.Row.Should().Be(row);
            result.ColumnChar.Should().Be(expectedColumnChar);
            result.RowString.Should().Be(expectedRowString);
        }
    }
}