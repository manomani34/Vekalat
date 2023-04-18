using Application.Common.Dto.Paging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vekalat.Application.Common.InfraServices
{
    public interface IPagerService<TOut, TIn> where TOut : class where TIn : class
    {
        BasePaging PageBuilder(int count, int pageId, int take);
        Task<PagingHandler<TOut>> SetItemsMapper(IQueryable<TIn> items, BasePaging pager);
        Task<PagingHandler<TOut>> SetItems(IQueryable<TOut> items, BasePaging pager);
        PagingHandler<TOut> SetListItems(List<TOut> items, BasePaging pager);
    }
}