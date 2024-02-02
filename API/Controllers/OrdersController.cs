﻿using System.Security.Claims;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
    {
        var email = User.RetrieveEmailFromPrincipal();
        var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress!);
        var order = await _orderService.CreateOrderAsync(email!, orderDto.DeliveryMethodId, orderDto.BasketId!, address);
        if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
    {
        var email = User.RetrieveEmailFromPrincipal();
        var orders = await _orderService.GetOrdersForUserAsync(email!);

        return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
    {
        var email = User.RetrieveEmailFromPrincipal();
        var order = await _orderService.GetOrderById(id, email!);
        if (order == null) return NotFound(new ApiResponse(404));

        return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
    }

    [HttpGet("deliveryMethods")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
        return Ok(deliveryMethods);
    }
}
