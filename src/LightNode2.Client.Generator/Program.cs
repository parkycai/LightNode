using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LightNode2.Client.Generator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var generatedClient = new LightNodeClientGenerator().TransformText();
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
