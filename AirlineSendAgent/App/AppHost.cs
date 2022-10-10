using System.Text;
using System.Text.Json;
using AirlineSendAgent.Client;
using AirlineSendAgent.Dtos;
using AirlineWeb.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TravelAgentWeb.Dtos;

namespace AirlineSendAgent.App
{
    public class AppHost : IAppHost
    {
        private readonly SendAgentDbContext _context;
        private readonly IWebhookClient _webhookClient;

        public AppHost(SendAgentDbContext context, IWebhookClient webhookClient)
        {
            _context = context;
            _webhookClient = webhookClient;
        }
        public void Run()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,

            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "trigger",
                type: ExchangeType.Fanout
                );
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "trigger",
                routingKey: string.Empty
                );
                var consumer = new EventingBasicConsumer(channel);
                Console.WriteLine("Listening on the message bus...");

                consumer.Received += async (ModuleHandle, ea) =>
                {
                    //get message published on the queue...
                    Console.WriteLine("Event is triggered!");
                    var body = ea.Body;
                    var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                    var message = JsonSerializer.Deserialize<NotificationMessageDto>(notificationMessage);

                    var webhookToSend = new FlightDetailChangePayloadDto()
                    {
                        WebhookType = message.WebhookType,
                        WebhookURI = string.Empty,
                        OldPrice = message.OldPrice,
                        NewPrice = message.NewPrice,
                        Secret = string.Empty,
                        Publisher = string.Empty,
                        FlightCode = message.FlightCode
                    };

                    //we loop through our database, we check all our subscriptions and then
                    //augment this data for each of them and then send it over...
                    foreach (var webhookSubscription in _context.WebhookSubscriptions.Where(x => x.WebhookType.Equals(message.WebhookType)))
                    {
                        webhookToSend.WebhookURI = webhookSubscription.WebhookURI;
                        webhookToSend.Secret = webhookSubscription.Secret;
                        webhookToSend.Publisher = webhookSubscription.WebhookPublisher;

                        await _webhookClient.SendWebhookNotification(webhookToSend);
                    }

                };

                channel.BasicConsume(queueName, autoAck: true, consumer: consumer);
                Console.ReadLine();


            }
        }
    }
}