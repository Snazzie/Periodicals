using System.Collections.Generic;
using System.Linq;

namespace Periodicals.Magazines
{
    public class MagazineService
    {
        public List<Magazine> Magazines { get; private set; }

        public MagazineService()
        {
            Magazines = new List<Magazine>();
        }

        public MagazineService(List<Magazine> magazines)
        {
            Magazines = magazines;
        }

        public void AddMagazine(Magazine magazine)
        {
            Magazines.Add(magazine);
        }
        public void AddMagazine(IEnumerable<Magazine> magazines)
        {
            magazines.ToList().ForEach(m => Magazines.Add(m));
        }

        public bool MagazineExists(string title, out Magazine magazine)
        {
            return (magazine = Magazines.Find(m => m.Title == title)) != null;
        }


    }

}
