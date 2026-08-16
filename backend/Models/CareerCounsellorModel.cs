using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("career_counselors")]
public class CareerCounselor
{
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("department")]
    public string Department { get; set; } = string.Empty;

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("bio")]
    public string? Bio { get; set; }

    [Column("specialization")]
    public string? Specialization { get; set; }

    [Column("years_of_experience")]
    public int? YearsOfExperience { get; set; }

    [Column("office_location")]
    public string? OfficeLocation { get; set; }

    [Column("phone_extension")]
    public string? PhoneExtension { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ─── NAVIGATION PROPERTIES ───
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<Program> Programs { get; set; } = [];

    public virtual ICollection<ProgramApplication> ReviewedApplications { get; set; } = [];
}