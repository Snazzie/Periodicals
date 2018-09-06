using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public static List<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        static void Main(string[] args)
        {
            ProccessCsv(@"C:\Users\aaron.cooper\Documents\csvExport.txt");
            Menu();
        }

        private static void Menu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Write("#Menu \n" +
                              "1. Get all magazine revenue in year\n" +
                              "2. See which customers have failed to pay\n \n" +
                              "Use your number key to select one. \n");

                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.Clear();
                        Console.Write("Get all magazine revenue in year: ");
                        var year = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                        GetRevenueOfAllMagazinesInYear(year);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case '2':
                        Console.Clear();
                        CustomersFailedToPlay();
                        break;
                    case '0':
                        Console.Clear();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        private static void CustomersFailedToPlay()
        {
            throw new NotImplementedException();
        }

        static void GetRevenueOfAllMagazinesInYear(int year)
        {
            Console.WriteLine($"# Monthly Revenue in {year} for all magazines");
            var magazinesMonthlyRevenue = new Dictionary<Magazine, List<float>>();
            foreach (var magazine in Magazines)
            {
                magazinesMonthlyRevenue.Add(magazine, GetMagazineMonthlyRevenueInYear(magazine, year, Subscriptions));
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
        public static List<float> GetMagazineMonthlyRevenueInYear(Magazine magazine, int year, List<Subscription> subscriptions)
        {
            var subs = subscriptions.FindAll(s => s.Magazine.Title == magazine.Title && s.StartDate.Year <= year && year <= s.EndDate.Year);
            var revs = new List<float>();
            for (int month = 1; month <= 12; month++)
            {
                var checkingMonth = Convert.ToDateTime($"01/{month}/{year}");
                var subsInMonth = subs.FindAll(s => s.StartDate <= checkingMonth && checkingMonth <= s.EndDate);
                revs.Add(subsInMonth.Count * (magazine.Price / 12));
            }

            return revs;
        }


        static void ProccessCsv(string path)
        {

            var lines = File.ReadAllLines(path);
            var skip = true;
            foreach (var line in lines)
            {
                if (skip)
                {
                    skip = false;
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
                Subscriptions.Add(new Subscription(user, magazine, startDate, lastPaid));

            }

        }


    }
}
