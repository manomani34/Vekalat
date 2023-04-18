using System.Threading.Tasks;

namespace Vekalat.Application.Common.InfraServices
{
    public interface IRenderViewToString
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}