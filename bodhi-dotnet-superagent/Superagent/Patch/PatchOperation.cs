namespace Bodhi.Superagent
{


    public class PatchOperation
    {
        public static PatchOperation ADD = new PatchOperation("add");
        public static PatchOperation REMOVE = new PatchOperation("remove");
        public static PatchOperation REPLACE = new PatchOperation("replace");
        public static PatchOperation MOVE = new PatchOperation("move");
        public static PatchOperation COPY = new PatchOperation("copy");
        public static PatchOperation TEST = new PatchOperation("test");


        private string op;

        PatchOperation(string op)
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
