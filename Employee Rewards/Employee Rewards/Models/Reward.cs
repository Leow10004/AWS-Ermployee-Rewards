using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Reward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RewardID { get; set; }
        public int EmployeeID { get; set; }
        public int IssuerID { get; set; }
        public string Reason { get; set; }
        public string RewardDescription { get; set; }
        public DateTime DateAwarded { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Employee Issuer { get; set; }
    }
}
