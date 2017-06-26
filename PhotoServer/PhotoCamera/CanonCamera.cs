using EOSDigital.API;
using EOSDigital.SDK;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PhotoServer.Exceptions;
using System.IO;

namespace PhotoServer.PhotoCamera
{
    public class CanonCamera
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public event VoidDelegate NewCameraConnected;
        private string lastPhotoFileName = "";
        private string photoShootId = "";

        private CanonAPI apiHandler;
        //private Camera mainCamera;
        private ManualResetEvent waitEvent = new ManualResetEvent(false);
        List<Camera> cameras = new List<Camera>();

        public string LastPhotoFileName
        {
            get { return lastPhotoFileName;}
        }

        public CanonCamera()
        {
            apiHandler = new CanonAPI();
            apiHandler.CameraAdded += CameraAdded;
            cameras.AddRange(apiHandler.GetCameraList());
            //OpenSession();
        }

        public void Dispose()
        {
            foreach (Camera camera in cameras)
            {
                camera?.Dispose();
            }
            apiHandler.Dispose();
        }

        public void TakePhoto()
        {
            if (cameras.Count == 0)
            {
                logger.Error("Active cameras not found");
                return;
            }

            Camera mainCamera = cameras.First();
            mainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Host);
            mainCamera.SetCapacity(4096, int.MaxValue);

            logger.Info("Taking photo with current settings...");
            CameraValue tv = TvValues.GetValue(mainCamera.GetInt32Setting(PropertyID.Tv));
            if (tv == TvValues.Bulb) mainCamera.TakePhotoBulb(2);
            else mainCamera.TakePhoto();
            waitEvent.WaitOne();
        }

        /*public void TakePhoto(PhotoShoot.PhotoShoot photoShoot)
        {
            if (cameras.Count == 0)
            {
                throw new CameraDisconnectException("Active cameras not found");
            }

            if (cameras.Count < photoShoot.CameraId | photoShoot.CameraId == 0)
            {
                throw new CameraDisconnectException("Camera not found");
            }

            photoShootId = photoShoot.PhotoId;
            Camera mainCamera = cameras[photoShoot.CameraId - 1];
            mainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Host);
            mainCamera.SetCapacity(4096, int.MaxValue);

            logger.Info("Taking photo with current settings...");
            CameraValue tv = TvValues.GetValue(mainCamera.GetInt32Setting(PropertyID.Tv));
            if (tv == TvValues.Bulb) mainCamera.TakePhotoBulb(2);
            else mainCamera.TakePhoto();
            waitEvent.WaitOne();
        }*/

        public List<string> TakePhoto(PhotoShoot.PhotoShoot photoShoot)
        {
            if (cameras.Count == 0)
            {
                throw new CameraDisconnectException("Active cameras not found");
            }

            if (cameras.Count < photoShoot.CameraId | photoShoot.CameraId == 0)
            {
                throw new CameraDisconnectException("Camera not found");
            }

            photoShootId = photoShoot.PhotoId;
            Camera mainCamera = cameras[photoShoot.CameraId - 1];
            mainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Host);
            mainCamera.SetCapacity(4096, int.MaxValue);

            logger.Info("Taking photo with current settings...");

            List<string> list = new List<string>();

            for (int i = 0; i < photoShoot.Count; i++)
            {
                CameraValue tv = TvValues.GetValue(mainCamera.GetInt32Setting(PropertyID.Tv));
                if (tv == TvValues.Bulb) mainCamera.TakePhotoBulb(2);
                else mainCamera.TakePhoto();

                waitEvent.Reset();
                waitEvent.WaitOne();
                
                list.Add(lastPhotoFileName);
            }

            return list;
        }

        public void OpenSession()
        {
            foreach (Camera camera in cameras)
            {
                camera.DownloadReady += DownloadReady;
                camera.CameraHasShutdown += CameraHasShutdown;
                camera.OpenSession();
                logger.Info($"Opened session with camera: {camera.DeviceName}");
            }
            logger.Info("Camera API init");
        }

        public List<string> GetCameraStringList()
        {
            List<string> result = new List<string>();
            int index = 1;
            foreach (Camera camera in cameras)
            {
                string item = "";
                item += "ID: " + index + "; ";  
                item += "Name: " + camera.DeviceName + "; ";
                result.Add(item);
                index++;
            }
            return result;
        }

        private void CameraAdded(CanonAPI sender)
        {
            List<Camera> cameraList = apiHandler.GetCameraList();
            foreach (Camera camera in cameraList)
            {
                try
                {
                    Camera cam = cameras.Find(x => x.ID == camera.ID);
                    if (cam == null)
                    {
                        camera.DownloadReady += DownloadReady;
                        camera.CameraHasShutdown += CameraHasShutdown;
                        camera.OpenSession();
                        cameras.Add(camera);

                        logger.Info($"Opened session with camera: {camera.DeviceName}");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                finally { waitEvent.Set(); }
            }
            NewCameraConnected?.Invoke();
        }

        private void CameraHasShutdown(Camera sender)
        {
            try
            {
                if (sender == null)
                    return;

                cameras.Remove(sender);
                logger.Info($"Shutdown camera: {sender.DeviceName}");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally { waitEvent.Set(); }
            NewCameraConnected?.Invoke();
        }

        private int GetCameraId(Camera sender)
        {
            return cameras.FindIndex(x => x.ID == sender.ID) + 1;
        }

        private void DownloadReady(Camera sender, DownloadInfo info)
        {
            try
            {
                string extension = Path.GetExtension(info.FileName);
                string fileName = "CAMERA_" + GetCameraId(sender) + extension;
                string dir = Settings.Instance.ImageDirectory;
                if (Settings.Instance.IsImageSubfolder)
                    dir = Path.Combine(dir, photoShootId);
                Utilits.CreateDirIfNotExist(dir);

                logger.Info("Starting image download...");
                info.FileName = Utilits.NextFileName(dir, fileName);
                sender.DownloadFile(info, dir);
                logger.Info("Photo taken and saved");
                lastPhotoFileName = info.FileName;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally { waitEvent.Set(); }
        }
    }
}
