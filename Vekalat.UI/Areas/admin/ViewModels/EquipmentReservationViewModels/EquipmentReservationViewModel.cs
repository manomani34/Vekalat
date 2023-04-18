using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.EquipmentReservationFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.EquipmentReservationViewModels
{
    public class EquipmentReservationViewModel
    {
        public EquipmentReservationViewModel()
        {
            Filter = new EquipmentReservationFilterInput();
        }
        public EquipmentReservationFilterInput Filter { get; set; }
        public PagingHandler<EquipmentReservationForViewDto> PagingHandler { get; set; }
    }
}