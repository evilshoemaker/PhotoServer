using NLog;
using NLog.Config;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoServer
{
    public partial class MainForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        PhotoShoot.PhotoShootQueue photoShootQueue;

        public MainForm()
        {
            InitializeComponent();
            photoShootQueue = new PhotoShoot.PhotoShootQueue();
        }

        private void SetupRichTextBoxLogger()
        {
            NLog.Windows.Forms.RichTextBoxTarget target = new NLog.Windows.Forms.RichTextBoxTarget();
            target.Name = "RichTextBox";
            target.Layout = "${longdate} ${level:uppercase=true} ${message}";
            target.ControlName = nameof(this.LogRichTextBox);
            target.FormName = nameof(MainForm);
            target.AutoScroll = true;
            target.MaxLines = 0;
            target.UseDefaultRowColoringRules = true;
            AsyncTargetWrapper asyncWrapper = new AsyncTargetWrapper();
            asyncWrapper.Name = "AsyncRichTextBox";
            asyncWrapper.WrappedTarget = target;

            SimpleConfigurator.ConfigureForTargetLogging(asyncWrapper, LogLevel.Trace);
        }

        private void LoadSettings()
        {
            imageDirectoryTextBox.Text = Settings.Instance.ImageDirectory;
            userNameTextBox.Text = Settings.Instance.RabbitMq.UserName;
            passwordTextBox.Text = Settings.Instance.RabbitMq.Password;
            hostNameTextBox.Text = Settings.Instance.RabbitMq.HostName;
            virtualHostTextBox.Text = Settings.Instance.RabbitMq.VirtualHost;
        }

        private void GetCameraList()
        {
            camerasTextBox.Text = string.Join("\r\n", photoShootQueue.CameraHelper.GetCameraStringList());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupRichTextBoxLogger();
            LoadSettings();
            photoShootQueue.Connect();
            photoShootQueue.CameraHelper.NewCameraConnected += GetCameraList;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //cameraHelper.TakePhoto();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //cameraHelper.DisposeHelper();
            //rabbitMqClient.Close();
            Settings.Instance.SaveSettings();
            Application.Exit();
        }

        private void selectImageDirButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = imageDirectoryTextBox.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imageDirectoryTextBox.Text = dialog.SelectedPath;
                Settings.Instance.ImageDirectory = dialog.SelectedPath;
                Settings.Instance.SaveSettings();
            }
        }

        private void userNameTextBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Instance.RabbitMq.UserName = userNameTextBox.Text;
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Instance.RabbitMq.Password = passwordTextBox.Text;
        }

        private void hostNameTextBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Instance.RabbitMq.HostName = hostNameTextBox.Text;
        }

        private void virtualHostTextBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Instance.RabbitMq.VirtualHost = virtualHostTextBox.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(textBox1.Text);
            try
            {
                cameraHelper.TakePhoto(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
