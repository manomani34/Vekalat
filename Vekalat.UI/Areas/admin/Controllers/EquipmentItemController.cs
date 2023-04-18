using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.EquipmentItemViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentItemFeature;

namespace AryanShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class EquipmentItemController : Controller
    {
        private readonly IMediator _mediator;
        public EquipmentItemController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(EquipmentItemFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetEquipmentItemListQuery { EquipmentItemFilterInput = filterInput }, cancellationToken);
            return View(new EquipmentItemViewModel { Filter = filterInput, PagingHandler = items });
        }

        public IActionResult Create(int id)
        {
            return PartialView(new CreateOrEditEquipmentItemDto { EquipmentId = id });
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditEquipmentItemDto EquipmentItemDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateEquipmentItemCommand { CreateOrEditEquipmentItemDto = EquipmentItemDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var EquipmentItem = await _mediator.Send(new GetEquipmentItemDetailForEditQuery { EquipmentItemId = id }, cancellationToken);
            return PartialView(EquipmentItem);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditEquipmentItemDto EquipmentItemDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditEquipmentItemCommand { CreateOrEditEquipmentItemDto = EquipmentItemDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteEquipmentItemCommand { EquipmentItemId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
