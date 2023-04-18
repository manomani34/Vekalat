using Application.Common.Dto.Paging;
using System.Collections.Generic;
using static Vekalat.Application.Features.CategoryFeature;
using static Vekalat.Application.Features.EquipmentFeature;

namespace Vekalat.UI.ViewModels.EquipmentViewModels
{
    public class EquipmentViewModel
    {
        public EquipmentViewModel()
        {
            Filter = new EquipmentFilterInput();
        }
        public EquipmentFilterInput Filter { get; set; }
        public PagingHandler<EquipmentForViewDto> PagingHandler { get; set; }
        public IReadOnlyList<CategoryForViewDto> Categories { get; set; }
    }
}