using System;
using System.Collections.Generic;
using System.Linq;
using Periodicals.Magazines;
using Periodicals.Users;

namespace Periodicals.Subscriptions
{
    public class SubscriptionService
    {
        public List<Subscription> Subscriptions { get; set; }
        private List<Magazine> Magazines { get; set; }

        public SubscriptionService(List<Magazine> magazines)
        {
            Subscriptions = new List<Subscription>();
            Magazines = magazines;
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
            var failedToPaySubscriptions = new List<Subscription>();

            foreach (var subscription in Subscriptions)
            {
                if (subscription.EndDate < DateTime.Today)
                {
                    failedToPaySubscriptions.Add(subscription);
                }
            }

            foreach (var subscription in failedToPaySubscriptions)
            {
                Console.WriteLine($"  -{subscription.User.Name} failed to pay for {subscription.Magazine.Title}\n" +
                                  $"      Last paid date: {subscription.LastPaid.ToShortDateString()}\n" +
                                  $"      Subscription end date: {subscription.EndDate.ToShortDateString()}\n");
            }
        }

        public List<float> GetMagazineMonthlyRevenueInYear(Magazine magazine, int year)
        {
            var subs = Subscriptions.FindAll(s => s.Magazine.Title == magazine.Title && s.StartDate.Year <= year && year <= s.EndDate.Year);
            var revs = new List<float>();
            for (int month = 1; month <= 12; month++)
            {
                var checkingMonth = Convert.ToDateTime($"01/{month}/{year}");
                var subsInMonth = subs.FindAll(s => s.StartDate <= checkingMonth && checkingMonth <= s.EndDate);
                revs.Add(subsInMonth.Count * (magazine.Price / 12));
            }
            return revs;
        }

        public void ShowRevenueOfAllMagazinesInYear(int year)
        {
            Console.WriteLine($"# Monthly Revenue in {year} for all magazines");
            var magazinesMonthlyRevenue = new Dictionary<Magazine, List<float>>();
            foreach (var magazine in Magazines)
            {
                magazinesMonthlyRevenue.Add(magazine, GetMagazineMonthlyRevenueInYear(magazine, year));
            }

            foreach (var magazineMonthlyRevenue in magazinesMonthlyRevenue)
            {
                Console.WriteLine($"--{magazineMonthlyRevenue.Key.Title} -> Sum: ${magazineMonthlyRevenue.Value.Sum()}");

                for (int month = 1; month <= 12; month++)
                {
                    Console.WriteLine($"   month:{month} = ${magazineMonthlyRevenue.Value[month - 1]}");
                }
            }

        }
    }
}
