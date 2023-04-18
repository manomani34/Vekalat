using Vekalat.Application.Common;
using Vekalat.Application.Common.Helpers;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.TeamViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.TeamFeature;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class TeamController : Controller
    {
        private readonly IMediator _mediator;
        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(TeamFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetTeamListQuery { TeamFilterInput = filterInput }, cancellationToken);
            return View(new TeamViewModel { Filter = filterInput, PagingHandler = items });
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditTeamDto TeamDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateTeamCommand { CreateOrEditTeamDto = TeamDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var Team = await _mediator.Send(new GetTeamDetailForEditQuery { TeamId = id }, cancellationToken);
            return View(Team);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditTeamDto TeamDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditTeamCommand { CreateOrEditTeamDto = TeamDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteTeamCommand { TeamId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
