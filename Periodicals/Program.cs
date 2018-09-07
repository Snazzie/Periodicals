using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Periodicals.Magazines;
using Periodicals.Subscriptions;
using Periodicals.Users;

namespace Periodicals
{

    public class Program
    {
        public static MagazineService MagazineService => new MagazineService(new List<Magazine>
        {
            new Magazine("Dogs Monthly", 399),
            new Magazine("Beekeeper", 249),
            new Magazine("Rock'n'Bass", 500),
            new Magazine("Amiga Gamer", 169),
            new Magazine("\"Snow, Ice and You\"", 749)

        });

        public static SubscriptionService SubscriptionService { get; set; } = new SubscriptionService(MagazineService.Magazines);

        private static void Main(string[] args)
        {
            var lines = CsvParser.CsvToLines(@"C:\Users\aaron.cooper\Documents\csvExport.txt");
            var subscriptions = CsvParser.LinesToSubscriptions(MagazineService, lines);
            SubscriptionService.AddSubscription(subscriptions);
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
                        if (int.TryParse(Console.ReadLine(), out int year))
                        {
                            SubscriptionService.ShowRevenueOfAllMagazinesInYear(year);
                        }
                        else
                        {
                            Console.WriteLine("Input Invalid");
                        }
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
    }
}
