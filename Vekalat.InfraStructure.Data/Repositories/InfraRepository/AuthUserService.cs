using Vekalat.Application.Common.InfraServices;
using Vekalat.Application.Common.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Vekalat.InfraStructure.Data.Repositories.InfraRepository
{
    public class AuthUserService : IAuthUserService
    {
        public async Task SignInWithCookieAsync(LoginResultDto user, HttpContext context)
        {
            var claims = new List<Claim>()
                    {
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Firstname", user.Firstname.ToString()),
                        new Claim("Lastname", user.Lastname.ToString()),
                        new Claim("Email", user.Email.ToString()),
                        new Claim(ClaimTypes.Name, user.Mobil)
                    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = false
            };
            await AuthenticationHttpContextExtensions.SignInAsync(context, principal, properties);
        }
    }
}
