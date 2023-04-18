
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Vekalat.UI.ViewComponents.Footer
{

    public class Footer : ViewComponent
    {
        private readonly IMediator _mediator;
        public Footer(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult((IViewComponentResult)View("Footer"));
        }

    }
}
