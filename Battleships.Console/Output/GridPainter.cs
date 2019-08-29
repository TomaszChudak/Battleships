using Battleships.Logic.Public;

namespace Battleships.Console.Output
{
    internal interface IGridPainter
    {
        void PaintNewGrid(GridSize gridSize);
    }

    internal class GridPainter : IGridPainter
    {
        private readonly IOutputWriter _outputWriter;

        public GridPainter(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void PaintNewGrid(GridSize gridSize)
        {
            _outputWriter.Clear();
            _outputWriter.SetCursorPosition(0, 0);

            PaintColumnHeaders(gridSize.ColumnCount);
            PaintRowSeparator(gridSize.ColumnCount);

            for (var r = 0; r < gridSize.RowCount; r++)
            {
                PaintRowHeader(r + 1);
                PaintRowCells(gridSize.ColumnCount);
                PaintRowSeparator(gridSize.ColumnCount);
            }
        }

        private void PaintColumnHeaders(int columnCount)
        {
            _outputWriter.Write("   |");
            for (var c = 0; c < columnCount; c++)
                _outputWriter.Write($" {(char) (c + 'A')} |");
            _outputWriter.WriteNewLine();
        }

        private void PaintRowSeparator(int columnCount)
        {
            for (var c = 0; c <= columnCount; c++)
                _outputWriter.Write("----");
            _outputWriter.WriteNewLine();
        }

        private void PaintRowHeader(int rowNumber)
        {
            if (rowNumber < 10)
                _outputWriter.Write(" ");
            _outputWriter.Write(rowNumber.ToString());
            _outputWriter.Write(" ");
        }

        private void PaintRowCells(int columnCount)
        {
            _outputWriter.Write("|");
            for (var c = 0; c < columnCount; c++)
                _outputWriter.Write("   |");
            _outputWriter.WriteNewLine();
        }
    }
}