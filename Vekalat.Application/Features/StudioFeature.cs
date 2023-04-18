using Application.Common.Dto.Paging;
using AutoMapper;
using Vekalat.Application.Common;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Application.Enums;
using Vekalat.Core.Common;
using Vekalat.Core.Entities;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.StudioGalleryFeature;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Vekalat.Application.Features
{
    public class StudioFeature
    {
        #region CQRS

        #region Create Studio
        public class CreateStudioCommand : IRequest
        {
            public CreateOrEditStudioDto CreateOrEditStudioDto { get; set; }
        }

        public class CreateStudioCommandHandler : IRequestHandler<CreateStudioCommand>
        {

            private readonly IStudioRepository _studioRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateStudioCommandHandler(IFileSaver fileSaver, IStudioRepository studioRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _studioRepository = studioRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateStudioCommand request, CancellationToken cancellationToken)
            {

                try
                {

                    request.CreateOrEditStudioDto.Imagename = "no-image";
                    if (request.CreateOrEditStudioDto.Image != null)
                    {
                        request.CreateOrEditStudioDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditStudioDto.Image, "images/studio-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditStudioDto.Imagename, "images/studio-images");
                    }

                    var studio = _mapper.Map<Studio>(request.CreateOrEditStudioDto);
                    studio.Features = JsonSerializer.Serialize(request.CreateOrEditStudioDto.FeatureList);
                    studio.CreationTime = DateTime.Now;

                    await _studioRepository.InsertNew(studio);
                    await _studioRepository.SaveChangesAsync();

                    //await _studioRepository.InsertSelectedCategories(request.CreateOrEditStudioDto.Categories, studio.Id, cancellationToken);
                    //await _studioRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit Studio

        public class EditStudioCommand : IRequest
        {
            public CreateOrEditStudioDto CreateOrEditStudioDto { get; set; }

        }

        public class EditStudioCommandHandler : IRequestHandler<EditStudioCommand>
        {
            private readonly IStudioRepository _studioRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public EditStudioCommandHandler(IFileSaver fileSaver, IStudioRepository studioRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _studioRepository = studioRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(EditStudioCommand request, CancellationToken cancellationToken)
            {
                var studio = await _studioRepository.GetById(request.CreateOrEditStudioDto.Id.Value);

                if (studio == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditStudioDto.Image != null)
                    {
                        if (studio.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(studio.Imagename, "images/studio-images");
                        request.CreateOrEditStudioDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditStudioDto.Image, "images/studio-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditStudioDto.Imagename, "images/studio-images");
                    }


                    var mappedStudio = _mapper.Map<Studio>(request.CreateOrEditStudioDto);
                    mappedStudio.CreationTime = studio.CreationTime;
                    mappedStudio.LastModifyTime = DateTime.Now;
                    mappedStudio.Features = JsonSerializer.Serialize(request.CreateOrEditStudioDto.FeatureList);

                    //mappedStudio.DiscountId = studio.DiscountId;
                    mappedStudio.Imagename = request.CreateOrEditStudioDto.Imagename ?? studio.Imagename;


                    await _studioRepository.Update(mappedStudio);

                    //await _studioRepository.DeleteSelectedCategories(studio.Id, cancellationToken);
                    //await _studioRepository.InsertSelectedCategories(request.CreateOrEditStudioDto.Categories, studio.Id, cancellationToken);
                    await _studioRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        //#region Add Studio Discount

        //public class AddStudioDiscountCommand : IRequest
        //{
        //    public CreateOrEditStudioDiscountDto createOrEditStudioDiscountDto { get; set; }

        //}

        //public class AddStudioDiscountCommandHandler : IRequestHandler<AddStudioDiscountCommand>
        //{
        //    private readonly IStudioRepository _studioRepository;
        //    private readonly IDiscountRepository _discountRepository;

        //    public AddStudioDiscountCommandHandler(IStudioRepository studioRepository, IDiscountRepository discountRepository)
        //    {
        //        _studioRepository = studioRepository;
        //        _discountRepository = discountRepository;
        //    }

        //    public async Task<Unit> Handle(AddStudioDiscountCommand request, CancellationToken cancellationToken)
        //    {
        //        var studio = await _studioRepository.GetById(request.createOrEditStudioDiscountDto.StudioId);
        //        if (studio == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

        //        var discount = await _discountRepository.GetById(request.createOrEditStudioDiscountDto.DiscountId);
        //        if (discount == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

        //        try
        //        {
        //            studio.DiscountId = discount.Id;
        //            await _studioRepository.Update(studio);
        //            await _studioRepository.SaveChangesAsync();

        //            return Unit.Value;
        //        }
        //        catch
        //        {
        //            throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
        //        }

        //    }
        //}

        //#endregion


        #region Delete Studio

        public class DeleteStudioCommand : IRequest
        {
            public int StudioId { get; set; }
        }

        public class DeleteStudioCommandHandler : IRequestHandler<DeleteStudioCommand>
        {
            private readonly IStudioRepository _studioRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteStudioCommandHandler(IFileSaver fileSaver, IStudioRepository studioRepository)
            {
                _fileSaver = fileSaver;
                _studioRepository = studioRepository;
            }

            public async Task<Unit> Handle(DeleteStudioCommand request, CancellationToken cancellationToken)
            {
                var studio = await _studioRepository.GetById(request.StudioId);

                if (studio == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    //await _studioRepository.DeleteSelectedCategories(request.StudioId, cancellationToken);

                    await _fileSaver.DeleteImageFromServer(studio.Imagename, "images/studio-images");

                    await _studioRepository.SoftDelete(request.StudioId);
                    await _studioRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get Studio Detail

        public class GetStudioDetailQuery : IRequest<StudioForViewDto>
        {
            public int StudioId { get; set; }
        }

        public class GetStudioDetailQueryHandler : IRequestHandler<GetStudioDetailQuery, StudioForViewDto>
        {
            private readonly IStudioRepository _studioRepository;
            private readonly IMapper _mapper;
            private readonly IStudioGalleryRepository _studioGalleryRepository;

            public GetStudioDetailQueryHandler(IStudioGalleryRepository studioGalleryRepository, IMapper mapper, IStudioRepository studioRepository)
            {
                _studioGalleryRepository = studioGalleryRepository;
                _mapper = mapper;
                _studioRepository = studioRepository;
            }

            public async Task<StudioForViewDto> Handle(GetStudioDetailQuery request, CancellationToken cancellationToken)
            {
                var studio = await _studioRepository.GetById(request.StudioId);
                if (studio == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                var mappedStudio = _mapper.Map<StudioForViewDto>(studio);
                var galleies = _studioGalleryRepository.GetAllWithFilter(new StudioGalleryFilterInput
                {
                    StudioId = mappedStudio.Id,
                    Take = 99999
                });
                mappedStudio.Galleries = _mapper.Map<List<StudioGalleryForViewDto>>(galleies.ToList());
                mappedStudio.FeatureList = JsonConvert.DeserializeObject<List<CreateOrEditFeatureDto>>(mappedStudio.Features);

                return mappedStudio;
            }
        }

        #endregion

        #region Get Studio Detail For Edit

        public class GetStudioDetailForEditQuery : IRequest<CreateOrEditStudioDto>
        {
            public int StudioId { get; set; }
        }

        public class GetStudioDetailForEditQueryHandler : IRequestHandler<GetStudioDetailForEditQuery, CreateOrEditStudioDto>
        {
            private readonly IStudioRepository _studioRepository;
            private readonly IMapper _mapper;
            public GetStudioDetailForEditQueryHandler(IStudioRepository studioRepository, IMapper mapper)
            {
                _studioRepository = studioRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditStudioDto> Handle(GetStudioDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var studio = await _studioRepository.GetById(request.StudioId);
                if (studio == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                var item = _mapper.Map<CreateOrEditStudioDto>(studio);
                item.FeatureList = JsonConvert.DeserializeObject<List<CreateOrEditFeatureDto>>(studio.Features);
                return item;
            }
        }

        #endregion


        #region Get Studio List
        public class GetStudioListQuery : IRequest<PagingHandler<StudioForViewDto>>
        {
            public StudioFilterInput StudioFilterInput { get; set; }
        }

        public class GetStudioListQueryHandler : IRequestHandler<GetStudioListQuery, PagingHandler<StudioForViewDto>>
        {
            private readonly IStudioRepository _StudioRepository;
            private readonly IPagerService<StudioForViewDto, Studio> _pager;
            private readonly IMapper _mapper;
            private readonly IStudioGalleryRepository _studioGalleryRepository;

            public GetStudioListQueryHandler(IMapper mapper, IPagerService<StudioForViewDto, Studio> pager, IStudioRepository studioRepository, IStudioGalleryRepository studioGalleryRepository)
            {
                _mapper = mapper;
                _pager = pager;
                _StudioRepository = studioRepository;
                _studioGalleryRepository = studioGalleryRepository;
            }

            public async Task<PagingHandler<StudioForViewDto>> Handle(GetStudioListQuery request, CancellationToken cancellationToken)
            {
                var studios = await Task.FromResult(_StudioRepository.GetAllWithFilter(request.StudioFilterInput));
                var pager = _pager.PageBuilder(studios.Count(), request.StudioFilterInput.PageId, request.StudioFilterInput.Take);
                var mappedStudios = await _pager.SetItemsMapper(studios, pager);

                foreach (var item in mappedStudios.Items)
                {
                    var galleies = _studioGalleryRepository.GetAllWithFilter(new StudioGalleryFilterInput
                    {
                        StudioId = item.Id,
                        DisplayFront = true
                    });
                    item.Galleries = _mapper.Map<List<StudioGalleryForViewDto>>(galleies.Take(request.StudioFilterInput.GalleryTake).ToList());
                    item.FeatureList = JsonConvert.DeserializeObject<List<CreateOrEditFeatureDto>>(item.Features);
                }
                return mappedStudios;
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface IStudioRepository : IRepository<Studio>
        {
            IQueryable<Studio> GetAllWithFilter(StudioFilterInput FilterInput);
            //Task InsertSelectedCategories(List<int> categories, int itemId, CancellationToken cancellationToken);
            //Task DeleteSelectedCategories(int itemId, CancellationToken cancellationToken);
        }

        public class StudioProfile : Profile
        {
            public StudioProfile()
            {
                CreateMap<Studio, CreateOrEditStudioDto>();
                CreateMap<Studio, CreateOrEditStudioDto>().ReverseMap();

                CreateMap<Studio, StudioForBasketDto>();
                CreateMap<Studio, StudioForBasketDto>().ReverseMap();

                CreateMap<Studio, StudioForViewDto>();
            }
        }

        #endregion


        #region Serialization

        public class CreateOrEditStudioDiscountDto
        {
            public int StudioId { get; set; }
            public int DiscountId { get; set; }
        }
        public class CreateOrEditFeatureDto
        {
            public string Title { get; set; }
            public string Value { get; set; }
        }
        public class CreateOrEditStudioDto
        {
            public int? Id { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public string Address { get; set; }
            public string Location { get; set; }
            public string Email { get; set; }
            public string Tell { get; set; }
            public List<CreateOrEditFeatureDto> FeatureList { get; set; }
            public string Features { get; set; }
            public string Imagename { get; set; }
            public IFormFile Image { get; set; }
            public bool IsActive { get; set; }
        }

        public class StudioForViewDto : Entity
        {
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public string Address { get; set; }
            public string Location { get; set; }
            public string Email { get; set; }
            public string Tell { get; set; }
            public List<CreateOrEditFeatureDto> FeatureList { get; set; }
            public List<StudioGalleryForViewDto> Galleries { get; set; }
            public string Features { get; set; }
            public string Imagename { get; set; }
            public bool IsActive { get; set; }
            public DateTime? CreationTime { get; set; }
        }
        public class StudioForBasketDto : Entity
        {
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string Imagename { get; set; }
            public bool IsActive { get; set; }
        }
        public class StudioFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int GalleryTake { get; set; } = 3;
        }

        #endregion



    }
}
