using Battleships.Console.Output;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.Tests.Output
{
    public class GridPainterTests
    {
        private readonly Mock<IOutputWriter> _outputWriterMock;
        private readonly IGridPainter _sut;

        public GridPainterTests()
        {
            _outputWriterMock = new Mock<IOutputWriter>(MockBehavior.Strict);
            _sut = new GridPainter(_outputWriterMock.Object);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(10, 5)]
        [InlineData(5, 8)]   
        public void PaintNewGrid_RightSize_GridIsPainted(int columnCount, int rowCount)
        {
            _outputWriterMock.Setup(x => x.Clear());
            _outputWriterMock.Setup(x => x.SetCursorPosition(0, 0));
            _outputWriterMock.Setup(x => x.Write(It.IsAny<string>()));
            _outputWriterMock.Setup(x => x.WriteNewLine());
            var gridSize = new GridSize(columnCount, rowCount);

            _sut.PaintNewGrid(gridSize);
        }
    }
}
