using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services;

using System.Diagnostics;

using ScoreBig.Common;
using ScoreBig.SeatMapModel;

namespace ScoreBig.SeatMapStateful
{
    public class ScoreBigSeatMapStateful : StatefulService
    {
        public const string ServiceEventSourceName = "ScoreBigSeatMapStateful";

        public ScoreBigSeatMapStateful()
        {
            ServiceEventSource.Current.ServiceInstanceConstructed(ServiceEventSourceName);
        }

        protected override ICommunicationListener CreateCommunicationListener()
        {
            ServiceEventSource.Current.CreateCommunicationListener(ServiceEventSourceName);

            return new OwinCommunicationListener("scorebigseatmapstateful", new Startup(this.StateManager));
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //(CDLTLL) Set up my data structures accessible thru this.StateManager that will be injected thru DI
            //SeatMap Dictionary
            IReliableDictionary<string, SeatMap> seatMapDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, SeatMap>>("seatMapDictionary");


            //SAMPLE DATA - WARNING: WILL BE GENERATED IN EVERY PARTITION AND PRIMARY REPLICA
            //Trace.WriteLine("Generating sample data for SeatMap from RunAsync in Stateful service");
            
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
            //SeatMap seatMap4 = new SeatMap(seatMapId4, "00000000-0000-0000-0000-444444444444", "Test Seatmap",
            //                               null
            //                              );


            //string seatMapId5 = "c8fbfc02-a33b-4690-ac80-862ea76d9a5a";
            //SeatMap seatMap5 = new SeatMap(seatMapId5, "98c1edc2-a633-45e9-8755-18933b590bf7", "Concert",
            //                               null
            //                              );

            //try
            //{

            //    //Sample values being populated for the Counter dict
            //    using (ITransaction tx = this.StateManager.CreateTransaction())
            //    {
            //        var updatedSeatMap1 = await seatMapDictionary.AddOrUpdateAsync(tx, seatMapId1, seatMap1, (key, oldValue) => { return seatMap1; });
            //        var updatedSeatMap2 = await seatMapDictionary.AddOrUpdateAsync(tx, seatMapId2, seatMap2, (key, oldValue) => { return seatMap2; });
            //        var updatedSeatMap3 = await seatMapDictionary.AddOrUpdateAsync(tx, seatMapId3, seatMap3, (key, oldValue) => { return seatMap3; });
            //        var updatedSeatMap4 = await seatMapDictionary.AddOrUpdateAsync(tx, seatMapId4, seatMap4, (key, oldValue) => { return seatMap4; });
            //        var updatedSeatMap5 = await seatMapDictionary.AddOrUpdateAsync(tx, seatMapId5, seatMap5, (key, oldValue) => { return seatMap5; });

            //        await tx.CommitAsync();
            //    }
            //    Trace.WriteLine("Sample values added to the Dictionary");

            //    //(CDLTLL - Check/query value)
            //    using (ITransaction tx = this.StateManager.CreateTransaction())
            //    {
            //        var result = await seatMapDictionary.TryGetValueAsync(tx, seatMapId1);
            //        if (result.HasValue)
            //        {
            //            //SeatMap
            //            SeatMap seatMapTest = (SeatMap)result.Value;
            //            Trace.WriteLine("SeatMap Name: " + seatMapTest.Name);

            //            //RatingTiers
            //            RatingTier testRatingTier;
            //            seatMapTest.RatingTiers.TryGetValue("c88e9ca2-88fd-4331-866d-9fd300f838e7", out testRatingTier);
            //            Trace.WriteLine("Rating Tier FaceValue: " + testRatingTier.FaceValue);
            //        }
            //    }
            //}
            //catch (Exception exception)
            //{
            //    Trace.Assert(false, "Unexpected exception {0}", exception.Message);
            //    throw;
            //}


        }


    }
}
