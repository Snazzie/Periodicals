using System;
using System.Collections.Generic;
using System.Linq;
using Periodicals.Products;

namespace Periodicals.Subscriptions
{
    public class SubscriptionService
    {
        public SubscriptionService(List<Product> products)
        {
            Subscriptions = new List<Subscription>();
            Products = products;
        }

        public List<Subscription> Subscriptions { get; set; }
        private List<Product> Products { get; }

        public void AddSubscription(Subscription subscription)
        {
            Subscriptions.Add(subscription);
        }

        public void AddSubscription(IEnumerable<Subscription> subscriptions)
        {
            subscriptions.ToList().ForEach(s => Subscriptions.Add(s));
        }

        public List<Subscription> GetFailedToPaySubscriptions()
        {
            var failedToPaySubscriptions = new List<Subscription>();

            foreach (var subscription in Subscriptions)
                if (IsFailedToPay(subscription.EndDate, DateTime.Today, subscription.Product.GetType()))
                    failedToPaySubscriptions.Add(subscription);

            return failedToPaySubscriptions;
        }

        public static bool IsFailedToPay(DateTime subscriptionEndDate, DateTime today, Type type)
        {
            //var startOfThisMonth = Convert.ToDateTime($"1/{today.Month}/{today.Year}");    possibly not needed
            if (type == typeof(Magazine))
                return subscriptionEndDate <= today;
            if (type == typeof(Newspaper))
                return subscriptionEndDate < today;
            throw new Exception("Type doesnt exist");
        }

        public List<float> GetProductMonthlyRevenueInYear(Product product, int year)
        {
            var subs = Subscriptions.FindAll(s =>
                s.Product.Title == product.Title && s.StartDate.Year <= year && year <= s.EndDate.Year);
            var revs = new List<float>();
            for (var month = 1; month <= 12; month++)
            {
                var checkingMonth = Convert.ToDateTime($"01/{month}/{year}");
                var subsInMonth = subs.FindAll(s => s.StartDate <= checkingMonth && checkingMonth <= s.EndDate);
                revs.Add(subsInMonth.Count * (product.Price / 12));
            }

            return revs;
        }

        /// <param name="year"></param>
        /// <param name="productType">Null returns all</param>
        public Dictionary<Product, List<float>> GetProductMonthlyRevenueInYearByType(int year, Type productType = null)
        {
            var productsMonthlyRevenue = new Dictionary<Product, List<float>>();
            if (productType != null)
                foreach (var product in Products.FindAll(p => p.GetType() == productType))
                    productsMonthlyRevenue.Add(product, GetProductMonthlyRevenueInYear(product, year));
            else
                foreach (var product in Products)
                    productsMonthlyRevenue.Add(product, GetProductMonthlyRevenueInYear(product, year));


            return productsMonthlyRevenue;
        }
    }
}