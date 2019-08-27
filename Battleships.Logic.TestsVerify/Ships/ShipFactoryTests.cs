using System;
using System.Collections.Generic;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Battleships.Logic.TestsVerify.Ships
{
    public class ShipFactoryTests
    {
        private readonly Mock<IOptions<AppSettings>> _configMock;
        private readonly Mock<IRandomWrapper> _randomWrapperMock;
        private readonly IShipFactory _sut;

        public ShipFactoryTests()
        {
            _configMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);
            _randomWrapperMock = new Mock<IRandomWrapper>(MockBehavior.Strict);
            _sut = new ShipFactory(_configMock.Object, _randomWrapperMock.Object);
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

            _configMock.Verify(x => x.Value, Times.Once);
            _randomWrapperMock.Verify(x => x.Next(It.IsAny<int>()), Times.Never);
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
            _randomWrapperMock.Setup(x => x.Next(2))
                .Returns(1);
            _randomWrapperMock.Setup(x => x.Next(10 - size))
                .Returns(4);
            _randomWrapperMock.Setup(x => x.Next(10))
                .Returns(6);

            var result = _sut.BuildShip("Battleship");

            result.Size.Should().Be(size);
            result.Name.Should().Be("Battleship");
            result.Coordinates.Should().HaveCount(size);

            _configMock.Verify(x => x.Value, Times.Exactly(3));
            _randomWrapperMock.Verify(x => x.Next(It.IsAny<int>()), Times.Exactly(3));
        }
    }
}