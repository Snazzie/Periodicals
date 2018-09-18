namespace Periodicals.Users
{
    public class User
    {
        private readonly int Id;

        public User(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}