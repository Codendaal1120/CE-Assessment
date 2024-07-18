using CE.Assessment.Application;
using CE.Assessment.Application.Model;
using CE.Assessment.Application.Services;
using CE.Assessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace CE.Assessment.Web.Controllers;

[ExcludeFromCodeCoverage]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly OrderService _orderService;
    private readonly OffersService _offersService;
    private readonly IToastNotification _toastNotification;

    public HomeController(
        ILogger<HomeController> logger, 
        OrderService orderService, 
        OffersService offersService, 
        IToastNotification toastNotification)
    {
        _logger = logger;
        _orderService = orderService;
        _offersService = offersService;
        _toastNotification = toastNotification;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {       
        var response = await _orderService.GetTopNOrders(5, ct);
        if (!response.Try(out var topNorders, out var error)) 
        {
            _toastNotification.AddErrorToastMessage(response.Error);
            return RedirectToAction("Index");
        }

        return View(new HomeViewModel() { Records = topNorders?.ToArray() ?? Array.Empty<ProductSalesRecord>() });
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(string productNo)
    {
        var response = await _offersService.UpdateProductStock(productNo, 25, CancellationToken.None);
        if (!response.Success)
        {
            _toastNotification.AddErrorToastMessage(response.Error);
            return RedirectToAction("Index");
        }

        _toastNotification.AddInfoToastMessage("Updated");
        return RedirectToAction("Index");
    } 

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
