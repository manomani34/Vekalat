using AryanShop.Application.Common.Helpers;
using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.BlogViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BlogFeature;
using static Vekalat.Application.Features.BlogSubjectFeature;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IMediator _mediator;
        public BlogController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(BlogFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetBlogListQuery { BlogFilterInput = filterInput }, cancellationToken);
            return View(new BlogViewModel { Filter = filterInput, PagingHandler = items });
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewData["Subjects"] = await _mediator.Send(new GetAllBlogSubjectSelectListQuery() { }, cancellationToken);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditBlogDto blogDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                blogDto.UserId = User.GetUserId();
                await _mediator.Send(new CreateBlogCommand { CreateOrEditBlogDto = blogDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            ViewData["Subjects"] = await _mediator.Send(new GetAllBlogSubjectSelectListQuery() { }, cancellationToken);
            var blog = await _mediator.Send(new GetBlogDetailForEditQuery { BlogId = id }, cancellationToken);
            return View(blog);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditBlogDto blogDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditBlogCommand { CreateOrEditBlogDto = blogDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteBlogCommand { BlogId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
