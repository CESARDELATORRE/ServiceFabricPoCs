

namespace StatelessGatewayWebAPIService.Controllers
{
    using System;
    using System.Linq;
    using System.Fabric;
    using System.Fabric.Query;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.IO;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Concurrent;

    using StatelessGatewayWebAPIService.Commands;


    /// <summary>
    /// Default controller.
    /// </summary>
    public class CustomerController : ApiController
    {
        static FabricClient fabricClient = new FabricClient();

        /// <summary>
        /// This is the service name of the Stateful service.
        /// </summary>
        private readonly Uri statefulServiceName = new Uri("fabric:/StatefulGenericApp/StatefulWebAPIService");

        static CustomerController()
        {
            
        }

        private long GetPartitionKey(string originalKey)
        {
            return originalKey.GetHashCode();
            // return ((long)char.ToUpper(word[0])) - 64;
        }


        //// GET /customers/MSFT
        [HttpGet]
        [Route("customers/{customerKey}/{stateCode}", Name = "GetCustomer")]
        public async Task<IHttpActionResult> GetCustomer(string customerKey, string stateCode)
        {
            //(TBD)
            return Ok(0);

            // Use Fabric Client API to Resolve the Stateful Service Partition to access.
            // The Service partition that we want to connect to can move around, which means the address can change at any time.
            // This is expected, but when it happens we need to re-resolve the address,
            // which happens in the ResolveServicePartitionAsync loop.            

            //bool isConnected = false;
            //bool shouldDelay = false;
            //TimeSpan timeout = TimeSpan.FromMilliseconds(250);
            //ResolvedServicePartition currentRsp = null;

            //while (isConnected == false)
            //{
            //    try
            //    {
            //        ResolvedServicePartition tmpRsp = await fabricClient.ServiceManager.ResolveServicePartitionAsync(
            //            this.statefulServiceName,
            //            GetPartitionKey(stateCode),
            //            currentRsp);

            //        ResolvedServiceEndpoint resolvedServiceEndpoint = tmpRsp.GetEndpoint();
            //        string baseServiceAddress = resolvedServiceEndpoint.Address;

            //        // serviceAddress is the endpoint address of the Stateful service partition including the key.                                    
            //        Uri serviceAddress = new Uri(string.Format("{0}Customers/{1}", baseServiceAddress, customerKey));
            //        Trace.WriteLine("Service Address for " + customerKey + " is: " + serviceAddress.AbsoluteUri);

            //        HttpWebRequest request = WebRequest.CreateHttp(serviceAddress);
            //        request.Method = "GET";

            //        try
            //        {
            //            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //            if (response.StatusCode == HttpStatusCode.OK)
            //            {
            //                isConnected = true;

            //                Trace.WriteLine("Content type is {0}", response.ContentType);

            //                // Get the stream associated with the response.
            //                Stream receiveStream = response.GetResponseStream();

            //                // Pipes the stream to a higher level stream reader with the required encoding format. 
            //                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            //                Trace.WriteLine("Response stream received.");
            //                String retVal = readStream.ReadToEnd();
            //                Trace.WriteLine(retVal);

            //                response.Close();
            //                readStream.Close();

            //                return Ok(retVal);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Trace.WriteLine(ex);
            //        }

            //        if (isConnected == false)
            //        {
            //            if (currentRsp != null && tmpRsp.CompareVersion(currentRsp) == 0)
            //            {
            //                shouldDelay = true;
            //            }

            //            currentRsp = tmpRsp;
            //        }
            //    }
            //    catch (FabricTransientException fte)
            //    {
            //        Trace.WriteLine(fte);
            //        shouldDelay = true;
            //    }

            //    if (shouldDelay == true)
            //    {
            //        await Task.Delay(timeout);
            //        shouldDelay = false;
            //    }
            //}

            //return Ok(0);

        }

        [HttpGet]
        [Route("customers/test")]
        public IHttpActionResult RunTest()
        {
            return Ok("Cheers my!");
        }

        
        [HttpPost]
        [Route("customers/postaddorupdatecustomercommand")]
        public async Task<IHttpActionResult> PostAddorUpdateCustomerCommand(AddorUpdateCustomerCommand addorUpdateCustCommand)
        {
            //(TBD)
            return Ok(0);

            //Trace.WriteLine("Cutomer to Update thru Command: " + addorUpdateCustCommand.CustomerKey + " " + addorUpdateCustCommand.CompanyName);
            ////return Ok(0);

            //Tuple<string, Guid> endpoint;

            //for (int i = 0; i < 3; ++i)
            //{
            //    try
            //    {
            //        //The Partition KEY is a hash based on the "stateCode", in this case
            //        long hashPartitionKey = GetPartitionKey(addorUpdateCustCommand.StateCode);
            //        Trace.WriteLine("StateCode: " + addorUpdateCustCommand.StateCode + " is Hash: " + hashPartitionKey.ToString());
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
            //                                                addorUpdateCustCommand.CustomerKey,
            //                                                "addorupdate",
            //                                                addorUpdateCustCommand.CompanyName,
            //                                                addorUpdateCustCommand.ZipCode,
            //                                                addorUpdateCustCommand.StateCode,
            //                                                addorUpdateCustCommand.CountryCode,
            //                                                addorUpdateCustCommand.ContactFullName,
            //                                                addorUpdateCustCommand.ContactEmail
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
            //            String retVal = String.Format("<h1>{0}</h1> added to partition <h2>{1}</h2> at {2}",
            //                addorUpdateCustCommand.CompanyName,
            //                endpoint.Item2,
            //                endpoint.Item1
            //                );

            //            Trace.WriteLine(retVal);

            //            return Ok(retVal);
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

            //return this.InternalServerError();
        }


        //POST 
        [HttpPost]
        [Route("customers/{customerKey}/addorupdate/{companyName}/{zipCode}/{stateCode}/{countryCode}/{contactFullName}/{contactEmail}")]
        public async Task<IHttpActionResult> addOrUpdate(string customerKey,
                                                         string companyName,
                                                         string zipCode,
                                                         string stateCode,
                                                         string countryCode,
                                                         string contactFullName,
                                                         string contactEmail
                                                        )
        {
            AddorUpdateCustomerCommand addorUpdateCustCommand = new AddorUpdateCustomerCommand();
            addorUpdateCustCommand.DateOfCommandSubmission = System.DateTime.Now;
            addorUpdateCustCommand.CustomerKey = customerKey;
            addorUpdateCustCommand.CompanyName = companyName;
            addorUpdateCustCommand.ZipCode = Convert.ToUInt32(zipCode);
            addorUpdateCustCommand.StateCode = stateCode;
            addorUpdateCustCommand.CountryCode = countryCode;
            addorUpdateCustCommand.ContactFullName = contactFullName;
            addorUpdateCustCommand.ContactEmail = contactEmail;
                                                      
            return await PostAddorUpdateCustomerCommand(addorUpdateCustCommand);
            
        }
    }
}
