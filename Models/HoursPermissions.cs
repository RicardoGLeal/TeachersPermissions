using System;
using System.Collections.Generic;

namespace TeachersPermissions.Models
{
    public partial class HoursPermissions
    {
        public int HoursPermissionsId { get; set; }
        public string HoursRange { get; set; }
        public int PermissionId { get; set; }
        public string Reason { get; set; }
        public DateTime? Day { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
