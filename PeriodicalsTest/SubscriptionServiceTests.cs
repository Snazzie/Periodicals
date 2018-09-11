using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Periodicals.Products;
using Periodicals.Subscriptions;
using Periodicals.Users;

namespace PeriodicalsTest
{
    [TestFixture]
    public class SubscriptionServiceTests
    {
        public ProductService ProductService;

        [SetUp]
        public void Setup()
        {
            ProductService = new ProductService();
        }

        [Test]
        public void CreateSubscriptionWithExistingProduct_CreateSuccess()
        {
            var user = new User("Jeff");
            var type = typeof(Magazine);
            var startDate = Convert.ToDateTime("01/09/2014");
            var lastPaidDate = Convert.ToDateTime("31/08/2017");
            var magazine = new Magazine("blah", 0);
            ProductService.AddProduct(magazine);
            var sub = new Subscription(ProductService, type, user, magazine.Title, startDate, lastPaidDate);

            Assert.That(sub.User.Name, Is.EqualTo(user.Name));
            Assert.That(sub.Product.GetType(), Is.EqualTo(type));
            Assert.That(sub.Product.Title, Is.EqualTo(magazine.Title));
            Assert.That(sub.StartDate, Is.EqualTo(startDate));
            Assert.That(sub.EndDate, !Is.Empty);
        }
        [Test]
        public void GetMagazineMonthlyRevenueInYear_ReturnsCorrectMonthlyRevenues()
        {
            // ARRANGE
            var year = 2017;
            var user = new User("Jeff");
            var user2 = new User("Geo");
            var magazine = new Magazine("mag", 500);
            var newspaper = new Newspaper("haha", 400);
            ProductService.AddProduct(new List<Product> { magazine, newspaper });
            var subscriptionService = new SubscriptionService(ProductService.Products);
            subscriptionService.AddSubscription(new List<Subscription>()
            {
                new Subscription(ProductService,typeof(Magazine), user, magazine.Title, Convert.ToDateTime("01/01/2017"),
                    Convert.ToDateTime("01/01/2018")),
                new Subscription(ProductService, typeof(Newspaper), user2, newspaper.Title, Convert.ToDateTime("01/04/2017"),
                    Convert.ToDateTime("01/04/2017")),
                new Subscription(ProductService, typeof(Newspaper), user, newspaper.Title, Convert.ToDateTime("01/09/2014"),
                    Convert.ToDateTime("01/09/2017"))
            });


            // ACT
            List<float> rev = subscriptionService.GetProductMonthlyRevenueInYear(magazine, year);
            List<float> rev2 = subscriptionService.GetProductMonthlyRevenueInYear(newspaper, year);

            // ASSERT
            Assert.AreEqual((magazine.Price / 12) * 12, rev.Sum());
            Assert.AreEqual((newspaper.Price / 12) * (9 + 12), rev2.Sum());
        }
        
        [TestCase(0, -1, "01/02/2018", ExpectedResult = true)]
        [TestCase(-1, 0, "01/02/2018", ExpectedResult = true)]
        [TestCase(0, 0, "01/02/2018", ExpectedResult = true)]  
        [TestCase(0, 1, "01/02/2018", ExpectedResult = false)]
        [TestCase(1, 0, "01/02/2018", ExpectedResult = false)]
        public bool IsFailedToPay_ReturnsCorrectly(int yearOffset, int monthOffset, string firstDayOfMonth)
        {
            var subscriptionEndDate = Convert.ToDateTime(firstDayOfMonth).AddYears(yearOffset).AddMonths(monthOffset).AddDays(-1); // always set to end of last month

            return SubscriptionService.IsFailedToPay(subscriptionEndDate, Convert.ToDateTime(firstDayOfMonth));
        }
    }
}
