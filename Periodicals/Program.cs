using System;
using System.Collections.Generic;
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
            new Newspaper("The Bugle", 7249),
            new Newspaper("Plympton Gazette", 3800)
        });

        public static SubscriptionService SubscriptionService { get; set; } =
            new SubscriptionService(ProductService.Products);

        [STAThread]
        private static void Main(string[] args)
        {
            var menu = new Menu(ProductService, SubscriptionService);
            menu.Start();
        }
    }
}