using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Features.MyOrders;
using Microsoft.eShopWeb.Web.Features.OrderDetails;

namespace Microsoft.eShopWeb.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
[Route("[controller]/[action]")]
public class OrderController : Controller
{
    private readonly IMediator _mediator;
    private readonly IOrderService _orderService;
    public OrderController(IMediator mediator, IOrderService service)
    {
        _mediator = mediator;
        _orderService = service;    
    }
    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {   
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        var viewModel = await _mediator.Send(new GetMyOrders(User.Identity.Name));

        return View(viewModel);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> Detail(int orderId)
    {
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId));

        if (viewModel == null)
        {
            return BadRequest("No such order found for this user.");
        }

        return View(viewModel);
    }
    [HttpGet("{orderId}")]
    public async Task<IActionResult> Management(int orderId)
    {
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId));

        if (viewModel == null)
        {
            return BadRequest("No such order found for this user.");
        }

        return PartialView("_ManagementPartialView",viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> ApproveOrder(int orderId)
    {
        await _orderService.ChangeStatusAsync(orderId, OrderStatus.Approved);
        return Ok($"approved succesfully {orderId}"); 
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        await _orderService.ChangeStatusAsync(orderId, OrderStatus.Cancelled);
        return Ok("cancelled succesfully");
    }
}
