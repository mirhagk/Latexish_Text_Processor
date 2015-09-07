using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Latexish_Text_Processor.ParserEngine;
using static UnitTests.UnitTestHelpers;
using Latexish_Text_Processor.MacroProviders;

namespace UnitTests
{
    [TestClass]
    public class HumanizerTests
    {
        [TestMethod]
        public void UpperCase()
        {
            Assert.AreEqual("THIS IS UPPER CASE", StandardProcess(@"\system.Upper{This is upper case}", false, new HumanizerProvider()));
        }
    }
}
