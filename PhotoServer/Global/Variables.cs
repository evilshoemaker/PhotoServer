using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer
{
    public class Variables
    {
        public static readonly string AppDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static readonly string SettigsFile = AppDir + "\\settings.json";

        private static readonly string photoTestDir = AppDir + "\\Photo";
        public static string PhotoTestDir
        {
            get {
                Utilits.CreateDirIfNotExist(photoTestDir);
                return photoTestDir;
            }

        }
    }
}
