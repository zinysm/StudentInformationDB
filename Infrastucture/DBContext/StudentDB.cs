using Microsoft.EntityFrameworkCore;

public class StudentDB : DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Lecture> Lectures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Students)
            .WithOne(s => s.Department)
            .HasForeignKey(s => s.DepartmentId);

        modelBuilder.Entity<Department>()
            .HasMany(d => d.Lectures)
            .WithMany(l => l.Departments);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Lectures)
            .WithMany(l => l.Students);
    }
}
