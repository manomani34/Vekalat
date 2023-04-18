using AutoMapper;
using FluentValidation;
using Vekalat.Application.Common;
using Vekalat.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;
using Vekalat.Core.Errors;
using Vekalat.Application.Common.InfraServices;

namespace Vekalat.Application.Features
{
    public class LinkFeature
    {

        #region CQRS

        public class GetAllQuery : IRequest<ICollection<Link>>
        {

        }
        public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ICollection<Link>>
        {

            private readonly ILinkRepository _repository;
            private readonly IMapper _mapper;

            public GetAllQueryHandler(ILinkRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ICollection<Link>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                var lists = await _repository.GetAll();
                return lists;
            }
        }
        public class GetActiveQuery : IRequest<ICollection<Link>>
        {

        }
        public class GetActiveQueryHandler : IRequestHandler<GetActiveQuery, ICollection<Link>>
        {

            private readonly ILinkRepository _repository;
            private readonly IMapper _mapper;

            public GetActiveQueryHandler(ILinkRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ICollection<Link>> Handle(GetActiveQuery request, CancellationToken cancellationToken)
            {
                var lists = await _repository.GetActive();
                return lists;
            }
        }
        public class GetByIdQuery : IRequest<Link>
        {
            public int Id { get; set; }
        }
        public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Link>
        {
            private readonly ILinkRepository _repository;
            private readonly IMapper _mapper;

            public GetByIdQueryHandler(ILinkRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Link> Handle(GetByIdQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetById(request.Id);
            }
        }

        #region Create Link

        public class CreateCommand : IRequest
        {
            public Link item { get; set; }
            public IFormFile ImageFile { get; set; }

        }
        public class CreateCommandHandler : IRequestHandler<CreateCommand>
        {
            private readonly ILinkRepository _repository;
            private readonly IFileSaver _fileSaver;

            public CreateCommandHandler(ILinkRepository repository, IFileSaver fileSaver)
            {
                _repository = repository;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(CreateCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _repository.InsertNew(request.item);
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

        #region Edit 

        public class EditCommand : IRequest
        {
            public Link item { get; set; }
            public IFormFile ImageFile { get; set; }
        }
        public class EditCommandHandler : IRequestHandler<EditCommand>
        {
            private readonly ILinkRepository _repository;
            private readonly IFileSaver _fileSaver;

            public EditCommandHandler(ILinkRepository repository, IFileSaver fileSaver)
            {
                _repository = repository;
                _fileSaver = fileSaver;
            }
            public async Task<Unit> Handle(EditCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _repository.Update(request.item);
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


        public class DeleteCommand : IRequest
        {
            public int Id { get; set; }
        }

        public class DeleteCommandHandler : IRequestHandler<DeleteCommand>
        {
            private readonly ILinkRepository _repository;

            public DeleteCommandHandler(ILinkRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
            {

                var item = await _repository.GetById(request.Id);
                if (item == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>("Not Found", "یافت نشد"));
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


        #region Mapper And Repo

        public interface ILinkRepository : IRepository<Link>
        {
            Task<List<Link>> GetActive();

        }

        public class LinkProfile : Profile
        {
            public LinkProfile()
            {
              
            }
        }

        #endregion


        #region Serialization

        public class LinkDto
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public int Order { get; set; }
            public bool IsActive { get; set; } = false;
        }

        #endregion


        #region PipelineBehaviors

        public class CreateCommandValidator : AbstractValidator<CreateCommand>
        {
            public CreateCommandValidator()
            {
                RuleFor(x => x.item.Title)
                    .NotEmpty()
                    .WithMessage("Invalid Link.");
                RuleFor(e => e.item.Title)
                     .NotEmpty()
                    .WithMessage("Invalid Link.");
            }
        }
        #endregion
    }
}
