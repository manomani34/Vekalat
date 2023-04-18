using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vekalat.Application.Features;
using static Vekalat.Application.Features.CategoryFeature;
using Vekalat.UI.ViewModels.CategoryViewModels;
using static Vekalat.Application.Features.EquipmentFeature;
using Vekalat.UI.ViewModels.EquipmentViewModels;

namespace Vekalat.UI.Controllers
{

    public class EquipmentController : Controller
    {
        private readonly IMediator _mediator;

        public EquipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("/Equipment")]
        public async Task<IActionResult> Equipment(EquipmentFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetEquipmentListQuery { EquipmentFilterInput = filterInput }, cancellationToken);
            var categories = await _mediator.Send(new GetCategoryListQuery
            {
                CategoryFilterInput = new CategoryFilterInput
                {
                    Take= 99999,
                }
            }, cancellationToken);
            return View(new EquipmentViewModel
            {
                Filter = filterInput,
                PagingHandler = items,
                Categories = categories.Items
            });
        }

        [Route("/EquipmentDetails/{id}/{Name}")]
        public async Task<IActionResult> EquipmentDetails(int id, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(new GetEquipmentDetailQuery { EquipmentId = id }, cancellationToken);
            return View(item);
        }
    }
}
