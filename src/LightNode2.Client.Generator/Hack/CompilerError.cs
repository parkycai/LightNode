#region Assembly System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7\System.dll
#endregion

namespace System.CodeDom.Compiler
{
    //
    // Summary:
    //     Represents a compiler error or warning.
    public class CompilerError
    {
        //
        // Summary:
        //     Initializes a new instance of the System.CodeDom.Compiler.CompilerError class.
        public CompilerError() { }
        //
        // Summary:
        //     Initializes a new instance of the System.CodeDom.Compiler.CompilerError class
        //     using the specified file name, line, column, error number, and error text.
        //
        // Parameters:
        //   fileName:
        //     The file name of the file that the compiler was compiling when it encountered
        //     the error.
        //
        //   line:
        //     The line of the source of the error.
        //
        //   column:
        //     The column of the source of the error.
        //
        //   errorNumber:
        //     The error number of the error.
        //
        //   errorText:
        //     The error message text.
        public CompilerError(string fileName, int line, int column, string errorNumber, string errorText) { }

        //
        // Summary:
        //     Gets or sets the line number where the source of the error occurs.
        //
        // Returns:
        //     The line number of the source file where the compiler encountered the error.
        public int Line { get; set; }
        //
        // Summary:
        //     Gets or sets the column number where the source of the error occurs.
        //
        // Returns:
        //     The column number of the source file where the compiler encountered the error.
        public int Column { get; set; }
        //
        // Summary:
        //     Gets or sets the error number.
        //
        // Returns:
        //     The error number as a string.
        public string ErrorNumber { get; set; }
        //
        // Summary:
        //     Gets or sets the text of the error message.
        //
        // Returns:
        //     The text of the error message.
        public string ErrorText { get; set; }
        //
        // Summary:
        //     Gets or sets a value that indicates whether the error is a warning.
        //
        // Returns:
        //     true if the error is a warning; otherwise, false.
        public bool IsWarning { get; set; }
        //
        // Summary:
        //     Gets or sets the file name of the source file that contains the code which caused
        //     the error.
        //
        // Returns:
        //     The file name of the source file that contains the code which caused the error.
        public string FileName { get; set; }

        //
        // Summary:
        //     Provides an implementation of Object's System.Object.ToString method.
        //
        // Returns:
        //     A string representation of the compiler error.
        public override string ToString() => "";
    }
}