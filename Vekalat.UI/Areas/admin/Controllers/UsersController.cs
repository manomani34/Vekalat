using Vekalat.UI.Areas.admin.ViewModels.UserViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.UserFeature;

namespace Vekalat.UI.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(UserFilterInput filterInput, CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetUserListQuery() { UserFilterInput = filterInput }, cancellationToken);
            return View(new UserViewModel { PagingHandler = users, Filter = filterInput });
        }

        public async Task<IActionResult> IndexPartial(UserFilterInput filterInput, CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetUserListQuery() { UserFilterInput = filterInput }, cancellationToken);
            return PartialView(new UserViewModel { PagingHandler = users, Filter = filterInput });
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditUserDto userDto, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Messages.InvalidState;
        //    }
        //    try
        //    {
        //        await _mediator.Send(new CreateUserCommand { CreateOrEditUserDto = userDto }, cancellationToken);
        //        return Messages.SuccessState;
        //    }
        //    catch (WebAppException e)
        //    {
        //        return Messages.FailExceptionState(e);
        //    }
        //}


        //public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        //{
        //    var user = await _mediator.Send(new GetUserDetailForEditQuery { UserId = id }, cancellationToken);
        //    return View(user);
        //}

        //[HttpPut]
        //public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditUserDto userDto, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Messages.InvalidState;
        //    }
        //    try
        //    {
        //        await _mediator.Send(new EditUserCommand { CreateOrEditUserDto = userDto }, cancellationToken);
        //        return Messages.SuccessState;
        //    }
        //    catch (WebAppException e)
        //    {
        //        return Messages.FailExceptionState(e);
        //    }
        //}

        //[HttpDelete]
        //public async Task<ActionResult<ReturnedDto>> Delete(int id, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var result = await _mediator.Send(new DeleteUserCommand { UserId = id }, cancellationToken);
        //        return Messages.SuccessState;
        //    }
        //    catch (WebAppException e)
        //    {
        //        return Messages.FailExceptionState(e);
        //    }
        //}

    }
}
