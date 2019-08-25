using System;
using System.ComponentModel.DataAnnotations;
using Battleships.Logic.Coordinates;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests.Coordinates
{
    public class CoordinateParserTests
    {
        public CoordinateParserTests()
        {
            _coordinateValidatorMock = new Mock<ICoordinateValidator>(MockBehavior.Strict);
            _sut = new CoordinateParser(_coordinateValidatorMock.Object);
        }

        private readonly Mock<ICoordinateValidator> _coordinateValidatorMock;
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
            _coordinateValidatorMock.Setup(x => x.Validate(coordinatesFromClient))
                .Returns(ValidationResult.Success);

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
    }
}