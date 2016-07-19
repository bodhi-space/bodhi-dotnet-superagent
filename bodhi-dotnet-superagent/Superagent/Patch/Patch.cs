using Newtonsoft.Json.Linq;

namespace Bodhi.Superagent
{
    public class Patch : JObject
    {
        public Patch(PatchOperation op, string path, JToken value)
        {
            this["op"] = op.Op;
            this["path"] = path;
            this["value"] = value;
        }
    }
}
