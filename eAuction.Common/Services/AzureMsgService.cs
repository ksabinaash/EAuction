using Azure.Messaging.ServiceBus;
using eAuction.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    public class AzureMsgService : IAzureMsgService
    {
        private readonly ILogger<AzureMsgService> _logger;

        public AzureMsgService(ILogger<AzureMsgService> logger)
        {
            this._logger = logger;
        }

        public async Task SendMessage(string connectionString, string queueName, string message)
        {
            try
            {
                // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
                await using var client = new ServiceBusClient(connectionString);

                // create the sender
                ServiceBusSender sender = client.CreateSender(queueName);

                // create a message that we can send. UTF-8 encoding is used when providing a string.
                var serviceBusMessage = new ServiceBusMessage(message);

                // send the message
                await sender.SendMessageAsync(serviceBusMessage);

                _logger.LogInformation("Message sent to Queue!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<string> ReceiveMessage(string connectionString, string queueName)
        {
          try
            {
                await using var client = new ServiceBusClient(connectionString);

                ServiceBusReceiverOptions options = new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete};
              
                // create a receiver that we can use to receive the message
                ServiceBusReceiver receiver = client.CreateReceiver(queueName, options);

                // the received message is a different type as it contains some service set properties
                ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

                // get the message body as a string
                string body = receivedMessage.Body.ToString();

                return body;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, ex.Message);

                return "";
            }
        }
    }
}