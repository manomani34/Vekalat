using AryanShop.Application.Common.Helpers;
using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BasketFeature;

namespace Vekalat.UI.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IMediator _mediator;
        public CheckoutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult<ReturnedDto>> Index(CreateOrEditInvoiceDto dto, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Messages.InvalidState;
        //    }
        //    try
        //    {
        //        var basket = await _mediator.Send(new GetBasketDetailQuery
        //        {
        //            BasketId = User.GetUserId().ToString(),
        //            Session = HttpContext.Session,
        //        }, cancellationToken);
        //        var result = await _mediator.Send(new CreateOrderCommand
        //        {
        //            CreateOrEditInvoiceDto = dto,
        //            Basket = basket
        //        }, cancellationToken);
        //        return Messages.SuccessState;
        //    }
        //    catch (WebAppException e)
        //    {
        //        return Messages.FailExceptionState(e);
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> ChangePayment(int type, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Messages.InvalidState;
            }
            try
            {
                var basket = await _mediator.Send(new GetBasketDetailQuery
                {
                    UserId = User.GetUserId(),
                    Session = HttpContext.Session,

                }, cancellationToken);
                basket.PaymentType = type;

                await _mediator.Send(new EditBasketCommand
                {
                    Basket = basket,
                    Session = HttpContext.Session,
                }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

    }
}
