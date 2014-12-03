﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Latexish_Text_Processor.ParserEngine;
using UnitTests.UnitTestHelpers;

namespace UnitTests
{
    [TestClass]
    public class StandardTests
    {
        public string StandardProcess(string input)
        {
            var parser = new Parser();
            return parser.Process(input, new string[] { "standardLibrary.texi" });
        }
        [TestMethod]
        public void NoReplacements()
        {
            var result = StandardProcess("No replacements");
            Assert.AreEqual("No replacements", result);
        }
        [TestMethod]
        public void SimpleDefine()
        {
            var result = StandardProcess(@"\define{name}{mirhagk}\name is the best");
            Assert.AreEqual("mirhagk is the best", result);
        }
        [TestMethod]
        public void UnknownMacro()
        {
            try
            {
                StandardProcess(@"\notDefined");
            }
            catch (InvalidOperationException ex)
            {
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void NowWorks()
        {
            var result = StandardProcess(@"\now");
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd hh:mm"), result);
        }
        [TestMethod]
        public void CommentIgnoresDefine()
        {
            var parser = new Parser();
            parser.MacroProviders.Add(new Latexish_Text_Processor.MacroProviders.PreprocessProvider());
            var result = parser.Process(@"[[[\define{ignoreThis}{wasn't ignored}]]]\ignoreThis");
            Assert.AreEqual(@"\ignoreThis", result);
        }
        [TestMethod]
        public void TestImport()
        {
            using (var testFolder = new TemporaryTestFolder())
            {
                var parser = new Parser();
                parser.ActiveFolder = testFolder.DirectoryName;
                System.IO.File.WriteAllText(System.IO.Path.Combine(parser.ActiveFolder, "test.texi"), @"\define{name}{mirhagk}");
                var result = parser.Process(@"\import{test.texi}Hello \name", new string[] { "standardLibrary.texi" });
                Assert.AreEqual("Hello mirhagk", result);
            }
        }
    }
}
