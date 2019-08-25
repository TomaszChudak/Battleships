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
            var coordinate = new Coordinate('B', "4");
            _coordinateParserMock.Setup(x => x.TryParse("B4", out coordinate))
                .Returns(ValidationResult.Success);
            _settingsCheckerMock.Setup(x => x.Check());
            var grid = new Logic.Grid.Grid();
            grid.Build(10, 8);
            _gridBuilderMock.Setup(x => x.Build())
                .Returns(grid);

            _sut.StartNewGame();

            var result = _sut.MakeNewMove("B4");

            result.Coordinate.Column.Should().Be(1);
            result.Coordinate.Row.Should().Be(3);
            result.Description.Should().Be("Water");

            _settingsCheckerMock.Verify(x => x.Check(), Times.Once);
            _gridBuilderMock.Verify(x => x.Build(), Times.Once);
            _coordinateParserMock.Verify(x => x.TryParse(It.IsAny<string>(), out coordinate), Times.Once);
            _coordinateParserMock.Verify(x => x.TryParse("B4", out coordinate), Times.Once);
            _coordinateParserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Never);
            _coordinateParserMock.Verify(x => x.Parse("B4"), Times.Never);
        }

        [Fact]
        public void MakeNewMove_WrongCoordinates_ResultWithWrongCoordinatesDesc()
        {
            var coordinate = (Coordinate) null;
            _coordinateParserMock.Setup(x => x.TryParse("AA", out coordinate))
                .Returns(new ValidationResult("Expected coordinate are one letter and number (from 1 to 999)."));

            var result = _sut.MakeNewMove("AA");

            result.Kind.Should().Be(ShotResult.Kinds.WrongCoordinates);
            result.Description.Should().Be("Expected coordinate are one letter and number (from 1 to 999).");

            _settingsCheckerMock.Verify(x => x.Check(), Times.Never);
            _gridBuilderMock.Verify(x => x.Build(), Times.Never);
            _coordinateParserMock.Verify(x => x.TryParse(It.IsAny<string>(), out coordinate), Times.Once);
            _coordinateParserMock.Verify(x => x.TryParse("AA", out coordinate), Times.Once);
            _coordinateParserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Never);
            _coordinateParserMock.Verify(x => x.Parse("AA"), Times.Never);
        }

        [Fact]
        public void StartNewGame_EvertyhingOk_RightGridSize()
        {
            _settingsCheckerMock.Setup(x => x.Check());
            var grid = new Logic.Grid.Grid();
            grid.Build(10, 8);
            _gridBuilderMock.Setup(x => x.Build())
                .Returns(grid);

            var result = _sut.StartNewGame();

            result.ColumnCount.Should().Be(10);
            result.RowCount.Should().Be(8);

            _settingsCheckerMock.Verify(x => x.Check(), Times.Once);
            _gridBuilderMock.Verify(x => x.Build(), Times.Once);
            _coordinateParserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void StartNewGame_IssueWithConfiguration_ExceptionWillBeThrown()
        {
            _settingsCheckerMock.Setup(x => x.Check())
                .Throws(new ApplicationException("Some issue with settings."));

            Action act = () => _sut.StartNewGame();

            act.Should().Throw<ApplicationException>().WithMessage("Some issue with settings.");

            _settingsCheckerMock.Verify(x => x.Check(), Times.Once);
            _gridBuilderMock.Verify(x => x.Build(), Times.Never);
            _coordinateParserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Never);
        }
    }
}