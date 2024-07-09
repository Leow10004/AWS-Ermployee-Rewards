using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }
        public int? ManagerID { get; set; }
        public int? AdminID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public int? EthnicityID { get; set; }
        public string Gender { get; set; }
        public int? DepartmentID { get; set; }
        public int? RegionID { get; set; }
        public string ProfileImageRoot { get; set; }
        public string AccountStatus { get; set; }
        public int Access { get; set; }

        public Manager Manager { get; set; }
        public Admin Admin { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public Department Department { get; set; }
        public Region Region { get; set; }
        public virtual ICollection<Password> Passwords { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
    }
}
