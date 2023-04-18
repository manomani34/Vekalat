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
using Vekalat.Application.Common.InfraServices;
using Application.Common.Dto.Paging;
using Vekalat.Core.Common;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace Vekalat.Application.Features
{
    public class EquipmentItemFeature
    {
        #region CQRS

        #region Create EquipmentItem
        public class CreateEquipmentItemCommand : IRequest
        {
            public CreateOrEditEquipmentItemDto CreateOrEditEquipmentItemDto { get; set; }
        }

        public class CreateEquipmentItemCommandHandler : IRequestHandler<CreateEquipmentItemCommand>
        {

            private readonly IEquipmentItemRepository _equipmentItemRepository;
            private readonly IMapper _mapper;
            public CreateEquipmentItemCommandHandler(IEquipmentItemRepository equipmentItemRepository, IMapper mapper)
            {
                _equipmentItemRepository = equipmentItemRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateEquipmentItemCommand request, CancellationToken cancellationToken)
            {
                try
                {

                    var equipmentItem = _mapper.Map<EquipmentItem>(request.CreateOrEditEquipmentItemDto);
                    equipmentItem.CreationTime = DateTime.Now;
                    await _equipmentItemRepository.InsertNew(equipmentItem);
                    await _equipmentItemRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit EquipmentItem

        public class EditEquipmentItemCommand : IRequest
        {
            public CreateOrEditEquipmentItemDto CreateOrEditEquipmentItemDto { get; set; }

        }

        public class EditEquipmentItemCommandHandler : IRequestHandler<EditEquipmentItemCommand>
        {
            private readonly IEquipmentItemRepository _equipmentItemRepository;
            private readonly IMapper _mapper;

            public EditEquipmentItemCommandHandler(IEquipmentItemRepository equipmentItemRepository, IMapper mapper)
            {
                _equipmentItemRepository = equipmentItemRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(EditEquipmentItemCommand request, CancellationToken cancellationToken)
            {
                var equipmentItem = await _equipmentItemRepository.GetById(request.CreateOrEditEquipmentItemDto.Id.Value);

                if (equipmentItem == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {

                    var mappedEquipmentItem = _mapper.Map<EquipmentItem>(request.CreateOrEditEquipmentItemDto);
                    mappedEquipmentItem.CreationTime = equipmentItem.CreationTime;
                    mappedEquipmentItem.LastModifyTime = DateTime.Now;

                    await _equipmentItemRepository.Update(mappedEquipmentItem);
                    await _equipmentItemRepository.SaveChangesAsync();

                    await _equipmentItemRepository.CalculateEquipmentQuantity(equipmentItem.EquipmentId);

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete EquipmentItem

        public class DeleteEquipmentItemCommand : IRequest
        {
            public int EquipmentItemId { get; set; }
        }

        public class DeleteEquipmentItemCommandHandler : IRequestHandler<DeleteEquipmentItemCommand>
        {
            private readonly IEquipmentItemRepository _equipmentItemRepository;
            public DeleteEquipmentItemCommandHandler(IEquipmentItemRepository equipmentItemRepository)
            {
                _equipmentItemRepository = equipmentItemRepository;
            }

            public async Task<Unit> Handle(DeleteEquipmentItemCommand request, CancellationToken cancellationToken)
            {
                var equipmentItem = await _equipmentItemRepository.GetById(request.EquipmentItemId);

                if (equipmentItem == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {

                    await _equipmentItemRepository.SoftDelete(request.EquipmentItemId);
                    await _equipmentItemRepository.CalculateEquipmentQuantity(equipmentItem.EquipmentId);
                    await _equipmentItemRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get EquipmentItem Detail

        public class GetEquipmentItemDetailQuery : IRequest<EquipmentItemForViewDto>
        {
            public int EquipmentItemId { get; set; }
        }

        public class GetEquipmentItemDetailQueryHandler : IRequestHandler<GetEquipmentItemDetailQuery, EquipmentItemForViewDto>
        {
            private readonly IEquipmentItemRepository _equipmentItemRepository;
            private readonly IMapper _mapper;

            public GetEquipmentItemDetailQueryHandler(IEquipmentItemRepository equipmentItemRepository, IMapper mapper)
            {
                _equipmentItemRepository = equipmentItemRepository;
                _mapper = mapper;
            }

            public async Task<EquipmentItemForViewDto> Handle(GetEquipmentItemDetailQuery request, CancellationToken cancellationToken)
            {
                var equipmentItem = await _equipmentItemRepository.GetById(request.EquipmentItemId);
                if (equipmentItem == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<EquipmentItemForViewDto>(equipmentItem);
            }
        }

        #endregion

        #region Get EquipmentItem Detail For Edit

        public class GetEquipmentItemDetailForEditQuery : IRequest<CreateOrEditEquipmentItemDto>
        {
            public int EquipmentItemId { get; set; }
        }

        public class GetEquipmentItemDetailForEditQueryHandler : IRequestHandler<GetEquipmentItemDetailForEditQuery, CreateOrEditEquipmentItemDto>
        {
            private readonly IEquipmentItemRepository _equipmentItemRepository;
            private readonly IMapper _mapper;
            public GetEquipmentItemDetailForEditQueryHandler(IEquipmentItemRepository equipmentItemRepository, IMapper mapper)
            {
                _equipmentItemRepository = equipmentItemRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditEquipmentItemDto> Handle(GetEquipmentItemDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var equipmentItem = await _equipmentItemRepository.GetById(request.EquipmentItemId);
                if (equipmentItem == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditEquipmentItemDto>(equipmentItem);
            }
        }

        #endregion

        #region Get EquipmentItem List
        public class GetEquipmentItemListQuery : IRequest<PagingHandler<EquipmentItemForViewDto>>
        {
            public EquipmentItemFilterInput EquipmentItemFilterInput { get; set; }
        }

        public class GetEquipmentItemListQueryHandler : IRequestHandler<GetEquipmentItemListQuery, PagingHandler<EquipmentItemForViewDto>>
        {
            private readonly IEquipmentItemRepository _equipmentItemRepository;
            private readonly IPagerService<EquipmentItemForViewDto, EquipmentItem> _pager;
            public GetEquipmentItemListQueryHandler(IEquipmentItemRepository equipmentItemRepository, IPagerService<EquipmentItemForViewDto, EquipmentItem> pager)
            {
                _equipmentItemRepository = equipmentItemRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<EquipmentItemForViewDto>> Handle(GetEquipmentItemListQuery request, CancellationToken cancellationToken)
            {
                var equipmentItems = await Task.FromResult(_equipmentItemRepository.GetAllWithFilter(request.EquipmentItemFilterInput));
                var pager = _pager.PageBuilder(equipmentItems.Count(), request.EquipmentItemFilterInput.PageId, request.EquipmentItemFilterInput.Take);
                return await _pager.SetItemsMapper(equipmentItems, pager);
            }
        }
        #endregion

        #region Get EquipmentItem Select List
        public class GetAllEquipmentItemSelectListQuery : IRequest<List<SelectListItem>>
        {

        }
        public class GetAllEquipmentItemSelectListQueryHandler : IRequestHandler<GetAllEquipmentItemSelectListQuery, List<SelectListItem>>
        {

            private readonly IEquipmentItemRepository _equipmentItemRepository;

            public GetAllEquipmentItemSelectListQueryHandler(IEquipmentItemRepository equipmentItemRepository)
            {
                _equipmentItemRepository = equipmentItemRepository;
            }

            public async Task<List<SelectListItem>> Handle(GetAllEquipmentItemSelectListQuery request, CancellationToken cancellationToken)
            {
                var result = new List<SelectListItem>();
                var items = await _equipmentItemRepository.GetAll();
                result.AddRange(items.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.SerialNumber
                }));
                return result;
            }
        }
        #endregion


        #endregion

        #region Mapper And Repo

        public interface IEquipmentItemRepository : IRepository<EquipmentItem>
        {
            Task CalculateEquipmentQuantity(int equipmentId);
            IQueryable<EquipmentItem> GetAllWithFilter(EquipmentItemFilterInput equipmentItemFilterInput);
        }

        public class EquipmentItemProfile : Profile
        {
            public EquipmentItemProfile()
            {
                CreateMap<EquipmentItem, CreateOrEditEquipmentItemDto>();
                CreateMap<EquipmentItem, CreateOrEditEquipmentItemDto>().ReverseMap();

                CreateMap<EquipmentItem, EquipmentItemForViewDto>()
                    .ForMember(des => des.Equipment, opt => opt.MapFrom(c => c.EquipmentFk.Title));

            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditEquipmentItemDto
        {
            public int? Id { get; set; }
            public string SerialNumber { get; set; }
            public int EquipmentId { get; set; }

        }

        public class EquipmentItemForViewDto : Entity
        {
            public string SerialNumber { get; set; }
            public string Equipment { get; set; }
        }

        public class EquipmentItemFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int? EquipmentId { get; set; }
        }


        #endregion

    }
}
