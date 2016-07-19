namespace Bodhi.Superagent
{

    public class ConfigOp
    {

        public static ConfigOp INSERT = new ConfigOp("insert");
        public static ConfigOp UPDATE = new ConfigOp("update");
        public static ConfigOp UPSERT = new ConfigOp("upsert");
        public static ConfigOp INVITE = new ConfigOp("invite");

        private string op;

        ConfigOp(string op)
        {
            this.op = op;
        }

        public string Op
        {
            get
            {
                return op;
            }
        }
    }
}
