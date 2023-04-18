using Microsoft.AspNetCore.Authentication;
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
using static Vekalat.Application.Features.BlogFeature;
using Vekalat.UI.ViewModels.BlogViewModels;

namespace Vekalat.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator, ILogger<HomeController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(BlogFilterInput filterInput, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetBlogListQuery { BlogFilterInput = filterInput }, cancellationToken);
            return View(new BlogViewModel { Filter = filterInput, PagingHandler = items });
        }


        [Route("contact")]
        public IActionResult Contact()

        {
            ViewBag.Message = "";
            return View();
        }

        [Route("contact")]
        [HttpPost]
        public async Task<IActionResult> Contact(ContactMessageFeature.ContactMessageDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                if (dto.Mobil.Substring(0, 2) != "09")
                    return View(dto);
                await _mediator.Send(new ContactMessageFeature.CreateContactMessageCommand() { ContactMessage = dto }, cancellationToken);
                ViewBag.Message = "پیام شما بدرستی ارسال شد";
                //var dto2 = new ContactMessageFeature.ContactMessageDto { Email = "", Message = "", Name = "" };
                dto.Message = "";
                dto.Mobil = "";
                dto.Email = "";
                dto.Message = "";
                return View(dto);
                //return RedirectToAction("contact");
            }
            catch
            {
                return View(dto);
            }
        }

        [Route("About")]
        public IActionResult About()
        {
            return View();
        }

    }
}
