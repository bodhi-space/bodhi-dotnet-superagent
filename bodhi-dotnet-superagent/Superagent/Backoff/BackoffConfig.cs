namespace Bodhi.Superagent.Backoff
{
    public class BackoffConfig
    {

        private int waitMillis = 1000;
        private int retries = 10;

        public BackoffConfig(int retries, int waitMillis)
        {
            this.retries = retries;
            this.waitMillis = waitMillis;
        }

        public BackoffConfig()
        {
        }

        public int WaitMillis
        {
            get
            {
                return waitMillis;
            }
            
        }

        public int Retries
        {
            get
            {
                return retries;
            }
        }
    }
}
