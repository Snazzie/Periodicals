using System;
using NUnit.Framework;
using Periodicals.Products;
using Periodicals.Subscriptions;
using Periodicals.Users;

namespace PeriodicalsTest
{
    class SubscriptionTest
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
            //ARRANGE
            var user = new User("Jeff");
            var type = typeof(Magazine);
            var startDate = Convert.ToDateTime("01/09/2014");
            var lastPaidDate = Convert.ToDateTime("31/08/2017");
            var magazine = new Magazine("blah", 0);
            ProductService.AddProduct(magazine);

            //ACT
            var sub = new Subscription(ProductService, type, user, magazine.Title, startDate, lastPaidDate);

            //ASSERT
            Assert.That(sub.User.Name, Is.EqualTo(user.Name));
            Assert.That(sub.Product.GetType(), Is.EqualTo(type));
            Assert.That(sub.Product.Title, Is.EqualTo(magazine.Title));
            Assert.That(sub.StartDate, Is.EqualTo(startDate));
            Assert.That(sub.LastPaid, Is.EqualTo(lastPaidDate));
            Assert.That(sub.EndDate, Is.Not.Null);
        }
        [Test]
        public void CreateSubscriptionWithNoneExistingProduct_CreateSuccess()
        {
            //ARRANGE
            var user = new User("Jeff");
            var type = typeof(Magazine);
            var startDate = Convert.ToDateTime("01/09/2014");
            var lastPaidDate = Convert.ToDateTime("31/08/2017");
            var magazine = new Magazine("blah", 0);

            //ACT
            var sub = new Subscription(ProductService, type, user, magazine.Title, startDate, lastPaidDate);

            //ASSERT
            Assert.That(sub.User.Name, Is.EqualTo(user.Name));
            Assert.That(sub.Product.GetType(), Is.EqualTo(type));
            Assert.That(sub.Product.Title, Is.EqualTo(magazine.Title));
            Assert.That(sub.StartDate, Is.EqualTo(startDate));
            Assert.That(sub.LastPaid, Is.EqualTo(lastPaidDate));
            Assert.That(sub.EndDate, Is.Not.Null);
        }
    }
}