
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Fabric;
using System.Fabric.Query;
using Microsoft.ServiceFabric.Services;

using ScoreBig.SeatMapModel;

namespace ScoreBig.OperationsClientConsoleApp
{
    class Program
    {
        //CONSTANTS FOR STATELESS GATEWAY ***************************
        private const string SeatMapGatewayServiceName = "fabric:/ScoreBig.SeatMapStatefulApplication/ScoreBigSeatMapGateway";
        private const string SeatMapGatewayServiceUrl = "http://localhost:88/scorebigseatmapgateway/";

        //CONSTANTS FOR STATEFUL SERVICE USAGE **********************
        //private const string SeatMapStatefulServiceName = "fabric:/ScoreBig.SeatMapStatefulApplication/ScoreBigSeatMapStateful";
        //private const int MaxQueryRetryCount = 20;
        //private static TimeSpan BackoffQueryDelay = TimeSpan.FromSeconds(3);
        //private static FabricClient fabricClient = new FabricClient();

        //private static CommunicationClientFactory clientFactory = new CommunicationClientFactory(
        //    ServicePartitionResolver.GetDefault(),
        //    TimeSpan.FromSeconds(10),
        //    TimeSpan.FromSeconds(3));

        public enum TicketClass
        {
            Unrated = 0,
            Rating1 = 1,
            Rating2 = 2,
            Rating3 = 3,
            Rating4 = 4,
            Rating5 = 5,
            Rating6 = 6,
            Rating7 = 7,
            Rating8 = 8,
            Rating9 = 9,
            Rating10 = 10,
            Rating11 = 11
        }

        private static void AddOrUpdateSeatMap(SeatMap seatMap)
        {
            Uri serviceAddress = new Uri(SeatMapGatewayServiceUrl + "AddOrUpdateSeatMap");
            HttpWebRequest req = WebRequest.CreateHttp(serviceAddress);

            string data = JsonConvert.SerializeObject(seatMap);
            //This gives you the byte array.

            var dataToSend = Encoding.UTF8.GetBytes(data);

            req.ContentType = "application/json";
            req.ContentLength = dataToSend.Length;
            req.Method = "POST";

            req.GetRequestStream().Write(dataToSend, 0, dataToSend.Length);

            var response = req.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine("********************************");
            Console.WriteLine(responseFromServer);
            Console.WriteLine(" ");

        }

