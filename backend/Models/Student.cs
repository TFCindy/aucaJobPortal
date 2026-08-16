using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("students")]
public class Student
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("user_id")]
    public int UserId{get;set;}
    [Column("student_id")]
    public string StudentId{get;set;}=string.Empty;
    [Column("program")]
    public string? Program{get;set;}
    [Column("year_of_study")]
    public int? YearOfStudy{get;set;}
    [Column("graduation_year")]
    public int? GraduationYear{get;set;}
    [Column("cv_file_path")]
    public string? CvFilePath{get;set;}
    [Column("skills")]
    public string? Skills{get;set;}
    [Column("profile_picture")]
    public string? ProfilePicture{get;set;}
    [Column("bio")]
    public string? Bio{get;set;}
    [Column("created_at")]
    public DateTime CreatedAt{get;private set;}=DateTime.UtcNow;
    [Column("faculty")]
    public string? Faculty{get;set;}
    [Column("department")]
    public string? Department{get;set;}
    [Column("email_notifications_enabled")]
    public bool EmailNotificationsEnabled{get;set;}=true;
    // ─── ALUMNI FIELDS (NEW) ───
    [Column("is_alumni")]
    public bool IsAlumni { get; set; } = false;

    [Column("alumni_since")]
    public DateTime? AlumniSince { get; set; }

    [Column("degree_classification")]
    public string? DegreeClassification { get; set; }
    // Options: "FirstClass", "UpperSecond", "LowerSecond", "Pass"

    [Column("current_employment_status")]
    public string? CurrentEmploymentStatus { get; set; }
    // Options: "Employed", "SelfEmployed", "Unemployed", "FurtherStudies", "NotSeeking"

    [Column("current_employer")]
    public string? CurrentEmployer { get; set; }

    [Column("current_job_title")]
    public string? CurrentJobTitle { get; set; }
    [Column("industry")]
    public string? Industry { get; set; }
    [Column("employment_start_date")]
    public DateTime? EmploymentStartDate { get; set; }

    // ─── DOCUMENT VERIFICATION FIELDS (NEW) ───
    [Column("transcript_file_path")]
    public string? TranscriptFilePath { get; set; }

    [Column("diploma_file_path")]
    public string? DiplomaFilePath { get; set; }

    [Column("transcript_verified")]
    public bool TranscriptVerified { get; set; } = false;

    [Column("diploma_verified")]
    public bool DiplomaVerified { get; set; } = false;

    [Column("document_verification_status")]
    public string DocumentVerificationStatus { get; set; } = "NotSubmitted";
    // Options: "NotSubmitted", "Pending", "Verified", "Rejected"

    [Column("document_verified_at")]
    public DateTime? DocumentVerifiedAt { get; set; }

    [Column("document_verified_by")]
    public int? DocumentVerifiedBy { get; set; }

    [Column("verification_notes")]
    public string? VerificationNotes { get; set; }

    // ─── ALUMNI UPGRADE REQUEST (SELF-DECLARATION) (NEW) ───
    [Column("alumni_upgrade_status")]
    public string AlumniUpgradeStatus { get; set; } = "NotRequested";
    // Options: "NotRequested", "Pending", "Verified", "Rejected"

    [Column("alumni_upgrade_requested_at")]
    public DateTime? AlumniUpgradeRequestedAt { get; set; }

    [Column("alumni_upgrade_reviewed_at")]
    public DateTime? AlumniUpgradeReviewedAt { get; set; }

    [Column("alumni_upgrade_rejection_reason")]
    public string? AlumniUpgradeRejectionReason { get; set; }

    // ─── PROGRAM TRACKING (NEW) ───
    [Column("total_programs_completed")]
    public int TotalProgramsCompleted { get; set; } = 0;

    [Column("last_program_completed_at")]
    public DateTime? LastProgramCompletedAt { get; set; }
    public virtual User User{get;set;}=null!;
    [ForeignKey("DocumentVerifiedBy")]
    public virtual User? DocumentVerifiedByUser { get; set; }
    public ICollection<Application> Applications{ get; set; } = [];
    public virtual ICollection<SavedJob> SavedJobs{get;set;}=[];
    public virtual ICollection<ProgramApplication> ProgramApplications { get; set; } = [];
}