using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Periodicals
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
            EndDate = lastPaid.AddMonths(12);
        }


    }
}
