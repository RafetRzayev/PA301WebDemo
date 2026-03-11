using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pa301Fiorelle.Areas.AdminPanel.Controllers
{
    public class DashboardController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
