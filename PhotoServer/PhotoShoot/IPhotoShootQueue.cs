using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.PhotoShoot
{
    public interface IPhotoShootQueue
    {
        event PhotoShootDelegate NewTask;
    }
}
