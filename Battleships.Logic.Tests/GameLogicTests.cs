using System;
using Battleships.Logic.Features;
using Battleships.Logic.Grid;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class GameLogicTests
    {
        private readonly Mock<ISettingsChecker> _settingsCheckerMock;
        private readonly Mock<IGridBuilder> _gridBuilderMock;
        private readonly Mock<ICoordinateParser> _coordinateParserMock;
        private readonly IGameLogic _sut;

        public GameLogicTests()
        {
            _settingsCheckerMock = new Mock<ISettingsChecker>(MockBehavior.Strict);
            _gridBuilderMock = new Mock<IGridBuilder>(MockBehavior.Strict);
            _coordinateParserMock = new Mock<ICoordinateParser>(MockBehavior.Strict);
            _sut = new GameLogic(_settingsCheckerMock.Object, _gridBuilderMock.Object, _coordinateParserMock.Object);
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
        public void MakeNewMove_WrongCoordinates_ExceptionIsThrown()
        {
            _coordinateParserMock.Setup(x => x.Parse("AA"))
                .Throws(new ArgumentException("Expected coordinate are one letter and number (from 1 to 999)."));

            Action act = () => _sut.MakeNewMove("AA");

            act.Should().Throw<ArgumentException>().WithMessage("Expected coordinate are one letter and number (from 1 to 999).");

            _settingsCheckerMock.Verify(x => x.Check(), Times.Never);
            _gridBuilderMock.Verify(x => x.Build(), Times.Never);
            _coordinateParserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Once);
            _coordinateParserMock.Verify(x => x.Parse("AA"), Times.Once);
        }

        [Fact]
        public void MakeNewMove_RightCoordinates_ExampleWaterAnswer()
        {
            _coordinateParserMock.Setup(x => x.Parse("B4"))
                .Returns(new Coordinate('B', "4"));
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
            _coordinateParserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Once);
            _coordinateParserMock.Verify(x => x.Parse("B4"), Times.Once);
        }
    }
}
