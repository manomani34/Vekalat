using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.EquipmentViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BrandFeature;
using static Vekalat.Application.Features.CategoryFeature;
using static Vekalat.Application.Features.EquipmentFeature;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class EquipmentController : Controller
    {
        private readonly IMediator _mediator;
        public EquipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(EquipmentFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetEquipmentListQuery { EquipmentFilterInput = filterInput }, cancellationToken);
            return View(new EquipmentViewModel { Filter = filterInput, PagingHandler = items });
        }

        public async Task<IActionResult> IndexPartial(EquipmentFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetEquipmentListQuery { EquipmentFilterInput = filterInput }, cancellationToken);
            return PartialView(new EquipmentViewModel { Filter = filterInput, PagingHandler = items });
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewData["Categories"] = await _mediator.Send(new GetAllCategoriesSelectListQuery() { }, cancellationToken);
            ViewData["Brands"] = await _mediator.Send(new GetAllBrandSelectListQuery() { }, cancellationToken);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditEquipmentDto equipmentDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateEquipmentCommand { CreateOrEditEquipmentDto = equipmentDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            ViewData["Categories"] = await _mediator.Send(new GetAllCategoriesSelectListQuery() { }, cancellationToken);
            ViewData["Brands"] = await _mediator.Send(new GetAllBrandSelectListQuery() { }, cancellationToken);

            var equipment = await _mediator.Send(new GetEquipmentDetailForEditQuery { EquipmentId = id }, cancellationToken);
            return View(equipment);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditEquipmentDto equipmentDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditEquipmentCommand { CreateOrEditEquipmentDto = equipmentDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteEquipmentCommand { EquipmentId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
