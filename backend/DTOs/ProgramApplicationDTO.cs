using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;
public class ProgramApplicationDTO
{
    [Required]
    public int ProgramId { get; set; }    

    public string? MotivationLetter { get; set; } 
    public string? CurrentProgram { get; set; }
    public int? CurrentYearOfStudy { get; set; }
}