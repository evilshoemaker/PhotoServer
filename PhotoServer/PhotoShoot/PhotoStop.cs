using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.PhotoShoot
{
    public class PhotoStop
    {
        private int stend;
        private int cameraId;
        private string id = "";
        private List<string> images = new List<string>();

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

        [JsonProperty("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty("images")]
        public List<string> Images
        {
            get { return images; }
            set { images = value; }
        }

        public string Json()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
