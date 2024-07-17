using CE.Assessment.Application;
using CE.Assessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CE.Assessment.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OrderService _orderService;

        public HomeController(ILogger<HomeController> logger, OrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            await _orderService.GetTop5Orders(ct);
            return View();
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
}
