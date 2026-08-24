using System.ComponentModel.DataAnnotations;

namespace AucaJobPortalDotNet.DTOs;

public class UpdateProgramApplicationStatusDTO
{
    [Required]
    public string Status { get; set; } = string.Empty;
}