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
using Vekalat.Application.Common.Helpers;
using Vekalat.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class LinkController : Controller
    {
        private readonly IMediator _mediator;

        public LinkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [PermissionChecker(new[] { 1 })]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var List = await _mediator.Send(new LinkFeature.GetAllQuery {}, cancellationToken);
            return View(List);
        }

        [PermissionChecker(new[] { 1 })]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [PermissionChecker(new[] { 1 })]
        public async Task<IActionResult> Create(Link item, IFormFile img, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                await _mediator.Send(new LinkFeature.CreateCommand()
                {
                    item = item,
                    ImageFile = img
                }, cancellationToken);

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = true;
                return RedirectToAction("Index");
            }
        }
        [PermissionChecker(new[] { 1 })]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new LinkFeature.GetByIdQuery { Id = id }, cancellationToken);
            return View(items);
        }
        [HttpPost]
        [PermissionChecker(new[] { 1 })]
        public async Task<IActionResult> Edit(Link  command, IFormFile img, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            try
            {
                await _mediator.Send(new LinkFeature.EditCommand { item = command, ImageFile = img }, cancellationToken);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = true;
                return RedirectToAction("Index");
            }
        }


        public async Task<JsonResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LinkFeature.DeleteCommand { Id = id }, cancellationToken);
            return Json(result);
        }

    }
}
