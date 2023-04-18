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
    public class EquipmentGalleryFeature
    {
        #region CQRS

        #region Create EquipmentGallery
        public class CreateEquipmentGalleryCommand : IRequest
        {
            public CreateOrEditEquipmentGalleryDto CreateOrEditEquipmentGalleryDto { get; set; }
        }

        public class CreateEquipmentGalleryCommandHandler : IRequestHandler<CreateEquipmentGalleryCommand>
        {

            private readonly IEquipmentGalleryRepository _equipmentGalleryRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateEquipmentGalleryCommandHandler(IFileSaver fileSaver, IEquipmentGalleryRepository equipmentGalleryRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _equipmentGalleryRepository = equipmentGalleryRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateEquipmentGalleryCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    request.CreateOrEditEquipmentGalleryDto.Imagename = "no-image";
                    if (request.CreateOrEditEquipmentGalleryDto.Image != null)
                    {
                        request.CreateOrEditEquipmentGalleryDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditEquipmentGalleryDto.Image, "images/equipmentGallery-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditEquipmentGalleryDto.Imagename, "images/equipmentGallery-images");
                    }

                    var equipmentGallery = _mapper.Map<EquipmentGallery>(request.CreateOrEditEquipmentGalleryDto);
                    equipmentGallery.CreationTime = DateTime.Now;

                    await _equipmentGalleryRepository.InsertNew(equipmentGallery);
                    await _equipmentGalleryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit EquipmentGallery

        public class EditEquipmentGalleryCommand : IRequest
        {
            public CreateOrEditEquipmentGalleryDto CreateOrEditEquipmentGalleryDto { get; set; }

        }

        public class EditEquipmentGalleryCommandHandler : IRequestHandler<EditEquipmentGalleryCommand>
        {
            private readonly IEquipmentGalleryRepository _equipmentGalleryRepository;
            private readonly IMapper _mapper;
            private readonly IFileSaver _fileSaver;

            public EditEquipmentGalleryCommandHandler(IEquipmentGalleryRepository equipmentGalleryRepository, IMapper mapper, IFileSaver fileSaver)
            {
                _equipmentGalleryRepository = equipmentGalleryRepository;
                _mapper = mapper;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditEquipmentGalleryCommand request, CancellationToken cancellationToken)
            {
                var equipmentGallery = await _equipmentGalleryRepository.GetById(request.CreateOrEditEquipmentGalleryDto.Id.Value);

                if (equipmentGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditEquipmentGalleryDto.Image != null)
                    {
                        if (equipmentGallery.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(equipmentGallery.Imagename, "images/equipmentGallery-images");
                        request.CreateOrEditEquipmentGalleryDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditEquipmentGalleryDto.Image, "images/equipmentGallery-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditEquipmentGalleryDto.Imagename, "images/equipmentGallery-images");
                    }

                    var mappedEquipmentGallery = _mapper.Map<EquipmentGallery>(request.CreateOrEditEquipmentGalleryDto);
                    mappedEquipmentGallery.CreationTime = equipmentGallery.CreationTime;
                    mappedEquipmentGallery.LastModifyTime = DateTime.Now;
                    mappedEquipmentGallery.Imagename = request.CreateOrEditEquipmentGalleryDto.Imagename ?? equipmentGallery.Imagename;

                    await _equipmentGalleryRepository.Update(mappedEquipmentGallery);
                    await _equipmentGalleryRepository.SaveChangesAsync();


                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete EquipmentGallery

        public class DeleteEquipmentGalleryCommand : IRequest
        {
            public int EquipmentGalleryId { get; set; }
        }

        public class DeleteEquipmentGalleryCommandHandler : IRequestHandler<DeleteEquipmentGalleryCommand>
        {
            private readonly IEquipmentGalleryRepository _equipmentGalleryRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteEquipmentGalleryCommandHandler(IFileSaver fileSaver, IEquipmentGalleryRepository equipmentGalleryRepository)
            {
                _fileSaver = fileSaver;
                _equipmentGalleryRepository = equipmentGalleryRepository;
            }

            public async Task<Unit> Handle(DeleteEquipmentGalleryCommand request, CancellationToken cancellationToken)
            {
                var equipmentGallery = await _equipmentGalleryRepository.GetById(request.EquipmentGalleryId);

                if (equipmentGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    await _fileSaver.DeleteImageFromServer(equipmentGallery.Imagename, "images/equipmentGallery-images");

                    await _equipmentGalleryRepository.Delete(request.EquipmentGalleryId);
                    await _equipmentGalleryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get EquipmentGallery Detail

        public class GetEquipmentGalleryDetailQuery : IRequest<EquipmentGalleryForViewDto>
        {
            public int EquipmentGalleryId { get; set; }
        }

        public class GetEquipmentGalleryDetailQueryHandler : IRequestHandler<GetEquipmentGalleryDetailQuery, EquipmentGalleryForViewDto>
        {
            private readonly IEquipmentGalleryRepository _equipmentGalleryRepository;
            private readonly IMapper _mapper;

            public GetEquipmentGalleryDetailQueryHandler(IEquipmentGalleryRepository equipmentGalleryRepository, IMapper mapper)
            {
                _equipmentGalleryRepository = equipmentGalleryRepository;
                _mapper = mapper;
            }

            public async Task<EquipmentGalleryForViewDto> Handle(GetEquipmentGalleryDetailQuery request, CancellationToken cancellationToken)
            {
                var equipmentGallery = await _equipmentGalleryRepository.GetById(request.EquipmentGalleryId);
                if (equipmentGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<EquipmentGalleryForViewDto>(equipmentGallery);
            }
        }

        #endregion

        #region Get EquipmentGallery Detail For Edit

        public class GetEquipmentGalleryDetailForEditQuery : IRequest<CreateOrEditEquipmentGalleryDto>
        {
            public int EquipmentGalleryId { get; set; }
        }

        public class GetEquipmentGalleryDetailForEditQueryHandler : IRequestHandler<GetEquipmentGalleryDetailForEditQuery, CreateOrEditEquipmentGalleryDto>
        {
            private readonly IEquipmentGalleryRepository _equipmentGalleryRepository;
            private readonly IMapper _mapper;
            public GetEquipmentGalleryDetailForEditQueryHandler(IEquipmentGalleryRepository equipmentGalleryRepository, IMapper mapper)
            {
                _equipmentGalleryRepository = equipmentGalleryRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditEquipmentGalleryDto> Handle(GetEquipmentGalleryDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var equipmentGallery = await _equipmentGalleryRepository.GetById(request.EquipmentGalleryId);
                if (equipmentGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditEquipmentGalleryDto>(equipmentGallery);
            }
        }

        #endregion


        #region Get EquipmentGallery List
        public class GetEquipmentGalleryListQuery : IRequest<PagingHandler<EquipmentGalleryForViewDto>>
        {
            public EquipmentGalleryFilterInput EquipmentGalleryFilterInput { get; set; }
        }

        public class GetEquipmentGalleryListQueryHandler : IRequestHandler<GetEquipmentGalleryListQuery, PagingHandler<EquipmentGalleryForViewDto>>
        {
            private readonly IEquipmentGalleryRepository _equipmentGalleryRepository;
            private readonly IPagerService<EquipmentGalleryForViewDto, EquipmentGallery> _pager;
            public GetEquipmentGalleryListQueryHandler(IEquipmentGalleryRepository equipmentGalleryRepository, IPagerService<EquipmentGalleryForViewDto, EquipmentGallery> pager)
            {
                _equipmentGalleryRepository = equipmentGalleryRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<EquipmentGalleryForViewDto>> Handle(GetEquipmentGalleryListQuery request, CancellationToken cancellationToken)
            {
                var equipmentGallerys = await Task.FromResult(_equipmentGalleryRepository.GetAllWithFilter(request.EquipmentGalleryFilterInput));
                var pager = _pager.PageBuilder(equipmentGallerys.Count(), request.EquipmentGalleryFilterInput.PageId, request.EquipmentGalleryFilterInput.Take);
                return await _pager.SetItemsMapper(equipmentGallerys, pager);
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface IEquipmentGalleryRepository : IRepository<EquipmentGallery>
        {
            IQueryable<EquipmentGallery> GetAllWithFilter(EquipmentGalleryFilterInput equipmentGalleryFilterInput);
        }

        public class EquipmentGalleryProfile : Profile
        {
            public EquipmentGalleryProfile()
            {
                CreateMap<EquipmentGallery, CreateOrEditEquipmentGalleryDto>();
                CreateMap<EquipmentGallery, CreateOrEditEquipmentGalleryDto>().ReverseMap();

                CreateMap<EquipmentGallery, EquipmentGalleryForViewDto>().ForMember(c => c.Equipment, opt => opt.MapFrom(c => c.EquipmentFk.Title));
            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditEquipmentGalleryDto
        {
            public int? Id { get; set; }
            public string Imagename { get; set; } = string.Empty;
            public bool DisplayFront { get; set; }
            public int? EquipmentId { get; set; }
            public IFormFile Image { get; set; }
        }

        public class EquipmentGalleryForViewDto : Entity
        {
            public string Imagename { get; set; } = string.Empty;
            public string Equipment { get; set; } = string.Empty;
            public int EquipmentId { get; set; }
            public bool DisplayFront { get; set; }

            public DateTime CreateDate { get; set; }
        }

        public class EquipmentGalleryFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int? EquipmentId { get; set; }
        }


        #endregion
    }
}
