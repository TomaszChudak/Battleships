using System;
using System.ComponentModel.DataAnnotations;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Grid;
using Battleships.Logic.Public;
using Battleships.Logic.Settings;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class GameLogicTests
    {
        public GameLogicTests()
        {
            _settingsCheckerMock = new Mock<ISettingsChecker>(MockBehavior.Strict);
            _gridBuilderMock = new Mock<IGridBuilder>(MockBehavior.Strict);
            _coordinateParserMock = new Mock<ICoordinateParser>(MockBehavior.Strict);
            _sut = new GameLogic(_settingsCheckerMock.Object, _gridBuilderMock.Object, _coordinateParserMock.Object);
        }

        private readonly Mock<ISettingsChecker> _settingsCheckerMock;
        private readonly Mock<IGridBuilder> _gridBuilderMock;
        private readonly Mock<ICoordinateParser> _coordinateParserMock;
        private readonly IGameLogic _sut;

        [Fact]
        public void MakeNewMove_RightCoordinates_ExampleWaterAnswer()
        {
            _settingsCheckerMock.Setup(x => x.Check())
                .Returns(ValidationResult.Success);
            var coordinate = new Coordinate('B', "4");
            _coordinateParserMock.Setup(x => x.TryParse("B4", out coordinate))
                .Returns(ValidationResult.Success);
            var grid = new Logic.Grid.Grid();
            grid.Build(10, 8);
            _gridBuilderMock.Setup(x => x.Build())
                .Returns(grid);

            _sut.StartNewGame();

            var result = _sut.MakeNewMove("B4");

            result.Success.Should().BeTrue();
            result.ErrorDescription.Should().BeNull();
            result.Content.Coordinate.Column.Should().Be(1);
            result.Content.Coordinate.Row.Should().Be(3);
            result.Content.Description.Should().Be("Water");
        }

        [Fact]
        public void MakeNewMove_WrongCoordinates_ResultWithWrongCoordinatesDesc()
        {
            var coordinate = (Coordinate) null;
            _coordinateParserMock.Setup(x => x.TryParse("AA", out coordinate))
                .Returns(new ValidationResult("Expected coordinate are one letter and number (from 1 to 999)."));

            var result = _sut.MakeNewMove("AA");

            result.Success.Should().BeTrue();
            result.ErrorDescription.Should().BeNull();
            result.Content.Kind.Should().Be(ShotResult.Kinds.WrongCoordinates);
            result.Content.Description.Should().Be("Expected coordinate are one letter and number (from 1 to 999).");
        }

        [Fact]
        public void StartNewGame_EvertyhingOk_RightGridSize()
        {
            _settingsCheckerMock.Setup(x => x.Check())
                .Returns(ValidationResult.Success);
            var grid = new Logic.Grid.Grid();
            grid.Build(10, 8);
            _gridBuilderMock.Setup(x => x.Build())
                .Returns(grid);

            var result = _sut.StartNewGame();

            result.Success.Should().BeTrue();
            result.ErrorDescription.Should().BeNull();
            result.Content.ColumnCount.Should().Be(10);
            result.Content.RowCount.Should().Be(8);
        }

        [Fact]
        public void StartNewGame_IssueWithConfiguration_ErrorResponse()
        {
            _settingsCheckerMock.Setup(x => x.Check())
                .Throws(new ApplicationException("Some issue with settings."));

            var result = _sut.StartNewGame();

            result.Success.Should().BeFalse();
            result.Content.Should().BeNull();
            result.ErrorDescription.Should().Be("Some issue with settings.");
        }
    }
}