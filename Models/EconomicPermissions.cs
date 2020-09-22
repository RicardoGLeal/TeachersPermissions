using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeachersPermissions.Models
{
    public partial class EconomicPermissions : IValidatableObject
    {
        public int EconomicPermissionId { get; set; }
        public int? PermissionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public int? NumberOfDays { get; set; }

        public virtual Permission Permission { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TimeSpan? interval = FinalDate - StartDate;
            if(interval.Value.Days > 2)
            {
                yield return new ValidationResult("No te puedes tomar más de tres días consecutivos");
            }
            if(StartDate > FinalDate)
            {
                yield return new ValidationResult("El día de inicio no puede ser superior al día de termino");
            }
        }
    }
}
