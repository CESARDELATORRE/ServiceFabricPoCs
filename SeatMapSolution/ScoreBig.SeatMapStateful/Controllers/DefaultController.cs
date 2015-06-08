

namespace ScoreBig.SeatMapStateful.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Data.Collections;

    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using ScoreBig.SeatMapModel;

    /// <summary>
    /// Default controller.
    /// </summary>
    public class DefaultController : ApiController
    {
        private readonly IReliableStateManager stateManager;

        public DefaultController(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }        

        [HttpGet]
        public async Task<IHttpActionResult> MapTicketBlock(string seatMapId, string section, string row)
        {
            IReliableDictionary<string, SeatMap> seatMapDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, SeatMap>>("seatMapDictionary");

            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                ConditionalResult<SeatMap> result = await seatMapDictionary.TryGetValueAsync(tx, seatMapId);

                if (result.HasValue)
                {
                    //Have the SeatMapDict, now we need to find the Rating Tier

                    SeatMap seatMap = result.Value;
                    IDictionary<string, RatingTier> ratingTiersDictionary = seatMap.RatingTiers;

                    var retVal = from rTier in ratingTiersDictionary
                                 where (rTier.Value.Section) == section
                                 where (rTier.Value.Row) == row
                                 select rTier.Value;

                    //RatingTier ratingTier = ratingTiersDictionary.Where(x => x.Value.Section == section, x => x.Value.Row == row).Select(x => x.Value).FirstOrDefault();

                    return this.Ok(retVal);
                    //return this.Ok(result.Value);
                }
            }

            return this.Ok(0);
        }

        [HttpPost]        
        public async Task<IHttpActionResult> AddOrUpdateSeatMap(SeatMap seatMap)
        {
            IReliableDictionary<string, SeatMap> seatMapDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, SeatMap>>("seatMapDictionary");
            try
            {
                using (ITransaction tx = this.stateManager.CreateTransaction())
                {
                    //Add or Update a SeatMap
                    var updatedSeatMap = await seatMapDictionary.AddOrUpdateAsync(tx,
                                                                                  seatMap.Id,
                                                                                  seatMap,
                                                                                  (key, oldValue) => { return seatMap; });
                   
                    //Commit the Transaction
                    await tx.CommitAsync();

                    if (updatedSeatMap.Name != "")
                    {
                        Trace.WriteLine("SeatMap with Name: " + updatedSeatMap.Name + " was Added or Updated");

                        //(CDLTLL - Check/query value)                        
                        Trace.WriteLine("Sample values added to Customer Dictionary");
                        var result = await seatMapDictionary.TryGetValueAsync(tx, seatMap.Id);
                        if (result.HasValue)
                        {
                            SeatMap readedSeatMap = (SeatMap)result.Value;
                            Trace.WriteLine("SeatMap Name queried: " + readedSeatMap.Name);
                        }

                        return Ok(updatedSeatMap);
                    }
                    else
                    {
                        Trace.WriteLine("SeatMap " + seatMap.Name + " was NOT updated");
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.Assert(false, "Unexpected exception {0}", exception.Message);
                throw;
            }

            return Ok(0);
        }

        //[HttpGet]
        //public async Task<IHttpActionResult> Ping()
        //{
        //    return this.Ok();
        //}

    }
}