using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Settings
{
    public class RabbitMqSettings
    {
        private string userName = "test";
        private string password = "test";
        private string virtualHost = "/";
        private string hostName = "localhost";

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
    }
}
