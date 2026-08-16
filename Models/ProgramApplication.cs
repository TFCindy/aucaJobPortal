using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("program_applications")]
public class ProgramApplication
{
    [Column("id")]
    public int Id { get; set; }

    [Column("program_id")]
    public int ProgramId { get; set; }

    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("application_date")]
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    [Column("status")]
    public string Status { get; set; } = "Pending";
    // "Pending", "Approved", "Rejected", "Waitlisted"

    [Column("motivation_letter")]
    public string? MotivationLetter { get; set; }

    [Column("additional_notes")]
    public string? AdditionalNotes { get; set; }

    // ─── REVIEW ───
    [Column("reviewed_by")]
    public int? ReviewedBy { get; set; }

    [Column("reviewed_at")]
    public DateTime? ReviewedAt { get; set; }

    [Column("reviewer_notes")]
    public string? ReviewerNotes { get; set; }

    // ─── ATTENDANCE & COMPLETION ───
    [Column("attended")]
    public bool Attended { get; set; } = false;

    [Column("completed")]
    public bool Completed { get; set; } = false;

    [Column("completion_certificate")]
    public string? CompletionCertificate { get; set; }

    // ─── TIMESTAMPS ───
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ─── NAVIGATION PROPERTIES ───
    [ForeignKey("ProgramId")]
    public virtual Program Program { get; set; } = null!;

    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("ReviewedBy")]
    public virtual CareerCounselor? ReviewedByCounselor { get; set; }
}