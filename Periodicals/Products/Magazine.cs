namespace Periodicals.Products
{
    public class Magazine : Product
    {
        public Magazine(string title, int price, int id = 0)
        {
            Id = id;
            Title = title;
            Price = price;
        }
    }
}
