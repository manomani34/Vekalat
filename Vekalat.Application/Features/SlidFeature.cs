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
using System.ComponentModel.DataAnnotations;

namespace Vekalat.Application.Features
{
    public class SlidFeature
    {

        #region CQRS

        public class GetAllQuery : IRequest<ICollection<Slid>>
        {

        }
        public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ICollection<Slid>>
        {

            private readonly ISlidRepository _repository;
            private readonly IMapper _mapper;

            public GetAllQueryHandler(ISlidRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ICollection<Slid>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                var lists = await _repository.GetAll();
                return lists;
            }
        }
        public class GetActiveQuery : IRequest<ICollection<Slid>>
        {

        }
        public class GetActiveQueryHandler : IRequestHandler<GetActiveQuery, ICollection<Slid>>
        {

            private readonly ISlidRepository _repository;
            private readonly IMapper _mapper;

            public GetActiveQueryHandler(ISlidRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ICollection<Slid>> Handle(GetActiveQuery request, CancellationToken cancellationToken)
            {
                var lists = await _repository.GetActive();
                return lists;
            }
        }
        public class GetByIdQuery : IRequest<Slid>
        {
            public int Id { get; set; }
        }
        public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Slid>
        {
            private readonly ISlidRepository _repository;
            private readonly IMapper _mapper;

            public GetByIdQueryHandler(ISlidRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Slid> Handle(GetByIdQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetById(request.Id);
            }
        }

        #region Create slid

        public class CreateCommand : IRequest
        {
            public Slid item { get; set; }
            public IFormFile ImageFile { get; set; }

        }
        public class CreateCommandHandler : IRequestHandler<CreateCommand>
        {
            private readonly ISlidRepository _repository;
            private readonly IFileSaver _fileSaver;

            public CreateCommandHandler(ISlidRepository repository, IFileSaver fileSaver)
            {
                _repository = repository;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(CreateCommand request, CancellationToken cancellationToken)
            {
                if (request.ImageFile != null)
                {
                    request.item.PictureURL = await _fileSaver.SaveImageToServer(request.ImageFile, "SlidPic");
                }
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
            public Slid item { get; set; }
            public IFormFile ImageFile { get; set; }
        }
        public class EditCommandHandler : IRequestHandler<EditCommand>
        {
            private readonly ISlidRepository _repository;
            private readonly IFileSaver _fileSaver;

            public EditCommandHandler(ISlidRepository repository, IFileSaver fileSaver)
            {
                _repository = repository;
                _fileSaver = fileSaver;
            }
            public async Task<Unit> Handle(EditCommand request, CancellationToken cancellationToken)
            {
                if (request.ImageFile != null)
                {
                    await _fileSaver.DeleteImageFromServer(request.item.PictureURL , "SlidPic");
                    request.item.PictureURL = await _fileSaver.SaveImageToServer(request.ImageFile, "SlidPic");
                }
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
            private readonly ISlidRepository _repository;

            public DeleteCommandHandler(ISlidRepository repository)
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

        public interface ISlidRepository : IRepository<Slid>
        {
            Task<List<Slid>> GetActive();

        }

        public class SlidProfile : Profile
        {
            public SlidProfile()
            {
              
            }
        }

        #endregion


        #region Serialization

        public class SlideDto
        {
            public string PictureURL { get; set; }
            public string Title { get; set; }
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
                    .WithMessage("Invalid Slid.");
                RuleFor(e => e.item.Title)
                     .NotEmpty()
                    .WithMessage("Invalid Slid.");
            }
        }
        #endregion
    }
}
