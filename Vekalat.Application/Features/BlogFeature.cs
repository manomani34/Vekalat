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
using System;
using Vekalat.Core.Common;
using Vekalat.Core.Localization;

namespace Vekalat.Application.Features
{
    public class BlogFeature
    {
        #region CQRS

        #region Create Blog
        public class CreateBlogCommand : IRequest
        {
            public CreateOrEditBlogDto CreateOrEditBlogDto { get; set; }
        }

        public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand>
        {

            private readonly IBlogRepository _blogRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateBlogCommandHandler(IFileSaver fileSaver, IBlogRepository blogRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _blogRepository = blogRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    request.CreateOrEditBlogDto.Imagename = "no-image";
                    if (request.CreateOrEditBlogDto.Image != null)
                    {
                        request.CreateOrEditBlogDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditBlogDto.Image, "images/blog-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditBlogDto.Imagename, "images/blog-images");
                    }

                    var blog = _mapper.Map<Blog>(request.CreateOrEditBlogDto);
                    blog.CreationTime = DateTime.Now;

                    await _blogRepository.InsertNew(blog);
                    await _blogRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit Blog

        public class EditBlogCommand : IRequest
        {
            public CreateOrEditBlogDto CreateOrEditBlogDto { get; set; }

        }

        public class EditBlogCommandHandler : IRequestHandler<EditBlogCommand>
        {
            private readonly IBlogRepository _blogRepository;
            private readonly IMapper _mapper;
            private readonly IFileSaver _fileSaver;

            public EditBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper, IFileSaver fileSaver)
            {
                _blogRepository = blogRepository;
                _mapper = mapper;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditBlogCommand request, CancellationToken cancellationToken)
            {
                var blog = await _blogRepository.GetById(request.CreateOrEditBlogDto.Id.Value);

                if (blog == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditBlogDto.Image != null)
                    {
                        if (blog.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(blog.Imagename, "images/blog-images");
                        request.CreateOrEditBlogDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditBlogDto.Image, "images/blog-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditBlogDto.Imagename, "images/blog-images");
                    }


                    var mappedBlog = _mapper.Map<Blog>(request.CreateOrEditBlogDto);
                    mappedBlog.CreationTime = blog.CreationTime;
                    mappedBlog.LastModifyTime = DateTime.Now;
                    mappedBlog.Imagename = request.CreateOrEditBlogDto.Imagename ?? blog.Imagename;

                    await _blogRepository.Update(mappedBlog);
                    await _blogRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete Blog

        public class DeleteBlogCommand : IRequest
        {
            public int BlogId { get; set; }
        }

        public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand>
        {
            private readonly IBlogRepository _blogRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteBlogCommandHandler(IFileSaver fileSaver, IBlogRepository blogRepository)
            {
                _fileSaver = fileSaver;
                _blogRepository = blogRepository;
            }

            public async Task<Unit> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
            {
                var blog = await _blogRepository.GetById(request.BlogId);

                if (blog == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {

                    await _fileSaver.DeleteImageFromServer(blog.Imagename, "images/blog-images");

                    await _blogRepository.SoftDelete(request.BlogId);
                    await _blogRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get Blog Detail

        public class GetBlogDetailQuery : IRequest<BlogForViewDto>
        {
            public int BlogId { get; set; }
        }

        public class GetBlogDetailQueryHandler : IRequestHandler<GetBlogDetailQuery, BlogForViewDto>
        {
            private readonly IBlogRepository _blogRepository;
            private readonly IMapper _mapper;

            public GetBlogDetailQueryHandler(IBlogRepository blogRepository, IMapper mapper)
            {
                _blogRepository = blogRepository;
                _mapper = mapper;
            }

            public async Task<BlogForViewDto> Handle(GetBlogDetailQuery request, CancellationToken cancellationToken)
            {
                var blog = await _blogRepository.GetById(request.BlogId);
                if (blog == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<BlogForViewDto>(blog);
            }
        }

        #endregion

        #region Get Blog Detail For Edit

        public class GetBlogDetailForEditQuery : IRequest<CreateOrEditBlogDto>
        {
            public int BlogId { get; set; }
        }

        public class GetBlogDetailForEditQueryHandler : IRequestHandler<GetBlogDetailForEditQuery, CreateOrEditBlogDto>
        {
            private readonly IBlogRepository _blogRepository;
            private readonly IMapper _mapper;
            public GetBlogDetailForEditQueryHandler(IBlogRepository blogRepository, IMapper mapper)
            {
                _blogRepository = blogRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditBlogDto> Handle(GetBlogDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var blog = await _blogRepository.GetById(request.BlogId);
                if (blog == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditBlogDto>(blog);
            }
        }

        #endregion

        #region Get Blog List
        public class GetBlogListQuery : IRequest<PagingHandler<BlogForViewDto>>
        {
            public BlogFilterInput BlogFilterInput { get; set; }
        }

        public class GetBlogListQueryHandler : IRequestHandler<GetBlogListQuery, PagingHandler<BlogForViewDto>>
        {
            public IBlogRepository _BlogRepository { get; set; }
            private readonly IPagerService<BlogForViewDto, Blog> _pager;
            public GetBlogListQueryHandler(IBlogRepository blogRepository, IPagerService<BlogForViewDto, Blog> pager)
            {
                _BlogRepository = blogRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<BlogForViewDto>> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
            {
                request.BlogFilterInput.Take = 10;
                var blogs = await Task.FromResult(_BlogRepository.GetAllWithFilter(request.BlogFilterInput));
                var pager = _pager.PageBuilder(blogs.Count(), request.BlogFilterInput.PageId, request.BlogFilterInput.Take);
                return await _pager.SetItemsMapper(blogs, pager);
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface IBlogRepository : IRepository<Blog>
        {
            IQueryable<Blog> GetAllWithFilter(BlogFilterInput blogFilterInput);
        }

        public class BlogProfile : Profile
        {
            public BlogProfile()
            {
                CreateMap<Blog, CreateOrEditBlogDto>();
                CreateMap<Blog, CreateOrEditBlogDto>().ReverseMap();

                CreateMap<Blog, BlogForViewDto>()
                    .ForMember(opt => opt.BlogSubject, des => des.MapFrom(c => c.BlogSubjectFk.Title));
            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditBlogDto
        {
            public int? Id { get; set; }
            public string Title { get; set; }
            public IFormFile Image { get; set; }
            public string Imagename { get; set; }
            public string Tag { get; set; }
            public string Description { get; set; }
            public int? LikeCount { get; set; }
            public int? ViewCount { get; set; }
            public string Abstract { get; set; }
            public bool IsVisible { get; set; }
            public int BlogSubjectId { get; set; }
            public int UserId { get; set; }

        }

        public class BlogForViewDto : Entity
        {
            public string Title { get; set; }
            public string Imagename { get; set; }
            public string Tag { get; set; }
            public string Description { get; set; }
            public int? LikeCount { get; set; }
            public int? ViewCount { get; set; }
            public string Abstract { get; set; }
            public bool IsVisible { get; set; }
            public string BlogSubject { get; set; }
            public DateTime? CreationTime { get; set; }

        }

        public class BlogFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
        }


        #endregion

    }
}
