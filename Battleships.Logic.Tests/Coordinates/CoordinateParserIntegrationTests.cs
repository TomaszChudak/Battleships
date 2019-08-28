using System;
using System.ComponentModel.DataAnnotations;
using Battleships.Logic.Coordinates;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests.Coordinates
{
    public class CoordinateParserIntegrationTests
    {
        public CoordinateParserIntegrationTests()
        {
            var coordinateValidator = new CoordinateValidator();
            _sut = new CoordinateParser(coordinateValidator);
        }

        private readonly ICoordinateParser _sut;

        [Theory]
        [InlineData("A1", 'A', "1")]
        [InlineData(" A 1 ", 'A', "1")]
        [InlineData("A10", 'A', "10")]
        [InlineData("  B5  ", 'B', "5")]
        [InlineData(" E6 ", 'E', "6")]
        [InlineData("K8", 'K', "8")]
        [InlineData("H99", 'H', "99")]
        [InlineData("Z100", 'Z', "100")]
        public void Parse_SensibleCoordinate_ReturnRightResult(string coordinatesFromClient, char expectedColumn, string expectedRow)
        {
            var result = _sut.Parse(coordinatesFromClient);

            result.ColumnChar.Should().Be(expectedColumn);
            result.RowString.Should().Be(expectedRow);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("A")]
        [InlineData("1A")]
        [InlineData("AA")]
        [InlineData("C-10")]
        [InlineData("11")]
        [InlineData("battleship")]
        [InlineData("@3")]
        [InlineData("&8")]
        public void Parse_WrongCoordinate_ThrowException(string coordinatesFromClient)
        {
            Action act = () => _sut.Parse(coordinatesFromClient);

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Parse_NullCoordinate_ThrowException()
        {
            Action act = () => _sut.Parse(null);

            act.Should().Throw<Exception>();
        }
        
        [Theory]
        [InlineData("A1", 'A', "1")]
        [InlineData(" A 1 ", 'A', "1")]
        [InlineData("A10", 'A', "10")]
        [InlineData("  B5  ", 'B', "5")]
        [InlineData(" E6 ", 'E', "6")]
        [InlineData("K8", 'K', "8")]
        [InlineData("H99", 'H', "99")]
        [InlineData("Z100", 'Z', "100")]
        public void TryParse_SensibleCoordinateWith_ReturnRightResult(string coordinatesFromClient, char expectedColumn, string expectedRow)
        {
            var result = _sut.TryParse(coordinatesFromClient, out var coordinate);

            result.Should().Be(ValidationResult.Success);
            coordinate.Should().Be(new Coordinate(expectedColumn, expectedRow));
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("A")]
        [InlineData("1A")]
        [InlineData("AA")]
        [InlineData("C-10")]
        [InlineData("11")]
        [InlineData("battleship")]
        [InlineData("@3")]
        [InlineData("&8")]
        public void TryParse_WrongCoordinate_ReturnFalse(string coordinatesFromClient)
        {
            var result = _sut.TryParse(coordinatesFromClient, out var coordinate);

            result.Should().NotBe(ValidationResult.Success);
            result.ErrorMessage.Should().Be("Wrong coordinate. Expected coordinate are one letter and number (from 1 to 999).");
        }

        public void TryParse_NullCoordinate_ReturnFalse()
        {
            var result = _sut.TryParse(null, out var coordinate);

            result.Should().NotBe(ValidationResult.Success);
            result.ErrorMessage.Should().Be("Wrong coordinate. Expected coordinate are one letter and number (from 1 to 999).");
        }
    }
}