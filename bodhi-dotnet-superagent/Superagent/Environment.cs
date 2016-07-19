namespace Bodhi.Superagent
{
    public class Environment
    {
        public static Environment DEV = new Environment("https://api.bodhi-dev.io");
        public static Environment TEST = new Environment("https://api.bodhi-qa.io");
        public static Environment PROD = new Environment("https://api.bodhi.space");

        private string url;

        Environment(string url)
        {
            this.url = url;
        }

        public string getUrl()
        {
            return url;
        }
    }
}
