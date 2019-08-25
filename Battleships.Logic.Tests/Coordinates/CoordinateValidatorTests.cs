using System.ComponentModel.DataAnnotations;
using Battleships.Logic.Coordinates;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.Tests.Coordinates
{
    public class CoordinateValidatorTests
    {
        public CoordinateValidatorTests()
        {
            _sut = new CoordinateValidator();
        }

        private readonly ICoordinateValidator _sut;

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
            var result = _sut.Validate(coordinatesFromClient);

            result.Should().Be(ValidationResult.Success);
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
            var result = _sut.Validate(coordinatesFromClient);

            result.Should().NotBe(ValidationResult.Success);
            result.ErrorMessage.Should().Be("Wrong coordinate. Expected coordinate are one letter and number (from 1 to 999).");
        }

        [Fact]
        public void Parse_NullCoordinate_ThrowException()
        {
            var result = _sut.Validate(null);

            result.Should().NotBe(ValidationResult.Success);
            result.ErrorMessage.Should().Be("Coordinate can't be null.");
        }
    }
}