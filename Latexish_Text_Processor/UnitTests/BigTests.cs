using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnitTests.UnitTestHelpers;

namespace UnitTests
{
    [TestClass]
    public class BigTests
    {
        [TestMethod]
        public void BQL()
        {
            var result =  StandardProcess(@"[\newCommand{update}{3}{update dbo.#1 set #2 = #3}
\newCommand{for}{3}{
DECLARE @index_middle
SET @index_middle = #1
WHILE @index_middle < #2
BEGIN
	#3
	SET @index_middle = @index_middle + 1
END
}

]
--Update all employee
\update{employee}{employeeid}{100}

--for loop, print index
\for{1}{3}{print @index_middle}");
            Assert.AreEqual(@"
--Update all employee
update dbo.employee set employeeid = 100

--for loop, print index

DECLARE @index_middle
SET @index_middle = 1
WHILE @index_middle < 3
BEGIN
	print @index_middle
	SET @index_middle = @index_middle + 1
END
", result);
        }
        [TestMethod]
        public void Email()
        {
            var result = StandardProcess(@"[\define{Money}{150}
\define{Title}{Prince}
\define{Country}{Nigeria}
]\
Dear \Target{},

I am the \Title of \Country{}. I was awarded a sum of $\Money{}, but my government is corrupt. I must transfer the funds to a \TargetCountry bank account, please send me your account number so I can transfer it into your account. For your troubles I will leave $\Math.Percentage{10}{\Money} of my money in your account.

Thank you for your troubles.",true,new Latexish_Text_Processor.MacroProviders.MathProvider());
            Assert.AreEqual(@"Dear \Target{},

I am the Prince of Nigeria. I was awarded a sum of $150, but my government is corrupt. I must transfer the funds to a \TargetCountry bank account, please send me your account number so I can transfer it into your account. For your troubles I will leave $15 of my money in your account.

Thank you for your troubles.", result);
        }
        [TestMethod]
        public void HTMLDocument()
        {
            var result = StandardProcess(@"\import{html}
\document{
	\heading{About texi}
	\p{This is a paragraph}
	\p{fun stuff}
}");
            Assert.AreEqual(@"

	<html>
		<head></head>
		<body>
	<h1>About texi</h1>
	<p>This is a paragraph</p>
	<p>fun stuff</p>
</body></html>
", result);
        }
    }
}
