using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Entities;
using Application.Common.Dto.Paging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Vekalat.Core.Common;
using Vekalat.Core.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Vekalat.Application.Features.CategoryFeature;

namespace Vekalat.Application.Features
{
    public class BlogSubjectFeature
    {
        #region CQRS

        #region Create BlogSubject
        public class CreateBlogSubjectCommand : IRequest
        {
            public CreateOrEditBlogSubjectDto CreateOrEditBlogSubjectDto { get; set; }
        }

        public class CreateBlogSubjectCommandHandler : IRequestHandler<CreateBlogSubjectCommand>
        {

            private readonly IBlogSubjectRepository _blogSubjectRepository;
            private readonly IMapper _mapper;
            public CreateBlogSubjectCommandHandler( IBlogSubjectRepository blogSubjectRepository, IMapper mapper)
            {
                _blogSubjectRepository = blogSubjectRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateBlogSubjectCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    
                    var blogSubject = _mapper.Map<BlogSubject>(request.CreateOrEditBlogSubjectDto);
                    await _blogSubjectRepository.InsertNew(blogSubject);
                    await _blogSubjectRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit BlogSubject

        public class EditBlogSubjectCommand : IRequest
        {
            public CreateOrEditBlogSubjectDto CreateOrEditBlogSubjectDto { get; set; }

        }

        public class EditBlogSubjectCommandHandler : IRequestHandler<EditBlogSubjectCommand>
        {
            private readonly IBlogSubjectRepository _blogSubjectRepository;
            private readonly IMapper _mapper;

            public EditBlogSubjectCommandHandler(IBlogSubjectRepository blogSubjectRepository, IMapper mapper)
            {
                _blogSubjectRepository = blogSubjectRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(EditBlogSubjectCommand request, CancellationToken cancellationToken)
            {
                var blogSubject = await _blogSubjectRepository.GetById(request.CreateOrEditBlogSubjectDto.Id.Value);

                if (blogSubject == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                   
                    var mappedBlogSubject = _mapper.Map<BlogSubject>(request.CreateOrEditBlogSubjectDto);
                   
                    await _blogSubjectRepository.Update(mappedBlogSubject);
                    await _blogSubjectRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete BlogSubject

        public class DeleteBlogSubjectCommand : IRequest
        {
            public int BlogSubjectId { get; set; }
        }

        public class DeleteBlogSubjectCommandHandler : IRequestHandler<DeleteBlogSubjectCommand>
        {
            private readonly IBlogSubjectRepository _blogSubjectRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteBlogSubjectCommandHandler(IFileSaver fileSaver, IBlogSubjectRepository blogSubjectRepository)
            {
                _fileSaver = fileSaver;
                _blogSubjectRepository = blogSubjectRepository;
            }

            public async Task<Unit> Handle(DeleteBlogSubjectCommand request, CancellationToken cancellationToken)
            {
                var blogSubject = await _blogSubjectRepository.GetById(request.BlogSubjectId);

                if (blogSubject == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {


                    await _blogSubjectRepository.SoftDelete(request.BlogSubjectId);
                    await _blogSubjectRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get BlogSubject Detail

        public class GetBlogSubjectDetailQuery : IRequest<BlogSubjectForViewDto>
        {
            public int BlogSubjectId { get; set; }
        }

        public class GetBlogSubjectDetailQueryHandler : IRequestHandler<GetBlogSubjectDetailQuery, BlogSubjectForViewDto>
        {
            private readonly IBlogSubjectRepository _blogSubjectRepository;
            private readonly IMapper _mapper;

            public GetBlogSubjectDetailQueryHandler(IBlogSubjectRepository blogSubjectRepository, IMapper mapper)
            {
                _blogSubjectRepository = blogSubjectRepository;
                _mapper = mapper;
            }

            public async Task<BlogSubjectForViewDto> Handle(GetBlogSubjectDetailQuery request, CancellationToken cancellationToken)
            {
                var blogSubject = await _blogSubjectRepository.GetById(request.BlogSubjectId);
                if (blogSubject == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<BlogSubjectForViewDto>(blogSubject);
            }
        }

        #endregion

        #region Get BlogSubject Detail For Edit

        public class GetBlogSubjectDetailForEditQuery : IRequest<CreateOrEditBlogSubjectDto>
        {
            public int BlogSubjectId { get; set; }
        }

        public class GetBlogSubjectDetailForEditQueryHandler : IRequestHandler<GetBlogSubjectDetailForEditQuery, CreateOrEditBlogSubjectDto>
        {
            private readonly IBlogSubjectRepository _blogSubjectRepository;
            private readonly IMapper _mapper;
            public GetBlogSubjectDetailForEditQueryHandler(IBlogSubjectRepository blogSubjectRepository, IMapper mapper)
            {
                _blogSubjectRepository = blogSubjectRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditBlogSubjectDto> Handle(GetBlogSubjectDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var blogSubject = await _blogSubjectRepository.GetById(request.BlogSubjectId);
                if (blogSubject == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditBlogSubjectDto>(blogSubject);
            }
        }

        #endregion

        #region Get BlogSubject List
        public class GetBlogSubjectListQuery : IRequest<PagingHandler<BlogSubjectForViewDto>>
        {
            public BlogSubjectFilterInput BlogSubjectFilterInput { get; set; }
        }

        public class GetBlogSubjectListQueryHandler : IRequestHandler<GetBlogSubjectListQuery, PagingHandler<BlogSubjectForViewDto>>
        {
            public IBlogSubjectRepository _BlogSubjectRepository { get; set; }
            private readonly IPagerService<BlogSubjectForViewDto, BlogSubject> _pager;
            public GetBlogSubjectListQueryHandler(IBlogSubjectRepository blogSubjectRepository, IPagerService<BlogSubjectForViewDto, BlogSubject> pager)
            {
                _BlogSubjectRepository = blogSubjectRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<BlogSubjectForViewDto>> Handle(GetBlogSubjectListQuery request, CancellationToken cancellationToken)
            {
                request.BlogSubjectFilterInput.Take = 10;
                var blogSubjects = await Task.FromResult(_BlogSubjectRepository.GetAllWithFilter(request.BlogSubjectFilterInput));
                var pager = _pager.PageBuilder(blogSubjects.Count(), request.BlogSubjectFilterInput.PageId, request.BlogSubjectFilterInput.Take);
                return await _pager.SetItemsMapper(blogSubjects, pager);
            }
        }
        #endregion

        #region Get BlogSubject Select List
        public class GetAllBlogSubjectSelectListQuery : IRequest<List<SelectListItem>>
        {

        }
        public class GetAllBlogSubjectSelectListQueryHandler : IRequestHandler<GetAllBlogSubjectSelectListQuery, List<SelectListItem>>
        {

            private readonly IBlogSubjectRepository _blogSubjectRepository;

            public GetAllBlogSubjectSelectListQueryHandler(IBlogSubjectRepository blogSubjectRepository)
            {
                _blogSubjectRepository = blogSubjectRepository;
            }

            public async Task<List<SelectListItem>> Handle(GetAllBlogSubjectSelectListQuery request, CancellationToken cancellationToken)
            {
                var result = new List<SelectListItem>();
                var items = await _blogSubjectRepository.GetAll();
                var allBlogSubject = items.OrderBy(c => c.Title);
                result.AddRange(allBlogSubject.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                }));
                return result;
            }
        }
        #endregion


        #endregion

        #region Mapper And Repo

        public interface IBlogSubjectRepository : IRepository<BlogSubject>
        {
            IQueryable<BlogSubject> GetAllWithFilter(BlogSubjectFilterInput blogSubjectFilterInput);
        }

        public class BlogSubjectProfile : Profile
        {
            public BlogSubjectProfile()
            {
                CreateMap<BlogSubject, CreateOrEditBlogSubjectDto>();
                CreateMap<BlogSubject, CreateOrEditBlogSubjectDto>().ReverseMap();

                CreateMap<BlogSubject, BlogSubjectForViewDto>();
                CreateMap<BlogSubject, BlogSubjectForViewDto>().ReverseMap();
            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditBlogSubjectDto
        {
            public int? Id { get; set; }
            public string Title { get; set; }

        }

        public class BlogSubjectForViewDto : Entity
        {
            public string Title { get; set; }

        }

        public class BlogSubjectFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
        }


        #endregion

    }
}
