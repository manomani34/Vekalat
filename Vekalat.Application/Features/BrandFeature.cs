using Application.Common.Dto.Paging;
using AutoMapper;
using Vekalat.Application.Common;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Common;
using Vekalat.Core.Entities;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.CategoryFeature;

namespace Vekalat.Application.Features
{
    public class BrandFeature
    {
        #region CQRS
        #region Create Brand
        public class CreateBrandCommand : IRequest
        {
            public CreateOrEditBrandDto CreateOrEditBrandDto { get; set; }
        }

        public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand>
        {

            private readonly IBrandRepository _BrandRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateBrandCommandHandler(IFileSaver fileSaver, IBrandRepository BrandRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _BrandRepository = BrandRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    request.CreateOrEditBrandDto.Imagename = "no-image";
                    if (request.CreateOrEditBrandDto.Image != null)
                    {
                        request.CreateOrEditBrandDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditBrandDto.Image, "images/Brand-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditBrandDto.Imagename, "images/Brand-images");
                    }

                    var Brand = _mapper.Map<Brand>(request.CreateOrEditBrandDto);

                    await _BrandRepository.InsertNew(Brand);
                    await _BrandRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit Brand

        public class EditBrandCommand : IRequest
        {
            public CreateOrEditBrandDto CreateOrEditBrandDto { get; set; }

        }

        public class EditBrandCommandHandler : IRequestHandler<EditBrandCommand>
        {
            private readonly IBrandRepository _BrandRepository;
            private readonly IMapper _mapper;
            private readonly IFileSaver _fileSaver;

            public EditBrandCommandHandler(IBrandRepository BrandRepository, IMapper mapper, IFileSaver fileSaver)
            {
                _BrandRepository = BrandRepository;
                _mapper = mapper;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditBrandCommand request, CancellationToken cancellationToken)
            {
                var Brand = await _BrandRepository.GetById(request.CreateOrEditBrandDto.Id.Value);

                if (Brand == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditBrandDto.Image != null)
                    {
                        if (Brand.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(Brand.Imagename, "images/Brand-images");
                        request.CreateOrEditBrandDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditBrandDto.Image, "images/Brand-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditBrandDto.Imagename, "images/Brand-images");
                    }


                    var mappedBrand = _mapper.Map<Brand>(request.CreateOrEditBrandDto);
                    mappedBrand.Imagename = request.CreateOrEditBrandDto.Imagename ?? Brand.Imagename;

                    await _BrandRepository.Update(mappedBrand);
                    await _BrandRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete Brand

        public class DeleteBrandCommand : IRequest
        {
            public int BrandId { get; set; }
        }

        public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
        {
            private readonly IBrandRepository _BrandRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteBrandCommandHandler(IFileSaver fileSaver, IBrandRepository BrandRepository)
            {
                _fileSaver = fileSaver;
                _BrandRepository = BrandRepository;
            }

            public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
            {
                var Brand = await _BrandRepository.GetById(request.BrandId);

                if (Brand == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {

                    await _fileSaver.DeleteImageFromServer(Brand.Imagename, "images/Brand-images");

                    await _BrandRepository.SoftDelete(request.BrandId);
                    await _BrandRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get Brand Detail

        public class GetBrandDetailQuery : IRequest<BrandForViewDto>
        {
            public int BrandId { get; set; }
        }

        public class GetBrandDetailQueryHandler : IRequestHandler<GetBrandDetailQuery, BrandForViewDto>
        {
            private readonly IBrandRepository _BrandRepository;
            private readonly IMapper _mapper;

            public GetBrandDetailQueryHandler(IBrandRepository BrandRepository, IMapper mapper)
            {
                _BrandRepository = BrandRepository;
                _mapper = mapper;
            }

            public async Task<BrandForViewDto> Handle(GetBrandDetailQuery request, CancellationToken cancellationToken)
            {
                var Brand = await _BrandRepository.GetById(request.BrandId);
                if (Brand == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<BrandForViewDto>(Brand);
            }
        }

        #endregion

        #region Get Brand Detail For Edit

        public class GetBrandDetailForEditQuery : IRequest<CreateOrEditBrandDto>
        {
            public int BrandId { get; set; }
        }

        public class GetBrandDetailForEditQueryHandler : IRequestHandler<GetBrandDetailForEditQuery, CreateOrEditBrandDto>
        {
            private readonly IBrandRepository _BrandRepository;
            private readonly IMapper _mapper;
            public GetBrandDetailForEditQueryHandler(IBrandRepository BrandRepository, IMapper mapper)
            {
                _BrandRepository = BrandRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditBrandDto> Handle(GetBrandDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var Brand = await _BrandRepository.GetById(request.BrandId);
                if (Brand == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditBrandDto>(Brand);
            }
        }

        #endregion

        #region Get Brand List
        public class GetBrandListQuery : IRequest<PagingHandler<BrandForViewDto>>
        {
            public BrandFilterInput BrandFilterInput { get; set; }
        }

        public class GetBrandListQueryHandler : IRequestHandler<GetBrandListQuery, PagingHandler<BrandForViewDto>>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IPagerService<BrandForViewDto, Brand> _pager;
            public GetBrandListQueryHandler(IBrandRepository brandRepository, IPagerService<BrandForViewDto, Brand> pager)
            {
                _brandRepository = brandRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<BrandForViewDto>> Handle(GetBrandListQuery request, CancellationToken cancellationToken)
            {
                var Brands = await Task.FromResult(_brandRepository.GetAllWithFilter(request.BrandFilterInput));
                var pager = _pager.PageBuilder(Brands.Count(), request.BrandFilterInput.PageId, request.BrandFilterInput.Take);
                return await _pager.SetItemsMapper(Brands, pager);
            }
        }
        #endregion

        #region Get Brand Select List
        public class GetAllBrandSelectListQuery : IRequest<List<SelectListItem>>
        {

        }
        public class GetAllBrandSelectListQueryHandler : IRequestHandler<GetAllBrandSelectListQuery, List<SelectListItem>>
        {

            private readonly IBrandRepository _brandRepository;

            public GetAllBrandSelectListQueryHandler(IBrandRepository brandRepository)
            {
                _brandRepository = brandRepository;
            }

            public async Task<List<SelectListItem>> Handle(GetAllBrandSelectListQuery request, CancellationToken cancellationToken)
            {
                var result = new List<SelectListItem>();
                var items = await _brandRepository.GetAll();
                var allBrand = items.OrderBy(c => c.Title);
                result.AddRange(allBrand.Select(c => new SelectListItem()
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

        public interface IBrandRepository : IRepository<Brand>
        {
            IQueryable<Brand> GetAllWithFilter(BrandFilterInput BrandFilterInput);
        }

        public class BrandProfile : Profile
        {
            public BrandProfile()
            {
                CreateMap<Brand, CreateOrEditBrandDto>();
                CreateMap<Brand, CreateOrEditBrandDto>().ReverseMap();

                CreateMap<Brand, BrandForViewDto>();
                CreateMap<Brand, BrandForViewDto>().ReverseMap();

            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditBrandDto
        {
            public int? Id { get; set; }
            public string Title { get; set; }
            public IFormFile Image { get; set; }
            public string Imagename { get; set; }
            public bool IsActive { get; set; }

        }

        public class BrandForViewDto : Entity
        {
            public string Title { get; set; }
            public string Imagename { get; set; }
            public bool IsActive { get; set; }

        }

        public class BrandFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
        }


        #endregion
    }
}
