using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucaJobPortalDotNet.Interfaces;

namespace AucaJobPortalDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobTypesController : ControllerBase
{
    private readonly IJobTypeService _jobTypeService;

    public JobTypesController(IJobTypeService jobTypeService)
    {
        _jobTypeService = jobTypeService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var jobTypes = await _jobTypeService.GetAllJobTypesAsync();
            return Ok(new
            {
                success = true,
                data = jobTypes,
                count = jobTypes.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}