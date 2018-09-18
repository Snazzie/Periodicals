using System;
using Periodicals.Products;
using Periodicals.Users;

namespace Periodicals.Subscriptions
{
    public class Subscription
    {
        private readonly int Id;

        //TODO: Record none existing magazines
        public Subscription(ProductService productService, Type type, User user, string title, DateTime startDate,
            DateTime lastPaid, int id = 0)
        {
            if (!productService.ProductExists(title, type, out var product))
            {
                product = CreateProductFromType(type, title);
                productService.AddProduct(product);
            }

            User = user;
            Id = id;
            Product = product;
            StartDate = startDate;
            LastPaid = lastPaid;
            EndDate = CalculateEndDate(type, startDate, lastPaid);
        }

        public User User { get; }
        public Product Product { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public DateTime LastPaid { get; }

        public DateTime CalculateEndDate(Type type, DateTime startDate, DateTime lastPaid)
        {
            if (type == typeof(Magazine))
                return new DateTime(lastPaid.Year + 1, startDate.Month, 1).AddDays(-1);
            if (type == typeof(Newspaper))
                return new DateTime(lastPaid.Year, startDate.Month, startDate.Day).AddDays(365);

            throw new Exception("Type doesnt exist");
        }

        private Product CreateProductFromType(Type type, string title)
        {
            if (type == typeof(Magazine))
                return new Magazine(title, 0);
            if (type == typeof(Newspaper))
                return new Newspaper(title, 0);
            throw new Exception("Type doesnt exist");
        }
    }
}