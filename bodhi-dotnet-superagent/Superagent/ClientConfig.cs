using Bodhi.Superagent.Backoff;

namespace Bodhi.Superagent
{
    public class ClientConfig
    {

        private string uri;
        private string ns;
        private Credentials credentials;
        private BackoffConfig backoffConfig;

        public ClientConfig(string uri, string ns, Credentials credentials, BackoffConfig backoffConfig)
        {
            this.uri = uri;
            this.ns = ns;
            this.credentials = credentials;
            this.backoffConfig = backoffConfig;
        }

        public string Uri
        {
            get
            {
                return uri;
            }
        }

        public string Namespace
        {
            get
            {
                return ns;
            }
        }

        public Credentials Credentials
        {
            get
            {
                return credentials;
            }
        }

        public BackoffConfig BackoffConfig
        {
            get
            {
                return backoffConfig;
            }
        }

        public string GetNamespaceUri()
        {
            return uri + "/" + ns;
        }

    }
}
