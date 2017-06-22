using System;
using System.IO;

namespace PhotoServer
{
    public class Utilits
    {
        public static void CreateDirIfNotExist(string path)
        {
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }
    }
}
