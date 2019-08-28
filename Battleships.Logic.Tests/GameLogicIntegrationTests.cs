using System.Collections.Generic;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Grid;
using Battleships.Logic.Helpers;
using Battleships.Logic.Public;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests
{
    public class GameLogicIntegrationTests
    {
        public GameLogicIntegrationTests()
        {
            _configMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);
            var settingsChecker = new SettingsChecker(_configMock.Object);
            var randomWrapper = new RandomWrapper();
            var shipFactory = new ShipFactory(_configMock.Object, randomWrapper);
            var grid = new Logic.Grid.Grid();
            var gridBuilder = new GridBuilder(_configMock.Object, shipFactory, grid);
            var coordinateValidator = new CoordinateValidator();
            var coordinateParser = new CoordinateParser(coordinateValidator);
            _sut = new GameLogic(settingsChecker, gridBuilder, coordinateParser);
        }

        private readonly Mock<IOptions<AppSettings>> _configMock;
        private readonly IGameLogic _sut;

        [Fact]
        public void MakeNewMove_RightCoordinates_ExampleWaterAnswer()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 7},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 2}}
                });

            _sut.StartNewGame();

            var result = _sut.MakeNewMove("B4");

            result.Success.Should().BeTrue();
            result.ErrorDescription.Should().BeNull();
            result.Content.Coordinate.Column.Should().Be(1);
            result.Content.Coordinate.Row.Should().Be(3);
            result.Content.Description.Should().BeOneOf(new List<string> {"Water", "Battleship has been hit."});
        }

        [Fact]
        public void MakeNewMove_WrongCoordinates_ResultWithWrongCoordinatesDesc()
        {
            var result = _sut.MakeNewMove("AA");

            result.Success.Should().BeTrue();
            result.ErrorDescription.Should().BeNull();
            result.Content.Kind.Should().Be(ShotResult.Kinds.WrongCoordinates);
            result.Content.Description.Should().Be("Wrong coordinate. Expected coordinate are one letter and number (from 1 to 999).");
        }

        [Fact]
        public void StartNewGame_EvertyhingOk_RightGridSize()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 8},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 2}}
                });

            var result = _sut.StartNewGame();

            result.Success.Should().BeTrue();
            result.ErrorDescription.Should().BeNull();
            result.Content.ColumnCount.Should().Be(10);
            result.Content.RowCount.Should().Be(8);
        }

        [Fact]
        public void StartNewGame_IssueWithConfiguration_ErrorResponse()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 8}
                });

            var result = _sut.StartNewGame();

            result.Success.Should().BeFalse();
            result.Content.Should().BeNull();
            result.ErrorDescription.Should().Be($"An issue with settings in {SettingsRules.SettingFileName} file has been found. Lack of ShipTypes setting.");
        }
    }
}