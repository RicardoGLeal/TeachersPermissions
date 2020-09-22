using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeachersPermissions.Models
{
    public partial class SchoolContext : DbContext
    {
        public SchoolContext()
        {
        }

        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BirthdayPermissions> BirthdayPermissions { get; set; }
        public virtual DbSet<EconomicPermissions> EconomicPermissions { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<HoursPermissions> HoursPermissions { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionTypes> PermissionTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LAPTOP-NHT9K1JQ;Database=School;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BirthdayPermissions>(entity =>
            {
                entity.HasKey(e => e.BirthdayPermissionId)
                    .HasName("PK_GrantedDay");

                entity.Property(e => e.BirthdayPermissionId).HasColumnName("birthdayPermissionID");

                entity.Property(e => e.GrantedDayDate)
                    .HasColumnName("grantedDayDate")
                    .HasColumnType("date");

                entity.Property(e => e.PermissionId).HasColumnName("permissionID");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.BirthdayPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_GrantedDay_GrantedDay");
            });

            modelBuilder.Entity<EconomicPermissions>(entity =>
            {
                entity.HasKey(e => e.EconomicPermissionId);

                entity.Property(e => e.EconomicPermissionId).HasColumnName("economicPermissionID");

                entity.Property(e => e.FinalDate)
                    .HasColumnName("finalDate")
                    .HasColumnType("date");

                entity.Property(e => e.NumberOfDays).HasColumnName("numberOfDays");

                entity.Property(e => e.PermissionId).HasColumnName("permissionID");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .HasColumnType("date");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.EconomicPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_EconomicPermissions_Permission");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("employeeID");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.ContractType)
                    .HasColumnName("contractType")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FirstLastName)
                    .HasColumnName("firstLastName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HireDate)
                    .HasColumnName("hireDate")
                    .HasColumnType("date");

                entity.Property(e => e.SecondLastName)
                    .HasColumnName("secondLastName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecondName)
                    .HasColumnName("secondName")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HoursPermissions>(entity =>
            {
                entity.Property(e => e.HoursPermissionsId).HasColumnName("hoursPermissionsID");

                entity.Property(e => e.Day)
                    .HasColumnName("day")
                    .HasColumnType("date");

                entity.Property(e => e.HoursRange)
                    .IsRequired()
                    .HasColumnName("hoursRange")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionId).HasColumnName("permissionID");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.HoursPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GrantedHour_Permission");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.PermissionId).HasColumnName("permissionID");

                entity.Property(e => e.Autorize)
                    .HasColumnName("autorize")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId).HasColumnName("employeeID");

                entity.Property(e => e.PermissionType).HasColumnName("permissionType");

                entity.Property(e => e.RequestDate)
                    .HasColumnName("requestDate")
                    .HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Permission_Employee");

                entity.HasOne(d => d.PermissionTypeNavigation)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.PermissionType)
                    .HasConstraintName("FK_PermissionTypes_Permission");
            });

            modelBuilder.Entity<PermissionTypes>(entity =>
            {
                entity.HasKey(e => e.PermissionTypeId);

                entity.Property(e => e.PermissionTypeId).HasColumnName("permissionTypeID");

                entity.Property(e => e.PermissionTypeDesc)
                    .HasColumnName("permissionTypeDesc")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
