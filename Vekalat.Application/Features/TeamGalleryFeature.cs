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
    public class TeamGalleryFeature
    {
        #region CQRS

        #region Create TeamGallery
        public class CreateTeamGalleryCommand : IRequest
        {
            public CreateOrEditTeamGalleryDto CreateOrEditTeamGalleryDto { get; set; }
        }

        public class CreateTeamGalleryCommandHandler : IRequestHandler<CreateTeamGalleryCommand>
        {

            private readonly ITeamGalleryRepository _TeamGalleryRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateTeamGalleryCommandHandler(IFileSaver fileSaver, ITeamGalleryRepository TeamGalleryRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _TeamGalleryRepository = TeamGalleryRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateTeamGalleryCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    request.CreateOrEditTeamGalleryDto.Imagename = "no-image";
                    if (request.CreateOrEditTeamGalleryDto.Image != null)
                    {
                        request.CreateOrEditTeamGalleryDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditTeamGalleryDto.Image, "images/TeamGallery-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditTeamGalleryDto.Imagename, "images/TeamGallery-images");
                    }

                    var TeamGallery = _mapper.Map<TeamGallery>(request.CreateOrEditTeamGalleryDto);
                    TeamGallery.CreationTime = DateTime.Now;

                    await _TeamGalleryRepository.InsertNew(TeamGallery);
                    await _TeamGalleryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit TeamGallery

        public class EditTeamGalleryCommand : IRequest
        {
            public CreateOrEditTeamGalleryDto CreateOrEditTeamGalleryDto { get; set; }

        }

        public class EditTeamGalleryCommandHandler : IRequestHandler<EditTeamGalleryCommand>
        {
            private readonly ITeamGalleryRepository _TeamGalleryRepository;
            private readonly IMapper _mapper;
            private readonly IFileSaver _fileSaver;

            public EditTeamGalleryCommandHandler(ITeamGalleryRepository TeamGalleryRepository, IMapper mapper, IFileSaver fileSaver)
            {
                _TeamGalleryRepository = TeamGalleryRepository;
                _mapper = mapper;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditTeamGalleryCommand request, CancellationToken cancellationToken)
            {
                var TeamGallery = await _TeamGalleryRepository.GetById(request.CreateOrEditTeamGalleryDto.Id.Value);

                if (TeamGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditTeamGalleryDto.Image != null)
                    {
                        if (TeamGallery.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(TeamGallery.Imagename, "images/TeamGallery-images");
                        request.CreateOrEditTeamGalleryDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditTeamGalleryDto.Image, "images/TeamGallery-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditTeamGalleryDto.Imagename, "images/TeamGallery-images");
                    }

                    var mappedTeamGallery = _mapper.Map<TeamGallery>(request.CreateOrEditTeamGalleryDto);
                    mappedTeamGallery.CreationTime = TeamGallery.CreationTime;
                    mappedTeamGallery.LastModifyTime = DateTime.Now;
                    mappedTeamGallery.Imagename = request.CreateOrEditTeamGalleryDto.Imagename ?? TeamGallery.Imagename;

                    await _TeamGalleryRepository.Update(mappedTeamGallery);
                    await _TeamGalleryRepository.SaveChangesAsync();


                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete TeamGallery

        public class DeleteTeamGalleryCommand : IRequest
        {
            public int TeamGalleryId { get; set; }
        }

        public class DeleteTeamGalleryCommandHandler : IRequestHandler<DeleteTeamGalleryCommand>
        {
            private readonly ITeamGalleryRepository _TeamGalleryRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteTeamGalleryCommandHandler(IFileSaver fileSaver, ITeamGalleryRepository TeamGalleryRepository)
            {
                _fileSaver = fileSaver;
                _TeamGalleryRepository = TeamGalleryRepository;
            }

            public async Task<Unit> Handle(DeleteTeamGalleryCommand request, CancellationToken cancellationToken)
            {
                var TeamGallery = await _TeamGalleryRepository.GetById(request.TeamGalleryId);

                if (TeamGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    await _fileSaver.DeleteImageFromServer(TeamGallery.Imagename, "images/TeamGallery-images");

                    await _TeamGalleryRepository.Delete(request.TeamGalleryId);
                    await _TeamGalleryRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get TeamGallery Detail

        public class GetTeamGalleryDetailQuery : IRequest<TeamGalleryForViewDto>
        {
            public int TeamGalleryId { get; set; }
        }

        public class GetTeamGalleryDetailQueryHandler : IRequestHandler<GetTeamGalleryDetailQuery, TeamGalleryForViewDto>
        {
            private readonly ITeamGalleryRepository _TeamGalleryRepository;
            private readonly IMapper _mapper;

            public GetTeamGalleryDetailQueryHandler(ITeamGalleryRepository TeamGalleryRepository, IMapper mapper)
            {
                _TeamGalleryRepository = TeamGalleryRepository;
                _mapper = mapper;
            }

            public async Task<TeamGalleryForViewDto> Handle(GetTeamGalleryDetailQuery request, CancellationToken cancellationToken)
            {
                var TeamGallery = await _TeamGalleryRepository.GetById(request.TeamGalleryId);
                if (TeamGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<TeamGalleryForViewDto>(TeamGallery);
            }
        }

        #endregion

        #region Get TeamGallery Detail For Edit

        public class GetTeamGalleryDetailForEditQuery : IRequest<CreateOrEditTeamGalleryDto>
        {
            public int TeamGalleryId { get; set; }
        }

        public class GetTeamGalleryDetailForEditQueryHandler : IRequestHandler<GetTeamGalleryDetailForEditQuery, CreateOrEditTeamGalleryDto>
        {
            private readonly ITeamGalleryRepository _TeamGalleryRepository;
            private readonly IMapper _mapper;
            public GetTeamGalleryDetailForEditQueryHandler(ITeamGalleryRepository TeamGalleryRepository, IMapper mapper)
            {
                _TeamGalleryRepository = TeamGalleryRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditTeamGalleryDto> Handle(GetTeamGalleryDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var TeamGallery = await _TeamGalleryRepository.GetById(request.TeamGalleryId);
                if (TeamGallery == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditTeamGalleryDto>(TeamGallery);
            }
        }

        #endregion


        #region Get TeamGallery List
        public class GetTeamGalleryListQuery : IRequest<PagingHandler<TeamGalleryForViewDto>>
        {
            public TeamGalleryFilterInput TeamGalleryFilterInput { get; set; }
        }

        public class GetTeamGalleryListQueryHandler : IRequestHandler<GetTeamGalleryListQuery, PagingHandler<TeamGalleryForViewDto>>
        {
            private readonly ITeamGalleryRepository _TeamGalleryRepository;
            private readonly IPagerService<TeamGalleryForViewDto, TeamGallery> _pager;
            public GetTeamGalleryListQueryHandler(ITeamGalleryRepository TeamGalleryRepository, IPagerService<TeamGalleryForViewDto, TeamGallery> pager)
            {
                _TeamGalleryRepository = TeamGalleryRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<TeamGalleryForViewDto>> Handle(GetTeamGalleryListQuery request, CancellationToken cancellationToken)
            {
                request.TeamGalleryFilterInput.Take = 10;
                var TeamGallerys = await Task.FromResult(_TeamGalleryRepository.GetAllWithFilter(request.TeamGalleryFilterInput));
                var pager = _pager.PageBuilder(TeamGallerys.Count(), request.TeamGalleryFilterInput.PageId, request.TeamGalleryFilterInput.Take);
                return await _pager.SetItemsMapper(TeamGallerys, pager);
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface ITeamGalleryRepository : IRepository<TeamGallery>
        {
            IQueryable<TeamGallery> GetAllWithFilter(TeamGalleryFilterInput TeamGalleryFilterInput);
        }

        public class TeamGalleryProfile : Profile
        {
            public TeamGalleryProfile()
            {
                CreateMap<TeamGallery, CreateOrEditTeamGalleryDto>();
                CreateMap<TeamGallery, CreateOrEditTeamGalleryDto>().ReverseMap();

                CreateMap<TeamGallery, TeamGalleryForViewDto>();
                CreateMap<TeamGallery, TeamGalleryForViewDto>().ReverseMap();
            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditTeamGalleryDto
        {
            public int? Id { get; set; }
            public string Imagename { get; set; } = string.Empty;
            public int? TeamId { get; set; }
            public IFormFile Image { get; set; }
        }

        public class TeamGalleryForViewDto : Entity
        {
            public string Imagename { get; set; } = string.Empty;
            public int TeamId { get; set; }

            public DateTime CreateDate { get; set; }
        }

        public class TeamGalleryFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int? TeamId { get; set; }
        }


        #endregion
    }
}
