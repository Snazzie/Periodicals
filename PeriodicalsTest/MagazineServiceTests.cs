using System;
using System.Collections.Generic;
using NUnit.Framework;
using Periodicals.Products;

namespace PeriodicalsTest
{
    [TestFixture]
    public class MagazineServiceTests
    {
        public ProductService ProductService;

        [SetUp]
        public void SetUp()
        {
            ProductService = new ProductService(new List<Product>
            {
                new Magazine("Dogs Monthly", 399),
                new Magazine("Beekeeper", 249),
                new Magazine("Rock'n'Bass", 500),
                new Magazine("Amiga Gamer", 169),
                new Magazine("\"Snow, Ice and You\"", 749),
                new Newspaper("Dunthorpe Daily", 5999),
                new Newspaper("The Bugle",7249),
                new Newspaper("Plympton Gazette", 3800)
            });
        }

        [TestCase("You and i", typeof(Magazine), ExpectedResult = false)]
        [TestCase("Beekeeper", typeof(Magazine), ExpectedResult = true)]
        [TestCase("You and i", typeof(Newspaper), ExpectedResult = false)]
        [TestCase("The Bugle", typeof(Newspaper), ExpectedResult = true)]
        public bool ProductExists_ReturnsCorrectly(string title, Type type)
        {
            return ProductService.ProductExists(title, type, out _);
        }

    }
}
