using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Vekalat.UI.Areas.admin.ViewModels.BlogSubjectViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BlogSubjectFeature;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class BlogSubjectController : Controller
    {
        private readonly IMediator _mediator;
        public BlogSubjectController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(BlogSubjectFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetBlogSubjectListQuery { BlogSubjectFilterInput = filterInput }, cancellationToken);
            return View(new BlogSubjectViewModel { Filter = filterInput, PagingHandler = items });
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditBlogSubjectDto blogSubjectDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateBlogSubjectCommand { CreateOrEditBlogSubjectDto = blogSubjectDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {

            var blogSubject = await _mediator.Send(new GetBlogSubjectDetailForEditQuery { BlogSubjectId = id }, cancellationToken);
            return PartialView(blogSubject);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditBlogSubjectDto blogSubjectDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditBlogSubjectCommand { CreateOrEditBlogSubjectDto = blogSubjectDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteBlogSubjectCommand { BlogSubjectId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
