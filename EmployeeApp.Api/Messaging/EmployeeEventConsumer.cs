using EmployeeApp.Api.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EmployeeApp.Api.Messaging
{
    public class EmployeeEventConsumer : BackgroundService
    {
        private readonly ILogger<EmployeeEventConsumer> _logger;
        private readonly IConfiguration _config;
        private  IConnection _connection;
        private  IChannel _channel;

        public EmployeeEventConsumer(ILogger<EmployeeEventConsumer> logger,IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var rabbitConfig = _config.GetSection("RabbitMq");
            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"]!,
                Port=int.Parse(rabbitConfig["Port"]!),
                UserName = rabbitConfig["UserName"]!,
                Password=rabbitConfig["Password"]!,
                VirtualHost = rabbitConfig["VirtualHost"]!

               
            };
            _connection=await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(null, cancellationToken);
            _logger.LogInformation("EmployeeConsumer Started. Listening at Queue+++++++++++++");
            await base.StartAsync(cancellationToken);
        }
       

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = _config.GetSection("RabbitMq")["EmployeeQueue"]!;

            await _channel.QueueDeclareAsync(

                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken

                );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var employeeEvent = JsonSerializer.Deserialize<EmployeeEvent>(message,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (employeeEvent != null)
                {
                    _logger.LogInformation("Employee Event {Id}, {Name},{Time}",
                        employeeEvent.EmployeeId, employeeEvent.EmployeeName,
                        employeeEvent.OccurredAt

                        );
                }
                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);

               
            };
            await _channel.BasicConsumeAsync(
                   queue: queueName,
                   autoAck: false,
                   consumer: consumer,
                   cancellationToken: stoppingToken



                   );
            await Task.Delay(Timeout.Infinite, stoppingToken);




        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmployeeEvent Consumer Stopping...");
            if(_channel != null) _channel.CloseAsync();
            if(_connection !=null) _connection.CloseAsync();


            await base.StopAsync(cancellationToken);
        }
    }
}
