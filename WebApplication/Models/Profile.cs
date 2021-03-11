using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public Profile(string firstname, string lastname, Department dep, Account acc)
        {
            FirstName = firstname;
            LastName = lastname;
            Department = dep;
            Account = acc;
        }
        public Profile(Department dep, Account acc)
        {
            Department = dep;
            Account = acc;
        }

        //public Profile(string firstname, string lastname, Department dep)
        //{
        //    FirstName = firstname;
        //    LastName = lastname;
        //    Department = dep;

        //}

        public Profile()
        {

        }
    }
}
