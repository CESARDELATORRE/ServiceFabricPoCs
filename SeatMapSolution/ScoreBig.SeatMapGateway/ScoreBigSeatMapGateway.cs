using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Services;

using ScoreBig.Common;

namespace ScoreBig.SeatMapGateway
{
    public class ScoreBigSeatMapGateway : StatelessService
    {
        protected override ICommunicationListener CreateCommunicationListener()
        {
            return new OwinCommunicationListener("scorebigseatmapgateway", new Startup());
        }

    }
}
