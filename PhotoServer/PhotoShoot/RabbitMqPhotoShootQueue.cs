using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.PhotoShoot
{
    public class RabbitMqPhotoShootQueue : IPhotoShootQueue
    {
        public event PhotoShootDelegate NewTask;

        public void Emit()
        {
            NewTask?.Invoke(new PhotoShoot());
        }
    }
}
