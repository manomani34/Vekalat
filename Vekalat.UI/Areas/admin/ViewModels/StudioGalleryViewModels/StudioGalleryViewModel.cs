using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.StudioGalleryFeature;

namespace AryanShop.UI.Areas.Admin.ViewModels.StudioGalleryViewModels
{
    public class StudioGalleryViewModel
    {
        public StudioGalleryViewModel()
        {
            Filter = new StudioGalleryFilterInput();
        }

        public StudioGalleryFilterInput Filter { get; set; }
        public PagingHandler<StudioGalleryForViewDto> PagingHandler { get; set; }

    }
}
