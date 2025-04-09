using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Web.Attributes;

namespace FitnessTracker.Web.Controllers
{
    public class SpaController : Controller
    {
        [VirtualDom]
        public IActionResult Index()
        {
            return View();
        }
    }
}