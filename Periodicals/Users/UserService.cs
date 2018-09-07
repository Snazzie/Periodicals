using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodicals.Users
{
    public class UserService
    {
        public List<User> Users { get; private set; }

        public UserService()
        {
            Users = new List<User>();
        }

        public UserService(List<User> users)
        {
            Users = users;
        }

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
