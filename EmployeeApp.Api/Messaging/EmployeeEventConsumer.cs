using EmployeeApp.Api.Events;
using MassTransit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EmployeeApp.Api.Messaging
{
    public class EmployeeEventConsumer : IConsumer<EmployeeEvent>
    {
        private readonly ILogger<EmployeeEventConsumer> _logger;
        public EmployeeEventConsumer(ILogger<EmployeeEventConsumer> logger)
        {
            _logger = logger;

        }

        public Task Consume(ConsumeContext<EmployeeEvent> context)
        {
            var employeeEvent = context.Message;
            _logger.LogInformation("Employee Event : {Type}, {It}, {Name}, {Time}===========",

                employeeEvent.EventType, employeeEvent.EmployeeId, employeeEvent.EmployeeName,
                employeeEvent.OccurredAt
                );

            return Task.CompletedTask;

               
        }
    }
}
