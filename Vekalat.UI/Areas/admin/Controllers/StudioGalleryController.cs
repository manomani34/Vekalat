using AryanShop.UI.Areas.Admin.ViewModels.StudioGalleryViewModels;
using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.StudioGalleryFeature;

namespace AryanShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class StudioGalleryController : Controller
    {
        private readonly IMediator _mediator;
        public StudioGalleryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(StudioGalleryFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetStudioGalleryListQuery { StudioGalleryFilterInput = filterInput }, cancellationToken);
            return View(new StudioGalleryViewModel { Filter = filterInput, PagingHandler = items });
        }

        public IActionResult Create(int? id)
        {
            return PartialView(new CreateOrEditStudioGalleryDto { StudioId = id });
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditStudioGalleryDto studioGalleryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateStudioGalleryCommand { CreateOrEditStudioGalleryDto = studioGalleryDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var studioGallery = await _mediator.Send(new GetStudioGalleryDetailForEditQuery { StudioGalleryId = id }, cancellationToken);
            return PartialView(studioGallery);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditStudioGalleryDto studioGalleryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditStudioGalleryCommand { CreateOrEditStudioGalleryDto = studioGalleryDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteStudioGalleryCommand { StudioGalleryId = id }, cancellationToken);
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
                var studioGallery = await _mediator.Send(new GetStudioGalleryDetailForEditQuery { StudioGalleryId = id }, cancellationToken);
                studioGallery.DisplayFront = !studioGallery.DisplayFront;
                await _mediator.Send(new EditStudioGalleryCommand { CreateOrEditStudioGalleryDto = studioGallery }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
