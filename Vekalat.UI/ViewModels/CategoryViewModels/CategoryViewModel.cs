using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.CategoryFeature;

namespace Vekalat.UI.ViewModels.CategoryViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            Filter = new CategoryFilterInput();
        }
        public CategoryFilterInput Filter { get; set; }
        public PagingHandler<CategoryForViewDto> PagingHandler { get; set; }
    }
}