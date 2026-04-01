using Agricultural_For_CV_Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Agricultural_For_CV_Shared.Dtos.OrdersDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Results;
using Agricultural_For_CV_API.Controllers;

namespace Agricultural_For_CV.Controllers.v1
{

    [Route("api/v{version:ApiVersion}/Orders")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class FarmerOrdersController : ControllerBase
    {
        private readonly IFarmerService _farmerService;
        private readonly ILogger<OrdersController> _logger;
        public FarmerOrdersController(IFarmerService farmerService, ILogger<OrdersController> logger)
        {
            _farmerService = farmerService;
            _logger = logger;
        }



        /// <summary>
        /// Retrieves all orders placed by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders are being fetched.</param>
        /// <returns>A list of orders belonging to the specified user.</returns>
        /// <response code="200">List of user orders retrieved successfully.</response>
        /// <response code="500">Server error while fetching the user's orders.</response>
        [HttpGet("my-orders")]
        [ProducesResponseType(typeof(Result<IEnumerable<FarmerOrderDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<FarmerOrderDto>>), StatusCodes.Status500InternalServerError)]
        [Role(UserRole.Admin, UserRole.Farmer)]
        public async Task<IActionResult> GetMyOrders(ItemStatus itemStatus)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int identity))
                return BadRequest(Result<FarmerOrderDto>.Failure("Invalid or missing user ID ."));

            try
            {

                var result = await _farmerService.GetFarmerDashboardItemsAsync(identity, itemStatus);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching orders for user {UserId}", identity);
                return StatusCode(500, new { message = "An error occurred while fetching user orders." });
            }
        }


        [HttpPatch("farmer/cancel-item/{id:int}")]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status500InternalServerError)]
        [Role(UserRole.Admin, UserRole.Customer)]
        public async Task<IActionResult> CanceledOrderItem(int id, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                if (id < 1)
                    return BadRequest(Result<int>.Failure("Invalid or missing order item  ID ."));

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userId, out int authenticatedStudentId))
                    return BadRequest(Result<int>.Failure("Invalid or missing user ID ."));


                var authResult = await authorizationService.AuthorizeAsync(User, authenticatedStudentId, "FarmerOwnerOrAdmin");

                if (!authResult.Succeeded)
                    return Forbid(); // Returns HTTP 403 Forbidden


                var result = await _farmerService.CanceledOrderItemAsync(id);
                if (result == null)
                    return NotFound(new { message = $"Order item with ID {id} not found." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while canceled order item with ID {OrderId}", id);
                return StatusCode(500, new { message = "An error occurred while fetching the order item." });
            }
        }



        [HttpPatch()]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status500InternalServerError)]
        [Role(UserRole.Admin, UserRole.Farmer)]
        public async Task<IActionResult> AcceptedOrderItem(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int identity) || id < 0)
                return BadRequest(Result<FarmerOrderDto>.Failure("Invalid or missing user ID ."));

            try
            {
                var result = await _farmerService.AcceptedOrderItemAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching orders for user {UserId}", identity);
                return StatusCode(500, new { message = "An error occurred while fetching user orders." });
            }
        }



        [HttpPatch("pickup")]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status500InternalServerError)]
        [Role(UserRole.Admin, UserRole.Farmer)]
        public async Task<IActionResult> ReadyForPickUpOrderItem(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int identity) || id < 0)
                return BadRequest(Result<FarmerOrderDto>.Failure("Invalid or missing user ID ."));

            try
            {
                var result = await _farmerService.ReadyForPickUpOrderItemAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching orders for user {UserId}", identity);
                return StatusCode(500, new { message = "An error occurred while fetching user orders." });
            }
        }










    }
}
