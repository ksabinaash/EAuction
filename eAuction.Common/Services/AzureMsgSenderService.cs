using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    public class AzureMsgSenderService
    {
        private string _connectionString = "";

        // name of your Service Bus queue
        private string _queueName = "";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient _client;

        // the sender used to publish messages to the queue
        static ServiceBusSender _sender;

        // number of messages to be sent to the queue
        public static int numOfMessages = 1;

        private readonly ILogger<AzureMsgSenderService> _logger;

        private readonly IConfiguration _config;

        public AzureMsgSenderService(ILogger<AzureMsgSenderService> logger, IConfiguration config)
        {
            this._logger = logger;

            this._config = config;

            this._connectionString = _config.GetSection("AzureSBConnectionString").Value;
        }

        public async Task SendMessage(string message, string queueName)
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            _client = new ServiceBusClient(_connectionString);

            _sender = _client.CreateSender(queueName);

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numOfMessages; i++)
            {
                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                {
                    var exception = new Exception($"The message {i} is too large to fit in the batch.");

                    _logger.LogWarning(exception, exception.Message);

                    // if it is too large for the batch
                    throw exception;
                }
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await _sender.SendMessagesAsync(messageBatch);

                _logger.LogInformation($"A batch of {numOfMessages} messages has been published to the queue.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                throw;
            }

            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await _sender.DisposeAsync();

                await _client.DisposeAsync();
            }
        }
    }
}