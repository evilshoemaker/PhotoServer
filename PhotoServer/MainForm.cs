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

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Debug("длдпд");
            logger.Info("sdflkjsl");
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupRichTextBoxLogger();
        }
    }
}
