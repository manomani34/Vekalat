using Vekalat.UI.ViewModels.StudioViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.StudioFeature;

namespace Vekalat.UI.Controllers
{

    public class StudioController : Controller
    {
        private readonly IMediator _mediator;

        public StudioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("/Studio")]
        public async Task<IActionResult> Studio(StudioFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetStudioListQuery { StudioFilterInput = filterInput }, cancellationToken);
            return View(new StudioViewModel { Filter = filterInput, PagingHandler = items });
        }

        [Route("/StudioDetails/{id}/{Name}")]
        public async Task<IActionResult> StudioDetails(int id, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(new GetStudioDetailQuery { StudioId = id }, cancellationToken);
            return View(item);
        }

        [Route("/Location")]
        public async Task<IActionResult> Location(StudioFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetStudioListQuery { StudioFilterInput = filterInput }, cancellationToken);
            return View(new StudioViewModel { Filter = filterInput, PagingHandler = items });
        }
    }
}
