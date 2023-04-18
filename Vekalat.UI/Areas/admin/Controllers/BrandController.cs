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
using static Vekalat.Application.Features.BrandFeature;
using Vekalat.UI.Areas.admin.ViewModels.BrandViewModels;
using Vekalat.Core.Localization;
using Vekalat.Application.Common;
using Vekalat.Core.Errors;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class BrandController : Controller
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [PermissionChecker(new[] { 1 })]
        public async Task<IActionResult> Index(BrandFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetBrandListQuery { BrandFilterInput = filterInput }, cancellationToken);
            return View(new BrandViewModel { Filter = filterInput, PagingHandler = items });
        }

        [PermissionChecker(new[] { 1 })]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditBrandDto BrandDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new CreateBrandCommand { CreateOrEditBrandDto = BrandDto }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var Brand = await _mediator.Send(new GetBrandDetailForEditQuery { BrandId = id }, cancellationToken);
            return View(Brand);
        }

        [HttpPut]
        public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditBrandDto BrandDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                await _mediator.Send(new EditBrandCommand { CreateOrEditBrandDto = BrandDto }, cancellationToken);
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
                var result = await _mediator.Send(new DeleteBrandCommand { BrandId = id }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
