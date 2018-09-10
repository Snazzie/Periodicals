using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Periodicals.Magazines;
using Periodicals.Subscriptions;
using Periodicals.Users;

namespace PeriodicalsTest
{
    [TestFixture]
    public class SubscriptionServiceTests
    {
        public MagazineService MagazineService;

        [SetUp]
        public void Setup()
        {
            MagazineService = new MagazineService();
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
            MagazineService.AddMagazine(new List<Magazine> { magazine, magazine2 });
            var subscriptionService = new SubscriptionService(MagazineService.Magazines);
            subscriptionService.AddSubscription(new List<Subscription>()
            {
                new Subscription(MagazineService, user, magazine.Title, Convert.ToDateTime("01/01/2017"),
                    Convert.ToDateTime("01/01/2018")),
                new Subscription(MagazineService, user2, magazine2.Title, Convert.ToDateTime("01/04/2017"),
                    Convert.ToDateTime("01/04/2017")),
                new Subscription(MagazineService, user, magazine2.Title, Convert.ToDateTime("01/09/2014"),
                    Convert.ToDateTime("01/09/2017"))
            });


            // ACT
            List<float> rev = subscriptionService.GetMagazineMonthlyRevenueInYear(magazine, year);
            List<float> rev2 = subscriptionService.GetMagazineMonthlyRevenueInYear(magazine2, year);

            // ASSERT
            Assert.AreEqual((magazine.Price / 12) * 12, rev.Sum());
            Assert.AreEqual((magazine2.Price / 12) * (9 + 12), rev2.Sum());
        }

        [TestCase(0, -1, "01/02/2018", ExpectedResult = true)]  // oneMonthBefore
        [TestCase(-1, 0, "01/02/2018", ExpectedResult = true)]  // oneYearBefore
        [TestCase(0, 0, "01/02/2018", ExpectedResult = false)]  // sameDay
        [TestCase(0, 1, "01/02/2018", ExpectedResult = false)]  // oneMonthAfter
        [TestCase(1, 0, "01/02/2018", ExpectedResult = false)]  // oneYearAfter
        public bool IsFailedToPay_ReturnsCorrectly(int yearOffset, int monthOffset, string todayDate)
        {
            var subscriptionEndDate = Convert.ToDateTime(todayDate).AddYears(yearOffset).AddMonths(monthOffset);

            return SubscriptionService.IsFailedToPay(subscriptionEndDate, Convert.ToDateTime(todayDate));
        }
    }
}
