using Latexish_Text_Processor.MacroProviders;
using Latexish_Text_Processor.ParserEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class UnitTestHelpers
    {
        public static string StandardProcess(string input, bool PreprocessorActive = false, params IMacroProvider[] AdditionalMacroProviders)
        {
            var parser = new Parser();
            parser.MacroProviders.AddRange(AdditionalMacroProviders);
            if (PreprocessorActive)
                parser.MacroProviders.Add(new PreprocessProvider());
            return parser.Process(input, new string[] { "standardLibrary.texi" });
        }
        public class TemporaryTestFolder : IDisposable
        {
            public string DirectoryName { get; set; } = "test";
            public TemporaryTestFolder()
            {
                DirectoryName = Path.GetFullPath(Path.Combine(".", DirectoryName));
                Directory.CreateDirectory(DirectoryName);
            }
            public void Dispose()
            {
                Directory.Delete(DirectoryName, true);
            }
        }
    }
}
