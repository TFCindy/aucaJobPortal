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

    [Column("motivation_letter")]
    public string? MotivationLetter { get; set; }

    [Column("student_program_at_application")]
    public string StudentProgramAtApplication { get; set; } = string.Empty;

    [Column("student_year_at_application")]
    public int StudentYearAtApplication { get; set; }

    [Column("reviewed_at")]
    public DateTime? ReviewedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual CareerProgram Program { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}