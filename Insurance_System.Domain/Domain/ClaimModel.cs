using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance_System.Domain.Domain
{
    public class ClaimModel : BaseDomain
    {
        [Required]
        public string ClaimId { get; set; }

        [Required]
        public string PolicyholderNationalId { get; set; }

        public List<Expense> Expenses { get; set; }

        [Required]
        public DateTime DateOfExpense { get; set; }

        public double TotalClaimAmount => Expenses.Sum(e => e.Amount);

        [Required, DefaultValue("Submitted")]
        public string ClaimStatus { get; set; } = string.Empty;
    }
}
