using Apartaments.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Apartaments.Models.PageModels;

namespace Apartaments.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int byRooms)
        {
            return View(new IndexModel(byRooms));
        }

        [HttpGet]
        public JsonResult LineChart(int id)
        {
            return new JsonResult(IndexModel.GetDataChart(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}