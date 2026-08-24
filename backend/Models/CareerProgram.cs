using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("career_programs")]
public class CareerProgram
{
    [Column("id")]
    public int Id { get; private set; }

    [Column("career_counselor_id")]
    public int CareerCounselorId { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("short_description")]
    public string? ShortDescription { get; set; }

    [Column("is_external")]
    public bool IsExternal { get; set; } = false;

    [Column("external_organization")]
    public string? ExternalOrganization { get; set; }

    [Column("type")]
    public string Type { get; set; } = string.Empty;
    // "training", "workshop", "seminar", "webinar", "bootcamp"
    [Column("category_id")]
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public virtual ProgramCategory? Category { get; set; }

    [Column("level")]
    public string? Level { get; set; }
    // "beginner", "intermediate", "advanced", "all_levels"

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("registration_deadline")]
    public DateTime RegistrationDeadline { get; set; }

    [Column("is_online")]
    public bool IsOnline { get; set; } = false;

    [Column("location")]
    public string? Location { get; set; }

    [Column("venue")]
    public string? Venue { get; set; }

    [Column("meeting_link")]
    public string? MeetingLink { get; set; }

    [Column("max_participants")]
    public int MaxParticipants { get; set; }

    [Column("current_participants")]
    public int CurrentParticipants { get; set; } = 0;

    [Column("is_free")]
    public bool IsFree { get; set; } = true;

    [Column("cost")]
    public decimal? Cost { get; set; }

    [Column("status")]
    public string Status { get; set; } = "draft";
    // "draft", "published", "upcoming", "ongoing", "completed", "cancelled", "full"

    [Column("published_at")]
    public DateTime? PublishedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("is_featured")]
    public bool IsFeatured { get; set; } = false;

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [Column("application_count")]
    public int ApplicationCount { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public bool IsFull => CurrentParticipants >= MaxParticipants;

    [NotMapped]
    public bool IsRegistrationOpen =>
        !IsFull &&
        DateTime.UtcNow < RegistrationDeadline &&
        Status != "cancelled" &&
        Status != "completed";

    [NotMapped]
    public int AvailableSpots => MaxParticipants - CurrentParticipants;

    [ForeignKey("CareerCounselorId")]
    public virtual CareerCounselor CareerCounselor { get; set; } = null!;

    public virtual ICollection<ProgramApplication> Applications { get; set; } = [];
}