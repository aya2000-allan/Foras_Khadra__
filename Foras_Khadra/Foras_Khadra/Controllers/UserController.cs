using Microsoft.AspNetCore.Mvc;

namespace Foras_Khadra.Controllers
{
    public class UserController : Controller
    {
        // GET: User/Dashboard
        public IActionResult Dashboard()
        {
            return View(); // يجب أن يكون لديك View باسم Dashboard.cshtml داخل Views/User
        }
    }
}
