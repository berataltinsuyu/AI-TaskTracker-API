using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class DashboardController : ControllerBase
{
  private readonly IDashboardService _dashboardService;

  public DashboardController(IDashboardService dashboardService)
  {
    _dashboardService = dashboardService;
  }

  [HttpGet("summary")]
  public async Task<IActionResult> GetSummary()
  {
    var summary = await _dashboardService.GetSummaryAsync();

    return Ok(ApiResponse<DashboardSummaryDto>.SuccessResponse(
      summary,
      "Dashboard summary retrieved successfully."
      
      ));
  }

}
