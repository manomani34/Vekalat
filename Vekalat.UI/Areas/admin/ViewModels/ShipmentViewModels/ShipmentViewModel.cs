using AryanShop.Application.Common.Dto.Paging;
using AryanShop.Application.Features;
using static AryanShop.Application.Features.PaymentFeature;
using static AryanShop.Application.Features.ShipmentFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.ShipmentViewModels
{
    public class ShipmentViewModel
    {
        public ShipmentViewModel()
        {
            Filter = new ShipmentFilterInput();
        }
        public ShipmentFilterInput Filter { get; set; }
        public PagingHandler<ShipmentForViewDto> PagingHandler { get; set; }
    }
}