namespace AucaJobPortalDotNet.DTOs;

public class JobTypeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string GroupName { get; set; } = string.Empty;
}