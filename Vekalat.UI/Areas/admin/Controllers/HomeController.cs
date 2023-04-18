using AryanShop.Application.Common.Helpers;
using Vekalat.Application.Common.Helpers;
using Vekalat.Application.Common.InfraServices;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class HomeController : Controller
    {
        private readonly IFileSaver _fileSaver;
        private readonly IMediator _mediator;

        public HomeController(IFileSaver fileSaver, IMediator mediator)
        {
            _fileSaver = fileSaver;
            _mediator = mediator;
        }

        [PermissionChecker(new[] { 1 })]
        public IActionResult Index()
        {
            ViewData["FirstName"] = User.GetFirstName();
            ViewData["LastName"] = User.GetLastName();

            return View();
        }
        [Route("admin/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
