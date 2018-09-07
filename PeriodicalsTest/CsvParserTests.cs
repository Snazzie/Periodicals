using System;
using System.Collections.Generic;
using NUnit.Framework;
using Periodicals;
using Periodicals.Magazines;
using Periodicals.Subscriptions;
using Periodicals.Users;

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
        public bool TryCreateSubscriptionFromLine_ReturnsSubscription(string line)
        {
            return CsvParser.TryCreateSubscriptionFromLine(MagazineService, line, out _);
        }
        [Test]
        public void TryCreateSubscriptionFromLine_ReturnedSubscriptionIsCorrect()
        {
            // ARRANGE
            var line = "Joan Krook,Amiga Gamer,20/01/2017,20/09/2017";
            var user = new User("Joan Krook");
            var expected = new Subscription(MagazineService, user, "Amiga Gamer", DateTime.Parse("20/01/2017"),
                DateTime.Parse("20/09/2017"));

            // ACT
            CsvParser.TryCreateSubscriptionFromLine(MagazineService, line, out var subscription);

            // ASSERT
            Assert.That(subscription.User.Name, Is.EqualTo(expected.User.Name));
            Assert.That(subscription.Magazine.Title, Is.EqualTo(expected.Magazine.Title));
            Assert.That(subscription.StartDate, Is.EqualTo(expected.StartDate));
            Assert.That(subscription.EndDate, Is.EqualTo(expected.EndDate));
        }
    }
}
