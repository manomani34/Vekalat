using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.BrandFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.BrandViewModels
{
    public class BrandViewModel
    {
        public BrandViewModel()
        {
            Filter = new BrandFilterInput();
        }

        public BrandFilterInput Filter { get; set; }
        public PagingHandler<BrandForViewDto> PagingHandler { get; set; }
    }
}