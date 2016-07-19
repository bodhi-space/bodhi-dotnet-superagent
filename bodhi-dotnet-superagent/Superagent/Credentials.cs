using unirest_net.request;

namespace Bodhi.Superagent
{
    public interface Credentials
    {
        HttpRequest SetAuthentication(HttpRequest request);
    }
}
