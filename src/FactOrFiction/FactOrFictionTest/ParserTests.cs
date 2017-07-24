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
        [Owner("daverhur")]
        public void ShittyParserTest()
        {
            var input = "fuck this fucking shit";
            var result = ShittyParser.Parse(input, '.');
            result.Should().Equal(input);
        }

        [TestMethod]
        [Owner("daverhur")]
        public void ShittyParserTestwithPeriod()
        {
            var input = "fuck this.fucking shit. and a third";
            var result = ShittyParser.Parse(input, '.');
            result.Length.Should().Equals(3);
            result.GetValue(2).Should().Equals("and a third");
        }
    }
}
