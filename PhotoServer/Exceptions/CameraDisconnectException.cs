using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Exceptions
{
    public class CameraDisconnectException : Exception
    {
        public CameraDisconnectException(string message) 
            : base(message)
        {
        }
    }
}
