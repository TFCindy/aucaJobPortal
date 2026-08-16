using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;

[Table("programs")]
public class Program
{
    [Column("id")]
    public int Id { get; set; }

    [Column("career_counselor_id")]
    public int CareerCounselorId { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("short_description")]
    public string? ShortDescription { get; set; }

    // ─── INTERNAL/EXTERNAL ───
    [Column("is_external")]
    public bool IsExternal { get; set; } = false;

    [Column("external_organization")]
    public string? ExternalOrganization { get; set; }

    // ─── PROGRAM DETAILS ───
    [Column("type")]
    public string Type { get; set; } = string.Empty;
    // "Training", "Workshop", "Seminar", "Webinar", "Bootcamp"

    [Column("category")]
    public string? Category { get; set; }
    // "Soft Skills", "Technical", "Career Prep", "Interview Prep"

    [Column("level")]
    public string? Level { get; set; }
    // "Beginner", "Intermediate", "Advanced", "All Levels"

    // ─── SCHEDULE ───
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

    // ─── CAPACITY ───
    [Column("max_participants")]
    public int MaxParticipants { get; set; }

    [Column("current_participants")]
    public int CurrentParticipants { get; set; } = 0;

    // ─── COST ───
    [Column("is_free")]
    public bool IsFree { get; set; } = true;

    [Column("cost")]
    public decimal? Cost { get; set; }

    // ─── STATUS ───
    [Column("status")]
    public string Status { get; set; } = "Draft";
    // "Draft", "Published", "Upcoming", "Ongoing", "Completed", "Cancelled", "Full"

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

    // ─── TIMESTAMPS ───
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ─── COMPUTED PROPERTIES ───
    public bool IsFull => CurrentParticipants >= MaxParticipants;

    public bool IsRegistrationOpen =>
        !IsFull &&
        DateTime.UtcNow < RegistrationDeadline &&
        Status != "Cancelled" &&
        Status != "Completed";

    public int AvailableSpots => MaxParticipants - CurrentParticipants;

    // ─── NAVIGATION PROPERTIES ───
    [ForeignKey("CareerCounselorId")]
    public virtual CareerCounselor CareerCounselor { get; set; } = null!;

    public virtual ICollection<ProgramApplication> Applications { get; set; } = [];
}