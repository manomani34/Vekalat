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

namespace Vekalat.Application.Features
{
    public class CityFeature
    {

        #region CQRS

        public class GetAllQuery : IRequest<PagingHandler<CityDto>>
        {
            public int PageId;
            public string Title;
        }
       
        public class GetAllQueryHandler : IRequestHandler<GetAllQuery, PagingHandler<CityDto>>
        {

            private readonly ICityRepository _repository;
            private readonly IPagerService<CityDto, CityDto> _pager;

            public GetAllQueryHandler(ICityRepository repository, IPagerService<CityDto, CityDto> pager)
            {
                _repository = repository;
                _pager = pager;
            }

            public async Task<PagingHandler<CityDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                const int take = 15;
                var item = await Task.FromResult(_repository.GetCityQuery(request.Title));
                var pager = _pager.PageBuilder(item.Count(), request.PageId, take);
                return await _pager.SetItems(item, pager);
            }
        }


        public class GetAllData : IRequest<ICollection<City>>
        {
        }

        public class GetAllDataHandler : IRequestHandler<GetAllData, ICollection<City>>
        {

            private readonly ICityRepository _repository;

            public GetAllDataHandler(ICityRepository repository)
            {
                _repository = repository;
            }

            public async Task<ICollection<City>> Handle(GetAllData request, CancellationToken cancellationToken)
            {
                var items = await _repository.GetAll();
                return items;
            }
        }


        #endregion


        #region Mapper And Repo

        public interface ICityRepository : IRepository<City>
        {
            IQueryable<CityFeature.CityDto> GetCityQuery(string Title);

        }

        public class CityDtoProfile : Profile
        {
            public CityDtoProfile()
            {
              
            }
        }

        #endregion


        #region Serialization

        public class CityDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }


        #endregion


        #region PipelineBehaviors



        #endregion

    }
}
