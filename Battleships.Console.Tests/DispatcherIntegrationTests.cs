using System.Collections.Generic;
using Battleships.Console.Input;
using Battleships.Console.Output;
using Battleships.Logic;
using Battleships.Logic.Coordinates;
using Battleships.Logic.Public;
using Moq;
using Xunit;

namespace Battleships.Console.Tests
{
    public class DispatcherIntegrationTests
    {
        public DispatcherIntegrationTests()
        {
            _gameLogicMock = new Mock<IGameLogic>(MockBehavior.Strict);
            _inputReaderMock = new Mock<IInputReader>(MockBehavior.Strict);
            _outputWritterMock = new Mock<IOutputWriter>(MockBehavior.Strict);
            _soundPlayerMock = new Mock<ISoundPlayer>(MockBehavior.Strict);
            var cursorHelper = new CursorHelper();
            var gridPainter = new GridPainter(_outputWritterMock.Object);
            var gridResultPainter = new GridResultPainter(cursorHelper, _outputWritterMock.Object);
            var textResultDisplayer = new TextResultDisplayer(_outputWritterMock.Object, cursorHelper);
            var outputFacade = new OutputFacade(cursorHelper, gridPainter, gridResultPainter, textResultDisplayer, _soundPlayerMock.Object, _outputWritterMock.Object);
            _sut = new Dispatcher(_gameLogicMock.Object, _inputReaderMock.Object, outputFacade);
        }

        private readonly Mock<IGameLogic> _gameLogicMock;
        private readonly Mock<IOutputWriter> _outputWritterMock;
        private readonly Mock<ISoundPlayer> _soundPlayerMock;
        private readonly Mock<IInputReader> _inputReaderMock;
        private readonly IDispatcher _sut;

        [Fact]
        public void PlayGame_Exception_InfoAboutExceptionIsShown()
        {
            _outputWritterMock.Setup(x => x.SetCursorPosition(0, 0));
            var exception = "Something wrong with config.";
            _gameLogicMock.Setup(x => x.StartNewGame())
                .Returns(new ResponseEnvelope<GridSize> {Success = false, ErrorDescription = exception});
            _outputWritterMock.Setup(x => x.Write(exception));
            _inputReaderMock.Setup(x => x.ReadUserKey())
                .Returns(null);

            _sut.PlayGame();
        }

        [Fact]
        public void PlayGame_MakeNewMoveWithRightData_ResultWithData()
        {
            _outputWritterMock.Setup(x => x.SetCursorPosition(0, 0));
            _outputWritterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWritterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWritterMock.Setup(x => x.SetCursorPosition(2, 23));
            _outputWritterMock.Setup(x => x.SetCursorPosition(5, 2));
            _outputWritterMock.Setup(x => x.SetCursorPosition(9, 4));
            var gridSize = new GridSize(10, 10);
            _gameLogicMock.Setup(x => x.StartNewGame())
                .Returns(new ResponseEnvelope<GridSize> {Success = true, Content = gridSize});
            _outputWritterMock.Setup(x => x.Write(It.Is<string>(y => y.Length == 1 || y.Length == 2 || y.Length == 4 || y.Length >= 200)));
            _outputWritterMock.Setup(x => x.Write(It.Is<string>(y => y.Contains("A1") || y.Contains("B2") || y.Contains("Game End"))));
            _outputWritterMock.Setup(x => x.WriteNewLine());
            _inputReaderMock.SetupSequence(x => x.ReadUserCommand())
                .Returns("a1")
                .Returns("b2");
            _gameLogicMock.Setup(x => x.MakeNewMove("a1"))
                .Returns(new ResponseEnvelope<ShotResult> {Success = true, Content = new ShotResult {Kind = ShotResult.Kinds.Water, Coordinate = new Coordinate('A', "1")}});
            _gameLogicMock.Setup(x => x.MakeNewMove("b2"))
                .Returns(new ResponseEnvelope<ShotResult>
                    {Success = true, Content = new ShotResult {Kind = ShotResult.Kinds.GameEnd, Coordinate = new Coordinate('B', "2"), SinkShip = new List<Coordinate>()}});
            _inputReaderMock.Setup(x => x.ReadUserKey())
                .Returns(null);
            _soundPlayerMock.Setup(x => x.PlayResult(It.Is<ShotResult>(y => y.Kind == ShotResult.Kinds.Water && y.Coordinate.Equals(new Coordinate('A', "1")))));
            _soundPlayerMock.Setup(x => x.PlayResult(It.Is<ShotResult>(y => y.Kind == ShotResult.Kinds.GameEnd && y.Coordinate.Equals(new Coordinate('B', "2")))));

            _sut.PlayGame();
        }

        [Fact]
        public void PlayGame_MakeNewMoveWithWrongData_ResultWithIssueDescription()
        {
            _outputWritterMock.Setup(x => x.SetCursorPosition(0, 0));
            _outputWritterMock.Setup(x => x.SetCursorPosition(0, 22));
            _outputWritterMock.Setup(x => x.SetCursorPosition(0, 23));
            _outputWritterMock.Setup(x => x.SetCursorPosition(2, 23));
            var gridSize = new GridSize(10, 10);
            _gameLogicMock.Setup(x => x.StartNewGame())
                .Returns(new ResponseEnvelope<GridSize> {Success = true, Content = gridSize});
            _outputWritterMock.Setup(x => x.Write(It.Is<string>(y => y.Length == 1 || y.Length == 2 || y.Length == 4 || y.Length >= 200)));
            _outputWritterMock.Setup(x => x.Write(It.Is<string>(y => y.Contains("A1") || y.Contains("Game End"))));
            _outputWritterMock.Setup(x => x.WriteNewLine());
            _inputReaderMock.SetupSequence(x => x.ReadUserCommand())
                .Returns("aaa")
                .Returns("a1");
            _gameLogicMock.Setup(x => x.MakeNewMove("aaa"))
                .Returns(new ResponseEnvelope<ShotResult> {Success = true, Content = new ShotResult {Kind = ShotResult.Kinds.WrongCoordinates}});
            _gameLogicMock.Setup(x => x.MakeNewMove("a1"))
                .Returns(new ResponseEnvelope<ShotResult>
                    {Success = true, Content = new ShotResult {Kind = ShotResult.Kinds.GameEnd, Coordinate = new Coordinate('A', "1"), SinkShip = new List<Coordinate>()}});
            _inputReaderMock.Setup(x => x.ReadUserKey())
                .Returns(null);
            _soundPlayerMock.Setup(x => x.PlayResult(It.Is<ShotResult>(y => y.Kind == ShotResult.Kinds.GameEnd && y.Coordinate.Equals(new Coordinate('A', "1")))));

            _sut.PlayGame();
        }
    }
}