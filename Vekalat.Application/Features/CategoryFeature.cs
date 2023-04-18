using Application.Common.Dto.Paging;
using AutoMapper;
using Vekalat.Core.Entities;
using Vekalat.Application.Common;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Vekalat.Application.Features
{
    public class CategoryFeature
    {
        #region CQRS

        #region Create Category
        public class CreateCategoryCommand : IRequest
        {
            public CreateOrEditCategoryDto CreateOrEditCategoryDto { get; set; }
        }

        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
        {

            private readonly ICategoryRepository _CategoryRepository;
            private readonly IMapper _mapper;
            public CreateCategoryCommandHandler(ICategoryRepository CategoryRepository, IMapper mapper)
            {
                _CategoryRepository = CategoryRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {

                try
                {

                    var Category = _mapper.Map<Category>(request.CreateOrEditCategoryDto);

                    await _CategoryRepository.InsertNew(Category);
                    await _CategoryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit Category

        public class EditCategoryCommand : IRequest
        {
            public CreateOrEditCategoryDto CreateOrEditCategoryDto { get; set; }

        }

        public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand>
        {
            private readonly ICategoryRepository _CategoryRepository;
            private readonly IMapper _mapper;
            public EditCategoryCommandHandler(ICategoryRepository CategoryRepository, IMapper mapper)
            {
                _CategoryRepository = CategoryRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
            {

                try
                {

                    var Category = _mapper.Map<Category>(request.CreateOrEditCategoryDto);
                    await _CategoryRepository.Update(Category);
                    await _CategoryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete Category

        public class DeleteCategoryCommand : IRequest
        {
            public int CategoryId { get; set; }
        }

        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
        {
            private readonly ICategoryRepository _CategoryRepository;
            public DeleteCategoryCommandHandler(ICategoryRepository CategoryRepository)
            {
                _CategoryRepository = CategoryRepository;
            }

            public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var subCategories = _CategoryRepository.GetAllWithFilter(new CategoryFilterInput { ParentId = request.CategoryId }).ToList();

                    await _CategoryRepository.SoftDeleteRang(subCategories);
                    await _CategoryRepository.SoftDelete(request.CategoryId);
                    await _CategoryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get Category Detail

        public class GetCategoryDetailQuery : IRequest<Category>
        {
            public int CategoryId { get; set; }
        }

        public class GetCategoryDetailQueryHandler : IRequestHandler<GetCategoryDetailQuery, Category>
        {
            private readonly ICategoryRepository _CategoryRepository;
            public GetCategoryDetailQueryHandler(ICategoryRepository CategoryRepository)
            {
                _CategoryRepository = CategoryRepository;
            }

            public async Task<Category> Handle(GetCategoryDetailQuery request, CancellationToken cancellationToken)
            {
                var Category = await _CategoryRepository.GetById(request.CategoryId);
                if (Category == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return Category;
            }
        }

        #endregion

        #region Get Category Detail For Edit

        public class GetCategoryDetailForEditQuery : IRequest<CreateOrEditCategoryDto>
        {
            public int CategoryId { get; set; }
        }

        public class GetCategoryDetailForEditQueryHandler : IRequestHandler<GetCategoryDetailForEditQuery, CreateOrEditCategoryDto>
        {
            private readonly ICategoryRepository _CategoryRepository;
            private readonly IMapper _mapper;
            public GetCategoryDetailForEditQueryHandler(ICategoryRepository CategoryRepository, IMapper mapper)
            {
                _CategoryRepository = CategoryRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditCategoryDto> Handle(GetCategoryDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var Category = await _CategoryRepository.GetById(request.CategoryId);
                if (Category == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditCategoryDto>(Category);
            }
        }

        #endregion

        #region Get Category List

        public class GetCategoryListQuery : IRequest<PagingHandler<CategoryForViewDto>>
        {
            public CategoryFilterInput CategoryFilterInput { get; set; }
        }

        public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, PagingHandler<CategoryForViewDto>>
        {
            private readonly ICategoryRepository _CategoryRepository;
            private readonly IPagerService<CategoryForViewDto, Category> _pager;
            public GetCategoryListQueryHandler(ICategoryRepository CategoryRepository, IPagerService<CategoryForViewDto, Category> pager)
            {
                _CategoryRepository = CategoryRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<CategoryForViewDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
            {
                var Categorys = await Task.FromResult(_CategoryRepository.GetAllWithFilter(request.CategoryFilterInput));
                var pager = _pager.PageBuilder(Categorys.Count(), request.CategoryFilterInput.PageId, request.CategoryFilterInput.Take);
                return await _pager.SetItemsMapper(Categorys, pager);
            }
        }
        #endregion

        #region Get Categories Select List
        public class GetAllCategoriesSelectListQuery : IRequest<List<SelectListItem>>
        {

        }
        public class GetAllCategoriesSelectListQueryHandler : IRequestHandler<GetAllCategoriesSelectListQuery, List<SelectListItem>>
        {

            private readonly ICategoryRepository _CategoryRepository;

            public GetAllCategoriesSelectListQueryHandler(ICategoryRepository CategoryRepository)
            {
                _CategoryRepository = CategoryRepository;
            }

            public async Task<List<SelectListItem>> Handle(GetAllCategoriesSelectListQuery request, CancellationToken cancellationToken)
            {
                var result = new List<SelectListItem>();
                var items = await _CategoryRepository.GetAll();
                var allCategories = items.OrderBy(c => c.Title);
                result.AddRange(allCategories.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                }));
                return result;
            }
        }
        #endregion

        //#region Get Selected  Category List
        //public class GetelectedCategoryListQuery : IRequest<List<SelectedCategoryDto>>
        //{
        //    public int Id { get; set; }
        //}

        //public class GetelectedCategoryListQueryHandler : IRequestHandler<GetelectedCategoryListQuery, List<SelectedCategoryDto>>
        //{
        //    private readonly ICategoryRepository _CategoryRepository;
        //    private readonly IMapper _mapper;

        //    public GetelectedCategoryListQueryHandler(ICategoryRepository CategoryRepository, IMapper mapper)
        //    {
        //        _CategoryRepository = CategoryRepository;
        //        _mapper = mapper;
        //    }
        //    public async Task<List<SelectedCategoryDto>> Handle(GetelectedCategoryListQuery request, CancellationToken cancellationToken)
        //    {
        //        var items = await _CategoryRepository.GetSelectedCategory(request.Id);
        //        return _mapper.Map<List<SelectedCategoryDto>>(items);
        //    }
        //}
        //#endregion

        #endregion

        #region Mapper And Repo

        public interface ICategoryRepository : IRepository<Category>
        {
            //Task<List<SelectedCategory>> GetSelectedCategory(int Id);
            IQueryable<Category> GetAllWithFilter(CategoryFilterInput FilterInput);

        }

        public class CategoryProfile : Profile
        {
            public CategoryProfile()
            {
                CreateMap<Category, CreateOrEditCategoryDto>();
                CreateMap<Category, CreateOrEditCategoryDto>().ReverseMap();

                CreateMap<Category, CategoryForViewDto>();
                CreateMap<Category, CategoryForViewDto>().ReverseMap();

                //CreateMap<SelectedCategory, SelectedCategoryDto>();
                //CreateMap<SelectedCategory, SelectedCategoryDto>().ReverseMap();
            }
        }

        #endregion


        #region Serialization


        public class CreateOrEditCategoryDto : Entity
        {
            public string Title { get; set; } = string.Empty;
            public int? ParentId { get; set; }
        }

        public class CategoryForViewDto : Entity
        {
            public string Title { get; set; } = string.Empty;
            public int? ParentId { get; set; }
            public List<CategoryForViewDto> Categories { get; set; }

        }

        public class SelectedCategoryDto : Entity
        {
            public int Id { get; set; }
            public int CategoryId { get; set; }
        }
        public class CategoryFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int? ParentId { get; set; }

        }

        #endregion



    }


}
