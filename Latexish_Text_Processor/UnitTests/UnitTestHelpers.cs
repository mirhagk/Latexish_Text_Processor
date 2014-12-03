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
