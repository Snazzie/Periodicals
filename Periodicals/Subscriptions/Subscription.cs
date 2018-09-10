using System;
using Periodicals.Products;
using Periodicals.Users;

namespace Periodicals.Subscriptions
{
    public class Subscription
    {
        private readonly int Id;
        public User User { get; private set; }
        public Product Product { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime LastPaid { get; private set; }

        //TODO: Record none existing magazines
        public Subscription(ProductService productService, Type type, User user, string title, DateTime startDate, DateTime lastPaid)
        {
            User = user;
            if (!productService.ProductExists(title, type, out var product))
            {
                if (type == typeof(Magazine))
                    Product = new Magazine(title, 0);
                else if (type == typeof(Magazine))
                    Product = new Newspaper(title, 0);
                productService.AddProduct(Product);
            }

            Product = product;
            StartDate = startDate;
            LastPaid = lastPaid;
            if(type == typeof(Magazine))
            EndDate = new DateTime(lastPaid.Year + 1, startDate.Month, 1).AddDays(-1);
            if(type == typeof(Newspaper))
                EndDate = new DateTime(lastPaid.Year, startDate.Month,startDate.Day).AddDays(365);
        }
    }
}
