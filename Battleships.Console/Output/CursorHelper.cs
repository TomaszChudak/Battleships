using Battleships.Logic.Features;
using Battleships.Logic.Public;

namespace Battleships.Console.Output
{
    internal interface ICursorHelper
    {
        void SetGridSize(GridSize gridSize);
        int TextResultTop { get; }
        int GetTopForCoordinate(Coordinate coordinate);
        int GetLeftForCoordinate(Coordinate coordinate);
    }

    internal class CursorHelper : ICursorHelper
    {
        private GridSize _gridSize;

        public void SetGridSize(GridSize gridSize)
        {
            _gridSize = gridSize;
        }

        public int TextResultTop => _gridSize.RowCount * GridSettings.CellHeight + GridSettings.CellHeight;

        public int GetTopForCoordinate(Coordinate coordinate)
        {
            return coordinate.Row * GridSettings.CellHeight + GridSettings.CellHeight;
        }

        public int GetLeftForCoordinate(Coordinate coordinate)
        {
            return coordinate.Column * GridSettings.CellWidth + GridSettings.CellWidth + 1;
        }
    }
}