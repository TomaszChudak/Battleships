using Battleships.Logic.Features;
using System;
using System.Text.RegularExpressions;

namespace Battleships.Logic.Helpers
{
    internal interface ICoordinateParser
    {
        Coordinate Parse(string coordinate);
    }

    internal class CoordinateParser : ICoordinateParser
    {
        public Coordinate Parse(string coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("Coordinate can't be null.");

            var coordinateTrimmed = coordinate
                .Replace(" ", "")
                .Replace("\t", "")
                .ToUpper();

            var match = Regex.Match(coordinateTrimmed, "[A-Z][0-9]{1,3}");

            if (!match.Success)
                throw new ArgumentException("Wrong coordinate. Expected coordinate are one letter and number (from 1 to 999).");

            var column = coordinateTrimmed[0];

            if(!int.TryParse(coordinateTrimmed.Substring(1), out var row))
                throw new ArgumentException("Wrong coordinate. Expected coordinate should end with number (from 1 to 999).");

            return new Coordinate(column, row.ToString());
        }
    }
}
