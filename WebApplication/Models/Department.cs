using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Profile> Profiles { get; set; }

        public Department(string title)
        {
            Title = title;
            Profiles = new List<Profile>();
        }
        public Department()
        {

        }
    }
}
