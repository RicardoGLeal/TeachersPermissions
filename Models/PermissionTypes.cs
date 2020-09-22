using System;
using System.Collections.Generic;

namespace TeachersPermissions.Models
{
    public partial class PermissionTypes
    {
        public PermissionTypes()
        {
            Permission = new HashSet<Permission>();
        }

        public short PermissionTypeId { get; set; }
        public string PermissionTypeDesc { get; set; }

        public virtual ICollection<Permission> Permission { get; set; }
    }
}
