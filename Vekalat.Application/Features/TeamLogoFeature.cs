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
    public class TeamLogoFeature
    {
        #region CQRS

        #region Create TeamLogo
        public class CreateTeamLogoCommand : IRequest
        {
            public CreateOrEditTeamLogoDto CreateOrEditTeamLogoDto { get; set; }
        }

        public class CreateTeamLogoCommandHandler : IRequestHandler<CreateTeamLogoCommand>
        {

            private readonly ITeamLogoRepository _TeamLogoRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateTeamLogoCommandHandler(IFileSaver fileSaver, ITeamLogoRepository TeamLogoRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _TeamLogoRepository = TeamLogoRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateTeamLogoCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    request.CreateOrEditTeamLogoDto.Imagename = "no-image";
                    if (request.CreateOrEditTeamLogoDto.Image != null)
                    {
                        request.CreateOrEditTeamLogoDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditTeamLogoDto.Image, "images/TeamLogo-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditTeamLogoDto.Imagename, "images/TeamLogo-images");
                    }

                    var TeamLogo = _mapper.Map<TeamLogo>(request.CreateOrEditTeamLogoDto);
                    TeamLogo.CreationTime = DateTime.Now;

                    await _TeamLogoRepository.InsertNew(TeamLogo);
                    await _TeamLogoRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit TeamLogo

        public class EditTeamLogoCommand : IRequest
        {
            public CreateOrEditTeamLogoDto CreateOrEditTeamLogoDto { get; set; }

        }

        public class EditTeamLogoCommandHandler : IRequestHandler<EditTeamLogoCommand>
        {
            private readonly ITeamLogoRepository _TeamLogoRepository;
            private readonly IMapper _mapper;
            private readonly IFileSaver _fileSaver;

            public EditTeamLogoCommandHandler(ITeamLogoRepository TeamLogoRepository, IMapper mapper, IFileSaver fileSaver)
            {
                _TeamLogoRepository = TeamLogoRepository;
                _mapper = mapper;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditTeamLogoCommand request, CancellationToken cancellationToken)
            {
                var TeamLogo = await _TeamLogoRepository.GetById(request.CreateOrEditTeamLogoDto.Id.Value);

                if (TeamLogo == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditTeamLogoDto.Image != null)
                    {
                        if (TeamLogo.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(TeamLogo.Imagename, "images/TeamLogo-images");
                        request.CreateOrEditTeamLogoDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditTeamLogoDto.Image, "images/TeamLogo-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditTeamLogoDto.Imagename, "images/TeamLogo-images");
                    }

                    var mappedTeamLogo = _mapper.Map<TeamLogo>(request.CreateOrEditTeamLogoDto);
                    mappedTeamLogo.CreationTime = TeamLogo.CreationTime;
                    mappedTeamLogo.LastModifyTime = DateTime.Now;
                    mappedTeamLogo.Imagename = request.CreateOrEditTeamLogoDto.Imagename ?? TeamLogo.Imagename;

                    await _TeamLogoRepository.Update(mappedTeamLogo);
                    await _TeamLogoRepository.SaveChangesAsync();


                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete TeamLogo

        public class DeleteTeamLogoCommand : IRequest
        {
            public int TeamLogoId { get; set; }
        }

        public class DeleteTeamLogoCommandHandler : IRequestHandler<DeleteTeamLogoCommand>
        {
            private readonly ITeamLogoRepository _TeamLogoRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteTeamLogoCommandHandler(IFileSaver fileSaver, ITeamLogoRepository TeamLogoRepository)
            {
                _fileSaver = fileSaver;
                _TeamLogoRepository = TeamLogoRepository;
            }

            public async Task<Unit> Handle(DeleteTeamLogoCommand request, CancellationToken cancellationToken)
            {
                var TeamLogo = await _TeamLogoRepository.GetById(request.TeamLogoId);

                if (TeamLogo == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    await _fileSaver.DeleteImageFromServer(TeamLogo.Imagename, "images/TeamLogo-images");

                    await _TeamLogoRepository.Delete(request.TeamLogoId);
                    await _TeamLogoRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get TeamLogo Detail

        public class GetTeamLogoDetailQuery : IRequest<TeamLogoForViewDto>
        {
            public int TeamLogoId { get; set; }
        }

        public class GetTeamLogoDetailQueryHandler : IRequestHandler<GetTeamLogoDetailQuery, TeamLogoForViewDto>
        {
            private readonly ITeamLogoRepository _TeamLogoRepository;
            private readonly IMapper _mapper;

            public GetTeamLogoDetailQueryHandler(ITeamLogoRepository TeamLogoRepository, IMapper mapper)
            {
                _TeamLogoRepository = TeamLogoRepository;
                _mapper = mapper;
            }

            public async Task<TeamLogoForViewDto> Handle(GetTeamLogoDetailQuery request, CancellationToken cancellationToken)
            {
                var TeamLogo = await _TeamLogoRepository.GetById(request.TeamLogoId);
                if (TeamLogo == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<TeamLogoForViewDto>(TeamLogo);
            }
        }

        #endregion

        #region Get TeamLogo Detail For Edit

        public class GetTeamLogoDetailForEditQuery : IRequest<CreateOrEditTeamLogoDto>
        {
            public int TeamLogoId { get; set; }
        }

        public class GetTeamLogoDetailForEditQueryHandler : IRequestHandler<GetTeamLogoDetailForEditQuery, CreateOrEditTeamLogoDto>
        {
            private readonly ITeamLogoRepository _TeamLogoRepository;
            private readonly IMapper _mapper;
            public GetTeamLogoDetailForEditQueryHandler(ITeamLogoRepository TeamLogoRepository, IMapper mapper)
            {
                _TeamLogoRepository = TeamLogoRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditTeamLogoDto> Handle(GetTeamLogoDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var TeamLogo = await _TeamLogoRepository.GetById(request.TeamLogoId);
                if (TeamLogo == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditTeamLogoDto>(TeamLogo);
            }
        }

        #endregion


        #region Get TeamLogo List
        public class GetTeamLogoListQuery : IRequest<PagingHandler<TeamLogoForViewDto>>
        {
            public TeamLogoFilterInput TeamLogoFilterInput { get; set; }
        }

        public class GetTeamLogoListQueryHandler : IRequestHandler<GetTeamLogoListQuery, PagingHandler<TeamLogoForViewDto>>
        {
            private readonly ITeamLogoRepository _TeamLogoRepository;
            private readonly IPagerService<TeamLogoForViewDto, TeamLogo> _pager;
            public GetTeamLogoListQueryHandler(ITeamLogoRepository TeamLogoRepository, IPagerService<TeamLogoForViewDto, TeamLogo> pager)
            {
                _TeamLogoRepository = TeamLogoRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<TeamLogoForViewDto>> Handle(GetTeamLogoListQuery request, CancellationToken cancellationToken)
            {
                request.TeamLogoFilterInput.Take = 10;
                var TeamLogos = await Task.FromResult(_TeamLogoRepository.GetAllWithFilter(request.TeamLogoFilterInput));
                var pager = _pager.PageBuilder(TeamLogos.Count(), request.TeamLogoFilterInput.PageId, request.TeamLogoFilterInput.Take);
                return await _pager.SetItemsMapper(TeamLogos, pager);
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface ITeamLogoRepository : IRepository<TeamLogo>
        {
            IQueryable<TeamLogo> GetAllWithFilter(TeamLogoFilterInput TeamLogoFilterInput);
        }

        public class TeamLogoProfile : Profile
        {
            public TeamLogoProfile()
            {
                CreateMap<TeamLogo, CreateOrEditTeamLogoDto>();
                CreateMap<TeamLogo, CreateOrEditTeamLogoDto>().ReverseMap();

                CreateMap<TeamLogo, TeamLogoForViewDto>();
                CreateMap<TeamLogo, TeamLogoForViewDto>().ReverseMap();
            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditTeamLogoDto
        {
            public int? Id { get; set; }
            public string Imagename { get; set; } = string.Empty;
            public int? TeamId { get; set; }
            public IFormFile Image { get; set; }
        }

        public class TeamLogoForViewDto : Entity
        {
            public string Imagename { get; set; } = string.Empty;
            public int TeamId { get; set; }

            public DateTime CreateDate { get; set; }
        }

        public class TeamLogoFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int? TeamId { get; set; }
        }


        #endregion
    }
}
