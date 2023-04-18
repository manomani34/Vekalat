using Vekalat.Application.Common;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.AccountFeature;

namespace Vekalat.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthUserService _authUser;

        public AccountController(IMediator mediator, IAuthUserService authUser)
        {
            _mediator = mediator;
            _authUser = authUser;
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/Login")]
        public async Task<ActionResult<ReturnedDto>> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                var user = await _mediator.Send(new LoginCommand() { Login = loginDto }, cancellationToken);
                await _authUser.SignInWithCookieAsync(user, HttpContext);

                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        //[Route("PasswordRecovery")]
        //public IActionResult PasswordRecovery()

        //{
        //    ViewBag.Message = "1";
        //    return View();
        //}


        //[HttpPost]
        //[Route("PasswordRecovery")]
        //public async Task<IActionResult> PasswordRecovery(AccountFeature.LoginDto loginDto, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(loginDto);
        //    }
        //    try
        //    {
        //        ViewBag.Message = "2";
        //        return View();
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("UserName", "شماره تلفن همراه وارد شده در وب سایت اهل قلم نیست");
        //        ViewBag.Message = "3";
        //        return View();
        //    }
        //}

    }
}
