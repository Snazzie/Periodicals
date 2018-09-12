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
            // ARRANGE
            var user = new User("Jeff");
            var type = typeof(Magazine);
            var startDate = Convert.ToDateTime("01/09/2014");
            var lastPaidDate = Convert.ToDateTime("31/08/2017");
            var magazine = new Magazine("blah", 0);
            ProductService.AddProduct(magazine);

            // ACT
            var sub = new Subscription(ProductService, type, user, magazine.Title, startDate, lastPaidDate);

            // ASSERT
            Assert.That(sub.User.Name, Is.EqualTo(user.Name));
            Assert.That(sub.Product.GetType(), Is.EqualTo(type));
            Assert.That(sub.Product.Title, Is.EqualTo(magazine.Title));
            Assert.That(sub.StartDate, Is.EqualTo(startDate));
            Assert.That(sub.EndDate, Is.Not.Null);
        }
        [Test]
        public void GetProductMonthlyRevenueInYear_ReturnsCorrectMonthlyRevenues()
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
            Assert.That((magazine.Price / 12) * 12,Is.EqualTo(rev.Sum()));
            Assert.That((newspaper.Price / 12) * (9 + 12), Is.EqualTo(rev2.Sum()));
        }

        [TestCase("20/01/2018", "01/02/2018", typeof(Magazine), ExpectedResult = true)]
        [TestCase("01/02/2018", "01/02/2018", typeof(Magazine), ExpectedResult = true)]
        [TestCase("01/02/2018", "01/02/2018", typeof(Newspaper), ExpectedResult = false)]
        [TestCase("02/02/2018", "01/02/2018", typeof(Magazine), ExpectedResult = false)]
        public bool IsFailedToPay_ReturnsCorrectly(string subscriptionEndDate, string today, Type type)
        {
            return SubscriptionService.IsFailedToPay(Convert.ToDateTime(subscriptionEndDate), Convert.ToDateTime(today), type);
        }
    }
}
