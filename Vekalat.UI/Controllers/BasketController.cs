using AryanShop.Application.Common.Helpers;
using Vekalat.Application.Common;
using Vekalat.Application.Enums;
using Vekalat.Core.Entities;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.BasketFeature;

namespace Vekalat.UI.Controllers
{
    public class BasketController : Controller
    {
        private readonly IMediator _mediator;
        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PartialBasket()
        {
            return ViewComponent("Basket");
        }
        public IActionResult FullPartialBasket()
        {
            return ViewComponent("BasketFull");
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> AddToBasket(int id, int type, CancellationToken cancellationToken)
        {
            try
            {

                await _mediator.Send(new CreateBasketCommand
                {
                    UserId = User.GetUserId(),
                    Session = HttpContext.Session,
                    ItemId = id,
                    ItemType = (BasketItemType)type
                }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> UpdateBasket(Basket basket, CancellationToken cancellationToken)
        {
            try
            {
                basket.UserId = User.GetUserId();
                await _mediator.Send(new EditBasketCommand
                {
                    Session = HttpContext.Session,
                    Basket = basket
                }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }

        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> Remove(int id, int type, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new DeleteBasketCommand
                {
                    UserId = User.GetUserId(),
                    ItemType = (BasketItemType)type,
                    Session = HttpContext.Session,
                    ItemId = id,
                }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }

        }

        [HttpPost]
        public async Task<ActionResult<ReturnedDto>> ClearBasket(CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new ClearBasketCommand
                {
                    UserId = User.GetUserId(),
                    Session = HttpContext.Session,
                }, cancellationToken);
                return Messages.SuccessState;
            }
            catch (WebAppException e)
            {
                return Messages.FailExceptionState(e);
            }

        }

        //[HttpPost]
        //public async Task<ActionResult<ReturnedDto<ValidationVoucherDto>>> ApplayVoucher(string code, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var result = await _mediator.Send(new ValidationVoucherQuery
        //        {
        //            Code = code,
        //        }, cancellationToken);

        //        if (result.Status == 404)
        //        {
        //            return Messages<ValidationVoucherDto>.SuccessState(result);
        //        }
        //        await _mediator.Send(new ApplayVoucherCommand
        //        {
        //            Code = code,
        //            Session = HttpContext.Session,
        //            BasketId = User.GetUserId().ToString()
        //        }, cancellationToken);
        //        return Messages<ValidationVoucherDto>.SuccessState(result);
        //    }
        //    catch (WebAppException e)
        //    {
        //        return Messages<ValidationVoucherDto>.FailExceptionState(e);
        //    }

        //}


    }
}
