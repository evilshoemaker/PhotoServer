using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer
{
    public class RabbitMqSettings
    {
        private string userName = "test";
        private string password = "test";
        private string virtualHost = "/";
        private string hostName = "localhost";
        private string queue = "PHOTO_SHOOT";
        private string cameraDisconnectQueue = "PHOTO_DISCONNECT";
        private string photoStopQueue = "PHOTO_STOP";
        private bool requeue = false;

        [JsonProperty("userName")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [JsonProperty("password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [JsonProperty("virtualHost")]
        public string VirtualHost
        {
            get { return virtualHost; }
            set { virtualHost = value; }
        }

        [JsonProperty("hostName")]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        [JsonProperty("requeue")]
        public bool Requeue
        {
            get { return requeue; }
            set { requeue = value; }
        }

        [JsonProperty("queue")]
        public string Queue
        {
            get { return queue; }
            set { queue = value; }
        }

        [JsonProperty("cameraDisconnectQueue")]
        public string CameraDisconnectQueue
        {
            get { return cameraDisconnectQueue; }
            set { cameraDisconnectQueue = value; }
        }

        [JsonProperty("photoStopQueue")]
        public string PhotoStopQueue
        {
            get { return photoStopQueue; }
            set { photoStopQueue = value; }
        }
    }
}
