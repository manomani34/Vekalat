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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Vekalat.Application.Features
{
    public class StudioGalleryFeature
    {
        #region CQRS

        #region Create StudioGallery
        public class CreateStudioGalleryCommand : IRequest
        {
            public CreateOrEditStudioGalleryDto CreateOrEditStudioGalleryDto { get; set; }
        }

        public class CreateStudioGalleryCommandHandler : IRequestHandler<CreateStudioGalleryCommand>
        {

            private readonly IStudioGalleryRepository _studioGalleryRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateStudioGalleryCommandHandler(IFileSaver fileSaver, IStudioGalleryRepository studioGalleryRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _studioGalleryRepository = studioGalleryRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateStudioGalleryCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    request.CreateOrEditStudioGalleryDto.Imagename = "no-image";
                    if (request.CreateOrEditStudioGalleryDto.Image != null)
                    {
                        request.CreateOrEditStudioGalleryDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditStudioGalleryDto.Image, "images/studioGallery-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditStudioGalleryDto.Imagename, "images/studioGallery-images");
                    }

                    var studioGallery = _mapper.Map<StudioGallery>(request.CreateOrEditStudioGalleryDto);
                    studioGallery.CreationTime = DateTime.Now;

                    await _studioGalleryRepository.InsertNew(studioGallery);
                    await _studioGalleryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit StudioGallery

        public class EditStudioGalleryCommand : IRequest
        {
            public CreateOrEditStudioGalleryDto CreateOrEditStudioGalleryDto { get; set; }

        }

        public class EditStudioGalleryCommandHandler : IRequestHandler<EditStudioGalleryCommand>
        {
            private readonly IStudioGalleryRepository _studioGalleryRepository;
            private readonly IMapper _mapper;
            private readonly IFileSaver _fileSaver;

            public EditStudioGalleryCommandHandler(IStudioGalleryRepository studioGalleryRepository, IMapper mapper, IFileSaver fileSaver)
            {
                _studioGalleryRepository = studioGalleryRepository;
                _mapper = mapper;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditStudioGalleryCommand request, CancellationToken cancellationToken)
            {
                var studioGallery = await _studioGalleryRepository.GetById(request.CreateOrEditStudioGalleryDto.Id.Value);

                if (studioGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditStudioGalleryDto.Image != null)
                    {
                        if (studioGallery.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(studioGallery.Imagename, "images/studioGallery-images");
                        request.CreateOrEditStudioGalleryDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditStudioGalleryDto.Image, "images/studioGallery-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditStudioGalleryDto.Imagename, "images/studioGallery-images");
                    }

                    var mappedStudioGallery = _mapper.Map<StudioGallery>(request.CreateOrEditStudioGalleryDto);
                    mappedStudioGallery.CreationTime = studioGallery.CreationTime;
                    mappedStudioGallery.LastModifyTime = DateTime.Now;
                    mappedStudioGallery.Imagename = request.CreateOrEditStudioGalleryDto.Imagename ?? studioGallery.Imagename;

                    await _studioGalleryRepository.Update(mappedStudioGallery);
                    await _studioGalleryRepository.SaveChangesAsync();


                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete StudioGallery

        public class DeleteStudioGalleryCommand : IRequest
        {
            public int StudioGalleryId { get; set; }
        }

        public class DeleteStudioGalleryCommandHandler : IRequestHandler<DeleteStudioGalleryCommand>
        {
            private readonly IStudioGalleryRepository _studioGalleryRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteStudioGalleryCommandHandler(IFileSaver fileSaver, IStudioGalleryRepository studioGalleryRepository)
            {
                _fileSaver = fileSaver;
                _studioGalleryRepository = studioGalleryRepository;
            }

            public async Task<Unit> Handle(DeleteStudioGalleryCommand request, CancellationToken cancellationToken)
            {
                var studioGallery = await _studioGalleryRepository.GetById(request.StudioGalleryId);

                if (studioGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    await _fileSaver.DeleteImageFromServer(studioGallery.Imagename, "images/studioGallery-images");

                    await _studioGalleryRepository.Delete(request.StudioGalleryId);
                    await _studioGalleryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get StudioGallery Detail

        public class GetStudioGalleryDetailQuery : IRequest<StudioGalleryForViewDto>
        {
            public int StudioGalleryId { get; set; }
        }

        public class GetStudioGalleryDetailQueryHandler : IRequestHandler<GetStudioGalleryDetailQuery, StudioGalleryForViewDto>
        {
            private readonly IStudioGalleryRepository _studioGalleryRepository;
            private readonly IMapper _mapper;

            public GetStudioGalleryDetailQueryHandler(IStudioGalleryRepository studioGalleryRepository, IMapper mapper)
            {
                _studioGalleryRepository = studioGalleryRepository;
                _mapper = mapper;
            }

            public async Task<StudioGalleryForViewDto> Handle(GetStudioGalleryDetailQuery request, CancellationToken cancellationToken)
            {
                var studioGallery = await _studioGalleryRepository.GetById(request.StudioGalleryId);
                if (studioGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<StudioGalleryForViewDto>(studioGallery);
            }
        }

        #endregion

        #region Get StudioGallery Detail For Edit

        public class GetStudioGalleryDetailForEditQuery : IRequest<CreateOrEditStudioGalleryDto>
        {
            public int StudioGalleryId { get; set; }
        }

        public class GetStudioGalleryDetailForEditQueryHandler : IRequestHandler<GetStudioGalleryDetailForEditQuery, CreateOrEditStudioGalleryDto>
        {
            private readonly IStudioGalleryRepository _studioGalleryRepository;
            private readonly IMapper _mapper;
            public GetStudioGalleryDetailForEditQueryHandler(IStudioGalleryRepository studioGalleryRepository, IMapper mapper)
            {
                _studioGalleryRepository = studioGalleryRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditStudioGalleryDto> Handle(GetStudioGalleryDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var studioGallery = await _studioGalleryRepository.GetById(request.StudioGalleryId);
                if (studioGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditStudioGalleryDto>(studioGallery);
            }
        }

        #endregion


        #region Get StudioGallery List
        public class GetStudioGalleryListQuery : IRequest<PagingHandler<StudioGalleryForViewDto>>
        {
            public StudioGalleryFilterInput StudioGalleryFilterInput { get; set; }
        }

        public class GetStudioGalleryListQueryHandler : IRequestHandler<GetStudioGalleryListQuery, PagingHandler<StudioGalleryForViewDto>>
        {
            private readonly IStudioGalleryRepository _studioGalleryRepository;
            private readonly IPagerService<StudioGalleryForViewDto, StudioGallery> _pager;
            public GetStudioGalleryListQueryHandler(IStudioGalleryRepository studioGalleryRepository, IPagerService<StudioGalleryForViewDto, StudioGallery> pager)
            {
                _studioGalleryRepository = studioGalleryRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<StudioGalleryForViewDto>> Handle(GetStudioGalleryListQuery request, CancellationToken cancellationToken)
            {
                var studioGallerys = await Task.FromResult(_studioGalleryRepository.GetAllWithFilter(request.StudioGalleryFilterInput));
                var pager = _pager.PageBuilder(studioGallerys.Count(), request.StudioGalleryFilterInput.PageId, request.StudioGalleryFilterInput.Take);
                return await _pager.SetItemsMapper(studioGallerys, pager);
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface IStudioGalleryRepository : IRepository<StudioGallery>
        {
            IQueryable<StudioGallery> GetAllWithFilter(StudioGalleryFilterInput studioGalleryFilterInput);
        }

        public class StudioGalleryProfile : Profile
        {
            public StudioGalleryProfile()
            {
                CreateMap<StudioGallery, CreateOrEditStudioGalleryDto>();
                CreateMap<StudioGallery, CreateOrEditStudioGalleryDto>().ReverseMap();

                CreateMap<StudioGallery, StudioGalleryForViewDto>().ForMember(c => c.Studio, opt => opt.MapFrom(c => c.StudioFk.Title));
            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditStudioGalleryDto
        {
            public int? Id { get; set; }
            public string Imagename { get; set; } = string.Empty;
            public int? StudioId { get; set; }
            public IFormFile Image { get; set; }
            public bool DisplayFront { get; set; }
        }

        public class StudioGalleryForViewDto : Entity
        {
            public string Imagename { get; set; } = string.Empty;
            public string Studio { get; set; } = string.Empty;
            public int StudioId { get; set; }
            public bool DisplayFront { get; set; }
            public DateTime CreateDate { get; set; }
        }

        public class StudioGalleryFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int? StudioId { get; set; }
            public bool? DisplayFront { get; set; }
        }


        #endregion
    }
}
