using AryanShop.Application.Common.Dto.Paging;
using AryanShop.core.Entities;
using static AryanShop.Application.Features.ProvinceFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.ProvinceViewModels
{
    public class ProvinceViewModel
    {
        public ProvinceViewModel()
        {
            Filter = new ProvinceFilterInput();
        }

        public ProvinceFilterInput Filter { get; set; }
        public PagingHandler<Province> PagingHandler { get; set; }

    }
}
