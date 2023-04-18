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
    public class CountryFeature
    {

        #region CQRS

        public class GetAllQuery : IRequest<PagingHandler<CountryDto>>
        {
            public int PageId;
            public string Title;
        }
       
        public class GetAllQueryHandler : IRequestHandler<GetAllQuery, PagingHandler<CountryDto>>
        {

            private readonly ICountryRepository _repository;
            private readonly IPagerService<CountryDto, CountryDto> _pager;

            public GetAllQueryHandler(ICountryRepository repository, IPagerService<CountryDto, CountryDto> pager)
            {
                _repository = repository;
                _pager = pager;
            }

            public async Task<PagingHandler<CountryDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                const int take = 15;
                var item = await Task.FromResult(_repository.GetCountryQuery(request.Title));
                var pager = _pager.PageBuilder(item.Count(), request.PageId, take);
                return await _pager.SetItems(item, pager);
            }
        }


        public class GetAllData : IRequest<ICollection<Country>>
        {
        }

        public class GetAllDataHandler : IRequestHandler<GetAllData, ICollection<Country>>
        {

            private readonly ICountryRepository _repository;

            public GetAllDataHandler(ICountryRepository repository)
            {
                _repository = repository;
            }

            public async Task<ICollection<Country>> Handle(GetAllData request, CancellationToken cancellationToken)
            {
                var items = await _repository.GetAll();
                return items;
            }
        }


        #endregion


        #region Mapper And Repo

        public interface ICountryRepository : IRepository<Country>
        {
            IQueryable<CountryFeature.CountryDto> GetCountryQuery(string Title);

        }

        public class CountryDtoProfile : Profile
        {
            public CountryDtoProfile()
            {
              
            }
        }

        #endregion


        #region Serialization

        public class CountryDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }


        #endregion


        #region PipelineBehaviors



        #endregion

    }
}
