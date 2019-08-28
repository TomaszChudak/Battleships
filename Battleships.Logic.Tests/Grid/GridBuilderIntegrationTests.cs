using System;
using System.Collections.Generic;
using Battleships.Logic.Grid;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests.Grid
{
    public class GridBuilderIntegrationTests
    {
        public GridBuilderIntegrationTests()
        {
            _configMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);
            var randomWrapper = new RandomWrapper();
            var shipFactory = new ShipFactory(_configMock.Object, randomWrapper);
            var grid = new Logic.Grid.Grid();
            _sut = new GridBuilder(_configMock.Object, shipFactory, grid);
        }

        private readonly Mock<IOptions<AppSettings>> _configMock;
        private readonly IGridBuilder _sut;

        [Fact]
        public void Build_CantFindPlaceForAllShips_ExceptionIsThrown()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 5},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "BigShip", Size = 5, Count = 10}}
                });

            Action act = () => _sut.Build();

            act.Should().Throw<ApplicationException>().WithMessage("Can't find any place for new ship.");
        }

        [Fact]
        public void Build_FiveShipCount_FiveShipOnGrid()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 5},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 5}}
                });

            var result = _sut.Build();

            result.Size.ColumnCount.Should().Be(10);
            result.Size.RowCount.Should().Be(5);
            result.Ships.Should().HaveCount(5);
            result.Ships.Should().Contain(x => x.Size == 5 && x.Name == "Battleship");
        }

        [Fact]
        public void Build_NoShipCount_EmptyGrid()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 5},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5}}
                });

            var result = _sut.Build();

            result.Size.ColumnCount.Should().Be(10);
            result.Size.RowCount.Should().Be(5);
            result.Ships.Should().BeEmpty();
        }

        [Fact]
        public void Build_OneShipCount_OneShipOnGrid()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 5},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 1}}
                });

            var result = _sut.Build();

            result.Size.ColumnCount.Should().Be(10);
            result.Size.RowCount.Should().Be(5);
            result.Ships.Should().HaveCount(1);
            result.Ships.Should().Contain(x => x.Size == 5 && x.Name == "Battleship");
        }
    }
}