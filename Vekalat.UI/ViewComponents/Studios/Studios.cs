using Vekalat.UI.ViewModels.StudioViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.StudioFeature;

namespace Vekalat.UI.ViewComponents.Studios
{

    public class Studios : ViewComponent
    {
        private readonly IMediator _mediator;
        public Studios(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            //StudioFeature.GetStudioListQuery query = new StudioFeature.GetStudioListQuery();
            //var allStudios = await _mediator.Send(query, new CancellationToken());
            var allStudios = await _mediator.Send(new GetStudioListQuery()
            {
                StudioFilterInput = new StudioFilterInput
                {
                    Take = 4,                    
                }
            }, cancellationToken);
            return await Task.FromResult((IViewComponentResult)View("Studios", allStudios));
        }

    }
}
