using ForkliftControlSystem.Application.DTOs;
using ForkliftControlSystem.Application.Interfaces;
using ForkliftControlSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftControlSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ForkliftsController : ControllerBase
{
    private readonly IForkliftService _forkliftService;

    public ForkliftsController(IForkliftService forkliftService)
    {
        _forkliftService = forkliftService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _forkliftService.GetAllForklifts();
            return Ok(ApiResponse<IEnumerable<ForkliftDto>>.Ok(result));
        }
        catch(Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByPaginated(int page = 1, int pageSize = 10)
    {
        try
        {
            //var result = await _forkliftService.GetPaginatedForkliftsAsync(page, pageSize); this for integraded with database
            var result = await _forkliftService.GetPaginatedForklifts(page, pageSize);
            return Ok(ApiResponse<PagedResult<ForkliftDto>>.Ok(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }
}
