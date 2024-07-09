using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Password
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PasswordID { get; set; }
        public int EmployeeID { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
