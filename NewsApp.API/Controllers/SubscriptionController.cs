using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Services;
using NewsApp.Shared.Models;
using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using Chatty.Shared.Exceptions;
using MediatR;
using NewsApp.API.Application.User;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Constants;

namespace NewsApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly LiqPayService _liqPayService;
    private readonly ILogger<SubscriptionController> _logger;
    private readonly IMediator _mediator;

    private readonly AccessControlService _accessControlService;

    public SubscriptionController(
        LiqPayService liqPayService,
        ILogger<SubscriptionController> logger,
        IMediator mediator,

        AccessControlService accessControlService)
    {
        _liqPayService = liqPayService;
        _logger = logger;
        _mediator = mediator;

        _accessControlService = accessControlService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionRequest request)
    {
        try
        {
            var user = await _accessControlService.GetCurrentUser();
            if (user == null)
            {
                return BadRequest("User not found");
            }

            _logger.LogInformation($"Creating subscription for user: {user.Email}");

            var checkoutUrl = await _liqPayService.CreateSubscriptionAsync(
                user.Email,
                user.Id,
                request.Description
            );

            return Ok(new { checkoutUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription");
            return StatusCode(500, "Error creating subscription");
        }
    }

    [HttpPost("callback")]
    public async Task<IActionResult> HandleCallback([FromForm] string data, [FromForm] string signature)
    {
        _logger.LogInformation($"Received callback");
        
        try
        {
            if (!_liqPayService.ValidateCallback(data, signature))
            {
                return BadRequest("Invalid signature");
            }

            var decodedData = Convert.FromBase64String(data);
            var jsonString = Encoding.UTF8.GetString(decodedData);
            var callbackData = JsonSerializer.Deserialize<LiqPayCallback>(jsonString);
            _logger.LogInformation(callbackData.ToString());

            if (callbackData.Status == "subscribed")
            {
                var userId = callbackData.Customer;
                if (userId != null)
                {
                    var command = new UpdateUserRoleCommand
                    {
                        UserId = userId,
                        NewRole = UserRoles.Premium
                    };

                    var result = await _mediator.Send(command);
                    
                }
            }
            else
            {
                _logger.LogWarning($"Failed to get user {data}");
                _logger.LogWarning(callbackData.Status);
                
                return StatusCode(500, "Faild to get user");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing callback");
            return StatusCode(500, "Error processing callback");
        }
    }
    protected async Task<string?> GetCurrentUserId()
    {
        if (User.Identity.IsAuthenticated == false)
        {
            throw new UnauthorizedException("User is not authenticated");
        }

        var id = (User.Identity as ClaimsIdentity).FindFirst(x => x.Type == ClaimTypes.NameIdentifier);

        return id.Value;
    }
    
}