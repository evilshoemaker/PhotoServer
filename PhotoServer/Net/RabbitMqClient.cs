using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Net
{
    public class RabbitMqClient
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public RabbitMqClient()
        {
            factory = new ConnectionFactory();
        }

        public RabbitMqClient(Settings.RabbitMqSettings settings)
        {
            factory = new ConnectionFactory();
            Settings = settings;
        }

        public Settings.RabbitMqSettings Settings
        {
            set
            {
                factory.UserName = value.UserName;
                factory.Password = value.Password;
                factory.VirtualHost = value.VirtualHost;
                factory.HostName = value.HostName;
            }
        }

        public void Connect(Settings.RabbitMqSettings settings)
        {
            Settings = settings;
            Connect();
        }

        public void Connect()
        {
            Close();

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            InitBasicConsume();
        }

        public void Close()
        {
            if (channel == null | connection == null)
                return;

            if (channel.IsOpen)
                channel.Close();
            if (connection.IsOpen)
                connection.Close();
        }

        private void InitBasicConsume()
        {
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: "hello",
                                 noAck: true,
                                 consumer: consumer);
        }

        public static void MessageSend()
        {

        }

    }
}
