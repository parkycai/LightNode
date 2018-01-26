#region Assembly Microsoft.VisualStudio.TextTemplating.Interfaces.10.0, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\PublicAssemblies\Microsoft.VisualStudio.TextTemplating.Interfaces.10.0.dll
#endregion

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.TextTemplating
{
    [CLSCompliant(true)]
    public interface ITextTemplatingEngineHost
    {
        IList<string> StandardAssemblyReferences { get; }
        IList<string> StandardImports { get; }
        string TemplateFile { get; }

        object GetHostOption(string optionName);
        bool LoadIncludeText(string requestFileName, out string content, out string location);
        void LogErrors(CompilerErrorCollection errors);
        AppDomain ProvideTemplatingAppDomain(string content);
        string ResolveAssemblyReference(string assemblyReference);
        Type ResolveDirectiveProcessor(string processorName);
        string ResolveParameterValue(string directiveId, string processorName, string parameterName);
        string ResolvePath(string path);
        void SetFileExtension(string extension);
        void SetOutputEncoding(Encoding encoding, bool fromOutputDirective);
    }
}