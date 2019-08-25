using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Battleships.Logic.Tests.Settings
{
    public class SettingsCheckerTests
    {
        public SettingsCheckerTests()
        {
            _configMock = new Mock<IOptions<AppSettings>>(MockBehavior.Strict);
            _fileWrapperMock = new Mock<IFileWrapper>(MockBehavior.Strict);
        }

        private const string AppSettingsFileName = "appsettings.json";
        private readonly Mock<IOptions<AppSettings>> _configMock;
        private readonly Mock<IFileWrapper> _fileWrapperMock;
        private ISettingsChecker _sut;

        public static IEnumerable<object[]> GetAllShipTypeSizes => 
            Enumerable.Range(SettingsRules.MinimalShipTypeSize, SettingsRules.MaximalShipTypeSize - SettingsRules.MinimalShipTypeSize)
                .Select(x => new object[] { (object)x})
                .ToArray();

        [Theory]
        [MemberData(nameof(GetAllShipTypeSizes))]
        public void Check_ShipTypeSizeOk_NoExceptionIsThrown(int size)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = size, Count = 1}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            _sut.Check();

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(7)]
        public void Check_ShipTypeSizeWrong_ExceptionIsThrown(int size)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = size, Count = 1}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage($"An issue with settings in appsettings.json file has been found. ShipTypes -> Size of 'Battleship' should be between {SettingsRules.MinimalShipTypeSize} and {SettingsRules.MaximalShipTypeSize}.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        public static IEnumerable<object[]> GetAllShipCounts => 
            Enumerable.Range(SettingsRules.MinimalShipCount, SettingsRules.MaximalShipCount - SettingsRules.MinimalShipCount)
                .Select(x => new object[] { (object)x})
                .ToArray();

        [Theory]
        [MemberData(nameof(GetAllShipCounts))]
        public void Check_ShipCountOk_NoExceptionIsThrown(int count)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = count}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            _sut.Check();

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(-1)]
        [InlineData(11)]
        public void Check_ShipCountWrong_ExceptionIsThrown(int count)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 4, Count = count}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage($"An issue with settings in appsettings.json file has been found. ShipTypes -> Count of 'Battleship' should be between {SettingsRules.MinimalShipCount} and {SettingsRules.MaximalShipCount}.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }


        [Fact]
        public void Check_IssueWithColumnCount_ExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 1}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage("An issue with settings in appsettings.json file has been found. Lack of Grid -> ColumnCount setting.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Fact]
        public void Check_IssueWithGrid_ExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 1}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage("An issue with settings in appsettings.json file has been found. Lack of Grid setting.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Fact]
        public void Check_IssueWithRowCount_ExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 1}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage("An issue with settings in appsettings.json file has been found. Lack of Grid -> RowCount setting.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Fact]
        public void Check_IssueWithShipCount_ExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5}}
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage("An issue with settings in appsettings.json file has been found. Lack of ShipTypes -> Count of 'Battleship' setting.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Fact]
        public void Check_IssueWithShipTypes_ExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Count = 1}}
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage("An issue with settings in appsettings.json file has been found. Lack of ShipTypes -> Size of 'Battleship' setting.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Fact]
        public void Check_IssueWithShipTypesCount_ExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Count = 1}}
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage("An issue with settings in appsettings.json file has been found. Lack of ShipTypes -> Size of 'Battleship' setting.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Fact]
        public void Check_NoAppSettingFile_ExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(false);
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage("An issue with settings in appsettings.json file has been found. No appsettings.json file was found.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.Never);
        }

        public static IEnumerable<object[]> GetAllGridColumnCounts => 
            Enumerable.Range(SettingsRules.MinimalGridColumnCount, SettingsRules.MaximalGridColumnCount - SettingsRules.MinimalGridColumnCount)
                .Select(x => new object[] { (object)x})
                .ToArray();

        [Theory]
        [MemberData(nameof(GetAllGridColumnCounts))]
        public void Check_GridColumnCountOk_NoExceptionIsThrown(int columnCount)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = columnCount, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 5}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            _sut.Check();

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(21)]
        [InlineData(121)]
        public void Check_GridColumnCountWrong_ExceptionIsThrown(int columnCount)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = columnCount, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 4, Count = 5}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage($"An issue with settings in appsettings.json file has been found. Grid -> ColumnCount should be between {SettingsRules.MinimalGridColumnCount} and {SettingsRules.MaximalGridColumnCount}.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        public static IEnumerable<object[]> GetAllGridRowCounts => 
            Enumerable.Range(SettingsRules.MinimalGridRowCount, SettingsRules.MaximalGridRowCount - SettingsRules.MinimalGridRowCount)
                .Select(x => new object[] { (object)x})
                .ToArray();

        [Theory]
        [MemberData(nameof(GetAllGridRowCounts))]
        public void Check_GridRowCountOk_NoExceptionIsThrown(int rowCount)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = rowCount},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 5}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            _sut.Check();

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(21)]
        [InlineData(111)]
        public void Check_GridRowCountWrong_ExceptionIsThrown(int rowCount)
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = rowCount},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Battleship", Size = 4, Count = 5}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            Action act = () => _sut.Check();

            act.Should().Throw<ApplicationException>().WithMessage($"An issue with settings in appsettings.json file has been found. Grid -> RowCount should be between {SettingsRules.MinimalGridRowCount} and {SettingsRules.MaximalGridRowCount}.");

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }

        [Fact]
        public void Check_ShipTypeNameAndShipCountNameAreTheSame_NoExceptionIsThrown()
        {
            _fileWrapperMock.Setup(x => x.Exists(AppSettingsFileName))
                .Returns(true);
            _configMock.Setup(x => x.Value)
                .Returns(new AppSettings
                {
                    Grid = new GridSettings {ColumnCount = 10, RowCount = 10},
                    ShipTypes = new List<ShipTypeSettings> {new ShipTypeSettings {Name = "Destroyer", Size = 4, Count = 2}, new ShipTypeSettings {Name = "Battleship", Size = 5, Count = 1}},
                });
            _sut = new SettingsChecker(_configMock.Object, _fileWrapperMock.Object);

            _sut.Check();

            _fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            _fileWrapperMock.Verify(x => x.Exists(AppSettingsFileName), Times.Once);
            _configMock.Verify(x => x.Value, Times.AtLeast(1));
        }
    }
}