        private static void LoadTestData()
        {

            //Read Sample data for a generic SeatMap (All SeatMaps will have the same sample data..)
            var random = new Random();
            var data = File.ReadAllText(".\\seatmap.txt");
            var ratingTiers = data.Split('\n').ToList().Select(r => new RatingTier()
            {
                Section = r.Substring(0, r.IndexOf("|")),
                Row = r.Substring(r.IndexOf("|") + 1),
                FaceValue = random.Next(20, 1000),
                Id = Guid.NewGuid().ToString(),  //Hardcoded as RatingTier Id is not coming in the file
                Rating = (uint) Convert.ToUInt32(Enum.Parse(typeof(TicketClass), (random.Next(1, 12)).ToString())),
            });

            SeatMap seatMap = new SeatMap();
            //seatMap.Id = "37d7aff3-1fc9-4500-9467-ef3b6e0a2093";  //Hardcoded as SeatMap Id is not coming in the file
            //seatMap.VenueId = Guid.NewGuid().ToString();  //Hardcoded as VenueId is not coming in the file
            seatMap.Name = "My SeatMap Name for testing";

            seatMap.RatingTiers = ratingTiers.ToDictionary(rt => rt.Id, rt => rt);
            //NO DUPLICATES seatMap.RatingTiers = ratingTiers.Distinct().ToDictionary(rt => rt.Id, rt => rt);

            //Create a List of SeatMaps based on an Event List file (need GUIDs and Venue names from the file)
            //Read Event Ids as SeatMap Ids
            //In reality that file is a list of Events, 
            //but I'm using it as if it were a list of SeatMaps with Id and Venue info
            var filename = @".\events.txt";            

            var lines = File.ReadAllLines(filename);
            var seatMapsList = new List<SeatMap>();
            lines.Skip(1).ToList().ForEach(r => {
                var cols = r.Split(new[] { ',' });
                
                seatMapsList.Add(new SeatMap()
                {
                      Id = Guid.Parse(cols[0]).ToString(),
                //    Name = cols[1],
                      VenueName = cols[2],
                //    LocalDateTime = DateTime.Parse(cols[3])
                });
            });


            foreach (var e in seatMapsList)
            {
                //Re-use the same SeatMap data but changing the SeatMap Id and Venue name and Id
                seatMap.Id = e.Id;
                seatMap.VenueName = e.VenueName;
                seatMap.VenueId = Guid.NewGuid().ToString();

                //Upload a SeatMap data to the SERVICE FABRIC Stateful Service
                AddOrUpdateSeatMap(seatMap);

            }
            Console.WriteLine("***** Updated SeatMaps: " + seatMapsList.Count + "*****");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting processes...");

            LoadTestData();

            //HARDCODED SEATMAPS
            //string seatMapId1 = "00000000-0000-0000-0000-000000000001";

            ////Group or Rating Tiers
            //RatingTier ratingTier1_1 = new RatingTier("c88e9ca2-88fd-4331-866d-9fd300f838e7",
            //                                          "MEZ",
            //                                          "1",
            //                                          4,
            //                                          10.0000
            //                                         );

            //RatingTier ratingTier1_2 = new RatingTier("22222222-88fd-4331-866d-9fd300f838e7",
            //                                          "MEZ",
            //                                          "2",
            //                                          5,
            //                                          8.0000
            //                                         );

            //RatingTier ratingTier1_3 = new RatingTier("d9373617-e247-47de-87ca-9fd300f83917",
            //                                          "ORC",
            //                                          "3",
            //                                          10,
            //                                          40.0000
            //                                         );

            //IDictionary<string, RatingTier> ratingTiersDictionary1 = new Dictionary<string, RatingTier>();
            //ratingTiersDictionary1.Add("c88e9ca2-88fd-4331-866d-9fd300f838e7", ratingTier1_1);
            //ratingTiersDictionary1.Add("d9373617-e247-47de-87ca-9fd300f83917", ratingTier1_2);

            //SeatMap seatMap1 = new SeatMap(seatMapId1, "00000000-0000-0000-0000-111111111111", "LA Kings Hockey Seat Map",
            //                               ratingTiersDictionary1
            //                              );

            //string seatMapId2 = "00000000-0000-0000-0000-000000000002";
            //SeatMap seatMap2 = new SeatMap(seatMapId2, "00000000-0000-0000-0000-222222222222", "MSG All-Star 2019",
            //                               null
            //                              );


            //string seatMapId3 = "46211f91-14a8-4353-999b-4ddd03993939";
            //SeatMap seatMap3 = new SeatMap(seatMapId3, "00000000-0000-0000-0000-333333333333", "Staples Center Default Seat Map",
            //                               null
            //                              );


            //string seatMapId4 = "2f7ebe15-43fd-42d2-aedf-542c9ef9cad3";
            //SeatMap seatMap4 = new SeatMap(seatMapId4, "00000000-0000-0000-0000-444444444444", "Seattle Safeco Arena Seatmap",
            //                               null
            //                              );


            //string seatMapId5 = "c8fbfc02-a33b-4690-ac80-862ea76d9a5a";
            //SeatMap seatMap5 = new SeatMap(seatMapId5, "98c1edc2-a633-45e9-8755-18933b590bf7", "Real Madrid Santiago Bernabeu",
            //                               null
            //                              );
            //AddOrUpdateSeatMap(seatMap1);
            //AddOrUpdateSeatMap(seatMap2);
            //AddOrUpdateSeatMap(seatMap3);
            //AddOrUpdateSeatMap(seatMap4);
            //AddOrUpdateSeatMap(seatMap5);


            //**************************************************************************************************
            // UPDATING DIRECTLY AGAINTS STATEFUL SERVICE 
            // Determine the partition key that should handle the request

            //long hashPartitionKey = seatMap1.Id.GetHashCode();                    
            //Trace.WriteLine("SeatMapId: " + seatMap1.Id + " is Hash: " + hashPartitionKey.ToString());

            //// Use service partition client to resolve the service and partition key.
            //// This determines the endpoint of the replica that should handle the request.
            //// Internally, the service partition client handles exceptions and retries appropriately.
            //ServicePartitionClient<CommunicationClient> servicePartitionClient = new ServicePartitionClient<CommunicationClient>(
            //    clientFactory,
            //    new Uri(SeatMapStatefulServiceName),
            //    hashPartitionKey);

            //var retVal = servicePartitionClient.InvokeWithRetryAsync(
            //                client =>
            //                {                                
            //                    // Create a request using a URL that can receive a post.                                 
            //                    Uri serviceAddress = new Uri(client.BaseAddress, string.Format("AddOrUpdateSeatMap"));
            //                    HttpWebRequest request = WebRequest.CreateHttp(serviceAddress);
            //                    request.Method = "POST";
            //                    request.ContentType = "application/json; charset=utf-8";
            //                    DataContractJsonSerializer ser = new DataContractJsonSerializer(seatMap1.GetType());
            //                    MemoryStream ms = new MemoryStream();
            //                    ser.WriteObject(ms, seatMap1);
            //                    String json = Encoding.UTF8.GetString(ms.ToArray());
            //                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
            //                    writer.Write(json);
            //                    writer.Close();

            //                    // Get the response.                                                                
            //                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //                    {
            //                        HttpResponseMessage message = new HttpResponseMessage();
            //                        message.Content = new StringContent(
            //                            String.Format("Added to partition {1} at {2}", seatMap1.Name, client.ResolvedServicePartition.Info.Id, serviceAddress),
            //                            Encoding.UTF8,
            //                            "text/html");

            //                        return Task.FromResult<HttpResponseMessage>(message);
            //                    }
            //                });
            //**************************************************************************************************

            Console.WriteLine("Press any key to stop the client console app...");
            Console.ReadKey();

        }


    }
}
