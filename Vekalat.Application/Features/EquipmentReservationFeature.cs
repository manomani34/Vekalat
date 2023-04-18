using Application.Common.Dto.Paging;
using AutoMapper;
using Vekalat.Application.Common;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Common;
using Vekalat.Core.Entities;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentFeature;
using static Vekalat.Application.Features.UserFeature;

namespace Vekalat.Application.Features
{
    public class EquipmentReservationFeature
    {
        #region CQRS

        #region Create EquipmentReservation
        public class CreateEquipmentReservationCommand : IRequest<CreateOrEditEquipmentReservationDto>
        {
            public CreateOrEditEquipmentReservationDto CreateOrEditEquipmentReservationDto { get; set; }
        }

        public class CreateEquipmentReservationCommandHandler : IRequestHandler<CreateEquipmentReservationCommand, CreateOrEditEquipmentReservationDto>
        {

            private readonly IEquipmentReservationRepository _equipmentReservationRepository;
            private readonly IMapper _mapper;
            public CreateEquipmentReservationCommandHandler(IEquipmentReservationRepository equipmentReservationRepository, IMapper mapper)
            {
                _equipmentReservationRepository = equipmentReservationRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditEquipmentReservationDto> Handle(CreateEquipmentReservationCommand request, CancellationToken cancellationToken)
            {
                var isReserved = await _equipmentReservationRepository
                    .IsDateAlreadyReserved(request.CreateOrEditEquipmentReservationDto.EquipmentId,
                    request.CreateOrEditEquipmentReservationDto.ReservedDate,
                    request.CreateOrEditEquipmentReservationDto.ReturnDate);

                if (isReserved)
                    throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.AlreadyReserved), Messages.AlreadyReserved));

