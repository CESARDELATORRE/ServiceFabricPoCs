using System;
using System.Runtime.Serialization;

namespace ScoreBig.SeatMapModel
{
    [DataContract]
    public class RatingTier
    {        
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Section { get; set; }

        [DataMember]
        public string Row { get; set; }

        [DataMember]
        public uint Rating { get; set; }

        [DataMember]
        public double FaceValue { get; set; }

        public RatingTier()
        { }

        public RatingTier(string id,
                       string section,
                       string row,
                       uint rating,
                       double faceValue
                      )
        {
            //Could place code with invariants checks here in the constructor
            // ...
            Id = id;
            Section = section;
            Row = row;
            Rating = rating;
            FaceValue= faceValue;
        }

    }
}