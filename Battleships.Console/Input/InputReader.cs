using System;

namespace Battleships.Console.Input
{
    internal interface IInputReader
    {
        string ReadUserCommand();
        ConsoleKeyInfo ReadUserKey();
    }

    internal class InputReader : IInputReader
    {
        public string ReadUserCommand()
        {
            return System.Console.ReadLine();
        }

        public ConsoleKeyInfo ReadUserKey()
        {
            return System.Console.ReadKey();
        }
    }
}