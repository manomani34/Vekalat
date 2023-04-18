using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.EquipmentReservationViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentReservationFeature;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class EquipmentReservationController : Controller
    {
        private readonly IMediator _mediator;
        public EquipmentReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(EquipmentReservationFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetEquipmentReservationListQuery { EquipmentReservationFilterInput = filterInput }, cancellationToken);
            return View(new EquipmentReservationViewModel { Filter = filterInput, PagingHandler = items });
        }
        public async Task<IActionResult> IndexPartial(EquipmentReservationFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetEquipmentReservationListQuery { EquipmentReservationFilterInput = filterInput }, cancellationToken);
            return PartialView(new EquipmentReservationViewModel { Filter = filterInput, PagingHandler = items });
        }
        [HttpPost]
        public async Task<ActionResult<ReturnedDto<CalenderInitData>>> CalenderInitData(CancellationToken cancellationToken)
        {
            try
            {
                var items = await _mediator.Send(new GetEquipmentReservationListQuery
                {
                    EquipmentReservationFilterInput = new EquipmentReservationFilterInput
                    {
                        Take = 9999
                    }
                }, cancellationToken);
                var result = new CalenderInitData
                {
                    JsonData = JsonSerializer.Serialize(items.Items)
                };
                return Messages<CalenderInitData>.SuccessState(result);
            }
            catch (WebAppException e)
            {
                return Messages<CalenderInitData>.FailExceptionState(e);
            }

        }

        public IActionResult Create(CreateOrEditEquipmentReservationDto equipmentReservationDto)
        {
            return PartialView(equipmentReservationDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto<CreateOrEditEquipmentReservationDto>>> Create(CreateOrEditEquipmentReservationDto equipmentReservationDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages<CreateOrEditEquipmentReservationDto>.InvalidState;
            }
            try
            {
                var reservation = await _mediator.Send(new CreateEquipmentReservationCommand { CreateOrEditEquipmentReservationDto = equipmentReservationDto }, cancellationToken);
                return Messages<CreateOrEditEquipmentReservationDto>.SuccessState(reservation);
            }
            catch (WebAppException e)
            {
                return Messages<CreateOrEditEquipmentReservationDto>.FailExceptionState(e);
            }
        }

        public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
        {
            var equipmentReservation = await _mediator.Send(new GetEquipmentReservationDetailQuery { EquipmentReservationId = id }, cancellationToken);
            return PartialView(equipmentReservation);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {

            var equipmentReservation = await _mediator.Send(new GetEquipmentReservationDetailForEditQuery { EquipmentReservationId = id }, cancellationToken);
            return PartialView(equipmentReservation);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditEquipmentReservationDto equipmentReservationDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditEquipmentReservationCommand { CreateOrEditEquipmentReservationDto = equipmentReservationDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteEquipmentReservationCommand { EquipmentReservationId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
