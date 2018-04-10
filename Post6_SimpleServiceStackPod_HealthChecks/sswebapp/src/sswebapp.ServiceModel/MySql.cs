using ServiceStack;
using System.Collections.Generic;

namespace sswebapp.ServiceModel
{
    [Route("/mysql","POST")]
    public class MySqlRequest : IReturn<MySqlResponse>
    {
        public SqlProps SqlProps { get; set; }
    }

    public class MySqlResponse
    {
        public IEnumerable<string> Results { get; set; }
    }


    public class SqlProps
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
