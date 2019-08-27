using System;
using System.ComponentModel.DataAnnotations;

namespace Battleships.Logic.Coordinates
{
    internal interface ICoordinateParser
    {
        ValidationResult TryParse(string coordinate, out Coordinate result);
        Coordinate Parse(string coordinate);
    }

    internal class CoordinateParser : ICoordinateParser
    {
        private readonly ICoordinateValidator _coordinateValidator;

        public CoordinateParser(ICoordinateValidator coordinateValidator)
        {
            _coordinateValidator = coordinateValidator;
        }

        public ValidationResult TryParse(string coordinate, out Coordinate result)
        {
            result = null;
            var validationResult = _coordinateValidator.Validate(coordinate);

            if (validationResult != ValidationResult.Success)
                return validationResult;

            result = Parse(coordinate);

            return ValidationResult.Success;
        }

        public virtual Coordinate Parse(string coordinate)
        {
            var validationResult = _coordinateValidator.Validate(coordinate);
            if (validationResult != ValidationResult.Success)
                throw new ArgumentException(validationResult.ErrorMessage);

            var coordinateTrimmed = coordinate
                .Replace(" ", "")
                .Replace("\t", "")
                .ToUpper();

            var column = coordinateTrimmed[0];
            var row = int.Parse(coordinateTrimmed.Substring(1));

            return new Coordinate(column, row.ToString());
        }
    }
}