using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Global
{
    public class PhotoShoot
    {
        private int stend;
        private int cameraId;
        private int count;
        private int delay;
        private string photoId;

        public int Stend
        {
            get { return stend; }
            set { stend = value; }
        }

        public int CameraId
        {
            get { return cameraId; }
            set { cameraId = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public int Delay
        {
            get { return delay; }
            set { delay = value;}
        }

        public string PhotoId
        {
            get { return photoId; }
            set { photoId = value; }
        }
    }
}
