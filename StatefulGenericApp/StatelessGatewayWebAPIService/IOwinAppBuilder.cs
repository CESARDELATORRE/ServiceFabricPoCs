using Owin;

namespace StatelessGatewayWebAPIService
{
    public interface IOwinAppBuilder
    {
        void Configuration(IAppBuilder appBuilder);
    }
}
