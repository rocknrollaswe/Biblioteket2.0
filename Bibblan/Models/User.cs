using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte[] Socialsecuritynumber { get; set; }
        public int Permissions { get; set; }
        public byte[] Username { get; set; }
        public byte[] Password { get; set; }
        public string Email { get; set; }
        public byte? HasLoanCard { get; set; }

        public virtual Permission PermissionsNavigation { get; set; }
    }
}
