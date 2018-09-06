using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodicals.Magazines
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
