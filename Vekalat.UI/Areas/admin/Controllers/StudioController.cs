using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.StudioViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.StudioFeature;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class StudioController : Controller
    {
        private readonly IMediator _mediator;
        public StudioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(StudioFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetStudioListQuery { StudioFilterInput = filterInput }, cancellationToken);
            return View(new StudioViewModel { Filter = filterInput, PagingHandler = items });
        }

        public async Task<IActionResult> IndexPartial(StudioFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetStudioListQuery { StudioFilterInput = filterInput }, cancellationToken);
            return PartialView(new StudioViewModel { Filter = filterInput, PagingHandler = items });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditStudioDto equipmentDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateStudioCommand { CreateOrEditStudioDto = equipmentDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var equipment = await _mediator.Send(new GetStudioDetailForEditQuery { StudioId = id }, cancellationToken);
            return View(equipment);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditStudioDto equipmentDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditStudioCommand { CreateOrEditStudioDto = equipmentDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteStudioCommand { StudioId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
