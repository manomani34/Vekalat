using System.Collections.Generic;

namespace Application.Common.Dto.Paging
{
    public class PagingHandler<T> : BasePaging where T : class
    {

        public IReadOnlyList<T> Items { get; set; }

       

        public PagingHandler<T> SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;

            SkipEntity = paging.SkipEntity;

            TakeEntity = paging.TakeEntity;

            ActivePage = paging.PageId;

            EndPage = paging.EndPage;

            PageCount = paging.PageCount;

            StartPage = paging.StartPage;

            return this;
        }

        public PagingHandler<T> SetProducts(IReadOnlyList<T> items)
        {
            Items = items;

            return this;
        }
    }
}
