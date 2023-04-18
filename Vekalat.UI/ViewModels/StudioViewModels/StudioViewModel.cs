using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.StudioFeature;

namespace Vekalat.UI.ViewModels.StudioViewModels
{
    public class StudioViewModel
    {
        public StudioViewModel()
        {
            Filter = new StudioFilterInput();
        }
        public StudioFilterInput Filter { get; set; }
        public PagingHandler<StudioForViewDto> PagingHandler { get; set; }
    }
}