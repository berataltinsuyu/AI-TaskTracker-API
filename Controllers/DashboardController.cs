using System.Security.Claims;
using AITaskTracker.API.DTOs;
using AITaskTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized(ApiResponse<DashboardSummaryDto>.ErrorResponse("User id claim not found."));
        }

        var userId = int.Parse(userIdClaim);

        var summary = await _dashboardService.GetSummaryAsync(userId);

        return Ok(ApiResponse<DashboardSummaryDto>.SuccessResponse(
            summary,
            "Dashboard summary retrieved successfully."
        ));
    }
}