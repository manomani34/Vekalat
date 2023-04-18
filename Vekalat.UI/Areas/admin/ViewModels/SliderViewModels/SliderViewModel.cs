using AryanShop.Application.Common.Dto.Paging;
using AryanShop.Application.Features;
using static AryanShop.Application.Features.SliderFeature;
using static AryanShop.Application.Features.UserFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.SliderViewModels
{
    public class SliderViewModel
    {
        public SliderViewModel()
        {
            Filter = new SliderFilterInput();
        }

        public SliderFilterInput Filter { get; set; }
        public PagingHandler<SliderForViewDto> PagingHandler { get; set; }
    }
}