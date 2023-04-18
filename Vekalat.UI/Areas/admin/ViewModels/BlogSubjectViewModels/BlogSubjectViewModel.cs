using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.BlogSubjectFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.BlogSubjectViewModels
{
    public class BlogSubjectViewModel
    {
        public BlogSubjectViewModel()
        {
            Filter = new BlogSubjectFilterInput();
        }

        public BlogSubjectFilterInput Filter { get; set; }
        public PagingHandler<BlogSubjectForViewDto> PagingHandler { get; set; }
    }
}