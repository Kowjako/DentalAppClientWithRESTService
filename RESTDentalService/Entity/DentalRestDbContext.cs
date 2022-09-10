using Microsoft.EntityFrameworkCore;

namespace RESTDentalService.Entity
{
    public partial class DentalRestDbContext : DbContext
    {
        public DentalRestDbContext()
        {
        }

        public DentalRestDbContext(DbContextOptions<DentalRestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.HasIndex(e => new { e.UniqueNumber, e.Name }, "IX__Clinics_NameUniqueNumber");

                entity.HasIndex(e => e.ManagerId, "UQ__Clinics_ManagerId")
                    .IsUnique();

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UniqueNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Manager)
                    .WithOne(p => p.Clinic)
                    .HasForeignKey<Clinic>(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Clinics_ManagerId");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Surname, e.Email }, "IX__Employees_NameSurnameEmail")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.ClinicNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.ClinicId)
                    .HasConstraintName("FK__Employees_ClinicId");
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Cost }, "IX__Operations_NameCost");

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Term).HasColumnType("date");

                entity.HasOne(d => d.Clinic)
                    .WithMany(p => p.Operations)
                    .HasForeignKey(d => d.ClinicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Operations_ClinicId");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Operations)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Operations_EmployeeId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
