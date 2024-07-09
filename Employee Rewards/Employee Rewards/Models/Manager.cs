using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerID { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public virtual ICollection<Employee> ManagedEmployees { get; set; } = new HashSet<Employee>();
    }
}
