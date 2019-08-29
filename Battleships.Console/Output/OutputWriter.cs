namespace Battleships.Console.Output
{
    internal interface IOutputWriter
    {
        void Write(string value);
        void WriteNewLine();
        void SetCursorPosition(int left, int top);
        void Clear();
    }

    internal class OutputWriter : IOutputWriter
    {
        public void Write(string value)
        {
            System.Console.Write(value);
        }

        public void WriteNewLine()
        {
            System.Console.WriteLine();
        }

        public void SetCursorPosition(int left, int top)
        {
            System.Console.SetCursorPosition(left, top);
        }

        public void Clear()
        {
            System.Console.Clear();
        }
    }
}