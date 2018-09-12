using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Periodicals.Products;
using Periodicals.Subscriptions;

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

        [STAThread]
        private static void Main(string[] args)
        {
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
                              "3. Import csv\n" +
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
                    case '3':
                        Console.Clear();
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        var result = openFileDialog.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            var subscriptionLines = CsvParser.CsvToLines(openFileDialog.FileName);
                            Console.Write("Available product types: Magazine, Newspaper\n" +
                                          "Import product type: ");
                            var typeInput = Console.ReadLine();
                            var type = Type.GetType(typeof(Product).Namespace + "." + typeInput);
                            if (type == null)
                            {
                                Console.WriteLine($"{typeInput} was not found!");
                                Console.ReadKey();
                                break;
                            }
                            var subscriptionsToImport = CsvParser.LinesToSubscriptions(ProductService, type, subscriptionLines);
                            SubscriptionService.AddSubscription(subscriptionsToImport);
                        }
                        Console.WriteLine("Successfully imported.");
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
