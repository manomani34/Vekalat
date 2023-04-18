using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vekalat.Application.Features;
using static Vekalat.Application.Features.TeamFeature;
using Vekalat.UI.ViewModels.TeamViewModels;

namespace Vekalat.UI.Controllers
{

    public class TeamController : Controller
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("/Team")]
        public async Task<IActionResult> Team(TeamFilterInput filterInput, CancellationToken cancellationToken)
        {

            var items = await _mediator.Send(new GetTeamListQuery { TeamFilterInput = filterInput }, cancellationToken);
            return View(new TeamViewModel { Filter = filterInput, PagingHandler = items });
        }

        [Route("/TeamDetails/{id}/{Name}")]
        public async Task<IActionResult> TeamDetails(int id, CancellationToken cancellationToken)
        {
            try
            {
                var news = await _mediator.Send(new TeamFeature.GetTeamDetailQuery { TeamId = id }, cancellationToken);
                return View(news);
            }
            catch
            {
                return Redirect("/");
            }
        }
    }
}
