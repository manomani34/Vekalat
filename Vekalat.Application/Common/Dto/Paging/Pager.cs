namespace Application.Common.Dto.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageCount, int pageId, int take, int totalCount)
        {
            if (pageId <= 1) pageId = 1;

            return new BasePaging
            {
                ActivePage = pageId,
                PageCount = pageCount,
                PageId = pageId,
                SkipEntity = (pageId - 1) * take,
                TakeEntity = take,
                StartPage = pageId - 3 <= 0 ? 1 : pageId - 3,
                EndPage = pageId + 3 > pageCount ? pageCount : pageId + 3,
                TotalCount = totalCount
            };
        }

    }
}
