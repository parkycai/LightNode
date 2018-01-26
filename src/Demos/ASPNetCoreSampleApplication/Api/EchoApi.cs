using LightNode2.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreSampleApplication.Api
{
    /// <summary>
    /// Echo to request
    /// </summary>
    public class EchoApi : LightNodeContract
    {
        /// <summary>
        /// Echo response for request
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public string Echo(string x)
        {
            return x;
        }
    }
}
