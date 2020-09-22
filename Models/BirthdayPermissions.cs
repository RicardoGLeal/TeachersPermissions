using System;
using System.Collections.Generic;

namespace TeachersPermissions.Models
{
    public partial class BirthdayPermissions
    {
        public int BirthdayPermissionId { get; set; }
        public int? PermissionId { get; set; }
        public DateTime? GrantedDayDate { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
