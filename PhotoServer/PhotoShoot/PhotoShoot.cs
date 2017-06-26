using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PhotoServer.PhotoShoot
{
    public class PhotoShoot
    {
        private int stend;
        private int cameraId;
        private int count;
        private int delay;
        private string photoId;
        private DataPhotoShoot data;

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

        public DataPhotoShoot Data
        {
            get { return data; }
            set { data = value; }
        }

        public static PhotoShoot Deserialize(string json)
        {
            PhotoShoot photoShoot = JsonConvert.DeserializeObject<PhotoShoot>(json);
            if (photoShoot.Data != null)
            {
                photoShoot.PhotoId = photoShoot.Data.Qr;
            }

            return photoShoot;
        }

        public string Json()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DataPhotoShoot
    {
        private string qr = "";

        [JsonProperty("qr")]
        public string Qr
        {
            get { return qr; }
            set { qr = value; }
        }
    }
}
