using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Periodicals.GeneralClasses;
using Periodicals.Subscriptions;

namespace PeriodicalsTest
{
    [TestFixture]
    public class SubscriptionServiceTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetMagazineMonthlyRevenueInYear_ReturnsCorrectMonthlyRevenues()
        {
            // ARRANGE
            var year = 2017;
            var user = new User("Jeff");
            var user2 = new User("Geo");
            var magazine = new Magazine("mag", 500);
            var magazine2 = new Magazine("haha", 400);
            var magazines = new List<Magazine> {magazine, magazine2};
            var subscriptionService = new SubscriptionService(magazines);
            subscriptionService.AddSubscription(new List<Subscription>()
            {
                new Subscription(user, magazine, Convert.ToDateTime("01/01/2017"),
                    Convert.ToDateTime("01/01/2018")),
                new Subscription(user2, magazine2, Convert.ToDateTime("01/04/2017"),
                    Convert.ToDateTime("01/04/2017")),
                new Subscription(user, magazine2, Convert.ToDateTime("01/09/2014"),
                    Convert.ToDateTime("01/09/2017"))
            });


            // ACT
            List<float> rev = subscriptionService.GetMagazineMonthlyRevenueInYear(magazine, year);
            List<float> rev2 = subscriptionService.GetMagazineMonthlyRevenueInYear(magazine2, year);

            // ASSERT
            Assert.AreEqual((magazine.Price / 12) * 12, rev.Sum());
            Assert.AreEqual((magazine2.Price / 12) * (9 + 12), rev2.Sum());
        }
    }
}
