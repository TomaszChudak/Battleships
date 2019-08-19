using System.IO;

namespace Battleships.Logic.Helpers
{
    internal interface IFileWrapper
    {
        bool Exists(string path);
    }

    internal class FileWrapper : IFileWrapper
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}