using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class User
    {
        public User()
        {
            Loanlogs = new HashSet<Loanlog>();
        }

        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte[] Socialsecuritynumber { get; set; }
        public int Permissions { get; set; }
        public byte[] Password { get; set; }
        public string Email { get; set; }
        public byte? HasLoanCard { get; set; }
        public string UserComment { get; set; }
        public decimal Debt { get; set; }

        public virtual Permission PermissionsNavigation { get; set; }
        public virtual ICollection<Loanlog> Loanlogs { get; set; }
    }
}
