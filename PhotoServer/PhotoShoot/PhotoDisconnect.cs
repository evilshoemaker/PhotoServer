using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.PhotoShoot
{
    public class PhotoDisconnect
    {
        private int stend;
        private int cameraId;

        [JsonProperty("stend")]
        public int Stend
        {
            get { return stend; }
        }

        [JsonProperty("camera")]
        public int CameraId
        {
            get { return cameraId; }
        }

        public PhotoDisconnect(int stend, int camera)
        {
            this.stend = stend;
            this.cameraId = camera;
        }

        public string Json()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
