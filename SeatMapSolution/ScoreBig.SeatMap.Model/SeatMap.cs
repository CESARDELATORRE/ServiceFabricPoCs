using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.ServiceFabric.Data.Collections;

namespace ScoreBig.SeatMapModel
{
    [DataContract]
    [Newtonsoft.Json.JsonObject(Title = "SeatMap")]
    public class SeatMap
    {
        [DataMember]
        //[Newtonsoft.Json.JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [DataMember]
        //[Newtonsoft.Json.JsonProperty(PropertyName = "VenueId")]
        public string VenueId { get; set; }

        [DataMember]
        //[Newtonsoft.Json.JsonProperty(PropertyName = "VenueName")]
        public string VenueName { get; set; }

        [DataMember]
        //[Newtonsoft.Json.JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [DataMember]
        //[Newtonsoft.Json.JsonObject(Title = "RatingTiers")]

        public IDictionary<string, RatingTier> RatingTiers { get; set; }
        //public IDictionary<Tuple<string, string>, RatingTier> RatingTiers  { get; set; }
        //Key of the Dictionary should be a Tuple based on Section & Row..

        public SeatMap()
        { }

        public SeatMap(string id,
                       string venueId,
                       string name,
                       IDictionary<string, RatingTier> ratingTiers
                      )
        {
            //Could place code with invariants checks here in the constructor
            // ...
            Id = id;
            VenueId = venueId;
            Name = name;
            RatingTiers = ratingTiers;
        }

        public void PopulateSampleData()
        {
            //TBD
        }

    }
}
