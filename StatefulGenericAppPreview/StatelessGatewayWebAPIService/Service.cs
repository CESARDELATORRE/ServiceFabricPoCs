using System;
using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Net;
using System.Web.Http;
using System.Diagnostics;

namespace StatelessGatewayWebAPIService
{
    public class Service : StatelessService
    {
        public const string ServiceTypeName = "StatelessGatewayWebAPIServiceType";

        // CHECK AND DELETE
        //static FabricClient fabricClient = new FabricClient();
        //private readonly Uri statefulServiceName = new Uri("fabric:/StatefulGenericApp/StatefulWebAPIService");
        //static IAddressChangeNotifier<long> notifier = new AddressChangeNotifier<long>("fabric:/StatefulGenericApp/StatefulWebAPIService", true);
        //

        /// <summary>
        /// Creates a listener for Web API.
        /// </summary>
        /// <returns></returns>
        protected override ICommunicationListener CreateCommunicationListener()
        {
            return new OwinCommunicationListener("StatelessGatewayWebAPIService", new Startup());
        }

        //(CDLTLL) If not Async:
        //protected override Task RunAsync(CancellationToken cancellationToken)        
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(15000)); //wait 15 secs

            //TEST Customer Add-Update
            //Customer customer = new Customer("MSFT", "Microsoft", 98052, "WA", "US", "Satya Nadella", "satya.nadella@microsoft.com");
            //Tuple<string, Guid> endpoint;

            //for (int i = 0; i < 3; ++i)
            //{
            //    try
            //    {
            //        long hashPartitionKey = GetPartitionKey("WA");
            //        endpoint = await notifier.GetAddressAsync(hashPartitionKey, CancellationToken.None);
            //    }
            //    catch (Exception e)
            //    {
            //        Trace.WriteLine(e.Message);
            //        throw;
            //    }

            //    //PUT / Service's Controller definition
            //    //[HttpPut]
            //    //[Route("customers/{customerKey}/addorupdate/{companyName}/{zipCode}/{stateCode}/{countryCode}/{contactFullName}/{contactEmail}", Name = "AddorUpdateCustomer")]

            //    Trace.WriteLine("endpoint.Item1: " + endpoint.Item1);
            //    Trace.WriteLine("endpoint.Item2: " + endpoint.Item2);                

            //    Uri serviceAddress = new Uri(string.Format("{0}{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}", 
            //                                                endpoint.Item1, 
            //                                                "customers",
            //                                                "MSFT", 
            //                                                "addorupdate",
            //                                                "Microsoft",
            //                                                98052,
            //                                                "WA",
            //                                                "US",
            //                                                "Satya Nadella",
            //                                                "ratya.nadella@microsoft.com"
            //                                                ));

            //    Trace.WriteLine("AbsoluteUri: " + serviceAddress.AbsoluteUri);
            //    Trace.WriteLine("AbsolutePath: " + serviceAddress.AbsolutePath);                

            //    HttpWebRequest request = WebRequest.CreateHttp(serviceAddress);
            //    request.Method = "PUT";
            //    request.ContentLength = 0;

            //    try
            //    {
            //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //        {
            //            Trace.WriteLine(String.Format("<h1>{0}</h1> added to partition <h2>{1}</h2> at {2}",
            //                "Microsoft",
            //                endpoint.Item2,
            //                endpoint.Item1
            //                ));

            //            return;
            //        }
            //    }
            //    catch (WebException we)
            //    {
            //        Trace.WriteLine("Exception when calling Web API Service: " + we.Message);
                    

            //        HttpWebResponse errorResponse = we.Response as HttpWebResponse;

            //        if (we.Status == WebExceptionStatus.ProtocolError)
            //        {
            //            int statusCode = (int)errorResponse.StatusCode;

            //            if (statusCode == 404)
            //            {
            //                // this could either mean we requested an endpoint that does not exist in the service API (a user error)
            //                // or the address that was resolved by fabric client is stale (transient runtime error) in which we should re-resolve.

            //                continue;
            //            }
            //        }

            //        if (we.Status == WebExceptionStatus.Timeout ||
            //            we.Status == WebExceptionStatus.RequestCanceled ||
            //            we.Status == WebExceptionStatus.ConnectionClosed ||
            //            we.Status == WebExceptionStatus.ConnectFailure)
            //        {
            //            continue;
            //        }

            //        throw;
            //    }
            //}

            //return Task.FromResult(true);
        }

        //private long GetPartitionKey(string originalKey)
        //{
        //    return originalKey.GetHashCode();
        //    // return ((long)char.ToUpper(word[0])) - 64;
        //}


    }

    
}
