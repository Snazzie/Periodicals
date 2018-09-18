using System.Collections.Generic;
using System.Linq;

namespace Periodicals.Users
{
    public class UserService
    {
        public UserService()
        {
            Users = new List<User>();
        }

        public UserService(List<User> users)
        {
            Users = users;
        }

        public List<User> Users { get; }

        public void AddUser(User newUser)
        {
            if (!Users.Exists(u => u.Name == newUser.Name))
                Users.Add(newUser);
        }

        public void Adduser(IEnumerable<User> users)
        {
            users.ToList().ForEach(u => Users.Add(u));
        }
    }
}