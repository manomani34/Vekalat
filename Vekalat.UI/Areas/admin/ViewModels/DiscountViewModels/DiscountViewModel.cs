using AryanShop.Application.Common.Dto.Paging;
using static AryanShop.Application.Features.DiscountFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.DiscountViewModels
{
    public class DiscountViewModel
    {
        public DiscountViewModel()
        {
            Filter = new DiscountFilterInput();
        }

        public DiscountFilterInput Filter { get; set; }
        public PagingHandler<DiscountForViewDto> PagingHandler { get; set; }
    }
}