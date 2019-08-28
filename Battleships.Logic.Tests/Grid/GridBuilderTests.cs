using System;
using System.Collections.Generic;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Grid;
using Battleships.Logic.Public;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests.Grid
{
    public class GridBuilderTests
    {
        public GridBuilderTests()
        {
            _configMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);
            _shipFactoryMock = new Mock<IShipFactory>(MockBehavior.Strict);
            _gridMock = new Mock<IGrid>(MockBehavior.Strict);
            _sut = new GridBuilder(_configMock.Object, _shipFactoryMock.Object, _gridMock.Object);
        }

        private readonly Mock<IOptions<AppSettings>> _configMock;
        private readonly Mock<IShipFactory> _shipFactoryMock;
        private readonly Mock<IGrid> _gridMock;
        private readonly IGridBuilder _sut;

        [Fact]
        public void Build_CantFindPlaceForAllShips_ExceptionIsThrown()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 5},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 5}}
                });
            _gridMock.Setup(x => x.Build(10, 5));
            var ship = new Ship("Battleship", new Coordinate(0, 0), true, 5);
            _shipFactoryMock.Setup(x => x.BuildShip("Battleship"))
                .Returns(ship);
            _gridMock.Setup(x => x.TryPlaceShip(ship))
                .Returns(false);

            Action act = () => _sut.Build();

            act.Should().Throw<ApplicationException>().WithMessage("Can't find any place for new ship.");
        }

        [Fact]
        public void Build_FiveShipCount_FiveShipOnGrid()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 8, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 5}}
                });
            _gridMock.Setup(x => x.Build(8, 10));
            var ships = new List<Ship>
            {
                new Ship("Battleship", new Coordinate(0, 0), true, 5), 
                new Ship("Battleship", new Coordinate(0, 2), true, 5), 
                new Ship("Battleship", new Coordinate(0, 4), true, 5),
                new Ship("Battleship", new Coordinate(0, 5), true, 5), 
                new Ship("Battleship", new Coordinate(0, 8), true, 5)
            };
            _shipFactoryMock.SetupSequence(x => x.BuildShip("Battleship"))
                .Returns(ships[0])
                .Returns(ships[1])
                .Returns(ships[2])
                .Returns(ships[3])
                .Returns(ships[4]);
            _gridMock.Setup(x => x.TryPlaceShip(It.IsAny<Ship>()))
                .Returns(true);
            _gridMock.Setup(x => x.Size)
                .Returns(new GridSize(8, 10));
            _gridMock.Setup(x => x.Ships)
                .Returns(ships);

            var result = _sut.Build();

            result.Size.ColumnCount.Should().Be(8);
            result.Size.RowCount.Should().Be(10);
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
            _gridMock.Setup(x => x.Build(10, 5));
            _gridMock.Setup(x => x.Size)
                .Returns(new GridSize(10, 5));
            _gridMock.Setup(x => x.Ships)
                .Returns(new List<Ship>());

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
            _gridMock.Setup(x => x.Build(10, 5));
            var ship = new Ship("Battleship", new Coordinate(0, 0), true, 5);
            _shipFactoryMock.Setup(x => x.BuildShip("Battleship"))
                .Returns(ship);
            _gridMock.Setup(x => x.TryPlaceShip(ship))
                .Returns(true);
            _gridMock.Setup(x => x.Size)
                .Returns(new GridSize(10, 5));
            _gridMock.Setup(x => x.Ships)
                .Returns(new List<Ship> {ship});

            var result = _sut.Build();

            result.Size.ColumnCount.Should().Be(10);
            result.Size.RowCount.Should().Be(5);
            result.Ships.Should().HaveCount(1);
            result.Ships.Should().Contain(x => x.Size == 5 && x.Name == "Battleship");
        }
    }
}