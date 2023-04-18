using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.EquipmentItemFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.EquipmentItemViewModels
{
    public class EquipmentItemViewModel
    {
        public EquipmentItemViewModel()
        {
            Filter = new EquipmentItemFilterInput();
        }
        public EquipmentItemFilterInput Filter { get; set; }
        public PagingHandler<EquipmentItemForViewDto> PagingHandler { get; set; }
    }
}