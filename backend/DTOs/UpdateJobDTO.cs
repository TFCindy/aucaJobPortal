namespace AucaJobPortalDotNet.DTOs;
public class UpdateJobDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Location { get; set; }
   public List<int> JobTypeIds { get; set; } = [];
    public DateTime? DeadlineDate { get; set; }
    public int? Openings { get; set; }
}