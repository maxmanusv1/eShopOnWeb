using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.eShopWeb.Web.Pages.Admin;

[Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS)]

public class OrderDetailsModel : PageModel
{
    private readonly CatalogContext _catalogContext;

    public OrderDetailsModel(CatalogContext catalogContext)
    {
        this._catalogContext = catalogContext;
    }

    public List<Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Order> orders { get; set; } = new List<Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Order>();
    public List<Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.OrderItem> _orderItems { get; set; } = new List<Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.OrderItem>();

    public async Task OnGet()
    {
        _orderItems = await _catalogContext.OrderItems.ToListAsync();
        orders = await _catalogContext.Orders.ToListAsync();    
    }
}
