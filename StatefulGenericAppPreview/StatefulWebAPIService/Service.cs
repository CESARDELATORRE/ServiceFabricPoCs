using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//(CDLTLL)
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services;
using System.Diagnostics;

//(CDLTLL)
using Model;

namespace StatefulWebAPIService
{

    public class Service : StatefulService
    {
        public const string ServiceTypeName = "StatefulWebAPIService";

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //(CDLTLL) Set up my data structures accessible thru this.StateManager that will be injected thru DI

            //Customers' Dictionary to store Customer's data
            IReliableDictionary<string, Customer> customersDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, Customer>>("customersDictionary");

            //Statistics Dictionary to store info about Processed Customers
            IReliableDictionary<string, long> statsDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("statsDictionary");

            Trace.WriteLine("Doing nothing (no sample data) from RunAsync in Stateful service");

            //Sample values being populated for the Customer Dictionary and Statistics Dictionary
            //string cust1Key = "MSFT";
            //Customer cust1 = new Customer(cust1Key, "Microsoft", 98052, "WA", "US", "Satya Nadella", "satya.nadella@microsoft.com");
            //string cust2Key = "APPL";
            //Customer cust2 = new Customer(cust2Key, "Apple", 95015, "CA", "US", "Tim Cook", "tim.cook@apple.com");
            //string cust3Key = "GOOG";
            //Customer cust3 = new Customer(cust3Key, "Google", 94105, "WA", "US", "Larry Page", "larry.page@microsoft.com");
            //string cust4Key = "AMAZ";
            //Customer cust4 = new Customer(cust4Key, "Amazon", 98047, "WA", "US", "Jeff Bezos", "jeff.bezos@microsoft.com");
            //string cust5Key = "IBM";
            //Customer cust5 = new Customer(cust5Key, "IBM", 10504, "NY", "US", "Thomas J. Watson", "thomas.watson@microsoft.com");            
            //try
            //{

            //    //Sample values being populated for the Counter dict
            //    using (ITransaction tx = this.StateManager.CreateTransaction())
            //    {
            //        var updatedCust1 = await customersDictionary.AddOrUpdateAsync(tx, cust1Key, cust1, (key, oldValue) => { return cust1; });
            //        var updatedCust2 = await customersDictionary.AddOrUpdateAsync(tx, cust2Key, cust2, (key, oldValue) => { return cust2; });
            //        var updatedCust3 = await customersDictionary.AddOrUpdateAsync(tx, cust3Key, cust3, (key, oldValue) => { return cust3; });
            //        var updatedCust4 = await customersDictionary.AddOrUpdateAsync(tx, cust4Key, cust4, (key, oldValue) => { return cust4; });
            //        var updatedCust5 = await customersDictionary.AddOrUpdateAsync(tx, cust5Key, cust5, (key, oldValue) => { return cust5; });

            //        await tx.CommitAsync();
            //    }
            //    Trace.WriteLine("Sample values added to Customer Dictionary");

            //    //(CDLTLL - Check/query value)
            //    using (ITransaction tx = this.StateManager.CreateTransaction())
            //    {
            //        var result = await customersDictionary.TryGetValueAsync(tx, cust1Key);
            //        if (result.HasValue)
            //        {
            //            Customer customer = (Customer)result.Value;
            //            Trace.WriteLine("Customer Name: " + customer.CompanyName);
            //        }
            //    }

            //    //(CDLTLL - TO DELETE)
            //    using (ITransaction tx = this.StateManager.CreateTransaction())
            //    {
            //        await dataStore.AddOrUpdateAsync(tx, "key", 77, (key, value) => { return (value + 1); });
            //        await tx.CommitAsync();
            //    }
            //    long currentValue = dataStore.FirstOrDefault(p => p.Key == "key").Value;
            //    Trace.WriteLine("Counter value: " + currentValue);

            //}
            //catch (Exception exception)
            //{
            //    Trace.Assert(false, "Unexpected exception {0}", exception.Message);
            //    throw;
            //}

            //while (true) //start using the counter data structure
            //{
            //    //Sample values being populated for the Counter dict
            //    using (ITransaction tx = this.StateManager.CreateTransaction())
            //    {
            //        await dataStore.AddOrUpdateAsync(tx, "key", 0, (key, value) => { return (value + 1); });
            //        await tx.CommitAsync();
            //    }
            //    long currentValue = dataStore.FirstOrDefault(p => p.Key == "key").Value;
            //    Trace.WriteLine("Counter value: " + currentValue);

            //    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            //}


        }

        /// <summary>
        /// Override this method to create a communication listener.
        /// The communication listener will be returned to and managed by the system.
        /// </summary>
        /// <returns></returns>
        protected override ICommunicationListener CreateCommunicationListener()
        {
            Trace.WriteLine("Creating Stateful Web API communication listener");

            return new OwinCommunicationListener(new Startup(this.StateManager));

        }
    }
}
