

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

    /// <summary>
    /// Default controller.
    /// </summary>
    public class DefaultController : ApiController
    {
        static FabricClient fabricClient = new FabricClient();
        

        static DefaultController()
        {
            
        }

        private long GetPartitionKey(string word)
        {
            return word.GetHashCode();
           // return ((long)char.ToUpper(word[0])) - 64;
        }

        [HttpGet]
        public HttpResponseMessage Index()
        {
            return this.View("StatelessGatewayWebAPIService.wwwroot.Index.html", "text/html");
        }

        [HttpGet]
        public async Task<IHttpActionResult> Count()
        {
            return Ok("TBD");
            ////(CDLTLL) Use of Fabric Client API to get the different PARTITIONS
            //ServicePartitionList partitions = await fabricClient.QueryManager.GetPartitionListAsync(new Uri("fabric:/StatefulGenericApp/StatefulWebAPIService"));

            //ConcurrentDictionary<Int64RangePartitionInformation, long> totals = new ConcurrentDictionary<Int64RangePartitionInformation, long>();
            //List<Task> tasks = new List<Task>(partitions.Count);

            ////We want to know totals per each Partition
            //foreach (var partition in partitions)
            //{
            //    tasks.Add(Task.Run(async () =>
            //    {
            //        //(CDLTLL) Use of Fabric Client API for name resolution of REPLICA addresses
            //        ServiceReplicaList replicas = await fabricClient.QueryManager.GetReplicaListAsync(partition.PartitionInformation.Id);

            //        Replica replica = replicas.First(item =>
            //        {
            //            StatefulServiceReplica primary = item as StatefulServiceReplica;
            //            return primary != null && primary.ReplicaStatus == ServiceReplicaStatus.Ready && primary.ReplicaRole == ReplicaRole.Primary;
            //        });


            //        Uri serviceAddress = new Uri(string.Format("{0}Count", replica.ReplicaAddress));

            //        HttpWebRequest request = WebRequest.CreateHttp(serviceAddress);
            //        request.Method = "GET";

            //        try
            //        {
            //            //Query my Controller /Count that has totals within the StatsDictionary in the Stateful Service
            //            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            //            {
            //                totals[partition.PartitionInformation as Int64RangePartitionInformation] = Int64.Parse(reader.ReadToEnd().Trim());
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Trace.WriteLine(ex);
            //        }
            //    }));
            //}

            //await Task.WhenAll(tasks);

            //StringBuilder sb = new StringBuilder();
            //sb.Append("<h1> Total:");
            //sb.Append(totals.Aggregate<KeyValuePair<Int64RangePartitionInformation, long>, long>(0, (total, next) => next.Value + total));
            //sb.Append("</h1>");
            //sb.Append("<table><tr><td>Partition ID</td><td>Key Range</td><td>Total</td></tr>");
            //foreach (var item in totals.OrderBy(item => item.Key.LowKey))
            //{
            //    sb.Append("<tr><td>");
            //    sb.Append(item.Key.Id);
            //    sb.Append("</td><td>");
            //    sb.AppendFormat("{0} - {1}", item.Key.LowKey, item.Key.HighKey);
            //    sb.Append("</td><td>");
            //    sb.Append(item.Value);
            //    sb.Append("</td></tr>");
            //}
            //sb.Append("</table>");

            //return Ok(sb.ToString());
        }

        //[HttpGet]
        //public async Task<IHttpActionResult> GetCurrentValueFromStatefulService()
        //{
        //    //(CDLTLL) Use of Fabric Client API to get the different PARTITIONS
        //    ServicePartitionList partitions = await fabricClient.QueryManager.GetPartitionListAsync(new Uri("fabric:/StatefulGenericApp/StatefulWebAPIService"));

        //    ConcurrentDictionary<Int64RangePartitionInformation, long> 
        //                values = new ConcurrentDictionary<Int64RangePartitionInformation, long>();

        //    List<Task> tasks = new List<Task>(partitions.Count);

        //    foreach (var partition in partitions)
        //    {
        //        tasks.Add(Task.Run(async () =>
        //        {
        //            //(CDLTLL) Use of Fabric Client API for name resolution of REPLICA addresses
        //            ServiceReplicaList replicas = await fabricClient.QueryManager.GetReplicaListAsync(partition.PartitionInformation.Id);

        //            Replica replica = replicas.First(item =>
        //            {
        //                StatefulServiceReplica primary = item as StatefulServiceReplica;
        //                return primary != null && primary.ReplicaStatus == ServiceReplicaStatus.Ready && primary.ReplicaRole == ReplicaRole.Primary;
        //            });

        //            Uri serviceAddress = new Uri(string.Format("{0}/CurrentValue", replica.ReplicaAddress));

        //            HttpWebRequest request = WebRequest.CreateHttp(serviceAddress);
        //            request.Method = "GET";

        //            try
        //            {
        //                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //                {
        //                    values[partition.PartitionInformation as Int64RangePartitionInformation] = Int64.Parse(reader.ReadToEnd().Trim());
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Trace.WriteLine(ex);
        //            }
        //        }));
        //    }

        //    await Task.WhenAll(tasks);
           
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<h1> Current Value:");
        //    sb.Append(values.Aggregate<KeyValuePair<Int64RangePartitionInformation, long>, long>(0, (value, next) => next.Value + value));
        //    sb.Append("</h1>");
        //    sb.Append("<table><tr><td>Partition ID</td><td>Key Range</td><td>Value</td></tr>");
        //    foreach (var item in values.OrderBy(item => item.Key.LowKey))
        //    {
        //        sb.Append("<tr><td>");
        //        sb.Append(item.Key.Id);
        //        sb.Append("</td><td>");
        //        sb.AppendFormat("{0} - {1}", item.Key.LowKey, item.Key.HighKey);
        //        sb.Append("</td><td>");
        //        sb.Append(item.Value);
        //        sb.Append("</td></tr>");
        //    }
        //    sb.Append("</table>");

        //    return Ok(sb.ToString());
        //}

        //[HttpPost]
        //public async Task<IHttpActionResult> SendValue(string word)
        //{
        //    for (int i = 0; i < 3; ++i)
        //    {
        //        Tuple<string, Guid> endpoint = await notifier.GetAddressAsync(GetPartitionKey(word), CancellationToken.None);

        //        Uri serviceAddress = new Uri(string.Format("{0}/{1}/{2}", endpoint.Item1, "SendValue", word));

        //        HttpWebRequest request = WebRequest.CreateHttp(serviceAddress);
        //        request.Method = "PUT";
        //        request.ContentLength = 0;

        //        try
        //        {
        //            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //            {
        //                return Ok(String.Format("<h1>{0}</h1> added to partition <h2>{1}</h2> at {2}",
        //                    word,
        //                    endpoint.Item2,
        //                    endpoint.Item1
        //                    ));
        //            }
        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse errorResponse = we.Response as HttpWebResponse;

        //            if (we.Status == WebExceptionStatus.ProtocolError)
        //            {
        //                int statusCode = (int)errorResponse.StatusCode;

        //                if (statusCode == 404)
        //                {
        //                    // this could either mean we requested an endpoint that does not exist in the service API (a user error)
        //                    // or the address that was resolved by fabric client is stale (transient runtime error) in which we should re-resolve.

        //                    continue;
        //                }
        //            }

        //            if (we.Status == WebExceptionStatus.Timeout ||
        //                we.Status == WebExceptionStatus.RequestCanceled ||
        //                we.Status == WebExceptionStatus.ConnectionClosed ||
        //                we.Status == WebExceptionStatus.ConnectFailure)
        //            {
        //                continue;
        //            }

        //            throw;
        //        }
        //    }

        //    return this.InternalServerError();
        //}
    }
}
