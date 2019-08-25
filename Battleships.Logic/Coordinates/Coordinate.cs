namespace Battleships.Logic.Coordinates
{
    public class Coordinate
    {
        public readonly int Column;
        internal readonly char ColumnChar;
        public readonly int Row;
        internal readonly string RowString;

        public Coordinate(char column, string row)
        {
            Column = ChangeColumnCharToInteger(column);
            Row = ChangeRowStringToInteger(row);
            ColumnChar = column;
            RowString = row;
        }

        public Coordinate(int column, int row)
        {
            ColumnChar = ChangeIntegerToColumnChar(column);
            RowString = ChangeIntegerToRowString(row);
            Column = column;
            Row = row;
        }

        private string ChangeIntegerToRowString(int row)
        {
            return (row + 1).ToString();
        }

        private int ChangeRowStringToInteger(string row)
        {
            return int.Parse(row) - 1;
        }

        private char ChangeIntegerToColumnChar(int column)
        {
            return (char) ('A' + column);
        }

        private int ChangeColumnCharToInteger(char column)
        {
            return column - 'A';
        }

        private bool Equals(Coordinate other)
        {
            return other != null
                   && Row == other.Row
                   && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row.GetHashCode() * 397) ^ Column.GetHashCode();
            }
        }

        public override string ToString()
        {
            return ColumnChar + RowString;
        }
    }
}