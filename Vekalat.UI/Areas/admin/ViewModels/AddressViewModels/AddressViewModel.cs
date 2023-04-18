using AryanShop.Application.Common.Dto.Paging;
using static AryanShop.Application.Features.AddressFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.AddressViewModels
{
    public class AddressViewModel
    {
        public AddressViewModel()
        {
            Filter = new AddressFilterInput();
        }

        public AddressFilterInput Filter { get; set; }
        public PagingHandler<AddressForViewDto> PagingHandler { get; set; }

    }
}
