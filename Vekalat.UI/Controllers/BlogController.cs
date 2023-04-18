using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vekalat.Application.Features;
using static Vekalat.Application.Features.BlogFeature;
using Vekalat.UI.ViewModels.BlogViewModels;

namespace Vekalat.UI.Controllers
{

    public class BlogController : Controller
    {
        private readonly IMediator _mediator;

        public BlogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("/Blog")]
        public async Task<IActionResult> Blog(BlogFilterInput filterInput, CancellationToken cancellationToken)
        {

            var items = await _mediator.Send(new GetBlogListQuery { BlogFilterInput = filterInput }, cancellationToken);
            return View(new BlogViewModel { Filter = filterInput, PagingHandler = items });
        }

        [Route("/BlogDetails/{id}/{Name}")]
        public async Task<IActionResult> BlogDetails(int id, CancellationToken cancellationToken)
        {
            try
            {
                var news = await _mediator.Send(new BlogFeature.GetBlogDetailQuery { BlogId = id }, cancellationToken);
                return View(news);
            }
            catch
            {
                return Redirect("/");
            }
        }
    }
}
