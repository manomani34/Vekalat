using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.BlogFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.BlogViewModels
{
    public class BlogViewModel
    {
        public BlogViewModel()
        {
            Filter = new BlogFilterInput();
        }

        public BlogFilterInput Filter { get; set; }
        public PagingHandler<BlogForViewDto> PagingHandler { get; set; }
    }
}