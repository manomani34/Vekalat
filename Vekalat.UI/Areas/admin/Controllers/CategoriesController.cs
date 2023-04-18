using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.CategoryFeature;

namespace AryanShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region For  Category

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new GetCategoryListQuery
            {
                CategoryFilterInput = new CategoryFilterInput
                {
                    PageId = 1,
                    Take = 999999,
                }
            }, cancellationToken);
            return View(categories);
        }

        public IActionResult Create(int? id)
        {
            return PartialView(new CreateOrEditCategoryDto { ParentId = id });
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditCategoryDto command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                var result = await _mediator.Send(new CreateCategoryCommand
                {
                    CreateOrEditCategoryDto = command
                }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var category = await _mediator.Send(new GetCategoryDetailForEditQuery { CategoryId = id }, cancellationToken);
            return PartialView(category);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditCategoryDto command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                var result = await _mediator.Send(new EditCategoryCommand
                {
                    CreateOrEditCategoryDto = command
                }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteCategoryCommand { CategoryId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

        #endregion
    }
}
