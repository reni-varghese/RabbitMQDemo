using EmployeeApp.Api.Events;
using RabbitMQ.Client;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.Json;

namespace EmployeeApp.Api.Messaging
{
    public class RabbitMqPublisher :IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly string _queueName;

        public RabbitMqPublisher(IConfiguration config) 
        { 
            var rabbitMqConfig=config.GetSection("RabbitMq");
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqConfig["HostName"]!,
                Port= int.Parse(rabbitMqConfig["Port"]!),
                UserName = rabbitMqConfig["UserName"]!,
                Password = rabbitMqConfig["Password"]!,
                VirtualHost = rabbitMqConfig["VirtualHost"]!    

            };
            _queueName = rabbitMqConfig["EmployeeQueue"]!;
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.QueueDeclareAsync(

                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            ).GetAwaiter().GetResult();


        }

        public void Dispose()
        {
            _channel.CloseAsync();
            _connection.CloseAsync();
        }

        public async Task PublishAsync(EmployeeEvent employeeEvent)
        {
            var message=JsonSerializer.Serialize(employeeEvent);
            var body=Encoding.UTF8.GetBytes(message);
            var properties=new BasicProperties { Persistent = true };
            await _channel.BasicPublishAsync(
                exchange:string.Empty,
                routingKey:_queueName,
                mandatory:false,
                basicProperties:properties,
                body:body

                );
        }
       


    }
}
