using System;
using System.Collections.Generic;

namespace TeachersPermissions.Models
{
    public partial class Permission
    {
        public Permission()
        {
            BirthdayPermissions = new HashSet<BirthdayPermissions>();
            EconomicPermissions = new HashSet<EconomicPermissions>();
            HoursPermissions = new HashSet<HoursPermissions>();
        }

        public int PermissionId { get; set; }
        public short? PermissionType { get; set; }
        public int? EmployeeId { get; set; }
        public string Autorize { get; set; }
        public DateTime? RequestDate { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual PermissionTypes PermissionTypeNavigation { get; set; }
        public virtual ICollection<BirthdayPermissions> BirthdayPermissions { get; set; }
        public virtual ICollection<EconomicPermissions> EconomicPermissions { get; set; }
        public virtual ICollection<HoursPermissions> HoursPermissions { get; set; }
    }
}
