using Vekalat.Application.Features;
using Vekalat.UI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ahmaditame.Tools;

namespace Vekalat.UI.Controllers
{
    public class SearchController : Controller
        
    {
        private readonly IMediator _mediator;
        public SearchController(IMediator mediator)
    {

        _mediator = mediator;
    }

     
      
    }
}
