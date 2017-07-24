using FluentAssertions;

namespace FactOrFictionTest
{

    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FactOrFictionWeb.Parser;

    [TestClass]
    public class Parser
    {
        [TestMethod]
        public void ShittyParserTest()
        {
            var input = "fuck this fucking shit";
            var result = ShittyParser.Parse(input, '.');
            result.Should().Equal(input);
        }
    }
}
