using System;
using Battleships.Console.Input;
using Battleships.Console.Output;
using Battleships.Logic;
using Battleships.Logic.Public;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Console.Tests
{
    public class DispatcherTests
    {
        public DispatcherTests()
        {
            _gameLogicMock = new Mock<IGameLogic>(MockBehavior.Strict);
            _inputReaderMock = new Mock<IInputReader>(MockBehavior.Strict);
            _outputFacadeMock = new Mock<IOutputFacade>(MockBehavior.Strict);
            _sut = new Dispatcher(_gameLogicMock.Object, _inputReaderMock.Object, _outputFacadeMock.Object);
        }

        private readonly Mock<IGameLogic> _gameLogicMock;
        private readonly Mock<IInputReader> _inputReaderMock;
        private readonly Mock<IOutputFacade> _outputFacadeMock;
        private readonly IDispatcher _sut;

        [Fact]
        public void PlayGame_MakeNewMoveWithWrongData_ResultWithIssueDescription()
        {
            var gridSize = new GridSize(10, 10);
            _gameLogicMock.Setup(x => x.StartNewGame())
                .Returns(gridSize);
            _outputFacadeMock.Setup(x => x.PaintNewGrid(gridSize));
            _inputReaderMock.SetupSequence(x => x.ReadUserCommand())
                .Returns("aaa")
                .Returns("a1");
            _gameLogicMock.Setup(x => x.MakeNewMove("aaa"))
                .Throws(new ArgumentException("Wrong coordinates"));
            _gameLogicMock.Setup(x => x.MakeNewMove("a1"))
                .Returns(new ShotResult {Kind = ShotResult.Kinds.GameEnd});
            _outputFacadeMock.Setup(x => x.MarkAndDisplayResult(It.IsAny<ShotResult>()));
            _inputReaderMock.Setup(x => x.ReadUserKey())
                .Returns(null);

            _sut.PlayGame();

            _gameLogicMock.Verify(x => x.StartNewGame(), Times.Once);
            _outputFacadeMock.Verify(x => x.PaintNewGrid(It.IsAny<GridSize>()), Times.Once);
            _inputReaderMock.Verify(x => x.ReadUserCommand(), Times.Exactly(2));
            _gameLogicMock.Verify(x => x.MakeNewMove(It.IsAny<string>()), Times.Exactly(2));
            _outputFacadeMock.Verify(x => x.MarkAndDisplayResult(It.IsAny<ShotResult>()), Times.Exactly(2));
            _inputReaderMock.Verify(x => x.ReadUserKey(), Times.Once);
            _outputFacadeMock.Verify(x => x.DisplayException(It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public void PlayGame_MakeNewMoveWithRightData_ResultWithData()
        {
            var gridSize = new GridSize(10, 10);
            _gameLogicMock.Setup(x => x.StartNewGame())
                .Returns(gridSize);
            _outputFacadeMock.Setup(x => x.PaintNewGrid(gridSize));
            _inputReaderMock.SetupSequence(x => x.ReadUserCommand())
                .Returns("a1")
                .Returns("b2");
            _gameLogicMock.Setup(x => x.MakeNewMove("a1"))
                .Returns(new ShotResult {Kind = ShotResult.Kinds.Water});
            _gameLogicMock.Setup(x => x.MakeNewMove("b2"))
                .Returns(new ShotResult {Kind = ShotResult.Kinds.GameEnd});
            _outputFacadeMock.Setup(x => x.MarkAndDisplayResult(It.IsAny<ShotResult>()));
            _inputReaderMock.Setup(x => x.ReadUserKey())
                .Returns(null);

            _sut.PlayGame();

            _gameLogicMock.Verify(x => x.StartNewGame(), Times.Once);
            _outputFacadeMock.Verify(x => x.PaintNewGrid(It.IsAny<GridSize>()), Times.Once);
            _inputReaderMock.Verify(x => x.ReadUserCommand(), Times.Exactly(2));
            _gameLogicMock.Verify(x => x.MakeNewMove(It.IsAny<string>()), Times.Exactly(2));
            _outputFacadeMock.Verify(x => x.MarkAndDisplayResult(It.IsAny<ShotResult>()), Times.Exactly(2));
            _inputReaderMock.Verify(x => x.ReadUserKey(), Times.Once);
            _outputFacadeMock.Verify(x => x.DisplayException(It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public void PlayGame_Exception_InfoAboutExceptionIsShown()
        {
            var exception = new ApplicationException("Something wrong with config.");

            _gameLogicMock.Setup(x => x.StartNewGame())
                .Throws(exception);
            _outputFacadeMock.Setup(x => x.DisplayException(exception));
            _inputReaderMock.Setup(x => x.ReadUserKey())
                .Returns(null);

            _sut.PlayGame();

            _gameLogicMock.Verify(x => x.StartNewGame(), Times.Once);
            _outputFacadeMock.Verify(x => x.PaintNewGrid(It.IsAny<GridSize>()), Times.Never);
            _inputReaderMock.Verify(x => x.ReadUserCommand(), Times.Never);
            _gameLogicMock.Verify(x => x.MakeNewMove(It.IsAny<string>()), Times.Never);
            _outputFacadeMock.Verify(x => x.MarkAndDisplayResult(It.IsAny<ShotResult>()), Times.Never);
            _outputFacadeMock.Verify(x => x.DisplayException(It.IsAny<Exception>()), Times.Once);
            _outputFacadeMock.Verify(x => x.DisplayException(exception), Times.Once);
            _inputReaderMock.Verify(x => x.ReadUserKey(), Times.Once);
        }
    }
}