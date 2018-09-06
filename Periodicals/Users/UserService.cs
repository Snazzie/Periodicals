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

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void Adduser(List<User> users)
        {
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }
}
