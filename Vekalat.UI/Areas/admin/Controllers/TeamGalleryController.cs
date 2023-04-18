using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.TeamGalleryViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.TeamGalleryFeature;

namespace AryanShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class TeamGalleryController : Controller
    {
        private readonly IMediator _mediator;
        public TeamGalleryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(TeamGalleryFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetTeamGalleryListQuery { TeamGalleryFilterInput = filterInput }, cancellationToken);
            return View(new TeamGalleryViewModel { Filter = filterInput, PagingHandler = items });
        }

        public IActionResult Create(int? id)
        {
            return PartialView(new CreateOrEditTeamGalleryDto { TeamId = id });
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditTeamGalleryDto TeamGalleryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateTeamGalleryCommand { CreateOrEditTeamGalleryDto = TeamGalleryDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var TeamGallery = await _mediator.Send(new GetTeamGalleryDetailForEditQuery { TeamGalleryId = id }, cancellationToken);
            return PartialView(TeamGallery);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditTeamGalleryDto TeamGalleryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditTeamGalleryCommand { CreateOrEditTeamGalleryDto = TeamGalleryDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ReturnedDto>> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new DeleteTeamGalleryCommand { TeamGalleryId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
