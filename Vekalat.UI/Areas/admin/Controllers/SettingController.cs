using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vekalat.Application.Features;
using Vekalat.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class SettingController : Controller
    {
        private readonly IMediator _mediator;

        public SettingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var slider = await _mediator.Send(new SettingFeature.GetSettingQuery { Id = 1 }, cancellationToken);
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Setting command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            await _mediator.Send(new SettingFeature.EditSettingCommand()
            {
                Setting = command
            }, cancellationToken);

            return RedirectToAction("index","Home");
        }

    }
}
