using AryanShop.Application.Common.Dto.Paging;
using static AryanShop.Application.Features.VoucherFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.VoucherViewModels
{
    public class VoucherViewModel
    {
        public VoucherViewModel()
        {
            Filter = new VoucherFilterInput();

        }
        public VoucherFilterInput Filter { get; set; }
        public PagingHandler<VoucherForViewDto> PagingHandler { get; set; }
    }
}