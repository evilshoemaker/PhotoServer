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

        public static string NextFileName(string dir, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);
            string fullFileName = Path.Combine(dir, fileName);
            string fileNameTemp = name + extension;
            int index = 1;
            while (File.Exists(fullFileName))
            {
                fileNameTemp = name + "_" + index + extension;
                fullFileName = Path.Combine(dir, fileNameTemp);
                index++;
            }
            return fileNameTemp;
        }
    }
}
