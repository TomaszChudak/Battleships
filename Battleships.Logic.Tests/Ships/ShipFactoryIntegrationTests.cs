using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests.Ships
{
    public class ShipFactoryIntegrationTests
    {
        private readonly Mock<IOptions<AppSettings>> _configMock;
        private readonly IShipFactory _sut;

        public ShipFactoryIntegrationTests()
        {
            _configMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);
            var randomWrapper = new RandomWrapper();
            _sut = new ShipFactory(_configMock.Object, randomWrapper);
        }

        [Fact]
        public void BuildShip_WrongName_ThrowException()
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 1}},
                });
            Action act = () => _sut.BuildShip("Boat");

            act.Should().Throw<InvalidOperationException>("Sequence contains no matching element");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void BuildShip_SensibleCoordinate_ReturnShip(int size)
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = size, Count = 1}},
                });

            var result = _sut.BuildShip("Battleship");

            result.Size.Should().Be(size);
            result.Name.Should().Be("Battleship");
            result.Coordinates.Should().HaveCount(size);
        }

        public static IEnumerable<object[]> GetAllShipCounts =>
            Enumerable.Range(SettingsRules.MinimalShipCount, SettingsRules.MaximalShipCount - SettingsRules.MinimalShipCount)
                .Select(x => new[] {(object) x})
                .ToArray();

        [Theory]
        [MemberData(nameof(GetAllShipCounts))]
        public void BuildShip_AllShipCounts_ReturnShip(int size)
        {
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = size, Count = 1}},
                });

            var result = _sut.BuildShip("Battleship");

            result.Size.Should().Be(size);
            result.Name.Should().Be("Battleship");
            result.Coordinates.Should().HaveCount(size);
        }
    }
}
