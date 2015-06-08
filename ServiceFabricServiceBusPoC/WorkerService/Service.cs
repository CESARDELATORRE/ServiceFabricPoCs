using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

//(CDLTLL)    
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;

namespace CommandHandlerService
{
    public class Service : StatelessService
    {
        public const string ServiceTypeName = "CommandHandlerServiceType";

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("Starting CommandHandler service.");

            //Boolean to control messages' process. False if I don't want to process/empty the queue
            bool processMessages = true;

            // In this scenario we are Polling for messages in the queue
            // For "message pumping" or event driven messages based on queues, check:
            // http://fabriccontroller.net/blog/posts/introducing-the-event-driven-message-programming-model-for-the-windows-azure-service-bus/
            // http://azure.microsoft.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-queues/#comments

            cancellationToken.ThrowIfCancellationRequested();
            Trace.WriteLine("Starting to poll for messages from the queue");            
            BrokeredMessage commandMessage = null;            
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"].ToString();

            // Create the SB-Topic (EventBus) in Azure with certain configuration
            //The topic will be used to publish events after commands are processed (like publishing integration events for other Bounded-Contexts or Apps)
            TopicDescription td = new TopicDescription("EventBus");
            td.EnablePartitioning = true; //Partitioning Topics is critical for HA
            td.MaxSizeInMegabytes = 5120;
            td.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.TopicExists("EventBus"))
            {
                namespaceManager.CreateTopic(td);
            }

            //Queue to work with
            QueueClient queue = QueueClient.CreateFromConnectionString(connectionString, "ingresscommands");

            //Take and Process messages from the Command-Queue
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    //receive messages from Queue // 5 seconds is the time span the server waits for receiving a message before it times out.
                    commandMessage = queue.Receive(TimeSpan.FromSeconds(5));
                    if (commandMessage != null & processMessages)
                    {
                        Trace.WriteLine(string.Format("Command-Message received: Id = {0}, Body = {1}", commandMessage.MessageId, commandMessage.GetBody<string>()));
                        // Custom message processing could go here...
                        // Like transactions in a DB storing the Command operations, etc.

                        //Depending on CommandName we should handle a different process
                        string commandName = commandMessage.Properties["Command"].ToString();

                        //Publish an Event since the Command process is completed
                        // Create the event message, passing a string message for the body
                        string eventMessageBody = "Event Name: EmailCampaignSent - Time: " + System.DateTime.Now.ToShortTimeString();
                        BrokeredMessage eventMessage = new BrokeredMessage(eventMessageBody);
                        // Set additional custom app-specific property

                        eventMessage.Properties["MessageType"] = "Event";
                        eventMessage.Properties["OriginalBC"] = commandMessage.Properties["OriginalBC"];
                        eventMessage.Properties["EventName"] = "EmailCampaignSent";
                        eventMessage.Properties["OriginalCommandId"] = commandMessage.MessageId;
                        
                        // Publish Event to the topic "EventBus"
                        TopicClient eventBusTopic = TopicClient.CreateFromConnectionString(connectionString, "EventBus");
                        eventBusTopic.Send(eventMessage);  // ;) This method should be called Publish() rather than Send() as it can have many subscriptions... ;)
                        Trace.WriteLine(string.Format("Event-Message published: EventId = {0}, Body = {1}, OriginalCommandId = {2}, EventName = {3}", eventMessage.MessageId, eventMessage.GetBody<string>(), eventMessage.Properties["OriginalCommandId"], eventMessage.Properties["EventName"]));

                        //Message processing from the queue is completed so unlock and mark as complete
                        commandMessage.Complete();
                    }                    
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }
                    else
                    {
                        HandleTransientErrors(e);
                    }
                }
                //Wait 5 seconds
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
            queue.Close();

            //Dumb code commented
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    cancellationToken.ThrowIfCancellationRequested();
            //    Trace.WriteLine("Check from CommandHandler Service at " + DateTime.Now.ToLongTimeString());
            //    await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            //}

            //(CDLTLL) return Task.FromResult(true);
        }

        private static void HandleTransientErrors(MessagingException e)
        {
            //If transient error/exception, let's back-off for 2 seconds and retry
            Trace.WriteLine(e.Message);
            Trace.WriteLine("Will retry polling the queue in 2 seconds");
            Thread.Sleep(2000);
        }

    }


}

