
namespace Periodicals.GeneralClasses
{
    public class Magazine
    {
        public string Title { get; private set; }
        public int Price { get; private set; }


        public Magazine(string title, int price)
        {
            Title = title;
            Price = price;
        }
    }
}
