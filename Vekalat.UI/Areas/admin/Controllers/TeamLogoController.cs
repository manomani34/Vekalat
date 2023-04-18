using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.TeamLogoViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.TeamLogoFeature;

namespace AryanShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class TeamLogoController : Controller
    {
        private readonly IMediator _mediator;
        public TeamLogoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(TeamLogoFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetTeamLogoListQuery { TeamLogoFilterInput = filterInput }, cancellationToken);
            return View(new TeamLogoViewModel { Filter = filterInput, PagingHandler = items });
        }

        public IActionResult Create(int? id)
        {
            return PartialView(new CreateOrEditTeamLogoDto { TeamId = id });
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditTeamLogoDto TeamLogoDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateTeamLogoCommand { CreateOrEditTeamLogoDto = TeamLogoDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var TeamLogo = await _mediator.Send(new GetTeamLogoDetailForEditQuery { TeamLogoId = id }, cancellationToken);
            return PartialView(TeamLogo);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditTeamLogoDto TeamLogoDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditTeamLogoCommand { CreateOrEditTeamLogoDto = TeamLogoDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteTeamLogoCommand { TeamLogoId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
