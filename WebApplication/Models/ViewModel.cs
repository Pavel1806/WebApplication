using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Db;

namespace WebApplication.Models
{
    public class ViewModel
    { 
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepId { get; set; }
        public string Title { get; set; }
    }
}
