using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Ethnicity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EthnicityID { get; set; }
        public string EthnicityName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
