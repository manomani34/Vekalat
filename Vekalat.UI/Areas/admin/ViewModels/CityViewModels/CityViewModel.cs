using AryanShop.Application.Common.Dto.Paging;
using AryanShop.core.Entities;
using static AryanShop.Application.Features.CityFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.CityViewModels
{
    public class CityViewModel
    {
        public CityViewModel()
        {
            Filter = new CityFilterInput();
        }

        public CityFilterInput Filter { get; set; }
        public PagingHandler<City> PagingHandler { get; set; }

    }
}
