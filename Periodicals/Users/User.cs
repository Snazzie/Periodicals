namespace Periodicals.Users
{
    public class User
    {
        private readonly int Id;
        public string Name { get; private set; }

        public User(string name)
        {
            Name = name;
        }
    }
}