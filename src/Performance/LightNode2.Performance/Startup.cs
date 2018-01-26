using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightNode2.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LightNode2.Performance
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var option = new LightNodeOptions(AcceptVerbs.Get | AcceptVerbs.Post, new Formatter.JsonContentFormatter());
            app.UseLightNode(option, new[] { typeof(Startup).Assembly });
        }
    }


    public class Perf : LightNodeContract
    {
        public MyClass Echo(string name, int x, int y, MyEnum e)
        {
            return new MyClass { Name = name, Sum = (x + y) * (int)e };
        }

        public void Test(string a = null, int? x = null, MyEnum2? z = null)
        {
        }

        public System.Threading.Tasks.Task Te()
        {
            return System.Threading.Tasks.Task.FromResult(1);
        }

        public void TestArray(string[] array, int[] array2, MyEnum[] array3)
        {
        }

        public void TeVoid()
        {
        }

        [Post]
        public void ByteArrayCheck1(int x, string y, byte[] byteArray)
        {
        }

        [Post]
        public void ByteArrayCheck2(string[] array, int[] array2, MyEnum[] array3, byte[] byteArray)
        {
        }

        [Post]
        public void ByteArrayCheck3(int x, string y, byte[] byteArray, string a = null, int? xxx = null, MyEnum2? z = null)
        {
        }

        public string Te4(string xs)
        {
            return xs;
        }

        [IgnoreOperation]
        public void Ignore(string a)
        {
        }

        [IgnoreClientGenerate]
        public void IgnoreClient(string a)
        {
        }

        [Post]
        public string PostString(string hoge)
        {
            return hoge;
        }
    }

    [DebugOnlyClientGenerate]
    public class DebugOnlyTest : LightNodeContract
    {
        public void Hoge()
        {
        }
    }

    [DebugOnlyClientGenerate]
    public class DebugOnlyMethodTest : LightNodeContract
    {
        [DebugOnlyClientGenerate]
        public void Hoge()
        {
        }
    }

    public class MyClass
    {
        public string Name { get; set; }
        public int Sum { get; set; }
    }

    public enum MyEnum
    {
        A = 2,
        B = 3,
        C = 4
    }

    public enum MyEnum2 : ulong
    {
        A = 100,
        B = 3000,
        C = 50000
    }
}
