using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Periodicals.GeneralClasses;
using Periodicals.Subscriptions;
namespace Periodicals
{
    public class Program
    {
        private static List<Magazine> Magazines => new List<Magazine>()
        {
            new Magazine("Dogs Monthly", 399),
            new Magazine("Beekeeper", 249),
            new Magazine("Rock'n'Bass", 500),
            new Magazine("Amiga Gamer", 169),
            new Magazine("\"Snow, Ice and You\"", 749)

        };
        private static List<User> users = new List<User>();
        public static SubscriptionService SubscriptionService { get; set; } = new SubscriptionService(Magazines);

        private static void Main(string[] args)
        {
            ProccessCsv(@"C:\Users\aaron.cooper\Documents\csvExport.txt");
            Menu();
        }

        private static void Menu()
        {
            var exit = false;

            while (!exit)
            {
                Console.Write("#Menu \n" +
                              "1. Get all magazine revenue in year\n" +
                              "2. See which customers have failed to pay\n" +
                              "0. Exit\n\n" +
                              "Use your number key to select one. \n");

                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.Clear();
                        Console.Write("Get all magazine revenue in year: ");
                        var year = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                        SubscriptionService.ShowRevenueOfAllMagazinesInYear(year);
                        Console.ReadKey();
                        break;
                    case '2':
                        Console.Clear();
                        SubscriptionService.ShowFailedToPayCustomers();
                        Console.ReadKey();
                        break;
                    case '0':
                        Console.Clear();
                        exit = true;
                        break;
                }
                Console.Clear();
            }
        }

        static void ProccessCsv(string path)
        {
            var lines = File.ReadAllLines(path);
            var skipFirstLine = true;
            foreach (var line in lines)
            {
                if (skipFirstLine)
                {
                    skipFirstLine = false;
                    continue;
                }
                var newLines = line.Split(',').ToList();
                if (newLines.Count > 4)
                {
                    newLines[1] += "," + newLines[2];
                    newLines.RemoveAt(2);
                }

                var user = new User(newLines[0]);
                var title = newLines[1];

                var magazine = Magazines.Find(m => m.Title == title);
                var startDate = DateTime.Parse(newLines[2]);
                var lastPaid = DateTime.Parse(newLines[3]);

                users.Add(user);
                SubscriptionService.AddSubscription(user, magazine, startDate, lastPaid);

            }
        }
    }
}
