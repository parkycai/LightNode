using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNode2.Server
{
    public abstract class LightNodeContract
    {
        public HttpContext Context { get; set; }
    }
}