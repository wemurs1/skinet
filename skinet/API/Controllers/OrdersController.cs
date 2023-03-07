using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User.RetrieveEmailFromPrincipal();
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress!);
            var order = await _orderService.CreateOrderAsync(email!, orderDto.DeliveryMethodID, orderDto.BasketId!, address);
            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
            return Ok(order);
        }
    }
}