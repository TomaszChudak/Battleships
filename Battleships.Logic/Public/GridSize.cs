namespace Battleships.Logic.Public
{
    public class GridSize
    {
        public int ColumnCount { get; }
        public int RowCount { get; }

        public GridSize(int columnCount, int rowCount)
        {
            ColumnCount = columnCount;
            RowCount = rowCount;
        }
    }
}