using System;
using System.Collections.Generic;
using NUnit.Framework;
using Periodicals.Magazines;

namespace PeriodicalsTest
{
    [TestFixture]
    public class MagazineServiceTests
    {
        public MagazineService MagazineService;

        [SetUp]
        public void SetUp()
        {
            MagazineService = new MagazineService(new List<Magazine>
            {
                new Magazine("Dogs Monthly", 399),
                new Magazine("Beekeeper", 249),
                new Magazine("Rock'n'Bass", 500),
                new Magazine("Amiga Gamer", 169),
                new Magazine("\"Snow, Ice and You\"", 749)

            });
        }

        [TestCase("You and i", ExpectedResult = false)]
        [TestCase("Beekeeper", ExpectedResult = true)]
        public bool MagazineExists_ReturnsCorrectly(string title)
        {
            return MagazineService.MagazineExists(title, out _);
        }
    }
}
