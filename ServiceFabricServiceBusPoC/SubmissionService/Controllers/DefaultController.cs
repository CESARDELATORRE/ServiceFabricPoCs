
namespace SubmissionService.Controllers
{
    using System;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Diagnostics;
    //(CDLTLL)    
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System.Configuration;

    /// <summary>
    /// Default controller.
    /// </summary>
    public class DefaultController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Cheers mate!");
        }

        [HttpGet]
        public IHttpActionResult SubmitCommandMessageAzureServiceBus()
        {
            string outputInfo;           
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"].ToString();

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            QueueDescription queueDescription = null;
            if (!namespaceManager.QueueExists("ingresscommands"))
            {                
                queueDescription = new QueueDescription("ingresscommands");
                queueDescription.EnablePartitioning = true;  //Partitioning Queues is critical for HA
                namespaceManager.CreateQueue(queueDescription);
            }

            //Queue to work with
            QueueClient queue = QueueClient.CreateFromConnectionString(connectionString, "ingresscommands");

            //Message creation
            string messageBody = "Command Name: SendEmailCampaign - Time: " + System.DateTime.Now.ToShortTimeString();
            string messageId = System.Guid.NewGuid().ToString();
            BrokeredMessage message = new BrokeredMessage(messageBody);
            message.MessageId = messageId;

            //Adding properties to the message header in order to identify the Command
            message.Properties.Add("CommandName", "SendEmailCampaignCommand");
            message.Properties.Add("OriginalBC", "MainBC");

            //Send the message with transient errors handling   
            while (true)
            {
                try
                {
                    queue.Send(message);
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
                outputInfo = string.Format("Message sent to queue: Body = {0}, Id = {1}", message.GetBody<string>(), message.MessageId);
                Trace.WriteLine(outputInfo);
                break;
            }

            return Ok(outputInfo);            
        }

        private static void HandleTransientErrors(MessagingException e)
        {
            //If transient error/exception, let's back-off for 2 seconds and retry
            Trace.WriteLine(e.Message);
            Trace.WriteLine("Will retry sending the message in 2 seconds");            
            Thread.Sleep(2000);
        }
    }
}
