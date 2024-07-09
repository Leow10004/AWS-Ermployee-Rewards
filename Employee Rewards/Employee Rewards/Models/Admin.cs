using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminID { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public virtual ICollection<Employee> AdministeredEmployees { get; set; } = new HashSet<Employee>();
    }
}
