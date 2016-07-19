using System.Threading.Tasks;
using unirest_net.http;

namespace Bodhi.Superagent.Backoff
{
    public delegate Task<HttpResponse<T>> BackoffResultHandler<T>();
}
