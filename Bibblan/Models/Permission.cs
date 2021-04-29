using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class Permission
    {
        public Permission()
        {
            Users = new HashSet<User>();
        }

        public int PermissionId { get; set; }
        public string Permissions { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
