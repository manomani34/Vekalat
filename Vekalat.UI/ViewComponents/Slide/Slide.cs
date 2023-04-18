using Vekalat.Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Vekalat.UI.ViewComponents.Slide
{

    public class Slide : ViewComponent
    {

        private readonly ILogger<Slide> _logger;
        private readonly IMediator _mediator;

        public Slide(ILogger<Slide> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            SlidFeature.GetActiveQuery query = new SlidFeature.GetActiveQuery();
            var allSlide = await _mediator.Send(query, new CancellationToken());
            return await Task.FromResult((IViewComponentResult)View("Slide", allSlide));
        }

    }
}
