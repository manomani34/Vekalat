using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.EquipmentGalleryViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentGalleryFeature;
using static Vekalat.Application.Features.StudioGalleryFeature;

namespace AryanShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class EquipmentGalleryController : Controller
    {
        private readonly IMediator _mediator;
        public EquipmentGalleryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(EquipmentGalleryFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetEquipmentGalleryListQuery { EquipmentGalleryFilterInput = filterInput }, cancellationToken);
            return View(new EquipmentGalleryViewModel { Filter = filterInput, PagingHandler = items });
        }

        public IActionResult Create(int? id)
        {
            return PartialView(new CreateOrEditEquipmentGalleryDto { EquipmentId = id });
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditEquipmentGalleryDto equipmentGalleryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateEquipmentGalleryCommand { CreateOrEditEquipmentGalleryDto = equipmentGalleryDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var equipmentGallery = await _mediator.Send(new GetEquipmentGalleryDetailForEditQuery { EquipmentGalleryId = id }, cancellationToken);
            return PartialView(equipmentGallery);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditEquipmentGalleryDto equipmentGalleryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditEquipmentGalleryCommand { CreateOrEditEquipmentGalleryDto = equipmentGalleryDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteEquipmentGalleryCommand { EquipmentGalleryId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> ApplayDisplayFront(int id, CancellationToken cancellationToken)
        {
            try
            {
                var equipmentGallery = await _mediator.Send(new GetEquipmentGalleryDetailForEditQuery { EquipmentGalleryId = id }, cancellationToken);
                equipmentGallery.DisplayFront = !equipmentGallery.DisplayFront;
                await _mediator.Send(new EditEquipmentGalleryCommand { CreateOrEditEquipmentGalleryDto = equipmentGallery }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
