using System.Linq;
using Battleships.Logic.Public;

namespace Battleships.Console.Output
{
    internal interface ITextResultDisplayer
    {
        void DisplayResult(ShotResult result);
    }

    internal class TextResultDisplayer : ITextResultDisplayer
    {
        private readonly ICursorHelper _cursorHelper;
        private readonly IOutputWriter _outputWriter;

        public TextResultDisplayer(IOutputWriter outputWriter, ICursorHelper cursorHelper)
        {
            _cursorHelper = cursorHelper;
            _outputWriter = outputWriter;
        }

        public void DisplayResult(ShotResult result)
        {
            _outputWriter.SetCursorPosition(0, _cursorHelper.TextResultTop);

            if (result?.Coordinate != null)
                _outputWriter.Write($"> {result.Coordinate} -> {result.Description}");
            else
                _outputWriter.Write($"> {result?.Description}");

            FillLineWithSpaces();

            _outputWriter.SetCursorPosition(0, _cursorHelper.TextResultTop + 1);

            _outputWriter.Write("> ");

            FillLineWithSpaces();

            _outputWriter.SetCursorPosition(2, _cursorHelper.TextResultTop + 1);

            if (result != null && result.Kind == ShotResult.Kinds.GameEnd)
                DisplayEndGameInfo();
        }

        private void FillLineWithSpaces()
        {
            _outputWriter.Write(string.Concat(Enumerable.Range(0, 200).Select(x => " ")));
        }

        private void DisplayEndGameInfo()
        {
            _outputWriter.Write("Game End - Press any key to exit.");
        }
    }
}