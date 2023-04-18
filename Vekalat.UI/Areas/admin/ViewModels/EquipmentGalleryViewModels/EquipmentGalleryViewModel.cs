using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.EquipmentGalleryFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.EquipmentGalleryViewModels
{
    public class EquipmentGalleryViewModel
    {
        public EquipmentGalleryViewModel()
        {
            Filter = new EquipmentGalleryFilterInput();
        }
        public EquipmentGalleryFilterInput Filter { get; set; }
        public PagingHandler<EquipmentGalleryForViewDto> PagingHandler { get; set; }
    }
}