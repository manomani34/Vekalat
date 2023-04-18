using AryanShop.Application.Common.Dto.Paging;
using AryanShop.Application.Features;

namespace AryanShop.UI.Areas.Admin.ViewModels.VoucherHistoryViewModels
{
    internal class VoucherHistoryViewModel
    {
        public VoucherHistoryFeature.VoucherHistoryFilterInput Filter { get; set; }
        public PagingHandler<VoucherHistoryFeature.VoucherHistoryForViewDto> PagingHandler { get; set; }
    }
}