using Vekalat.Application.Common.Helpers;
using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Vekalat.Application.Common.InfraServices;
using System.ComponentModel.DataAnnotations;

namespace Vekalat.Application.Features
{
    public class ContactMessageFeature
    {

        #region CQRS

        

        #region Get Ketabdary Messages For Admin

        //public class GetKetabdaryMessagesForAdminQuery : IRequest<PagingHandler<ContactMessage>>
        //{
        //    public int PageId { get; set; }
        //    public int MessageStatus { get; set; }
        //    public int MessageType { get; set; }
        //    public string Sender { get; set; }
        //    public string StartSendDate { get; set; }
        //    public string EndSendDate { get; set; }
        //    public string StartAnswerDate { get; set; }
        //    public string EndAnswerDate { get; set; }
        //    public int sortOrder { get; set; }
          

        //}
        //public class GetKetabdaryMessagesForAdminQueryHandler : IRequestHandler<GetKetabdaryMessagesForAdminQuery, PagingHandler<ContactMessage>>
        //{

        //    private readonly IContactMessageRepository _repository;
        //    private readonly IPagerService<ContactMessage, ContactMessage> _pager;

        //    public GetKetabdaryMessagesForAdminQueryHandler(IContactMessageRepository repository, IPagerService<ContactMessage, ContactMessage> pager)
        //    {
        //        _repository = repository;
        //        _pager = pager;
        //    }

        //    public async Task<PagingHandler<ContactMessage>> Handle(GetKetabdaryMessagesForAdminQuery request, CancellationToken cancellationToken)
        //    {
        //        const int take = 10;

        //        var item = await Task.FromResult(_repository.GetKetabdaryMessagesForAdmin(request.Sender, request.MessageType, request.MessageStatus, request.StartSendDate,
        //            request.EndSendDate, request.StartAnswerDate, request.EndAnswerDate, request.sortOrder));

        //        var pager = _pager.PageBuilder(item.Count(), request.PageId, take);
        //        return await _pager.SetItems(item, pager);
        //    }
        //}

        #endregion

        #region Create Response For Message 

        public class CreateResponseMessageCommand : IRequest
        {
            public ResponseMessageDto ResponseMessageDto { get; set; }
        }
        public class CreateResponseMessageCommandHandler : IRequestHandler<CreateResponseMessageCommand>
        {
            private readonly IContactMessageRepository _repository;
            private readonly IRenderViewToString _renderView;
            public CreateResponseMessageCommandHandler(IContactMessageRepository repository, IRenderViewToString renderView)
            {
                _repository = repository;
                _renderView = renderView;
            }

            public async Task<Unit> Handle(CreateResponseMessageCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var message = await _repository.GetById(request.ResponseMessageDto.Id);
                    message.IsRespone = true;
                    message.respone = request.ResponseMessageDto.Response;
                                    //message.Rdate = DateTime.Now.ToShamsi();
                    message.ResponeUser = "";

                    #region By Email

                    //var bodyEmail = await _renderView.RenderToStringAsync("_AuthorMessageEmail", request.ResponseMessageDto.Response);
                    //await _apiManager.SendEmailApi(new SendEmailInput()
                    //{
                    //    EmailAddress = message.Email ,
                    //    IsHtml = true,
                    //    Subject = "پاسخ از سایت اهل قلم",
                    //    Message = bodyEmail
                    //});

                    #endregion

                    await _repository.Update(message);
                    await _repository.SaveChangesAsync();
                    return Unit.Value;
                }
                catch(Exception e)
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }

            }
        }

        #endregion


        #region Edit ContactMessage

        public class EditContactMessageCommand : IRequest
        {
            public ContactMessage ContactMessage { get; set; }
        }
        public class EditContactMessageCommandHandler : IRequestHandler<EditContactMessageCommand>
        {

            private readonly IContactMessageRepository _repository;

            public EditContactMessageCommandHandler(IContactMessageRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(EditContactMessageCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _repository.Update(request.ContactMessage);
                    await _repository.SaveChangesAsync();
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }
            }
        }

        #endregion

        #region Delete 

        public class DeleteContactMessageCommand : IRequest
        {
            public int Id { get; set; }
        }

        public class DeleteContactMessageCommandHandler : IRequestHandler<DeleteContactMessageCommand>
        {
            private readonly IContactMessageRepository _repository;
            public DeleteContactMessageCommandHandler(IContactMessageRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteContactMessageCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    await _repository.Delete(request.Id);
                    await _repository.SaveChangesAsync();
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Error", "خطای ارتباط با سرور"));
                }

            }
        }

        #endregion

        #region Get ContactMessage

        public class GetContactMessageQuery : IRequest<ContactMessage>
        {
            public int ContactMessageId { get; set; }
        }
        public class GetContactMessageQueryHandler : IRequestHandler<GetContactMessageQuery, ContactMessage>
        {

            private readonly IContactMessageRepository _repository;

            public GetContactMessageQueryHandler(IContactMessageRepository repository)
            {
                _repository = repository;
            }

            public async Task<ContactMessage> Handle(GetContactMessageQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetById(request.ContactMessageId);
            }
        }

        #endregion

        #region Get All 

        public class GetAllQuery : IRequest<ICollection<ContactMessage>>
        {

        }
        public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ICollection<ContactMessage>>
        {

            private readonly IContactMessageRepository _repository;
            private readonly IMapper _mapper;

            public GetAllQueryHandler(IContactMessageRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ICollection<ContactMessage>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                var lists = await _repository.GetAll();
                return lists;
            }
        }

        #endregion

        #region Create ContactMessage

        public class CreateContactMessageCommand : IRequest
        {
            public ContactMessageDto ContactMessage { get; set; }
        }
        public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand>
        {
            private readonly IContactMessageRepository _repository;

            public CreateContactMessageCommandHandler(IContactMessageRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    var Item = new ContactMessage()
                    {
                        Name = request.ContactMessage.Name,
                        Email = request.ContactMessage.Email,
                        Message = request.ContactMessage.Message,
                        IsRespone = false,
                        //Fdate = DateTime.Now.ToShamsi(),
                        respone = "",
                        ResponeUser = "",
                        Mobil = request.ContactMessage.Mobil,
                        Rdate = "",
                    };
                    await _repository.InsertNew(Item);
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }

            }
        }

        #endregion


        #endregion


        #region Mapper And Repo

        public interface IContactMessageRepository : IRepository<ContactMessage>
        {
            IQueryable<ContactMessage> GetKetabdaryMessagesForAdmin(string sender, int messageType, int messageStatusType, string startSendDate, string endSendDate,
                string startAnswerDate, string endAnswerDate,int sortOrder);
                                   

        }

        public class ContactMessageProfile : Profile
        {

        }

        #endregion


        #region Serialization

        public class ContactMessageDto
        {
            [Required(ErrorMessage = "نام را وارد نمایید")]
            public string Name { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
            public string Mobil { get; set; }
        }

        public class ResponseMessageDto
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Response { get; set; }
            public string Note { get; set; }
            public int Page { get; set; }
        }


        public class SendMessageDto
        {
            public int MessageType { get; set; }
            public string Message { get; set; }
        }


        #endregion

        #region PipelineBehaviors

        public class ResponseCommandCommandValidator : AbstractValidator<ResponseMessageDto>
        {
            public ResponseCommandCommandValidator()
            {
                RuleFor(x => x.Response).NotEmpty().WithMessage("وارد کردن این فیلد الزامی است");
            }
        }

        #endregion

    }
}
