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
            var hoge = new LightNode2.Client.Generator.LightNodeClientGenerator().TransformText();
            //Run();
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        public static void Run()
        {
            // ------------- T4 Configuration ------------- //

            // 0. Output FileName
            var outputFileName = "LightNode2Client.Generated.cs";

            // 1. Set LightNodeContract assemblies(and all dependency) path to above #@ assembly name # directive
            var assemblies = new[] { Path.Combine(Environment.CurrentDirectory, @"../Performance/LightNode2.Performance/bin/Debug/netcoreapp2.0/LightNode2.Performance.dll") };

            // 2. Set Namespace & ClientName & Namespace
            var clientName = "LightNode2Client";
            var namespaceName = "LightNode2.Client";

            // 3. Set DefaultContentFormatter Construct String
            var defaultContentFormatter = "new LightNode2.Formatter.JsonContentFormatter()";

            // 4. Set Additional using Namespace
            var usingNamespaces = new[] { "System.Linq" };

            // 5. Set append "Async" suffix to method name(ex: CalcAsync or Calc)
            var addAsyncSuffix = true;

            // ----------End T4 Configuration ------------- //

            Func<Type, string> BeautifyType = null;
            BeautifyType = (Type t) =>
            {
                if (!t.IsGenericType) return t.FullName;

                var innerFormat = string.Join(", ", t.GetGenericArguments().Select(x => BeautifyType(x)));
                return Regex.Replace(t.GetGenericTypeDefinition().FullName, @"`.+$", "") + "<" + innerFormat + ">";
            };

            Func<Type, string> UnwrapTask = (Type t) =>
            {
                return BeautifyType(t.GetGenericArguments()[0]);
            };

            var ignoreMethods = new HashSet<string> { "Equals", "GetHashCode", "GetType", "ToString" };

            //var typeFromAssemblies = System.AppDomain.CurrentDomain.GetAssemblies()
            var typeFromAssemblies = assemblies.Select(x => Assembly.LoadFrom(x))
                .Where(x => !Regex.IsMatch(x.GetName().Name, "^(mscorlib|System|Sytem.Web|EnvDTE)$"))
                .SelectMany(x => x.GetTypes())
                .Where(x => x != null && x.FullName != "LightNode2.Server.LightNodeContract");

            var contracts = typeFromAssemblies
                .Where(x =>
                {
                    while (x != typeof(object) && x != null)
                    {
                        if (x.FullName == "LightNode2.Server.LightNodeContract") return true;
                        x = x.BaseType;
                    }
                    return false;
                })
                .Where(x => !x.IsAbstract && x.GetCustomAttributes(true).All(y => y.GetType().FullName != "LightNode2.Server.IgnoreOperationAttribute" && y.GetType().FullName != "LightNode2.Server.IgnoreClientGenerateAttribute"))
                .Select(x =>
                {
                    var methods = x.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(methodInfo => !(methodInfo.IsSpecialName && (methodInfo.Name.StartsWith("set_") || methodInfo.Name.StartsWith("get_"))))
                        .Where(methodInfo => !ignoreMethods.Contains(methodInfo.Name))
                        .Where(methodInfo => methodInfo.GetCustomAttributes(true).All(y => y.GetType().FullName != "LightNode2.Server.IgnoreOperationAttribute" && y.GetType().FullName != "LightNode2.Server.IgnoreClientGenerateAttribute"))
                        .Select(methodInfo =>
                        {
                            var retType = methodInfo.ReturnType;
                            var returnType =
                                (retType == typeof(void)) ? typeof(Task)
                                : (retType == typeof(Task)) ? retType
                                : (retType.IsGenericType && retType.GetGenericTypeDefinition() == typeof(Task<>)) ? retType
                                : typeof(Task<>).MakeGenericType(retType);

                            var parameter = methodInfo.GetParameters()
                                .Select(paramInfo => new Parameter
                                {
                                    Name = paramInfo.Name,
                                    ParameterType = paramInfo.ParameterType,
                                    IsOptional = paramInfo.IsOptional,
                                    DefaultValue = paramInfo.DefaultValue,
                                })
                                .Concat(new[]{new Parameter
                                {
                                    Name = "cancellationToken",
                                    ParameterType = typeof(CancellationToken),
                                    IsOptional = true,
                                    DefaultValue = (object)default(CancellationToken)
                                }})
                                .ToArray();

                            var parameterString = string.Join(", ", parameter.Select(p =>
                            {
                                return BeautifyType(p.ParameterType) + " " + p.Name;
                            }));

                            var parameterStringWithOptional = string.Join(", ", parameter.Select(p =>
                            {
                                var @base = BeautifyType(p.ParameterType) + " " + p.Name;
                                if (p.IsOptional)
                                {
                                    @base += " = " + (
                                        (p.DefaultValue == null) ? "null"
                                      : (p.DefaultValue is string) ? "\"" + p.DefaultValue + "\""
                                      : (p.DefaultValue is CancellationToken) ? "default(CancellationToken)"
                                      : (p.ParameterType.IsEnum) ? p.ParameterType.ToString() + "." + p.DefaultValue.ToString()
                                      : p.DefaultValue.ToString().ToLower());
                                }
                                return @base;
                            }));

                            var debugOnlyClientGenerateMethod = methodInfo.GetCustomAttributes(true).Any(y => y.GetType().FullName == "LightNode2.Server.DebugOnlyClientGenerateAttribute");

                            return new Method
                            {
                                OperationName = methodInfo.Name,
                                ReturnType = returnType,
                                Parameters = parameter,
                                ParameterString = parameterString,
                                ParameterStringWithOptional = parameterStringWithOptional,
                                IsDebugOnly = debugOnlyClientGenerateMethod
                            };
                        })
                        .ToArray();

                    var debugOnlyClientGenerate = x.GetCustomAttributes(true).Any(y => y.GetType().FullName == "LightNode2.Server.DebugOnlyClientGenerateAttribute");

                    return new Contract
                    {
                        RootName = x.Name,
                        InterfaceName = "_I" + x.Name,
                        Operations = methods,
                        IsDebugOnly = debugOnlyClientGenerate
                    };
                })
                .ToArray();

            var generatedCs = Template(usingNamespaces, namespaceName, clientName, contracts, defaultContentFormatter, BeautifyType, addAsyncSuffix);
            var currentDir = Environment.CurrentDirectory;
            var path = Path.Combine(currentDir, outputFileName);
            File.WriteAllText(path, generatedCs);
        }

        private static string Template(string[] usingNamespaces, string namespaceName, string clientName, Contract[] contracts, string defaultContentFormatter, Func<Type, string> beautifyType, bool addAsyncSuffix)
        {
            return $@"using LightNode2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
{string.Join("\r\n", usingNamespaces.Select(x => "using " + x + ";"))}

namespace {namespaceName}
{{

#if DEBUG
    public partial class {clientName} : {string.Join(", ", contracts.Select(x => x.InterfaceName))}
#else
    public partial class {clientName} : {string.Join(", ", contracts.Where(x => !x.IsDebugOnly).Select(x => x.InterfaceName))}
#endif
    {{
        static IContentFormatter defaultContentFormatter = {defaultContentFormatter};
        readonly string rootEndPoint;
        readonly HttpClient httpClient;

        partial void OnAfterInitialized();

        public System.Net.Http.Headers.HttpRequestHeaders DefaultRequestHeaders
        {{
            get {{ return httpClient.DefaultRequestHeaders; }}
        }}

        public long MaxResponseContentBufferSize
        {{
            get {{ return httpClient.MaxResponseContentBufferSize; }}
            set {{ httpClient.MaxResponseContentBufferSize = value; }}
        }}

        public TimeSpan Timeout
        {{
            get {{ return httpClient.Timeout; }}
            set {{ httpClient.Timeout = value; }}
        }}

        IContentFormatter contentFormatter;
        public IContentFormatter ContentFormatter
        {{
            get {{ return contentFormatter = (contentFormatter ?? defaultContentFormatter); }}
            set {{ contentFormatter = value; }}
        }}
{string.Join(Environment.NewLine, contracts.Where(x => !x.IsDebugOnly).Select(contract => $@"
        public {contract.InterfaceName} {contract.RootName} {{ get {{ return this; }} }}").ToArray())}
#if DEBUG
{string.Join(Environment.NewLine, contracts.Where(x => x.IsDebugOnly).Select(contract => $@"
        public {contract.InterfaceName} {contract.RootName} {{ get {{ return this; }} }}").ToArray())}
#endif

        public {clientName}(string rootEndPoint)
        {{
            this.httpClient = new HttpClient();
            this.rootEndPoint = rootEndPoint.TrimEnd('/');
            this.ContentFormatter = defaultContentFormatter;
            OnAfterInitialized();
        }}

        public {clientName}(string rootEndPoint, HttpMessageHandler innerHandler)
        {{
            this.httpClient = new HttpClient(innerHandler);
            this.rootEndPoint = rootEndPoint.TrimEnd('/');
            this.ContentFormatter = defaultContentFormatter;
            OnAfterInitialized();
        }}

        public {clientName}(string rootEndPoint, HttpMessageHandler innerHandler, bool disposeHandler)
        {{
            this.httpClient = new HttpClient(innerHandler, disposeHandler);
            this.rootEndPoint = rootEndPoint.TrimEnd('/');
            this.ContentFormatter = defaultContentFormatter;
            OnAfterInitialized();
        }}

        protected virtual async Task PostAsync(string method, HttpContent content, CancellationToken cancellationToken)
        {{
            var response = await httpClient.PostAsync(rootEndPoint + method, content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }}

        protected virtual async Task<T> PostAsync<T>(string method, HttpContent content, CancellationToken cancellationToken)
        {{
            var response = await httpClient.PostAsync(rootEndPoint + method, content, cancellationToken).ConfigureAwait(false);
            using (var stream = await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync().ConfigureAwait(false))
            {{
                return (T)ContentFormatter.Deserialize(typeof(T), stream);
            }}
        }}

{string.Join(Environment.NewLine, contracts.Select(contract => $@"#region {contract.InterfaceName}
{(contract.IsDebugOnly ? "#if DEBUG" : "")}
{string.Join(Environment.NewLine, contract.Operations.Select(operation => $@"
{(contract.IsDebugOnly ? "#if DEBUG" : "")}
        {beautifyType(operation.ReturnType)} {contract.InterfaceName}.{operation.OperationName}{(addAsyncSuffix ? "Async" : "")}({operation.ParameterString})
        {{
            HttpContent __content = null;
            {(operation.Parameters.Any(x => x.ParameterType == typeof(byte[]))
            ? $@"var __multi = new MultipartFormDataContent();
            {string.Join(Environment.NewLine, operation.Parameters.Where(x => x.Name != "cancellationToken").Select(parameter =>
            {
                if (parameter.ParameterType == typeof(byte[]))
                {
                    return $"__multi.Add(new ByteArrayContent({parameter.Name}), \"{parameter.Name}\");";
                }
                else if (parameter.ParameterType.IsArray)
                {
                    if (parameter.Name != null) return $"if ({parameter.Name} != null) foreach(var __x in {parameter.Name}) {{ __multi.Add(new StringContent({WriteParameter(parameter.ParameterType.GetElementType(), "__x")}), \"{parameter.Name}\"); }}";
                }
                else if (parameter.ParameterType.IsClass || (parameter.ParameterType.IsGenericType && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    if (parameter.Name != null) return $"__multi.Add(new StringContent({WriteParameter(parameter.ParameterType, parameter.Name)}), {parameter.Name});";
                }
                else
                {
                    //return $"__multi.Add(new StringContent({WriteParameter(parameter.ParameterType, parameter.Name)}), {parameter.Name});";
                    return "";
                }
                return "";
            }), 
            "__content = __multi;")}"
            : "")}
        }}
{(contract.IsDebugOnly ? "#endif" : "")}
{(contract.IsDebugOnly ? "#endif" : "")}
"))}
#endregion").ToArray())}
    }}
}}
";

        }

        static string WriteParameter(Type parameterType, string parameterName)
        {
            if (parameterType == typeof(string))
            {
                return Write(parameterName);
            }
            else if (parameterType.IsEnum)
            {
                var underlyingType = Enum.GetUnderlyingType(parameterType);
                return Write(string.Format("(({0}){1}).ToString()", underlyingType, parameterName));
            }
            else if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return WriteParameter(parameterType.GetGenericArguments()[0], parameterName);
            }
            else
            {
                return Write(parameterName + ".ToString()");
            }
        }

        static string Write(string value)
        {
            return value;
        }

        private class Parameter
        {
            public string Name { get; set; }
            public Type ParameterType { get; set; }
            public bool IsOptional { get; set; }
            public object DefaultValue { get; set; }
        }

        private class Method
        {
            public string OperationName { get; set; }
            public Type ReturnType { get; set; }
            public Parameter[] Parameters { get; set; }
            public string ParameterString { get; set; }
            public string ParameterStringWithOptional { get; set; }
            public bool IsDebugOnly { get; set; }
        }

        private class Contract
        {
            public string RootName { get; set; }
            public string InterfaceName { get; set; }
            public Method[] Operations { get; set; }
            public bool IsDebugOnly { get; set; }
        }
    }
}
