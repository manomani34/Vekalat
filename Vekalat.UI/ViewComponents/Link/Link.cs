using Vekalat.Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Vekalat.UI.ViewComponents.Link
{

    public class Link : ViewComponent
    {

        private readonly ILogger<Link> _logger;
        private readonly IMediator _mediator;

        public Link(ILogger<Link> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            LinkFeature.GetActiveQuery query = new LinkFeature.GetActiveQuery();
            var allLink = await _mediator.Send(query, new CancellationToken());
            return await Task.FromResult((IViewComponentResult)View("Link", allLink));
        }

    }
}
