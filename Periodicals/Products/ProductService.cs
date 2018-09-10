using System.Collections.Generic;
using System.Linq;
namespace Periodicals.Products
{
    public class ProductService
    {
        public List<Product> Products { get; private set; }

        public ProductService()
        {
            Products = new List<Product>();
        }

        public ProductService(List<Product> products)
        {
            Products = products;
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public void AddProduct(IEnumerable<Product> magazines)
        {
            magazines.ToList().ForEach(m => Products.Add(m));
        }

        public bool ProductExists(string title, out Product product)
        {
            return (product = Products.Find(m => m.Title == title)) != null;
        }


    }

}
