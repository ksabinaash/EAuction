using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    public class AzureMsgReceiverService
    {
        private string _connectionString = "";

        // name of your Service Bus queue
        private string _queueName = "";

        private string _message = "";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient _client;

        // the receiver used to publish messages to the queue
        static ServiceBusProcessor _processor;

        // number of messages to be sent to the queue
        public static int numOfMessages = 1;

        private readonly ILogger<AzureMsgReceiverService> _logger;

        private readonly IConfiguration _config;

        public AzureMsgReceiverService(ILogger<AzureMsgReceiverService> logger, IConfiguration config)
        {
            this._logger = logger;

            this._config = config;

            this._connectionString = _config.GetSection("AzureSBConnectionString").Value;
        }

        public async Task<string> ReceiveMessage(string queueName)
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            _client = new ServiceBusClient(_connectionString);

            _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                _processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                _processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await _processor.StartProcessingAsync();

                // stop processing 
                await _processor.StopProcessingAsync();

                return _message;
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await _processor.DisposeAsync();

                await _client.DisposeAsync();
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();

            _logger.LogInformation($"Received: {body}");

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);

            _message = body;
        }

        // handle any errors when receiving messages
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogWarning(args.Exception.ToString());

            return Task.CompletedTask;
        }
    }
}