using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Periodicals.Magazines;
using Periodicals.Subscriptions;
using Periodicals.Users;

namespace Periodicals
{
    public class CsvParser
    {
        public static List<string> CsvToLines(string path)
        {
            return File.ReadAllLines(path).ToList();
        }

        public static List<Subscription> LinesToSubscriptions(MagazineService magazineService, List<string> lines)
        {
            var subscriptions = new List<Subscription>();
            foreach (var line in lines)
            {
                if (TryCreateSubscriptionFromLine(magazineService, line, out var newSubscription))
                    subscriptions.Add(newSubscription);
            }

            return subscriptions;
        }

        public static bool TryCreateSubscriptionFromLine(MagazineService magazineService,string line, out Subscription subscription)
        {
            var newLines = line.Split(',').ToList();
            if (DateTime.TryParse(newLines.Last(), out _))
            {
                if (newLines.Count > 4)                         // only handles 1 comma in Title 
                {
                    newLines[1] += "," + newLines[2];
                    newLines.RemoveAt(2);
                }
                var user = new User(newLines[0]);
                var title = newLines[1];
                var magazine = title;
                var startDate = DateTime.Parse(newLines[2]);
                var lastPaid = DateTime.Parse(newLines[3]);
                subscription = new Subscription(magazineService, user, magazine, startDate, lastPaid);
                return true;
            }

            subscription = null;
            return false;
        }



    }
}
