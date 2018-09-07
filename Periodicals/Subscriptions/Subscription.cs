using System;
using Periodicals.Magazines;
using Periodicals.Users;

namespace Periodicals.Subscriptions
{
    public class Subscription
    {
        private readonly int Id;
        public User User { get; private set; }
        public Magazine Magazine { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime LastPaid { get; private set; }

        //TODO: Record none existing magazines
        public Subscription(MagazineService magazineService, User user, string magazineTitle, DateTime startDate, DateTime lastPaid)
        {
            User = user;
            if (!magazineService.MagazineExists(magazineTitle, out var magazine))
            {
                Magazine = new Magazine(magazineTitle, 0);
                magazineService.AddMagazine(Magazine);
            }

            Magazine = magazine;
            StartDate = startDate;
            LastPaid = lastPaid;
            EndDate = new DateTime(lastPaid.Year + 1, startDate.Month, 1).AddDays(-1);
        }
    }
}
