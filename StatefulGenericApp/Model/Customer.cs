//(CDLTLL)
using System.Runtime.Serialization;

namespace Model
{
    /// <summary>
    /// Customer entity to be used in customersDictionary
    /// </summary>
    [DataContract]
    public class Customer
    {
        //(CDLTLL TODO): Delete "set" when confirmed that works properly

        [DataMember]
        public string CustomerKey { get; set; }
        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public uint ZipCode { get; set; }

        [DataMember]
        public string ContactFullName { get; set; }

        [DataMember]
        public string ContactEmail { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string StateCode { get; set; }

        public Customer(string customerKey,
                        string companyName,
                        uint zipCode,
                        string stateCode,
                        string countryCode,
                        string contactFullName,
                        string contactEmail
                       )
        {
            //Could place code with invariants checks here in the constructor
            // ...
            CustomerKey = customerKey;
            CompanyName = companyName;
            ZipCode = zipCode;
            StateCode = stateCode;
            CountryCode = countryCode;
            ContactFullName = contactFullName;
            ContactEmail = contactEmail;
        }

        public void PopulateSampleData()
        {
            //TBD
        }

    }
}
