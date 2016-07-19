using Newtonsoft.Json.Linq;

namespace Bodhi.Superagent
{
    public class BulkConfig : JObject
    {

        public BulkConfig(ConfigOp op, bool report, string target)
        {
            this["op"] = op.Op;
            this["report"] = report;
            this["target"] = target;
        }

        public BulkConfig(ConfigOp op, string target) : this(op, false, target)
        {

        }

    }
}