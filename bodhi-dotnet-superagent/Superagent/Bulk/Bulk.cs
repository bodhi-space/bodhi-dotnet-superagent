using Newtonsoft.Json.Linq;

namespace Bodhi.Superagent
{
    public class Bulk : JObject
    {

        public Bulk(BulkConfig bulkConfig, JArray payload)
        {
            this["config"] = bulkConfig;
            this["payload"] = payload;
        }
    }
}
