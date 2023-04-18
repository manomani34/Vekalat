using AryanShop.Application.Common.Dto.Paging;
using AryanShop.core.Entities;
using static AryanShop.Application.Features.PaymentFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.PaymentViewModels
{
    public class PaymentViewModel
    {
        public PaymentViewModel()
        {
            Filter = new PaymentFilterInput();
        }

        public PaymentFilterInput Filter { get; set; }
        public PagingHandler<PaymentForViewDto> PagingHandler { get; set; }
    }
}