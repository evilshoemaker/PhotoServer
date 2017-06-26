using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PhotoServer
{
    public class Settings
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Описание шаблона "Одиночка"

        private static Settings instance = null;
        public static Settings Instance
        {
            get
            {
                if (instance == null) LoadSettings();
                return instance;
            }
        }

        public Settings()
        {
            rabbitMq = new RabbitMqSettings();
        }
        #endregion

        #region Свойства класса - поля настроек

        private RabbitMqSettings rabbitMq = null;

        [JsonProperty("rabbitMq")]
        public RabbitMqSettings RabbitMq
        {
            get { return rabbitMq; }
            set { rabbitMq = value; }
        }

        private string imageDirectory = Variables.PhotoTestDir;

        [JsonProperty("imageDirectory")]
        public string ImageDirectory
        {
            get
            {
                Utilits.CreateDirIfNotExist(imageDirectory);
                return imageDirectory;
            }
            set { imageDirectory = value; }
        }

        private bool isImageSubfolder = false;

        [JsonProperty("isImageSubfolder")]
        public bool IsImageSubfolder
        {
            get { return isImageSubfolder; }
            set { isImageSubfolder = value; }
        }

        #endregion

        #region Методы

        public void SaveSettings()
        {
            try
            {
                //string output = JsonConvert.SerializeObject(instance);
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Formatting.Indented;

                using (StreamWriter sw = new StreamWriter(Variables.SettigsFile))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, instance);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        // Загрузка настроек из файла
        private static void LoadSettings()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sr = new StreamReader(Variables.SettigsFile))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    instance = serializer.Deserialize<Settings>(reader);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                // если не удалось загрузить файл применяем настройки по умолчанию
                instance = new Settings();
                instance.SaveSettings();
            }
        }
        #endregion
    }
}
