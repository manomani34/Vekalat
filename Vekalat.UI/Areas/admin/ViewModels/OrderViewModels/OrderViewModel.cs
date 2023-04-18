using AryanShop.Application.Common.Dto.Paging;
using static AryanShop.Application.Features.OrderFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.UserViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            Filter = new OrderFilterInput();
        }

        public OrderFilterInput Filter { get; set; }
        public PagingHandler<OrderForViewDto> PagingHandler { get; set; }

    }
}
