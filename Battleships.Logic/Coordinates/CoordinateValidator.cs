using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Battleships.Logic.Coordinates
{
    internal interface ICoordinateValidator
    {
        ValidationResult Validate(string coordinate);
    }

    internal class CoordinateValidator : ICoordinateValidator
    {
        public ValidationResult Validate(string coordinate)
        {
            if (coordinate == null)
                return new ValidationResult("Coordinate can't be null.");

            var coordinateTrimmed = coordinate
                .Replace(" ", "")
                .Replace("\t", "")
                .ToUpper();

            var match = Regex.Match(coordinateTrimmed, "[A-Z][0-9]{1,3}");

            if (!match.Success)
                return new ValidationResult("Wrong coordinate. Expected coordinate are one letter and number (from 1 to 999).");

            if (!int.TryParse(coordinateTrimmed.Substring(1), out var row))
                return new ValidationResult("Wrong coordinate. Expected coordinate should end with number (from 1 to 999).");

            return ValidationResult.Success;
        }
    }
}