using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Vendas.Core.Options;
using System;
using Microsoft.Extensions.Options;

namespace Vendas.Domain.Services
{
   
    public class RabbitMQService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public readonly RabbitMQSettings _settings;

        public RabbitMQService(IOptions<RabbitMQSettings> settings)
        {
            _settings = settings.Value;

            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password,
                Port = _settings.Port,
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _settings.CompraCriadaQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: _settings.CompraAlteradaQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: _settings.CompraCanceladaQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishEvent<T>(T eventMessage, string queueName)
        {
            var message = JsonConvert.SerializeObject(eventMessage);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }

}
