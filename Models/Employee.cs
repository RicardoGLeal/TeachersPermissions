using System;
using System.Collections.Generic;

namespace TeachersPermissions.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Permission = new HashSet<Permission>();
        }

        public int EmployeeId { get; set; }
        public string ContractType { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? HireDate { get; set; }

        public virtual ICollection<Permission> Permission { get; set; }
    }
}
