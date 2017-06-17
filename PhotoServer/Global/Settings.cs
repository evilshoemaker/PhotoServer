using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer
{
    public class Settings
    {
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
        public Settings() { }
        #endregion

        #region Методы

        public void SaveSettings()
        {
            try
            {
                // Сохранение в файл
                /*XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                FileStream stream = new FileStream(Variables.SettigsFile, FileMode.Create);
                //TextWriter writer = new StreamWriter(stream, System.Text.Encoding.UTF8);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;

                XmlWriter writer = XmlWriter.Create(stream, settings);//new XmlTextWriter(stream, Encoding.UTF8);
                serializer.Serialize(writer, instance);
                writer.Close();
                stream.Close();*/
            }
            catch (Exception e)
            {
                
            }
        }

        // Загрузка настроек из файла
        private static void LoadSettings()
        {
            try
            {
                /*XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                StringReader stringReader = new StringReader(File.ReadAllText(Variables.SettigsFile));
                instance = (Settings)serializer.Deserialize(stringReader);*/
            }
            catch (Exception e)
            {
                // если не удалось загрузить файл применяем настройки по умолчанию
                instance = new Settings();
                instance.SaveSettings();
                //System.Windows.MessageBox.Show(e.Message+" (Настройки будут загружены по умолчанию)");
                //Log.Error("Target : Settings.LoadSettings, Exception : " + e.Message);
            }
        }
        #endregion
    }
}
