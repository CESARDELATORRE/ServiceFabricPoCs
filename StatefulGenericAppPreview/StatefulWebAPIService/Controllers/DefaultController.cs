
namespace StatefulWebAPIService.Controllers
{
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Data.Collections;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Default controller.
    /// </summary>
    public class DefaultController : ApiController
    {
        //(CDLTLL) Will come thru DI
        private readonly IReliableStateManager objectManager;

        //(Constructor using DI)
        public DefaultController(IReliableStateManager objManager)
        {
            this.objectManager = objManager;            
        }


        [HttpGet]
        public IHttpActionResult Index()
        {
                return Ok("Use specific ASP.NET WebAPI routeTemplates described in each attribute in Controllers' methods");            
        }

        [HttpGet]
        public async Task<IHttpActionResult> Count()
        {
            //(CDLTLL) Grab the statsDictionary from the stateManager Dictionary that was injected thru DI
            IReliableDictionary<string, long> statsDictionary = await this.objectManager.GetOrAddAsync<IReliableDictionary<string, long>>("statsDictionary");

            using (ITransaction tx = this.objectManager.CreateTransaction())
            {
                ConditionalResult<long> result = await statsDictionary.TryGetValueAsync(tx, "Number_of_Customers_Processed");

                if (result.HasValue)
                {
                    return Ok(result.Value);
                }
            }

            return Ok(0);
        }        

    }
}
