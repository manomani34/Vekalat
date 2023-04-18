using Vekalat.UI.ViewModels.BrandViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BrandFeature;

namespace Vekalat.UI.ViewComponents.Brand
{

    public class Brand : ViewComponent
    {
        private readonly IMediator _mediator;
        public Brand(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var allBrands = await _mediator.Send(new GetBrandListQuery()
            {
                BrandFilterInput = new BrandFilterInput
                {
                    Take = 4,                         
                }
            }, cancellationToken);
            return await Task.FromResult((IViewComponentResult)View("Brand", allBrands));
        }

    }
}
