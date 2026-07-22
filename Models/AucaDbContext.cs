using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AucaJobPortalDotNet.Models;

public partial class AucaDbContext : DbContext
{
    public AucaDbContext()
    {
    }

    public AucaDbContext(DbContextOptions<AucaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<EmailLog> EmailLogs { get; set; }

    public virtual DbSet<Employer> Employers { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PasswordResetLog> PasswordResetLogs { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<SavedJob> SavedJobs { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__applicat__3213E83F2D2CB570");

            entity.ToTable("applications");

            entity.HasIndex(e => new { e.JobId, e.StudentId }, "UQ_application_job_student").IsUnique();

            entity.HasIndex(e => e.Status, "idx_application_status");

            entity.HasIndex(e => e.JobId, "idx_applications_job_id");

            entity.HasIndex(e => e.StudentId, "idx_applications_student_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("(getdate())", "DF_applications_applied_at")
                .HasColumnType("datetime")
                .HasColumnName("applied_at");
            entity.Property(e => e.CoverLetter)
                .HasColumnType("text")
                .HasColumnName("cover_letter");
            entity.Property(e => e.CvFilePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("cv_file_path");
            entity.Property(e => e.EducationDescription)
                .HasColumnType("text")
                .HasColumnName("education_description");
            entity.Property(e => e.EducationLevel)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("education_level");
            entity.Property(e => e.EmployerFeedback)
                .HasColumnType("text")
                .HasColumnName("employer_feedback");
            entity.Property(e => e.InterviewDate)
                .HasColumnType("datetime")
                .HasColumnName("interview_date");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.ProfessionalExperience)
                .HasColumnType("text")
                .HasColumnName("professional_experience");
            entity.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
            entity.Property(e => e.Skills)
                .HasColumnType("text")
                .HasColumnName("skills");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())", "DF_applications_updated_at")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Job).WithMany(p => p.Applications)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__applicati__job_i__5BE2A6F2");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ReviewedBy)
                .HasConstraintName("FK__applicati__revie__5DCAEF64");

            entity.HasOne(d => d.Student).WithMany(p => p.Applications)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__applicati__stude__5CD6CB2B");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__audit_lo__3213E83F42B75426");

            entity.ToTable("audit_logs");

            entity.HasIndex(e => e.CreatedAt, "idx_audit_logs_created_at");

            entity.HasIndex(e => e.IsSuccessful, "idx_audit_logs_is_successful");

            entity.HasIndex(e => e.Severity, "idx_audit_logs_severity");

            entity.HasIndex(e => e.UserId, "idx_audit_logs_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("entity_type");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ip_address");
            entity.Property(e => e.IsSuccessful)
                .HasDefaultValue(true)
                .HasColumnName("is_successful");
            entity.Property(e => e.NewValue)
                .HasColumnType("text")
                .HasColumnName("new_value");
            entity.Property(e => e.OldValue)
                .HasColumnType("text")
                .HasColumnName("old_value");
            entity.Property(e => e.Severity)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("info")
                .HasColumnName("severity");
            entity.Property(e => e.UserAgent)
                .HasColumnType("text")
                .HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__audit_log__user___73BA3083");
        });

        modelBuilder.Entity<EmailLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__email_lo__3213E83F47C8670D");

            entity.ToTable("email_logs");

            entity.HasIndex(e => e.RecipientEmail, "idx_email_logs_recipient_email");

            entity.HasIndex(e => e.SentAt, "idx_email_logs_sent_at").IsDescending();

            entity.HasIndex(e => e.Status, "idx_email_logs_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_type");
            entity.Property(e => e.ErrorMessage)
                .HasColumnType("text")
                .HasColumnName("error_message");
            entity.Property(e => e.RecipientEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("recipient_email");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sent_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("sent")
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasColumnType("text")
                .HasColumnName("subject");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.EmailLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__email_log__user___787EE5A0");
        });

        modelBuilder.Entity<Employer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employer__3213E83F23B63E4D");

            entity.ToTable("employers");

            entity.HasIndex(e => e.UserId, "idx_employers_user_id");

            entity.HasIndex(e => e.Verified, "idx_employers_verified");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyDescription)
                .HasColumnType("text")
                .HasColumnName("company_description");
            entity.Property(e => e.CompanyLogo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("company_logo");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("company_name");
            entity.Property(e => e.CompanySize)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("company_size");
            entity.Property(e => e.CompanyWebsite)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("company_website");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EmailNotificationsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("email_notifications_enabled");
            entity.Property(e => e.Headquarters)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("headquarters");
            entity.Property(e => e.Industry)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("industry");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VerificationDocumentPath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("verification_document_path");
            entity.Property(e => e.Verified).HasColumnName("verified");

            entity.HasOne(d => d.User).WithMany(p => p.Employers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__employers__user___48CFD27E");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__jobs__3213E83F55FCADE3");

            entity.ToTable("jobs");

            entity.HasIndex(e => e.EmployerId, "idx_jobs_employer_id");

            entity.HasIndex(e => e.JobType, "idx_jobs_job_type");

            entity.HasIndex(e => e.Location, "idx_jobs_location");

            entity.HasIndex(e => e.Status, "idx_jobs_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApprovedAt)
                .HasColumnType("datetime")
                .HasColumnName("approved_at");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.Deadline).HasColumnName("deadline");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EmployerId).HasColumnName("employer_id");
            entity.Property(e => e.ExperienceLevel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("experience_level");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.JobType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Openings)
                .HasDefaultValue(1)
                .HasColumnName("openings");
            entity.Property(e => e.PostedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("posted_at");
            entity.Property(e => e.Requirements)
                .HasColumnType("text")
                .HasColumnName("requirements");
            entity.Property(e => e.Responsibilities)
                .HasColumnType("text")
                .HasColumnName("responsibilities");
            entity.Property(e => e.SalaryRange)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("salary_range");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.ViewsCount).HasColumnName("views_count");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__jobs__approved_b__5441852A");

            entity.HasOne(d => d.Employer).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.EmployerId)
                .HasConstraintName("FK__jobs__employer_i__534D60F1");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__messages__3213E83F4F14BA3B");

            entity.ToTable("messages");

            entity.HasIndex(e => e.IsRead, "idx_messages_is_read");

            entity.HasIndex(e => e.ReceiverId, "idx_messages_receiver_id");

            entity.HasIndex(e => e.SenderId, "idx_messages_sender_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicationId).HasColumnName("application_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sent_at");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("subject");

            entity.HasOne(d => d.Application).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("FK__messages__applic__6FE99F9F");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__messages__receiv__6EF57B66");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__messages__sender__6E01572D");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notifica__3213E83F1FA28F13");

            entity.ToTable("notifications");

            entity.HasIndex(e => e.IsRead, "idx_notifications_is_read");

            entity.HasIndex(e => e.UserId, "idx_notifications_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RelatedApplicationId).HasColumnName("related_application_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.RelatedApplication).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.RelatedApplicationId)
                .HasConstraintName("FK__notificat__relat__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__notificat__user___68487DD7");
        });

        modelBuilder.Entity<PasswordResetLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__password__3213E83F58881285");

            entity.ToTable("password_reset_logs");

            entity.HasIndex(e => e.ResetRequestedAt, "idx_password_reset_logs_reset_requested_at");

            entity.HasIndex(e => e.UserId, "idx_password_reset_logs_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ip_address");
            entity.Property(e => e.ResetCompletedAt)
                .HasColumnType("datetime")
                .HasColumnName("reset_completed_at");
            entity.Property(e => e.ResetRequestedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("reset_requested_at");
            entity.Property(e => e.UserAgent)
                .HasColumnType("text")
                .HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResetLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__password___user___7C4F7684");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__permissi__3213E83F942EFBA9");

            entity.ToTable("permissions");

            entity.HasIndex(e => e.Name, "UQ__permissi__72E12F1B1DB12C9F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F2BCBFAAC");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "UQ__roles__72E12F1B5EB5ED07").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsSystemRole).HasColumnName("is_system_role");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK__role_per__C85A54633C16AFCC");

            entity.ToTable("role_permissions");

            entity.HasIndex(e => e.RoleId, "idx_role_permissions_role_id");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.GrantedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("granted_at");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__role_perm__permi__0D7A0286");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__role_perm__role___0C85DE4D");
        });

        modelBuilder.Entity<SavedJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__saved_jo__3213E83F638C3AAD");

            entity.ToTable("saved_jobs");

            entity.HasIndex(e => new { e.StudentId, e.JobId }, "UQ_saved_job").IsUnique();

            entity.HasIndex(e => e.StudentId, "idx_saved_jobs_student_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.SavedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("saved_at");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Job).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__saved_job__job_i__6383C8BA");

            entity.HasOne(d => d.Student).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__saved_job__stude__628FA481");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__students__3213E83F335A3ACF");

            entity.ToTable("students");

            entity.HasIndex(e => e.StudentId, "UQ__students__2A33069BAA8A2811").IsUnique();

            entity.HasIndex(e => e.StudentId, "idx_students_student_id");

            entity.HasIndex(e => e.UserId, "idx_students_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bio)
                .HasColumnType("text")
                .HasColumnName("bio");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CvFilePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("cv_file_path");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("department");
            entity.Property(e => e.EmailNotificationsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("email_notifications_enabled");
            entity.Property(e => e.Faculty)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("faculty");
            entity.Property(e => e.GraduationYear).HasColumnName("graduation_year");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("profile_picture");
            entity.Property(e => e.Program)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("program");
            entity.Property(e => e.Skills)
                .HasColumnType("text")
                .HasColumnName("skills");
            entity.Property(e => e.StudentId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("student_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.YearOfStudy).HasColumnName("year_of_study");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__students__user_i__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F6CD07391");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E61642E2A44CD").IsUnique();

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.EmailVerificationToken, "idx_users_email_verification_token");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerificationExpires)
                .HasColumnType("datetime")
                .HasColumnName("email_verification_expires");
            entity.Property(e => e.EmailVerificationToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email_verification_token");
            entity.Property(e => e.EmailVerified).HasColumnName("email_verified");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastLoginAt)
                .HasColumnType("datetime")
                .HasColumnName("last_login_at");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.PasswordResetExpires)
                .HasColumnType("datetime")
                .HasColumnName("password_reset_expires");
            entity.Property(e => e.PasswordResetToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_reset_token");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_users_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
