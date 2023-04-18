using Application.Common.Dto;
using Vekalat.Application.Common.Dto;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace Vekalat.Application.Common.InfraServices
{
    public interface IAuthUserService
    {
        Task SignInWithCookieAsync(LoginResultDto user, HttpContext context);
    }

}
