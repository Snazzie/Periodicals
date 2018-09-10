using System;
using System.Collections.Generic;
using System.Linq;
using Periodicals.Products;
using Periodicals.Users;

namespace Periodicals.Subscriptions
{
    public class SubscriptionService
    {
        public List<Subscription> Subscriptions { get; set; }
        private List<Product> Products { get; set; }

        public SubscriptionService(List<Product> products)
        {
            Subscriptions = new List<Subscription>();
            Products = products;

        }

        public void AddSubscription(Subscription subscription)
        {
            Subscriptions.Add(subscription);
        }

        public void AddSubscription(IEnumerable<Subscription> subscriptions)
        {
            subscriptions.ToList().ForEach(s => Subscriptions.Add(s));
        }

        public void ShowFailedToPayCustomers()
        {
            Console.WriteLine($"# Users who have failed to pay as of today ({DateTime.Today.ToShortDateString()}): \n");


            foreach (var subscription in GetFailedToPaySubscriptions())
            {
                Console.WriteLine($"  -{subscription.User.Name} failed to pay for {subscription.Product.Title}\n" +
                                  $"      Subscription Started: {subscription.StartDate.ToShortDateString()}\n" +
                                  $"      Last paid date: {subscription.LastPaid.ToShortDateString()}\n" +
                                  $"      Subscription end date: {subscription.EndDate.ToShortDateString()}\n");
            }
        }

        public List<Subscription> GetFailedToPaySubscriptions()
        {
            var failedToPaySubscriptions = new List<Subscription>();

            foreach (var subscription in Subscriptions)
            {
                if (IsFailedToPay(subscription.EndDate, DateTime.Today))
                {
                    failedToPaySubscriptions.Add(subscription);
                }
            }

            return failedToPaySubscriptions;
        }

        public static bool IsFailedToPay(DateTime subscriptionEndDate, DateTime today)
        {
            var startOfThisMonth = Convert.ToDateTime($"1/{today.Month}/{today.Year}");
            return subscriptionEndDate < startOfThisMonth;
        }

        public List<float> GetProductMonthlyRevenueInYear(Product product, int year)
        {
            var subs = Subscriptions.FindAll(s => s.Product.Title == product.Title && s.StartDate.Year <= year && year <= s.EndDate.Year);
            var revs = new List<float>();
            for (int month = 1; month <= 12; month++)
            {
                var checkingMonth = Convert.ToDateTime($"01/{month}/{year}");
                var subsInMonth = subs.FindAll(s => s.StartDate <= checkingMonth && checkingMonth <= s.EndDate);
                revs.Add(subsInMonth.Count * (product.Price / 12));
            }
            return revs;
        }
        public void ShowRevenueOfAllProductsInYear(int year, Type type = null)
        {
            Console.WriteLine($"# Monthly Revenue in {year} for all magazines");
            var productsMonthlyRevenue = new Dictionary<Product, List<float>>();
            if (type != null)
                foreach (var product in Products.FindAll(p => p.GetType() == type))
                {
                    productsMonthlyRevenue.Add(product, GetProductMonthlyRevenueInYear(product, year));
                }
            else
                foreach (var product in Products)
                {
                    productsMonthlyRevenue.Add(product, GetProductMonthlyRevenueInYear(product, year));
                }


            foreach (var productMonthlyRevenue in productsMonthlyRevenue)
            {
                Console.WriteLine($"--{productMonthlyRevenue.Key.Title} -> Sum: ${productMonthlyRevenue.Value.Sum()}");

                for (int month = 1; month <= 12; month++)
                {
                    Console.WriteLine($"   month:{month} = ${productMonthlyRevenue.Value[month - 1]}");
                }
            }

        }
    }
}
