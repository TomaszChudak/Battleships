using Battleships.Logic.Public;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.TestsVerify.Public
{
    public class GridSizeTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 10)]
        [InlineData(5, 5)]
        public void Construct_ExampleSize_ConstructRightObject(int columnCount, int rowCount)
        {
            var result = new GridSize(columnCount, rowCount);

            result.ColumnCount.Should().Be(columnCount);
            result.RowCount.Should().Be(rowCount);
        }
    }
}
