using System;
using System.Linq;
using System.Windows.Forms;
using Periodicals.Products;
using Periodicals.Subscriptions;

namespace Periodicals
{
    public class Menu
    {
        public Menu(ProductService productService, SubscriptionService subscriptionService)
        {
            ProductService = productService;
            SubscriptionService = subscriptionService;
        }

        public ProductService ProductService { get; set; }
        public SubscriptionService SubscriptionService { get; set; }

        private void ShowFailedToPayCustomers()
        {
            Console.WriteLine($"# Users who have failed to pay as of today ({DateTime.Today.ToShortDateString()}): \n");

            foreach (var subscription in SubscriptionService.GetFailedToPaySubscriptions())
                Console.WriteLine(
                    $"  -{subscription.User.Name} failed to pay for {subscription.Product.Title} [{subscription.Product.GetType().Name}]\n" +
                    $"      Subscription Started: {subscription.StartDate.ToShortDateString()}\n" +
                    $"      Last paid date: {subscription.LastPaid.ToShortDateString()}\n" +
                    $"      Subscription end date: {subscription.EndDate.ToShortDateString()}\n");
        }

        public void Start()
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
                        ShowAllProductRevenueInYear();
                        Console.ReadKey();
                        break;
                    case '2':
                        Console.Clear();
                        ShowFailedToPayCustomers();
                        Console.ReadKey();
                        break;
                    case '3':
                        Console.Clear();
                        ImportCsv();
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

        private void ImportCsv()
        {
            var openFileDialog = new OpenFileDialog();
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
                    return;
                }

                var subscriptionsToImport = CsvParser.LinesToSubscriptions(ProductService, type, subscriptionLines);
                if (subscriptionsToImport.Count != 0)
                {
                    SubscriptionService.AddSubscription(subscriptionsToImport);
                    Console.WriteLine("Successfully imported.");
                }
                else
                {
                    Console.WriteLine("Nothing was imported\n" +
                                      "Check File Format.");
                }
            }
            else
            {
                Console.WriteLine("No file selected.");
            }
        }

        private void ShowAllProductRevenueInYear()
        {
            Console.Write("Get product revenue in year: ");
            if (int.TryParse(Console.ReadLine(), out var year))
            {
                Console.Write(
                    "Type in the product type you would like to filter. eg: Magazine, Newspaper \n" +
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
                        return;
                    }
                }

                var revenueFor = productType == null ? "all products" : productType.Name;
                Console.WriteLine($"# Monthly Revenue in {year} for {revenueFor}");
                var productMonthlyRevenueInYearByType =
                    SubscriptionService.GetProductMonthlyRevenueInYearByType(year, productType);

                foreach (var productMonthlyRevenue in productMonthlyRevenueInYearByType)
                {
                    Console.WriteLine(
                        $"[{productMonthlyRevenue.Key.GetType().Name}] {productMonthlyRevenue.Key.Title} -> Sum: ${productMonthlyRevenue.Value.Sum()}");

                    for (var month = 1; month <= 12; month++)
                        Console.WriteLine($"   month:{month} = ${productMonthlyRevenue.Value[month - 1]}");
                }
            }
            else
            {
                Console.WriteLine("Input Invalid");
            }
        }
    }
}