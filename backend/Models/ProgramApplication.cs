using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("program_applications")]
public class ProgramApplication
{
    [Column("id")]
    public int Id { get; private set; }

    [Column("program_id")]
    public int ProgramId { get; set; }

    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("status")]
    public string Status { get; set; } = "pending";
    // "pending", "approved", "rejected", "waitlisted"

    [Column("motivation_letter")]
    public string? MotivationLetter { get; set; }

    [Column("additional_notes")]
    public string? AdditionalNotes { get; set; }

    [Column("reviewed_by")]
    public int? ReviewedBy { get; set; }

    [Column("reviewed_at")]
    public DateTime? ReviewedAt { get; set; }

    [Column("reviewer_notes")]
    public string? ReviewerNotes { get; set; }

    [Column("attended")]
    public bool Attended { get; set; } = false;

    [Column("completed")]
    public bool Completed { get; set; } = false;

    [Column("completion_certificate")]
    public string? CompletionCertificate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("program_id")]
    public virtual CareerProgram Program { get; set; } = null!;

    [ForeignKey("student_id")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("reviewed_by")]
    public virtual CareerCounselor? ReviewedByCounselor { get; set; }
}