                try
                {
                    var equipmentReservation = _mapper.Map<EquipmentReservation>(request.CreateOrEditEquipmentReservationDto);
                    equipmentReservation.CreationTime = DateTime.Now;

                    var item = await _equipmentReservationRepository.InsertNew(equipmentReservation);
                    await _equipmentReservationRepository.SaveChangesAsync();

                    request.CreateOrEditEquipmentReservationDto.Id = item.Id;
                    return request.CreateOrEditEquipmentReservationDto;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit EquipmentReservation

        public class EditEquipmentReservationCommand : IRequest
        {
            public CreateOrEditEquipmentReservationDto CreateOrEditEquipmentReservationDto { get; set; }

        }

        public class EditEquipmentReservationCommandHandler : IRequestHandler<EditEquipmentReservationCommand>
        {
            private readonly IEquipmentReservationRepository _equipmentReservationRepository;
            private readonly IMapper _mapper;
            public EditEquipmentReservationCommandHandler(IEquipmentReservationRepository equipmentReservationRepository, IMapper mapper)
            {
                _equipmentReservationRepository = equipmentReservationRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(EditEquipmentReservationCommand request, CancellationToken cancellationToken)
            {
                var equipmentReservation = await _equipmentReservationRepository.GetById(request.CreateOrEditEquipmentReservationDto.Id.Value);

                if (equipmentReservation == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    var mappedEquipmentReservation = _mapper.Map<EquipmentReservation>(request.CreateOrEditEquipmentReservationDto);
                    mappedEquipmentReservation.CreationTime = equipmentReservation.CreationTime;
                    mappedEquipmentReservation.LastModifyTime = DateTime.Now;


                    await _equipmentReservationRepository.Update(mappedEquipmentReservation);
                    await _equipmentReservationRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete EquipmentReservation

        public class DeleteEquipmentReservationCommand : IRequest
        {
            public int EquipmentReservationId { get; set; }
        }

        public class DeleteEquipmentReservationCommandHandler : IRequestHandler<DeleteEquipmentReservationCommand>
        {
            private readonly IEquipmentReservationRepository _equipmentReservationRepository;
            public DeleteEquipmentReservationCommandHandler(IEquipmentReservationRepository equipmentReservationRepository)
            {
                _equipmentReservationRepository = equipmentReservationRepository;
            }

            public async Task<Unit> Handle(DeleteEquipmentReservationCommand request, CancellationToken cancellationToken)
            {
                var equipmentReservation = await _equipmentReservationRepository.GetById(request.EquipmentReservationId);

                if (equipmentReservation == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {

                    await _equipmentReservationRepository.SoftDelete(request.EquipmentReservationId);
                    await _equipmentReservationRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get EquipmentReservation Detail For Edit

        public class GetEquipmentReservationDetailForEditQuery : IRequest<CreateOrEditEquipmentReservationDto>
        {
            public int EquipmentReservationId { get; set; }
        }

        public class GetEquipmentReservationDetailForEditQueryHandler : IRequestHandler<GetEquipmentReservationDetailForEditQuery, CreateOrEditEquipmentReservationDto>
        {
            private readonly IEquipmentReservationRepository _equipmentReservationRepository;
            private readonly IMapper _mapper;
            public GetEquipmentReservationDetailForEditQueryHandler(IEquipmentReservationRepository equipmentReservationRepository, IMapper mapper)
            {
                _equipmentReservationRepository = equipmentReservationRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditEquipmentReservationDto> Handle(GetEquipmentReservationDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var equipmentReservation = await _equipmentReservationRepository.GetById(request.EquipmentReservationId);
                if (equipmentReservation == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                var item = _mapper.Map<CreateOrEditEquipmentReservationDto>(equipmentReservation);
                return item;
            }
        }

        #endregion

        #region Get EquipmentReservation Detail 

        public class GetEquipmentReservationDetailQuery : IRequest<EquipmentReservationForViewDto>
        {
            public int EquipmentReservationId { get; set; }
        }

        public class GetEquipmentReservationDetailQueryHandler : IRequestHandler<GetEquipmentReservationDetailQuery, EquipmentReservationForViewDto>
        {
            private readonly IEquipmentReservationRepository _equipmentReservationRepository;
            private readonly IUserRepository _userRepository;
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IMapper _mapper;
            public GetEquipmentReservationDetailQueryHandler(IEquipmentReservationRepository equipmentReservationRepository, IMapper mapper, IUserRepository userRepository, IEquipmentRepository equipmentRepository)
            {
                _equipmentReservationRepository = equipmentReservationRepository;
                _mapper = mapper;
                _userRepository = userRepository;
                _equipmentRepository = equipmentRepository;
            }

            public async Task<EquipmentReservationForViewDto> Handle(GetEquipmentReservationDetailQuery request, CancellationToken cancellationToken)
            {
                var equipmentReservation = await _equipmentReservationRepository.GetById(request.EquipmentReservationId);
                if (equipmentReservation == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                equipmentReservation.EquipmentFk = await _equipmentRepository.GetById(equipmentReservation.EquipmentId);
                equipmentReservation.UserFk = await _userRepository.GetById(equipmentReservation.UserId);
                var item = _mapper.Map<EquipmentReservationForViewDto>(equipmentReservation);
                return item;
            }
        }

        #endregion


        #region Get EquipmentReservation List
        public class GetEquipmentReservationListQuery : IRequest<PagingHandler<EquipmentReservationForViewDto>>
        {
            public EquipmentReservationFilterInput EquipmentReservationFilterInput { get; set; }
        }

        public class GetEquipmentReservationListQueryHandler : IRequestHandler<GetEquipmentReservationListQuery, PagingHandler<EquipmentReservationForViewDto>>
        {
            private readonly IEquipmentReservationRepository _EquipmentReservationRepository;
            private readonly IPagerService<EquipmentReservationForViewDto, EquipmentReservation> _pager;
            private readonly IMapper _mapper;

            public GetEquipmentReservationListQueryHandler(IMapper mapper, IPagerService<EquipmentReservationForViewDto, EquipmentReservation> pager, IEquipmentReservationRepository equipmentReservationRepository)
            {
                _mapper = mapper;
                _pager = pager;
                _EquipmentReservationRepository = equipmentReservationRepository;
            }

            public async Task<PagingHandler<EquipmentReservationForViewDto>> Handle(GetEquipmentReservationListQuery request, CancellationToken cancellationToken)
            {
                var equipmentReservations = await Task.FromResult(_EquipmentReservationRepository.GetAllWithFilter(request.EquipmentReservationFilterInput));
                var pager = _pager.PageBuilder(equipmentReservations.Count(), request.EquipmentReservationFilterInput.PageId, request.EquipmentReservationFilterInput.Take);
                var mappedEquipmentReservations = await _pager.SetItemsMapper(equipmentReservations, pager);

                return mappedEquipmentReservations;
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface IEquipmentReservationRepository : IRepository<EquipmentReservation>
        {
            IQueryable<EquipmentReservation> GetAllWithFilter(EquipmentReservationFilterInput FilterInput);
            Task<bool> IsDateAlreadyReserved(int equipmentId, DateTime ReservedDate, DateTime ReturnDate);
            //Task InsertSelectedCategories(List<int> categories, int itemId, CancellationToken cancellationToken);
            //Task DeleteSelectedCategories(int itemId, CancellationToken cancellationToken);
        }

        public class EquipmentReservationProfile : Profile
        {
            public EquipmentReservationProfile()
            {
                CreateMap<EquipmentReservation, CreateOrEditEquipmentReservationDto>();
                CreateMap<EquipmentReservation, CreateOrEditEquipmentReservationDto>().ReverseMap();

                CreateMap<EquipmentReservation, EquipmentReservationForViewDto>()
                    .ForMember(opt => opt.User, des => des.MapFrom(c => $"{c.UserFk.Firstname} {c.UserFk.Lastname}"))
                    .ForMember(opt => opt.Equipment, des => des.MapFrom(c => c.EquipmentFk.Title));
            }
        }

        #endregion


        #region Serialization

        public class CalenderInitData
        {
            public string JsonData { get; set; }
        }
        public class CreateOrEditEquipmentReservationDto
        {
            public int? Id { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public int EquipmentId { get; set; }

            [Required]
            public int UserId { get; set; }

            [Required]
            public DateTime ReservedDate { get; set; }

            [Required]
            public DateTime ReturnDate { get; set; }
            public string Description { get; set; } = "";
        }

        public class EquipmentReservationForViewDto : Entity
        {
            public string Title { get; set; }
            public string Equipment { get; set; }
            public string User { get; set; }
            public DateTime ReservedDate { get; set; }
            public DateTime ReturnDate { get; set; }
            public string Description { get; set; }
            public DateTime? CreationTime { get; set; }
        }

        public class EquipmentReservationFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
        }

        #endregion



    }
}
