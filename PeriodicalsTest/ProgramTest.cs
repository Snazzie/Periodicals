using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Periodicals;

namespace PeriodicalsTest
{
    [TestFixture]
    public class ProgramTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetMagazineMonthlyRevenueInYear_ReturnsCorrectMonthlyRevenues()
        {
            var year = 2017;
            var user = new User("Jeff");
            var user2 = new User("Geo");
            var magazine = new Magazine("mag", 500);
            var magazine2 = new Magazine("haha", 400);
            var subscriptions = new List<Subscription>()
            {
                new Subscription(user, magazine, Convert.ToDateTime("01/01/2017"), Convert.ToDateTime("01/01/2017")),
                new Subscription(user2, magazine2, Convert.ToDateTime("01/04/2017"), Convert.ToDateTime("01/04/2017"))

            };
        

            List<float> rev = Program.GetMagazineMonthlyRevenueInYear(magazine, year,subscriptions);
            List<float> rev2 = Program.GetMagazineMonthlyRevenueInYear(magazine2, year, subscriptions);
            Assert.AreEqual((magazine.Price/12) * 12, rev.Sum());
            Assert.AreEqual((magazine2.Price/12) * 8, rev2.Sum());
        }
    }
}
