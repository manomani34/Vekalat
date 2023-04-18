using AryanShop.Application.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BasketFeature;

namespace Vekalat.UI.ViewComponents.Basket
{
    public class BasketFullViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public BasketFullViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var user = (ClaimsPrincipal)User;
            var items = await _mediator.Send(new GetBasketDetailQuery
            {
                UserId = user.GetUserId(),
                Session = HttpContext.Session
            }, cancellationToken);
            return View("BasketFullComponent", items);
        }
    }
}
