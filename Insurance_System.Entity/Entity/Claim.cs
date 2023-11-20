using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance_System.Entity.Entity
{
    public class Claim : BaseEntity
    {
        public int PolicyholderId { get; set; }
        public string ExpensesType { get; set; } // 'Procedures' or 'Prescriptions'
        public string ExpenseName { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal TotalAmountClaimed { get; set; }
        public string ClaimStatus { get; set; } = "Submitted";
    }
}
