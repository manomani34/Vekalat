using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Entities;
using Application.Common.Dto.Paging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System;
using Vekalat.Core.Common;
using Vekalat.Core.Localization;
using static Vekalat.Application.Features.TeamGalleryFeature;
using static Vekalat.Application.Features.TeamLogoFeature;

namespace Vekalat.Application.Features
{
    public class TeamFeature
    {
        #region CQRS

        #region Create Team
        public class CreateTeamCommand : IRequest
        {
            public CreateOrEditTeamDto CreateOrEditTeamDto { get; set; }
        }

        public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand>
        {

            private readonly ITeamRepository _TeamRepository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;
            public CreateTeamCommandHandler(IFileSaver fileSaver, ITeamRepository TeamRepository, IMapper mapper)
            {
                _fileSaver = fileSaver;
                _TeamRepository = TeamRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    request.CreateOrEditTeamDto.Imagename = "no-image";
                    if (request.CreateOrEditTeamDto.Image != null)
                    {
                        request.CreateOrEditTeamDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditTeamDto.Image, "images/Team-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditTeamDto.Imagename, "images/Team-images");
                    }

                    var Team = _mapper.Map<Team>(request.CreateOrEditTeamDto);

                    await _TeamRepository.InsertNew(Team);
                    await _TeamRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Edit Team

        public class EditTeamCommand : IRequest
        {
            public CreateOrEditTeamDto CreateOrEditTeamDto { get; set; }

        }

        public class EditTeamCommandHandler : IRequestHandler<EditTeamCommand>
        {
            private readonly ITeamRepository _TeamRepository;
            private readonly IMapper _mapper;
            private readonly IFileSaver _fileSaver;

            public EditTeamCommandHandler(ITeamRepository TeamRepository, IMapper mapper, IFileSaver fileSaver)
            {
                _TeamRepository = TeamRepository;
                _mapper = mapper;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditTeamCommand request, CancellationToken cancellationToken)
            {
                var Team = await _TeamRepository.GetById(request.CreateOrEditTeamDto.Id.Value);

                if (Team == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {
                    if (request.CreateOrEditTeamDto.Image != null)
                    {
                        if (Team.Imagename != "no-image") await _fileSaver.DeleteImageFromServer(Team.Imagename, "images/Team-images");
                        request.CreateOrEditTeamDto.Imagename = await _fileSaver.SaveImageToServer(request.CreateOrEditTeamDto.Image, "images/Team-images");
                        _fileSaver.ImageToThumbnail(request.CreateOrEditTeamDto.Imagename, "images/Team-images");
                    }


                    var mappedTeam = _mapper.Map<Team>(request.CreateOrEditTeamDto);
                    mappedTeam.Imagename = request.CreateOrEditTeamDto.Imagename ?? Team.Imagename;

                    await _TeamRepository.Update(mappedTeam);
                    await _TeamRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }

            }
        }

        #endregion

        #region Delete Team

        public class DeleteTeamCommand : IRequest
        {
            public int TeamId { get; set; }
        }

        public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
        {
            private readonly ITeamRepository _TeamRepository;
            private readonly IFileSaver _fileSaver;
            public DeleteTeamCommandHandler(IFileSaver fileSaver, ITeamRepository TeamRepository)
            {
                _fileSaver = fileSaver;
                _TeamRepository = TeamRepository;
            }

            public async Task<Unit> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
            {
                var Team = await _TeamRepository.GetById(request.TeamId);

                if (Team == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                try
                {

                    await _fileSaver.DeleteImageFromServer(Team.Imagename, "images/Team-images");

                    await _TeamRepository.SoftDelete(request.TeamId);
                    await _TeamRepository.SaveChangesAsync();

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }


            }
        }

        #endregion

        #region Get Team Detail

        public class GetTeamDetailQuery : IRequest<TeamForViewDto>
        {
            public int TeamId { get; set; }
        }

        public class GetTeamDetailQueryHandler : IRequestHandler<GetTeamDetailQuery, TeamForViewDto>
        {
            private readonly ITeamRepository _TeamRepository;
            private readonly IMapper _mapper;
            private readonly ITeamGalleryRepository _teamGalleryRepository;
            private readonly ITeamLogoRepository _teamLogoRepository;

            public GetTeamDetailQueryHandler(ITeamRepository TeamRepository
                , IMapper mapper
                , ITeamGalleryRepository teamGalleryRepository
                , ITeamLogoRepository teamlogoRepository)
            {
                _TeamRepository = TeamRepository;
                _mapper = mapper;
                _teamGalleryRepository = teamGalleryRepository;
                _teamLogoRepository = teamlogoRepository;
            }

            public async Task<TeamForViewDto> Handle(GetTeamDetailQuery request, CancellationToken cancellationToken)
            {
                var Team = await _TeamRepository.GetById(request.TeamId);
                var mappedTeam = _mapper.Map<TeamForViewDto>(Team);
                var galleies = _teamGalleryRepository.GetAllWithFilter(new TeamGalleryFilterInput
                {
                    TeamId = mappedTeam.Id,
                    Take = 99999
                });
                mappedTeam.Galleries = _mapper.Map<List<TeamGalleryForViewDto>>(galleies.ToList());

                var logos = _teamLogoRepository.GetAllWithFilter(new TeamLogoFilterInput
                {
                    TeamId = mappedTeam.Id,
                    Take = 99999
                });
                mappedTeam.Logos = _mapper.Map<List<TeamLogoForViewDto>>(logos.ToList());


                if (mappedTeam == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<TeamForViewDto>(mappedTeam);
            }
        }

        #endregion

        #region Get Team Detail For Edit

        public class GetTeamDetailForEditQuery : IRequest<CreateOrEditTeamDto>
        {
            public int TeamId { get; set; }
        }

        public class GetTeamDetailForEditQueryHandler : IRequestHandler<GetTeamDetailForEditQuery, CreateOrEditTeamDto>
        {
            private readonly ITeamRepository _TeamRepository;
            private readonly IMapper _mapper;
            public GetTeamDetailForEditQueryHandler(ITeamRepository TeamRepository, IMapper mapper)
            {
                _TeamRepository = TeamRepository;
                _mapper = mapper;
            }

            public async Task<CreateOrEditTeamDto> Handle(GetTeamDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var Team = await _TeamRepository.GetById(request.TeamId);
                if (Team == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

                return _mapper.Map<CreateOrEditTeamDto>(Team);
            }
        }

        #endregion

        #region Get Team List
        public class GetTeamListQuery : IRequest<PagingHandler<TeamForViewDto>>
        {
            public TeamFilterInput TeamFilterInput { get; set; }
        }

        public class GetTeamListQueryHandler : IRequestHandler<GetTeamListQuery, PagingHandler<TeamForViewDto>>
        {
            public ITeamRepository _TeamRepository { get; set; }
            private readonly IPagerService<TeamForViewDto, Team> _pager;
            public GetTeamListQueryHandler(ITeamRepository TeamRepository, IPagerService<TeamForViewDto, Team> pager)
            {
                _TeamRepository = TeamRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<TeamForViewDto>> Handle(GetTeamListQuery request, CancellationToken cancellationToken)
            {
                request.TeamFilterInput.Take = 10;
                var Teams = await Task.FromResult(_TeamRepository.GetAllWithFilter(request.TeamFilterInput));
                var pager = _pager.PageBuilder(Teams.Count(), request.TeamFilterInput.PageId, request.TeamFilterInput.Take);
                return await _pager.SetItemsMapper(Teams, pager);
            }
        }
        #endregion

        #endregion

        #region Mapper And Repo

        public interface ITeamRepository : IRepository<Team>
        {
            IQueryable<Team> GetAllWithFilter(TeamFilterInput TeamFilterInput);
        }

        public class TeamProfile : Profile
        {
            public TeamProfile()
            {
                CreateMap<Team, CreateOrEditTeamDto>();
                CreateMap<Team, CreateOrEditTeamDto>().ReverseMap();

                CreateMap<Team, TeamForViewDto>();
                CreateMap<Team, TeamForViewDto>().ReverseMap();
            }
        }

        #endregion

        #region Serialization


        public class CreateOrEditTeamDto
        {
            public int? Id { get; set; }
            public IFormFile Image { get; set; }
            public string FirstName { get; set; }
            public string SurName { get; set; }
            public string Imagename { get; set; }
            public string Description { get; set; }
            public string Socials { get; set; }
            public string Tel { get; set; }
            public string Email { get; set; }
            public string Job { get; set; }
            public bool IsVisible { get; set; }

        }

        public class TeamForViewDto : Entity
        {
            public string FirstName { get; set; }
            public string SurName { get; set; }
            public string Imagename { get; set; }
            public string Description { get; set; }
            public string Socials { get; set; }
            public string Tel { get; set; }
            public string Email { get; set; }
            public string Job { get; set; }
            public bool IsVisible { get; set; }
            public List<TeamGalleryForViewDto> Galleries { get; set; }
            public List<TeamLogoForViewDto> Logos { get; set; }

        }

        public class TeamFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
        }


        #endregion

    }
}
