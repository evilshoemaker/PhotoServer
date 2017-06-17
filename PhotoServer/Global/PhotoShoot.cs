using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PhotoServer
{
    public class PhotoShoot
    {
        private int stend;
        private int cameraId;
        private int count;
        private int delay;
        private string photoId;

        [JsonProperty("stend")]
        public int Stend
        {
            get { return stend; }
            set { stend = value; }
        }

        [JsonProperty("camera")]
        public int CameraId
        {
            get { return cameraId; }
            set { cameraId = value; }
        }

        [JsonProperty("count")]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        [JsonProperty("delay")]
        public int Delay
        {
            get { return delay; }
            set { delay = value;}
        }

        [JsonProperty("id")]
        public string PhotoId
        {
            get { return photoId; }
            set { photoId = value; }
        }

        public static PhotoShoot Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<PhotoShoot>(json);
        }
    }
}
