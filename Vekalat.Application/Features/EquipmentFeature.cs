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
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentGalleryFeature;
using static Vekalat.Application.Features.EquipmentItemFeature;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Vekalat.Application.Features
{
    public class EquipmentFeature
    {
        #region CQRS

        #region Create Equipment
        public class CreateEquipmentCommand : IRequest
        {
            public CreateOrEditEquipmentDto CreateOrEditEquipmentDto { get; set; }
        }

        public class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand>
        {

            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IEquipmentItemRepository _equipmentItemRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateEquipmentCommandHandler(IFileSaver fileSaver, IEquipmentRepository equipmentRepository, IMapper mapper, IEquipmentItemRepository equipmentItemRepository)
            {
                _fileSaver = fileSaver;
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
                _equipmentItemRepository = equipmentItemRepository;
            }

            public async Task<Unit> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
            {
                _equipmentRepository.BeginTransaction();
                try
                {

                    request.CreateOrEditEquipmentDto.Imagename = "no-image";
                    if (request.CreateOrEditEquipmentDto.Image != null)
                    {
                        request.CreateOrEditEquipmentDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditEquipmentDto.Image, "images/equipment-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditEquipmentDto.Imagename, "images/equipment-images");
                    }

                    var equipment = _mapper.Map<Equipment>(request.CreateOrEditEquipmentDto);
                    equipment.Features = request.CreateOrEditEquipmentDto.Features;
                    equipment.CreationTime = DateTime.Now;
                    equipment.Quantity = 0;

                    await _equipmentRepository.InsertNew(equipment);
                    await _equipmentRepository.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(request.CreateOrEditEquipmentDto.SerialNumbers))
                    {
                        var items = JsonSerializer.Deserialize<List<CreateOrEditEquipmentItemDto>>
                            (request.CreateOrEditEquipmentDto.SerialNumbers, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        foreach (var item in items)
                        {
                            var equipmentItem = _mapper.Map<EquipmentItem>(item);
                            equipmentItem.EquipmentId = equipment.Id;
                            equipmentItem.CreationTime = DateTime.Now;
                            await _equipmentItemRepository.InsertNew(equipmentItem);
                        }
                        equipment.Quantity = items.Count;
                        await _equipmentRepository.SaveChangesAsync();
                    }

                    _equipmentRepository.Commit();
                    return Unit.Value;
                }
                catch
                {
                    _equipmentRepository.Rollback();
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit Equipment

        public class EditEquipmentCommand : IRequest
        {
            public CreateOrEditEquipmentDto CreateOrEditEquipmentDto { get; set; }

        }

        public class EditEquipmentCommandHandler : IRequestHandler<EditEquipmentCommand>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            private readonly IEquipmentItemRepository _equipmentItemRepository;

            public EditEquipmentCommandHandler(IFileSaver fileSaver, IEquipmentRepository equipmentRepository, IMapper mapper, IEquipmentItemRepository equipmentItemRepository)
            {
                _fileSaver = fileSaver;
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
                _equipmentItemRepository = equipmentItemRepository;
            }

            public async Task<Unit> Handle(EditEquipmentCommand request, CancellationToken cancellationToken)
            {
                var equipment = await _equipmentRepository.GetById(request.CreateOrEditEquipmentDto.Id.Value);

                if (equipment == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditEquipmentDto.Image != null)
                    {
                        if (equipment.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(equipment.Imagename, "images/equipment-images");
                        request.CreateOrEditEquipmentDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditEquipmentDto.Image, "images/equipment-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditEquipmentDto.Imagename, "images/equipment-images");
                    }


                    var mappedEquipment = _mapper.Map<Equipment>(request.CreateOrEditEquipmentDto);
                    mappedEquipment.CreationTime = equipment.CreationTime;
                    mappedEquipment.LastModifyTime = DateTime.Now;
                    mappedEquipment.Imagename = request.CreateOrEditEquipmentDto.Imagename ?? equipment.Imagename;


                    //var items = _equipmentItemRepository.GetAllWithFilter(new EquipmentItemFilterInput
                    // {
                    //     EquipmentId = equipment.Id,
                    //     Take = 99999
                    // });
                    // await _equipmentItemRepository.DeleteRang(items.ToList());

                    // if (!string.IsNullOrEmpty(request.CreateOrEditEquipmentDto.SerialNumbers))
                    // {
                    //     var equipmentItems = JsonSerializer.Deserialize<List<CreateOrEditEquipmentItemDto>>
                    //         (request.CreateOrEditEquipmentDto.SerialNumbers, new JsonSerializerOptions
                    //         {
                    //             PropertyNameCaseInsensitive = true
                    //         });
                    //     foreach (var item in equipmentItems)
                    //     {
                    //         var equipmentItem = _mapper.Map<EquipmentItem>(item);
                    //         equipmentItem.EquipmentId = equipment.Id;
                    //         await _equipmentItemRepository.InsertNew(equipmentItem);
                    //     }
                    //     mappedEquipment.Quantity = equipmentItems.Count;
                    // }

                    await _equipmentRepository.Update(mappedEquipment);
                    await _equipmentRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete Equipment

        public class DeleteEquipmentCommand : IRequest
        {
            public int EquipmentId { get; set; }
        }

        public class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IEquipmentItemRepository _equipmentItemRepository;

            public DeleteEquipmentCommandHandler(IFileSaver fileSaver, IEquipmentRepository equipmentRepository, IEquipmentItemRepository equipmentItemRepository)
            {
                _fileSaver = fileSaver;
                _equipmentRepository = equipmentRepository;
                _equipmentItemRepository = equipmentItemRepository;
            }

            public async Task<Unit> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
            {
                var equipment = await _equipmentRepository.GetById(request.EquipmentId);

                if (equipment == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    var items = _equipmentItemRepository.GetAllWithFilter(new EquipmentItemFilterInput
                    {
                        EquipmentId = equipment.Id,
                        Take = 99999
                    });

                    await _equipmentItemRepository.DeleteRang(items.ToList());

                    await _fileSaver.DeleteImageFromServer(equipment.Imagename, "images/equipment-images");

                    await _equipmentRepository.SoftDelete(request.EquipmentId);
                    await _equipmentRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get Equipment Detail

        public class GetEquipmentDetailQuery : IRequest<EquipmentForViewDto>
        {
            public int EquipmentId { get; set; }
        }

        public class GetEquipmentDetailQueryHandler : IRequestHandler<GetEquipmentDetailQuery, EquipmentForViewDto>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IMapper _mapper;
            private readonly IEquipmentGalleryRepository _equipmentGalleryRepository;
            private readonly IEquipmentItemRepository _equipmentItemRepository;

            public GetEquipmentDetailQueryHandler(IEquipmentRepository equipmentRepository, IMapper mapper, IEquipmentGalleryRepository equipmentGalleryRepository, IEquipmentItemRepository equipmentItemRepository)
            {
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
                _equipmentGalleryRepository = equipmentGalleryRepository;
                _equipmentItemRepository = equipmentItemRepository;
            }

            public async Task<EquipmentForViewDto> Handle(GetEquipmentDetailQuery request, CancellationToken cancellationToken)
            {
                var equipment = await _equipmentRepository.GetById(request.EquipmentId, false, nameof(Equipment.CategoryFk));
                if (equipment == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                var mappedEquipment = _mapper.Map<EquipmentForViewDto>(equipment);
                var galleies = _equipmentGalleryRepository.GetAllWithFilter(new EquipmentGalleryFilterInput
                {
                    EquipmentId = mappedEquipment.Id,
                    Take = 99999
                });
                mappedEquipment.Galleries = _mapper.Map<List<EquipmentGalleryForViewDto>>(galleies.ToList());

                var items = _equipmentItemRepository.GetAllWithFilter(new EquipmentItemFilterInput
                {
                    EquipmentId = mappedEquipment.Id,
                    Take = 99999
                });
                var mappedItems = _mapper.Map<List<EquipmentItemForViewDto>>(items.ToList());
                mappedEquipment.SerialNumberList = mappedItems;
                mappedEquipment.SerialNumbers = JsonConvert.SerializeObject(mappedItems);

                mappedEquipment.FeatureList = JsonConvert.DeserializeObject<List<CreateOrEditFeatureDto>>(equipment.Features);

                return mappedEquipment;
            }
        }

        #endregion

        #region Get Equipment Detail For Edit

        public class GetEquipmentDetailForEditQuery : IRequest<CreateOrEditEquipmentDto>
        {
            public int EquipmentId { get; set; }
        }

        public class GetEquipmentDetailForEditQueryHandler : IRequestHandler<GetEquipmentDetailForEditQuery, CreateOrEditEquipmentDto>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IMapper _mapper;
            private readonly IEquipmentItemRepository _equipmentItemRepository;

            public GetEquipmentDetailForEditQueryHandler(IEquipmentRepository equipmentRepository, IMapper mapper, IEquipmentItemRepository equipmentItemRepository)
            {
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
                _equipmentItemRepository = equipmentItemRepository;
            }

            public async Task<CreateOrEditEquipmentDto> Handle(GetEquipmentDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var equipment = await _equipmentRepository.GetById(request.EquipmentId);
                if (equipment == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                var item = _mapper.Map<CreateOrEditEquipmentDto>(equipment);
                item.FeatureList = JsonConvert.DeserializeObject<List<CreateOrEditFeatureDto>>(equipment.Features);

                //var items = _equipmentItemRepository.GetAllWithFilter(new EquipmentItemFilterInput
                //{
                //    EquipmentId = item.Id,
                //    Take = 99999
                //});
                //item.SerialNumberList = _mapper.Map<List<CreateOrEditEquipmentItemDto>>(items.ToList());
                //item.SerialNumbers = JsonConvert.SerializeObject(items.ToList());

                return item;
            }
        }

        #endregion

        #region Get Equipment List
        public class GetEquipmentListQuery : IRequest<PagingHandler<EquipmentForViewDto>>
        {
            public EquipmentFilterInput EquipmentFilterInput { get; set; }
        }

        public class GetEquipmentListQueryHandler : IRequestHandler<GetEquipmentListQuery, PagingHandler<EquipmentForViewDto>>
        {
            private readonly IEquipmentRepository _EquipmentRepository;
            private readonly IPagerService<EquipmentForViewDto, Equipment> _pager;
            private readonly IMapper _mapper;

            public GetEquipmentListQueryHandler(IMapper mapper, IPagerService<EquipmentForViewDto, Equipment> pager, IEquipmentRepository equipmentRepository)
            {
                _mapper = mapper;
                _pager = pager;
                _EquipmentRepository = equipmentRepository;
            }

            public async Task<PagingHandler<EquipmentForViewDto>> Handle(GetEquipmentListQuery request, CancellationToken cancellationToken)
            {
                var equipments = await Task.FromResult(_EquipmentRepository.GetAllWithFilter(request.EquipmentFilterInput));
                var pager = _pager.PageBuilder(equipments.Count(), request.EquipmentFilterInput.PageId, request.EquipmentFilterInput.Take);
                var mappedEquipments = await _pager.SetItemsMapper(equipments, pager);

                //foreach (var item in mappedEquipments.Items)
                //{
                //    var galleies = _equipmentGalleryRepository.GetAllWithFilter(new EquipmentGalleryFilterInput
                //    {
                //        EquipmentId = item.Id,
                //        Take = 99999
                //    });
                //    item.Galleries = _mapper.Map<List<EquipmentGalleryForViewDto>>(galleies.ToList());

                //    var categories = await _EquipmentCategoryRepository.GetEquipmentSelectedCategory(item.Id);
                //    item.Categories = _mapper.Map<List<SelectedEquipmentCategoryDto>>(categories);

                //    var discount = await _discountRepository.GetDiscountByEquipmentId(item.Id);
                //    item.discount = _mapper.Map<DiscountForViewDto>(discount);
                //}
                return mappedEquipments;
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface IEquipmentRepository : IRepository<Equipment>
        {
            IQueryable<Equipment> GetAllWithFilter(EquipmentFilterInput FilterInput);
            //Task InsertSelectedCategories(List<int> categories, int itemId, CancellationToken cancellationToken);
            //Task DeleteSelectedCategories(int itemId, CancellationToken cancellationToken);
        }

        public class EquipmentProfile : Profile
        {
            public EquipmentProfile()
            {
                CreateMap<Equipment, CreateOrEditEquipmentDto>();
                CreateMap<Equipment, CreateOrEditEquipmentDto>().ReverseMap();

                CreateMap<Equipment, EquipmentForBasketDto>();
                CreateMap<Equipment, EquipmentForBasketDto>().ReverseMap();

                CreateMap<Equipment, EquipmentForViewDto>()
                    .ForMember(opt => opt.CategoryTitle, des => des.MapFrom(c => c.CategoryFk.Title))
                    .ForMember(opt => opt.BrandTitle, des => des.MapFrom(c => c.BrandFk.Title));
            }
        }

        #endregion


        #region Serialization

        public class CreateOrEditEquipmentDiscountDto
        {
            public int EquipmentId { get; set; }
            public int DiscountId { get; set; }
        }
        public class CreateOrEditFeatureDto
        {
            public string Title { get; set; }
            public string Value { get; set; }
        }
        public class CreateOrEditEquipmentDto
        {
            public int? Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public EquipmentStatus EquipmentStatus { get; set; }
            public EquipmentPhysicalStatus EquipmentPhysicalStatus { get; set; }
            public int BrandId { get; set; }
            public int CategoryId { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; } = string.Empty;
            public List<CreateOrEditEquipmentItemDto> SerialNumberList { get; set; }
            public List<CreateOrEditFeatureDto> FeatureList { get; set; }
            public string Features { get; set; } = string.Empty;
            public string SerialNumbers { get; set; } = string.Empty;
            public string Imagename { get; set; }
            public IFormFile Image { get; set; }
            public bool IsActive { get; set; }
        }

        public class EquipmentForViewDto : Entity
        {
            public string Title { get; set; } = string.Empty;
            public EquipmentStatus EquipmentStatus { get; set; }
            public EquipmentPhysicalStatus EquipmentPhysicalStatus { get; set; }
            public string SerialNumbers { get; set; } = string.Empty;
            public string BrandTitle { get; set; }
            public string CategoryTitle { get; set; }
            public decimal Price { get; set; }
            public string Imagename { get; set; }
            public bool IsActive { get; set; }
            public int Quantity { get; set; }
            public string Description { get; set; } = string.Empty;
            public DateTime? CreationTime { get; set; }
            public List<EquipmentGalleryForViewDto> Galleries { get; set; }
            public List<EquipmentItemForViewDto> SerialNumberList { get; set; }
            public List<CreateOrEditFeatureDto> FeatureList { get; set; }
        }

        public class EquipmentForBasketDto : Entity
        {
            public string Title { get; set; } = string.Empty;
            public string BrandTitle { get; set; }
            public string CategoryTitle { get; set; }
            public decimal Price { get; set; }
            public string Imagename { get; set; }
            public bool IsActive { get; set; }
        }


        public class EquipmentFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
        }

        #endregion



    }
}
