using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Periodicals.Products;
using Periodicals.Subscriptions;
using Periodicals.Users;

namespace Periodicals
{

    public class Program
    {
        public static ProductService ProductService => new ProductService(new List<Product>
        {
            new Magazine("Dogs Monthly", 399),
            new Magazine("Beekeeper", 249),
            new Magazine("Rock'n'Bass", 500),
            new Magazine("Amiga Gamer", 169),
            new Magazine("\"Snow, Ice and You\"", 749),
            new Newspaper("Dunthorpe Daily", 5999),
            new Newspaper("The Bugle",7249),
            new Newspaper("Plympton Gazette", 3800)
        });

        public static SubscriptionService SubscriptionService { get; set; } = new SubscriptionService(ProductService.Products);

        private static void Main(string[] args)
        {
            var magazineLines = CsvParser.CsvToLines(@"C:\Users\aaron.cooper\Documents\csvMagazineExport.txt");
            var magazinesToImport = CsvParser.LinesToSubscriptions(ProductService, typeof(Magazine), magazineLines);
            var newspaperLines = CsvParser.CsvToLines(@"C:\Users\aaron.cooper\Documents\csvNewspaperExport.txt");
            var newspapersToImport =  CsvParser.LinesToSubscriptions(ProductService, typeof(Newspaper), newspaperLines);
            SubscriptionService.AddSubscription(magazinesToImport);
            SubscriptionService.AddSubscription(newspapersToImport);
            Menu();
        }

        private static void Menu()
        {
            var exit = false;

            while (!exit)
            {
                Console.Write("#Menu \n" +
                              "1. Get all product revenue in year\n" +
                              "2. See which customers have failed to pay\n" +
                              "0. Exit\n\n" +
                              "Use your number key to select one. \n");

                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.Clear();
                        Console.Write("Get product revenue in year: ");
                        if (int.TryParse(Console.ReadLine(), out int year))
                        {
                            Console.Write("Type in the product type you would like to filter. eg: Magazine, Newspaper \n" +
                                          "Leave blank for all.\n" +
                                          "Filter:");
                            var productTypeFilter = Console.ReadLine();
                            Type productType = null;
                            if (productTypeFilter != "")
                            {
                                productType = Type.GetType(typeof(Product).Namespace + "." + productTypeFilter);
                                if (productType == null)
                                {
                                    Console.WriteLine($"{productTypeFilter} was not found!");
                                    Console.ReadKey();
                                    break;
                                }
                            }
                            SubscriptionService.ShowRevenueOfAllProductsInYear(year, productType);
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
