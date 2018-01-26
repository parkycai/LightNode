using Mono.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LightNode2.Client.Generator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var options = new CommandlineOptions(args);
            if (!options.IsParsed)
            {
                Environment.ExitCode = 1;
                Environment.Exit(Environment.ExitCode);
                return;
            }

            var sw = Stopwatch.StartNew();
            Console.WriteLine($"Client Generation Start: {string.Join(",", options.InputDllPathes)}");

            // Generate template
            var clientTeamplate = new LightNodeClientGenerator() { CommandlineOptions = options }.TransformText();

            // Write file
            var sb = new StringBuilder();
            sb.AppendLine(clientTeamplate);
            Output(options.OutputPath, sb.ToString());

            Console.WriteLine("String Generation Complete:" + sw.Elapsed.ToString());
        }

        static void Output(string path, string text)
        {
            path = path.Replace("global::", "");

            const string prefix = "[Out]";
            Console.WriteLine(prefix + path);

            var fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            File.WriteAllText(path, text, Encoding.UTF8);
        }
    }

    public class CommandlineOptions
    {
        // Options
        public string[] InputDllPathes { get; private set; }
        public string OutputPath { get; private set; }
        public string ClientName { get; private set; } = "LightNode2Client";
        public string NameSpaceName { get; private set; } = "LightNode2.Client";
        public string ResolverName { get; private set; } = "new LightNode2.Formatter.JsonContentFormatter()";
        public string[] UsingNamespaces { get; private set; } = new[] { "System.Linq" };
        public bool RemoveAsyncSuffix { get; private set; } = false;

        // state
        public bool IsParsed { get; set; }

        public CommandlineOptions(string[] args)
        {
            var option = new OptionSet()
            {
                { "i|input=", "[required]input path of analyze dlls. (comma-separated)", x => { InputDllPathes = x?.Split(","); } },
                { "o|output=", "[required]output file path.", x => { OutputPath = x; } },
                { "c|clientname=", $"[optional, default={ClientName}]name for generated client.", x => { ClientName = x; } },
                { "n|namespace=", $"[optional, default={NameSpaceName}]namespace for generated client.", x => { NameSpaceName = x; } },
                { "u|usings=", $"[optional, default={string.Join(",", UsingNamespaces)}]additional using namespace during build. (comma separated)", x => { UsingNamespaces = x?.Split(","); } },
                { "r|removeasyncsuffix=", $"[optional, default={RemoveAsyncSuffix}]Force not use async suffix for generated client method.", x => { RemoveAsyncSuffix = true; } },
            };

            void ShowHelp()
            {
                Console.WriteLine("arguments help:");
                option.WriteOptionDescriptions(Console.Out);
                IsParsed = false;
            }

            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }
            else
            {
                option.Parse(args);

                if (InputDllPathes == null || !InputDllPathes.Any())
                {
                    Console.WriteLine("Invalid Argument:" + string.Join(" ", args));
                    Console.WriteLine("--input is required option. You can specify multiple path by comma-separated.");
                    Console.WriteLine();
                    ShowHelp();
                    return;
                }

                if (string.IsNullOrEmpty(OutputPath))
                {
                    Console.WriteLine("Invalid Argument:" + string.Join(" ", args));
                    Console.WriteLine("--output is required option. You can specify multiple path by comma-separated.");
                    Console.WriteLine();
                    ShowHelp();
                    return;
                }

                IsParsed = true;
                return;
            }
        }
    }
}
