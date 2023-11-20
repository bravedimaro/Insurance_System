using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance_System.Entity.Entity
{
    public class Policyholder : BaseEntity
    {
        public string NationalId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PolicyNumber { get; set; }
    }
}
