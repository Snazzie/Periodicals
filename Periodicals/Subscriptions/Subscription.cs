using System;
using Periodicals.Magazines;
using Periodicals.Users;

namespace Periodicals.Subscriptions
{
    public class Subscription
    {
        public User User { get; private set; }
        public Magazine Magazine { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime LastPaid { get; private set; }

        public Subscription(User user, Magazine magazine, DateTime startDate, DateTime lastPaid)
        {
            User = user;
            Magazine = magazine;
            StartDate = startDate;
            LastPaid = lastPaid;
            EndDate = new DateTime(lastPaid.Year + 1, startDate.Month, 1).AddDays(-1);
        }
    }
}
