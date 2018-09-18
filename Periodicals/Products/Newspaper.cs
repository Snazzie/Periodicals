namespace Periodicals.Products
{
    public class Newspaper : Product
    {
        public Newspaper(string title, int price, int id = 0)
        {
            Id = id;
            Title = title;
            Price = price;
        }
    }
}