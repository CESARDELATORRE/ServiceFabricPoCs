
namespace StatefulWebAPIService.Controllers
{
    using System.Fabric.Data;
    using System.Fabric.Data.Collections;
    using System.Threading.Tasks;
    using System.Web.Http;

    using System.Diagnostics;
    using System;
    using Model;

    /// <summary>
    /// Default controller.
    /// </summary>
    public class CustomerController : ApiController
    {
        //(CDLTLL) StateManager object will come thru DI
        private readonly IReliableObjectStateManager stateManager;

        //Customers' Dictionary to work with from this Controller's methods
        private IReliableDictionary<string, Customer> customersDictionary;

        ////Statistics Dictionary to store info about Processed Customers
        IReliableDictionary<string, long> statsDictionary;

        //(Constructor obtains the root State Manager Dictionary by using Dependency Injection)
        public CustomerController(IReliableObjectStateManager objStateManager)
        {
            this.stateManager = objStateManager;
            this.Initialize();              
        }

        public async void Initialize()
        {
            //(CDLTLL) Grab the customersDictionary from the root stateManager Dictionary that was injected thru DI
            this.customersDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, Customer>>("customersDictionary");

            //(CDLTLL) Grab the statsDictionary from the root stateManager Dictionary that was injected thru DI
            this.statsDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, long>>("statsDictionary");
        }

        //// GET /customers/MSFT
        [Route("customers/{customerKey}", Name = "Customer")]
        public async Task<IHttpActionResult> GetCustomer(string customerKey)
        {
            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                ConditionalResult<Customer> result = await this.customersDictionary.TryGetValueAsync(tx, customerKey);

                // Take an update lock on the relevant key.
                //var result = await this.customersDictionary.TryGetValueAsync(tx, customerKey, LockMode.Update);

                if (result.HasValue)
                {
                    Customer customer = (Customer)result.Value;
                    Trace.WriteLine("Returning a whole Customer with Name: " + customer.CompanyName);
                    return Ok(customer);
                }
            }

            return Ok(0);
        }

        //// GET /customers/MSFT/CustomerName
        [Route("customers/{customerKey}/CustomerName", Name = "CustomerName")]
        public async Task<IHttpActionResult> GetCustomerName(string customerKey)
        {                                    
            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                ConditionalResult<Customer> result = await this.customersDictionary.TryGetValueAsync(tx, customerKey);

                // Take an update lock on the relevant key.
                //var result = await this.customersDictionary.TryGetValueAsync(tx, customerKey, LockMode.Update);

                if (result.HasValue)
                {
                    Customer customer = (Customer)result.Value;
                    Trace.WriteLine("Customer Name: " + customer.CompanyName);
                    return Ok(customer.CompanyName);
                }
            }

            return Ok(0);
        }

       
        //PUT 
        [HttpPut]
        [Route("customers/{customerKey}/addorupdate/{companyName}/{zipCode}/{stateCode}/{countryCode}/{contactFullName}/{contactEmail}", Name = "AddorUpdateCustomer")]
        public async Task<IHttpActionResult> AddOrUpdateCustomer(string customerKey,
                                                                 string companyName,
                                                                 uint   zipCode,
                                                                 string stateCode,
                                                                 string countryCode,
                                                                 string contactFullName,
                                                                 string contactEmail
                                                                )
        {
            Customer customer = new Customer(customerKey,
                                             companyName,
                                             zipCode,
                                             stateCode,
                                             countryCode,
                                             contactFullName,
                                             contactEmail);

            try
            {
                using (ITransaction tx = this.stateManager.CreateTransaction())
                {
                    //Add or Update a Customer
                    var updatedCustomer = await customersDictionary.AddOrUpdateAsync(tx, 
                                                                                     customerKey, 
                                                                                     customer,                                                                                     
                                                                                     (key, oldValue) => { return customer; });

                    
                    long numberOfProcessedCustomers = await statsDictionary.AddOrUpdateAsync(tx, "Number_of_Customers_Processed", 1, (key, oldValue) => { return oldValue + 1; });
                                                                                                                                                                                                                                                                                                

                    //Commit the Transaction
                    await tx.CommitAsync();

                    if (updatedCustomer.CompanyName != "")
                    {
                        Trace.WriteLine("Customer with Name: " + updatedCustomer.CompanyName + " was Added or Updated");

                        //(CDLTLL - Check/query value)                        
                        //Trace.WriteLine("Sample values added to Customer Dictionary");
                        //var result = await customersDictionary.TryGetValueAsync(tx, customerKey);
                        //if (result.HasValue)
                        //{
                        //    Customer readCustomer = (Customer)result.Value;
                        //    Trace.WriteLine("Customer Name queried: " + readCustomer.CompanyName);
                        //}

                        return Ok(customer);
                    }
                    else
                    {
                        Trace.WriteLine("Customer " + customer.CompanyName + " was NOT updated");
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

        //Example with QUEUES
        //[HttpPut]
        //public async Task<IHttpActionResult> AddWord(string word)
        //{
        //    IReliableQueue<string> queue = await this.objectManager.GetOrAddAsync<IReliableQueue<string>>("inputQueue");

        //    using (ITransaction tx = this.objectManager.CreateTransaction())
        //    {
        //        await queue.EnqueueAsync(tx, word);

        //        await tx.CommitAsync();
        //    }

        //    return Ok();
        //}

        [HttpGet] 
        [Route("customers/test")]
        public IHttpActionResult RunTest()
        {
            return Ok("Cheers my!");
        }

    }
}
