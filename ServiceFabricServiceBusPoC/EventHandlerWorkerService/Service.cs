using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


//(CDLTLL)    
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using System.Diagnostics;

namespace EventHandlerService
{
    //Azure Service Bus Topics (Pub/subs model): http://azure.microsoft.com/EN-US/documentation/articles/service-bus-dotnet-how-to-use-topics-subscriptions/

    public class Service : StatelessService
    {
        public const string ServiceTypeName = "EventHandlerServiceType";

        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("Starting Event-Handler service.");
            cancellationToken.ThrowIfCancellationRequested();

            // My SB Connection String
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"].ToString();            
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            string topicName = "EventBus";
            string subscriptionName = "AllMessagesForMainBoundedContext";

            // Create the Subscription "AllMessagesForMainBoundedContext" to the SB-Topic "EventBus" if it doesn't exist yet.
            if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                namespaceManager.CreateSubscription(topicName, subscriptionName);
            }

            //Get messages thru channel/subscription "AllMessagesForMainBoundedContext"
            SubscriptionClient subscription = SubscriptionClient.
                                                    CreateFromConnectionString(connectionString, topicName, subscriptionName);

            // Configure the callback options
            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromSeconds(30);

            subscription.OnMessage((eventMessage) =>
            {
                try
                {
                    // Process message from subscription
                    Trace.WriteLine("*******************************************************");
                    Trace.WriteLine("** Event-Message Received **");
                    Trace.WriteLine("** PROCESS Event from AllMessagesForMainBoundedContext Subscription in SB-Topic EventBus**");
                    Trace.WriteLine("Body: " + eventMessage.GetBody<string>());
                    Trace.WriteLine("MessageID: " + eventMessage.MessageId);
                    Trace.WriteLine("Event Name: " + eventMessage.Properties["EventName"]);
                    Trace.WriteLine("Original CommandId: " + eventMessage.Properties["OriginalCommandId"]);
                    Trace.WriteLine("*******************************************************");

                    // Remove message from subscription
                    eventMessage.Complete();
                }
                catch (Exception)
                {
                    // Indicates a problem, unlock message in subscription
                    eventMessage.Abandon();
                }
            }, options);

            //Dumb code commented
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    cancellationToken.ThrowIfCancellationRequested();
            //    Trace.WriteLine("Future Event Processing tasks at " + DateTime.Now.ToLongTimeString());
            //    await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            //}

            //(CDLTLL) 
            return Task.FromResult(true);            
        }
    }
}
