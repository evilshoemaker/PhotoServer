using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NLog;

namespace PhotoServer
{
    public partial class Form1 : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "test";
            factory.Password = "test";
            factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
            factory.HostName = "192.168.0.40";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            logger.Debug("ЖЖолрфив");
            /*Settings.Settings settings = Settings.Settings.Instance;
            settings.SaveSettings();

            RabbitMqClient client = new RabbitMqClient(settings.RabbitMq);
            client.Connect();*/

            /*string testJson = @"{
                                    stend: 1,
                                    camera: 1,
                                    count: 2,
                                    delay: 34,
                                    id: 'id'
                                }";
            PhotoShoot ph = PhotoShoot.Deserialize(testJson);
            int h = ph.CameraId;*/
        }
    }
}
