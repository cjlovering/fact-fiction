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
            var result = WorkingParser.QuoteParse(input, '.');
            result.Should().Equal(input);
        }

        [TestMethod]
        [Owner("daverhur")]
        public void ShittyParserTestwithPeriod()
        {
            var input = "fuck this.fucking shit. and a third";
            var result = WorkingParser.QuoteParse(input, '.');
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

            var result = WorkingParser.PuctuationParse(textinput);
            result.Length.Should().Be(13);
            result.GetValue(12).Should().Equals("There had been fears that radical changes in government could have seen more insular economic policies and further questions over the future of the euro itself.");
        }

        [TestMethod]
        [Owner("tabald")]
        public void ParserTestOutOfBounds()
        {
            var textinput =
                @"President Trump inherited an economy that would barely budge – but under his watch, American businesses small and large have already created more than 800,000 new jobs since January. Company after company is responding to the president’s agenda with optimism – investing billions of dollars in American jobs, American workers and America’s future.";

            var result = WorkingParser.PuctuationParse(textinput);
            result.Length.Should().Be(4);
            result.GetValue(3).Should().Equals("Company after company is responding to the president’s agenda with optimism – investing billions of dollars in American jobs, American workers and America’s future.");
        }

        [TestMethod]
        [Owner("tabald")]
        public void QuoteParseIntegrate()
        {
            var textinput =
                @" ""It just depends on the case, and what had transpired, and what information was provided,"" he said.";
            var result = WorkingParser.PuctuationParse(textinput);
            result.Length.Should().Be(1);
            result.GetValue(0).Should().Equals("\"It just depends on the case, and what had transpired, and what information was provided,\" he said.");
        }

        [TestMethod]
        [Owner("tabald")]
        public void QuoteParseIntegrateMultipleSentence()
        {
            var textinput =
                @" ""It just depends on the case, and what had transpired, and what information was provided,/"" he said. ""In some cases, people could get sent back, and in some cases, people do stay.""";
            var result = WorkingParser.PuctuationParse(textinput);
            result.Length.Should().Be(2);
            result.GetValue(0).Should().Equals("\"It just depends on the case, and what had transpired, and what information was provided,\" he said.");
        }

        [TestMethod]
        [Owner("tabald")]
        public void ParseNewlineMultipleSentence()
        {
            var textinput =
                @"Line1
                  Line2
                  Line3";
            var result = WorkingParser.PuctuationParse(textinput);
            result.Length.Should().Be(3);
            result.GetValue(2).Should().Equals("Line 3");
        }

        [TestMethod]
        [Owner("tabald")]
        public void ParseNewlineHeader()
        {
            var textinput =
                @"'Irreversible brain damage'
                   The tip-off to authorities started when a man from the trailer asked a Walmart employee for water, the police chief said. The employee was concerned and called police for a welfare check.";
            var result = WorkingParser.PuctuationParse(textinput);
            result.Length.Should().Be(3);
            result.GetValue(0).Should().Equals("'Irreversible brain damage'");
        }
        
        [TestMethod]
        [Owner("davehur")]
        public void QuoteParseIntegrateMultipleSentences()
        {
            var textinput =
                @" The Republican chairman of the House Committee on Science, Space and Technology thinks you’re all worrying too much about climate change. Indeed Rep. Lamar Smith of Texas thinks global warming is actually a good thing.
Smith claimed in a post for the Daily Signal, a blog run by the right-wing Heritage Foundation — which has repeatedly questioned \""mainstream\"" climate science — that global warming has benefits that are ignored by \""alarmists.\"" Smith is taking a different tack than many of his Republican colleagues, who deny outright that the climate is changing at all.

Most research indicates that manmade climate change, caused by pumping greenhouse gases into the atmosphere (\""carbon enrichment,\"" in Smith’s words) will cause oceans to rise, economies to collapse, and biodiversity to plummet. If there are some silver linings to a warming planet, the destruction that will be caused by climate change far outweighs the few small benefits.
Let’s take a close look at what Smith’s op-ed claims:
A higher concentration of carbon dioxide in our atmosphere would aid photosynthesis, which in turn contributes to increased plant growth. This correlates to a greater volume of food production and better quality food.
The scientists whose work Smith is likely referring to here (we can’t say for sure because Smith didn’t cite any) warned against these arguments.
\""Although [skeptics] could use the results to say that CO2 is good for plants, it’s really missing the whole picture,\"" Trevor Keenan, an ecologist at Lawrence Berkeley National Laboratory who conducted a study that indicated that plants had ramped up photosynthesis, told PRI.
\""The kind of extreme temperatures we expect to see because of this CO2 is most definitely detrimental to plants, due to increased drought mortality, [and] increased fire frequencies globally,\"" he said.
As for the food supply, scientists have been saying the opposite for decades. And there’s evidence that global warming is already causing global food production to lag.
The world’s vegetated areas are becoming 25 to 50 percent greener, according to satellite images. Seventy percent of this greening is due to a rise in atmospheric carbon dioxide.
Professor Howie Epstein, who studies arctic greening through satellite images, told VICE News by email that Smith’s claim about global greening \""is at best misleading, and at worst completely false.\""
It’s true that parts of the Arctic have greened, and that the greening may be due to global warming, but we don’t know to what extent global warming is responsible — its greening isn’t nearly as beneficial as Smith suggests.
\""In the Arctic, we found that tundra vegetation had greened by a maximum of [about] 25 percent since the 1960s, and this trend actually began reversing itself sometime between 2000 and 2010 in different areas of the tundra,\"" Epstein said. \""In fact, there is now a great deal of interest in understanding tundra ‘browning.’\""
Greater vegetation assists in controlling water runoff, provides more habitats for many animal species, and even aids in climate stabilization, as more vegetation absorbs more carbon dioxide.
\""Greater vegetation can reduce water runoff, and potentially increase habitat,\"" says Epstein. But studies indicate that more carbon in the atmosphere actually mean plants absorb less water from the ground.
And most vegetation is close to being saturated with CO2 — they won’t absorb carbon out of the atmosphere at the rate they are now forever.  
When plant diversity increases, these vegetated areas can better eliminate carbon from the atmosphere.
The implication here is that getting carbon out of the atmosphere is a desirable outcome of more carbon in the atmosphere, which Smith says is a good thing. But regardless, plant diversity isn’t proven to increase carbon absorption.
\""There has been a lot of research on diversity effects on ecosystem functioning, and the results are rather mixed,\"" Epstein says. \""Often the right monoculture can be more productive than a diverse plant community.\""

Also, as the Earth warms, we are seeing beneficial changes to the earth’s geography. For instance, Arctic sea ice is decreasing. This development will create new commercial shipping lanes that provide faster, more convenient, and less costly routes between ports in Asia, Europe, and eastern North America.
The melting of arctic sea ice will likely create more icebergs, which may interfere with shipping lanes. This is already happening. In April off the coast of Newfoundland, cargo ships had to find new routes because their old ones were blocked off by icebergs.
And ice melting in polar regions isn’t something we should celebrate, anyway.
As sea ice drifts off into the ocean, the ice shelves that they’re a part of lose their structure and begin to collapse, and glacial ice flows into the ocean. So while sea ice, which is floating on water already, won’t contribute to rising sea levels, the flow Antarctic glacial ice into the oceans will.
And rising tides — which will put cities underwater, destroy property, and displace millions people — are not good for the economy.
Research has shown that regions that have enjoyed a major reduction in poverty achieved these gains by expanding the use of fossil fuels for energy sources.
There is research that suggests that coal is good for developing economies from places like the Institute for Energy Research, a D.C.-based non-profit think tank tank that advocates for free-market solutions to global energy and environment problems. But the World Bank refutes that research, and suggests instead that renewables will drive growth in the twenty first century.
And study after study indicates that global warming will be a major driver of global inequality. Suggesting that in the short-term developing economies might benefit from fossil fuels ignores the long-term damage burning those fuels will have on those developing economies down the line.
The Obama administration planned to spend hundreds of billions of dollars on policies that would have a negligible impact on the environment.
Obama did spend money to combat climate change. But the centerpiece of Obama’s legacy on climate change, the Clean Power Plan, left major decisions to the states, and, depending on how they would have complied if the plan were implemented, the costs would have varied.
But this is misleading, anyway: Having a negligible impact on the environment is exactly what policies that address climate change are designed to do. Not addressing climate change will have a non-negligible, and detrimental, impact on the environment.
The Clean Power Plan would have reduced global temperatures by only three one-hundredths of 1 degree Celsius.
Estimates suggest that the Clean Power Plan would have reduced emissions by 32 percent below 2005 levels by 2030. Because carbon in the atmosphere takes years to fully affect the environment, even if we stopped all carbon emissions now, global temperatures would continue to rise. A reduction in global temperatures isn’t likely to happen at all at this point, no matter what steps are taken to reduce the amount of carbon released into the atmosphere.
Bad deals like the Paris Agreement would cost the U.S. billions of dollars, a loss of hundreds of thousands of jobs, and have no discernible impact on global temperatures.
The Paris Agreement would have cost the U.S. billions of dollars. In January, President Obama paid $500 million into the UN Green Climate fund, and the U.S. would have continued to contribute to the fund had it stayed in the Paris deal.
But it wouldn’t have hurt job growth; CEOs of 30 of the biggest companies in the U.S. urged President Trump not to withdraw from Paris. The study Trump used to justify pulling out of the agreement was misleading. Leaving the deal is more likely to hurt job growth.";
            var result = WorkingParser.PuctuationParse(textinput);
            result.Length.Should().Be(69);
            result.GetValue(0).Should().Equals("The Republican chairman of the House Committee on Science, Space and Technology thinks you’re all worrying too much about climate change.");
        }
    }
}
