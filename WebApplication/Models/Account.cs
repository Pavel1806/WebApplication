using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Profile Profile { get; set; }

        public Account(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public Account()
        {

        }


    }
}
