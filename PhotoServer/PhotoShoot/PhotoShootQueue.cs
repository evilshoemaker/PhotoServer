using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.PhotoShoot
{
    public class PhotoShootQueue
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private RabbitMqPhotoShootQueue rabbitMqClient;
        private PhotoCamera.CanonCamera cameraHelper;

        public PhotoShootQueue()
        {
            cameraHelper = new PhotoCamera.CanonCamera();

            rabbitMqClient = new RabbitMqPhotoShootQueue(Settings.Instance.RabbitMq);
            rabbitMqClient.NewPhotoShoot += OnNewPhotoShoot;
        }

        public PhotoCamera.CanonCamera CameraHelper
        {
            get { return cameraHelper;  }
        }

        public void Connect()
        {
            try
            {
                cameraHelper.OpenSession();
                rabbitMqClient.Connect();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void Close()
        {
            rabbitMqClient.Close();
        }

        public void Dispose()
        {
            cameraHelper.Dispose();
            rabbitMqClient.Close();
        }

        public void Test()
        {
            string testJson = @"{
                                    stend: 1,
                                    camera: 1,
                                    count: 2,
                                    delay: 34,
                                    id: 'id'
                                }";
            RabbitMqPhotoShootQueue.Send(Settings.Instance.RabbitMq.Queue, testJson, Settings.Instance.RabbitMq);
        }

        private void OnNewPhotoShoot(PhotoShoot photoShoot)
        {
            logger.Info("Start photo shoot. " + photoShoot.Json());
            PhotoStop photoStop = new PhotoStop();
            photoStop.CameraId = photoShoot.CameraId;
            photoStop.Stend = photoShoot.Stend;
            photoStop.Id = photoShoot.PhotoId;

            photoStop.Images = cameraHelper.TakePhoto(photoShoot);

            /*for (int i = 0; i < photoShoot.Count; i++)
            {
                cameraHelper.TakePhoto(photoShoot);
                photoStop.Images.Add(cameraHelper.LastPhotoFileName);
            }*/

            RabbitMqPhotoShootQueue.Send(Settings.Instance.RabbitMq.PhotoStopQueue, photoStop.Json(), Settings.Instance.RabbitMq);
            logger.Info("Photo shoot end. " + photoStop.Json());
        }
    }
}
