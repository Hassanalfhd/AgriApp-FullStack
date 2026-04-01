using System.Security.Claims;
using Agricultural_For_CV_DAL.Entities;
using Agricultural_For_CV_Shared.Dtos.OrdersDtos;
using Agricultural_For_CV_Shared.Dtos.UserDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Results;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agricultural_For_CV_API.Controllers
{
    /// <summary>
    /// Responsible for handling order-related operations such as creating orders, 
    /// retrieving specific order details, and listing user orders.
    /// </summary>
    [Route("api/v{version:ApiVersion}/Orders")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderService">Service responsible for managing order logic.</param>
        /// <param name="logger">Logger instance for capturing logs and diagnostics.</param>
        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order for the user.
        /// </summary>
        /// <param name="dto">Order details including items, user ID, and payment information.</param>
        /// <returns>
        /// Returns the created order if successful, 
        /// or a validation/error message if something went wrong.
        /// </returns>
        /// <response code="200">Order created successfully.</response>
        /// <response code="400">Invalid input data or validation error.</response>
        /// <response code="500">Server error while processing the order.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Result<OrderResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OrderResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<OrderResponseDto>), StatusCodes.Status500InternalServerError)]
        [Role(UserRole.Admin, UserRole.Customer )]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto dto)
        {
            //  التحقق من صحة البيانات قبل المعالجة
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                //  استدعاء الخدمة لإنشاء الطلب في قاعدة البيانات
                var result= await _orderService.CreateOrderAsync(dto);

                if (!result.IsSuccess)
                return StatusCode(500, new { message = "An error occurred while creating the order." });

                return Ok(result);
            }

            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating order");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                return StatusCode(500, new { message = "An error occurred while creating the order." });
            }

        }


        /// <summary>
        /// Retrieves details of a specific order by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>Returns the order details if found, or 404 if not found.</returns>
        /// <response code="200">Order found successfully.</response>
        /// <response code="404">Order with the given ID not found.</response>
        /// <response code="500">Server error while retrieving the order.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Result<OrderResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OrderResponseDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<OrderResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound(new { message = $"Order with ID {id} not found." });

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting order by ID {OrderId}", id);
                return StatusCode(500, new { message = "An error occurred while fetching the order." });
            }
        }



        /// <summary>
        /// Retrieves details of a specific order by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>Returns the order details if found, or 404 if not found.</returns>
        /// <response code="200">Order found successfully.</response>
        /// <response code="404">Order with the given ID not found.</response>
        /// <response code="500">Server error while retrieving the order.</response>
        [HttpPatch("cancel/{id:int}")]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status500InternalServerError)]
        [Role(UserRole.Admin, UserRole.Customer)]
        public async Task<IActionResult> CanceledOrder(int id, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                if (id < 1)
                    return BadRequest(Result<int>.Failure("Invalid or missing order ID ."));

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    
                if(!int.TryParse(userId, out int authenticatedStudentId)) 
                    return BadRequest(Result<int>.Failure("Invalid or missing user ID ."));


                var authResult = await authorizationService.AuthorizeAsync(User, authenticatedStudentId, "CustomerOwnerOrAdmin");


                if (!authResult.Succeeded)
                    return Forbid(); // Returns HTTP 403 Forbidden


                var result = await _orderService.CanceledOrderAsync(id);
                if (result == null)
                    return NotFound(new { message = $"Order with ID {id} not found." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while canceled order with ID {OrderId}", id);
                return StatusCode(500, new { message = "An error occurred while fetching the order." });
            }
        }



        [HttpPatch("cancel-item/{id:int}")]
        [ProducesResponseType(typeof(Result<OrderDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OrderDetailDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<OrderDetailDto>), StatusCodes.Status500InternalServerError)]
        [Role(UserRole.Admin, UserRole.Customer)]
        public async Task<IActionResult> CanceledOrderItem(int id, [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                if (id < 1)
                    return BadRequest(Result<OrderDetailDto>.Failure("Invalid or missing order item  ID ."));

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userId, out int authenticatedStudentId))
                    return BadRequest(Result<OrderDetailDto>.Failure("Invalid or missing user ID ."));


                var authResult = await authorizationService.AuthorizeAsync(User, authenticatedStudentId, "CustomerOwnerOrAdmin");


                if (!authResult.Succeeded)
                    return Forbid(); // Returns HTTP 403 Forbidden


                var result = await _orderService.CanceledOrderItemAsync(id);
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




        /// <summary>
        /// Retrieves all orders placed by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders are being fetched.</param>
        /// <returns>A list of orders belonging to the specified user.</returns>
        /// <response code="200">List of user orders retrieved successfully.</response>
        /// <response code="500">Server error while fetching the user's orders.</response>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<OrderResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<OrderResponseDto>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Result<IEnumerable<OrderResponseDto>>), StatusCodes.Status403Forbidden)]
        [Role(UserRole.Admin,UserRole.Customer)]
        public async Task<IActionResult> GetUserOrders([FromQuery]OrderStatus status, [FromServices] IAuthorizationService authorizationService)
        {



            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int authenticatedStudentId))
                return BadRequest(Result<OrderResponseDto>.Failure("Invalid or missing user ID ."));


            var authResult = await authorizationService.AuthorizeAsync(User, authenticatedStudentId, "CustomerOwnerOrAdmin");


            if (!authResult.Succeeded)
                return Forbid(); // Returns HTTP 403 Forbidden


            try
            {
                var result = await _orderService.GetUserOrdersAsync(authenticatedStudentId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching orders for user {UserId}", authenticatedStudentId);
                return StatusCode(500, new { message = "An error occurred while fetching user orders." });
            }
        }


      


    }
}
