using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Periodicals;
using Periodicals.Magazines;

namespace PeriodicalsTest
{
    [TestFixture]
    public class CsvParserTests
    {
        public MagazineService MagazineService; 

        [SetUp]
        public void SetUp()
        {
            MagazineService = new MagazineService(new List<Magazine>
            {
                new Magazine("Dogs Monthly", 399),
                new Magazine("Beekeeper", 249),
                new Magazine("Rock'n'Bass", 500),
                new Magazine("Amiga Gamer", 169),
                new Magazine("\"Snow, Ice and You\"", 749)

            });
        }

        [TestCase("", ExpectedResult = false)]
        [TestCase("JibberishDatablahBlag2131", ExpectedResult = false)]
        [TestCase("Name,Title,Subscription Start Date,Last Paid", ExpectedResult = false)]
        [TestCase("Joan Krook,Amiga Gamer,20/01/2017,20/09/2017", ExpectedResult = true )]
        [TestCase("Joan Krook,HelloWorld,20/01/2017,20/09/2017", ExpectedResult = true)]
        [TestCase("Joan Krook,\"HelloWorld\",20/01/2017,20/09/2017", ExpectedResult = true)]
        [TestCase("Joan Krook,\"Hello, World\",20/01/2017,20/09/2017", ExpectedResult = true)]
        public bool TryCreateSubscriptionFromLine_ReturnsSubscription(string lines)
        {
            return CsvParser.TryCreateSubscriptionFromLine(MagazineService, lines, out _);
        }
    }
}
