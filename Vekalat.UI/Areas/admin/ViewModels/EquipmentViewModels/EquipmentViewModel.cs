using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.EquipmentFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.EquipmentViewModels
{
    public class EquipmentViewModel
    {
        public EquipmentViewModel()
        {
            Filter = new EquipmentFilterInput();
        }
        public EquipmentFilterInput Filter { get; set; }
        public PagingHandler<EquipmentForViewDto> PagingHandler { get; set; }
    }
}