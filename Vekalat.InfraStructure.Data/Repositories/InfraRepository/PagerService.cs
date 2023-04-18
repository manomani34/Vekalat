using AutoMapper;
using Application.Common.Dto.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vekalat.Application.Common.InfraServices;

namespace Vekalat.InfraStructure.Data.Repositories.InfraRepository
{
    public class PagerService<TOut, TIn> : IPagerService<TOut, TIn> where TOut : class where TIn : class
    {
        private readonly IMapper _mapper;
        public PagerService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public BasePaging PageBuilder(int count, int pageId, int take)
        {
            var itemCount = (int)Math.Ceiling(count / (double)take);
            return Pager.Build(itemCount, pageId, take, count);
        }

        public async Task<PagingHandler<TOut>> SetItems(IQueryable<TOut> items, BasePaging pager)
        {
            var pagingHandler = new PagingHandler<TOut>() { TotalCount = pager.TotalCount };
            var result = await items.Skip(pager.SkipEntity).Take(pager.TakeEntity).ToListAsync();
            IReadOnlyList<TOut> readOnlyListItems = result.AsReadOnly();
            return pagingHandler.SetProducts(readOnlyListItems).SetPaging(pager);
        }
        public PagingHandler<TOut> SetListItems(List<TOut> items, BasePaging pager)
        {
            var pagingHandler = new PagingHandler<TOut>() { TotalCount = pager.TotalCount };
            IReadOnlyList<TOut> readOnlyListItems = items.AsReadOnly();
            return pagingHandler.SetProducts(readOnlyListItems).SetPaging(pager);
        }

        public async Task<PagingHandler<TOut>> SetItemsMapper(IQueryable<TIn> items, BasePaging pager)
        {
            var pagingHandler = new PagingHandler<TOut>() { TotalCount = pager.TotalCount };
            var result = await items.Skip(pager.SkipEntity).Take(pager.TakeEntity).ToListAsync();
            var mapperItems = _mapper.Map<IReadOnlyList<TIn>, IReadOnlyList<TOut>>(result);

            return pagingHandler.SetProducts(mapperItems).SetPaging(pager);
        }

    }
}
