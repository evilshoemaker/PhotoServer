using NLog;
using PhotoServer.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.PhotoShoot
{
    public class RabbitMqPhotoShootQueue : IPhotoShootQueue
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public event PhotoShootDelegate NewPhotoShoot;

        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        RabbitMqSettings settings;

        public RabbitMqPhotoShootQueue(RabbitMqSettings settings)
        {
            this.settings = settings;
            factory = new ConnectionFactory();
        }

        public void Connect()
        {
            Close();

            factory.UserName = settings.UserName;
            factory.Password = settings.Password;
            factory.VirtualHost = settings.VirtualHost;
            factory.HostName = settings.HostName;

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            InitBasicConsume();
            logger.Info("RabbitMq client connect");
        }

        public void Close()
        {
            if (channel == null | connection == null)
                return;

            if (channel.IsOpen)
                channel.Close();
            if (connection.IsOpen)
                connection.Close();

            channel.Dispose();
            connection.Dispose();
        }

        private void InitBasicConsume()
        {
            channel.QueueDeclare(queue: settings.Queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnReceived;
            channel.BasicConsume(queue: settings.Queue,
                                 noAck: false,
                                 consumer: consumer);
        }

        private void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            byte[] body = e.Body;
            string json = Encoding.UTF8.GetString(body);

            PhotoShoot photoShoot = null;

            try
            {
                photoShoot = PhotoShoot.Deserialize(json);
                NewPhotoShoot?.Invoke(photoShoot);
                channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
            catch (CameraDisconnectException ex)
            {
                logger.Error(ex.Message + " Stend: " + photoShoot.Stend + "; Camera: " + photoShoot.CameraId);
                channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: settings.Requeue);
                if (photoShoot != null)
                {
                    PhotoDisconnect photoDisconnect = new PhotoDisconnect(stend: photoShoot.Stend, camera: photoShoot.CameraId);
                    SafetySend(settings.CameraDisconnectQueue, photoDisconnect.Json(), settings);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: settings.Requeue);
            }
        }

        public static bool SafetySend(string queue, string message, RabbitMqSettings settings)
        {
            try
            {
                Send(queue, message, settings);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        public static void Send(string queue, string message, RabbitMqSettings settings)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = settings.UserName;
            factory.Password = settings.Password;
            factory.VirtualHost = settings.VirtualHost;
            factory.HostName = settings.HostName;

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                byte[] body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
