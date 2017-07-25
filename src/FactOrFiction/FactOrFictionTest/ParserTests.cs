namespace FactOrFictionTest
{
    using FactOrFictionTextHandling.Parser;
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FluentAssertions;

    [TestClass]
    public class Parser
    {
        [TestMethod]
        [Owner("daverhur")]
        public void ShittyParserTest()
        {
            var input = "fuck this fucking shit";
            var result = ShittyParser.QuoteParse(input, '.');
            result.Should().Equal(input);
        }

        [TestMethod]
        [Owner("daverhur")]
        public void ShittyParserTestwithPeriod()
        {
            var input = "fuck this.fucking shit. and a third";
            var result = ShittyParser.QuoteParse(input, '.');
            result.Length.Should().Equals(3);
            result.GetValue(2).Should().Equals("and a third");
        }

        [TestMethod]
        [Owner("daverhur")]
        public void ShittyParserTestwithBigText()
        {
            var textinput =
                @"LONDON(AP) — The International Monetary is more optimistic about the economy of the 19-country eurozone after a run of elections saw populist politicians defeated and risks to its outlook abated.
                    In an update to its April projections published Monday, the IMF revised up its growth forecasts for many eurozone countries, including the big four of Germany, France, Italy and Spain, after stronger than anticipated first quarter figures.
                    Germany, Europe's biggest economy, is projected to grow by 1.8 percent, up 0.2 percentage point on the previous estimate, while France is forecast to expand 1.5 percent, up 0.1 percentage point. Projections for Italy and Spain have been revised higher by a substantial 0.5 percentage point. The two are now expected to grow by 1.3 percent and 3.1 percent, respectively. All four are also expected to grow by more than anticipated in 2018.
                    Overall, the IMF expects the eurozone to expand by 1.9 percent this year, 0.2 percentage point more than its previous projection.That's just shy of the IMF's 2.1 percent forecast for the U.S., which was trimmed by 0.2 percentage point. However, it's slightly ahead of Britain's, whose projected growth was revised down 0.3 percentage point to 1.7 percent following a weak first quarter that raised concerns about the country's economy ahead of its exit from the European Union.

                    The IMF's eurozone upgrades come amid rising confidence in the bloc following a series of elections that saw populist politicians defeated, most notably in France, where Emmanuel Macron defeated the far-right candidate Marine Le Pen in May's presidential election.
                    At the start of the year, political risks were considered the major hurdle facing the eurozone. There had been fears that radical changes in government could have seen more insular economic policies and further questions over the future of the euro itself.
                    ";

            var result = ShittyParser.PuctuationParse(textinput);
            result.Length.Should().Be(13);
            result.GetValue(12).Should().Equals("There had been fears that radical changes in government could have seen more insular economic policies and further questions over the future of the euro itself.");
        }

        [TestMethod]
        [Owner("tabald")]
        public void ParserTestOutOfBounds()
        {
            var textinput =
                @"President Trump inherited an economy that would barely budge – but under his watch, American businesses small 
                    and large have already created more than 800,000 new jobs since January. Company after company is responding to the 
                    president’s agenda with optimism – investing billions of dollars in American jobs, American workers and America’s future.";

            var result = ShittyParser.PuctuationParse(textinput);
            result.Length.Should().Be(4);
            result.GetValue(3).Should().Equals("Company after company is responding to the president’s agenda with optimism – investing billions of dollars in American jobs, American workers and America’s future.");
        }
    }
}
