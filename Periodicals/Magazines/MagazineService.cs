using System.Collections.Generic;

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

        public void AddMagazine(List<Magazine> magazines)
        {
            foreach (var magazine in magazines)
            {
                Magazines.Add(magazine);
            }

        }

    }
    
}
