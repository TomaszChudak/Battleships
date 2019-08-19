namespace Battleships.Console.Output
{
    internal interface IOutputWriter
    {
        //int CursorTop { get; }
        void Write(string value);
        void WriteNewLine();
        void SetCursorPosition(int left, int top);
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

        //public int CursorTop => System.Console.CursorTop;
    }
}