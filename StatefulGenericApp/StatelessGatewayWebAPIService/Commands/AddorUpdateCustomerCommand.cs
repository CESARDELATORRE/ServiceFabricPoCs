using System;
using System.ComponentModel.DataAnnotations;

namespace StatelessGatewayWebAPIService.Commands
{
    public class AddorUpdateCustomerCommand
    {
        public DateTime DateOfCommandSubmission { get; set; }
        [Required]
        public string CustomerKey { get; set; }
        [Required]
        public string CompanyName { get; set; }        
        [Required]
        public string StateCode { get; set; }
        public string CountryCode { get; set; }        
        public uint ZipCode { get; set; }
        public string ContactFullName { get; set; }
        public string ContactEmail { get; set; }
        

    }
}
