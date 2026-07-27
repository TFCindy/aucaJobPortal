using Microsoft.EntityFrameworkCore;

namespace AucaJobPortalDotNet.Models;
public class AucaDbContext : DbContext
{
    public AucaDbContext(DbContextOptions<AucaDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Student> Students{get;set;}=null!;
    public DbSet<Employer> Employers{get;set;}=null!;
    public DbSet<Role> Roles{get;set;}=null!;
    public DbSet<Permission> Permissions{get;set;}=null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<Notification>Notifications{ get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<EmailLog> EmailLogs { get; set; } = null!;
    public DbSet<SavedJob> SavedJobs { get; set; } = null!;
    public DbSet<PasswordResetLog> PasswordResetLogs { get; set; } = null!;
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<AuditLog> AUditLogs { get; set; } = null!;
    public DbSet<Application> Applications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<RolePermission>()
       .HasKey(rp => new { rp.RoleId, rp.PermissionId});//composite primary key

       modelBuilder.Entity<Application>()
       .HasOne(a => a.Job)
       .WithMany(j=> j.Applications)
       .HasForeignKey(a => a.JobId)
       .OnDelete(DeleteBehavior.NoAction);

       modelBuilder.Entity<Application>()
       .HasOne(a => a.Student)
       .WithMany(s => s.Applications)
       .HasForeignKey(a => a.StudentId)
       .OnDelete(DeleteBehavior.Cascade);

       modelBuilder.Entity<Application>()
       .HasOne(a => a.LastReviewedByNavigation)//LastReviewedByNavigation is the actual User object,thats why we dont use lastRevieweBy which is just a string
       .WithMany(u => u.ReviewedApplications)
       .HasForeignKey(a => a.LastReviewedBy)
       .OnDelete(DeleteBehavior.SetNull);

       modelBuilder.Entity<Job>()
       .HasOne( j=> j.Employer)
       .WithMany( e=> e.Jobs)
       .HasForeignKey(j=>j.EmployerId)
       .OnDelete(DeleteBehavior.Cascade);

       modelBuilder.Entity<EmailLog>()
       .HasOne( e=>e.User)
       .WithMany( u=> u.EmailLogs)
       .HasForeignKey(e=> e.UserId)
       .OnDelete(DeleteBehavior.SetNull);

       modelBuilder.Entity<Message>()
       .HasOne(m=>m.Sender)
       .WithMany(s=>s.SentMessages)
       .HasForeignKey(m=>m.SenderId)
       .OnDelete(DeleteBehavior.NoAction);

       modelBuilder.Entity<Message>()
       .HasOne(m=>m.Receiver)
       .WithMany(r=>r.ReceivedMessages)
       .HasForeignKey(m=>m.ReceiverId)
       .OnDelete(DeleteBehavior.NoAction);

       modelBuilder.Entity<Job>()
       .HasOne(j=>j.ApprovedByNavigation)
       .WithMany(a=>a.ApprovedJobs)
       .HasForeignKey(j=>j.ApprovedBy)
       .OnDelete(DeleteBehavior.SetNull);

       modelBuilder.Entity<User>()
       .HasMany(u=>u.Notifications)
       .WithOne(n=>n.User)
       .HasForeignKey(n=>n.UserId)
       .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Student>()
      .HasOne(s => s.User)        
      .WithOne(u => u.Student)   
      .HasForeignKey<Student>(s => s.UserId)  
      .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Employer>()
      .HasOne(e=>e.User)
      .WithOne(u=>u.Employer)
      .HasForeignKey<Employer>(e=>e.UserId)
      .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<User>()
      .HasOne(u=>u.Role)
      .WithMany(r=>r.Users)
      .HasForeignKey(u=>u.RoleId)
      .OnDelete(DeleteBehavior.NoAction);

      modelBuilder.Entity<PasswordResetLog>()
      .HasOne(p=>p.User)
      .WithMany(u=>u.PasswordResetLogs)
      .HasForeignKey(p=>p.UserId)
      .OnDelete(DeleteBehavior.SetNull);

      modelBuilder.Entity<SavedJob>()
      .HasOne(sj=>sj.Student)
      .WithMany(s=>s.SavedJobs)
      .HasForeignKey(sj=>sj.StudentId)
      .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<SavedJob>()
      .HasOne(s=>s.Job)
      .WithMany(j=>j.SavedJobs)
      .HasForeignKey(s=>s.JobId)
      .OnDelete(DeleteBehavior.NoAction);

      modelBuilder.Entity<AuditLog>()
      .HasOne(a=>a.User)
      .WithMany(u=>u.AuditLogs)
      .HasForeignKey(a=>a.UserId)
      .OnDelete(DeleteBehavior.SetNull);

      modelBuilder.Entity<SavedJob>()
      .HasIndex(sj=> new{sj.JobId,sj.StudentId})
      .IsUnique();

      modelBuilder.Entity<Application>()
      .HasIndex(a=>new { a.JobId, a.StudentId})
      .IsUnique();

      modelBuilder.Entity<User>()
      .HasIndex(u=>u.Email)
      .IsUnique();

      modelBuilder.Entity<Student>()
      .HasIndex(s=>s.StudentId)
      .IsUnique();

      modelBuilder.Entity<Role>()
      .HasIndex(r=>r.Name)
      .IsUnique();

      modelBuilder.Entity<Permission>()
      .HasIndex(p=>p.Name)
      .IsUnique();
      










       








        
    }

}