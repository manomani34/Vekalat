using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.TeamFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.TeamViewModels
{
    public class TeamViewModel
    {
        public TeamViewModel()
        {
            Filter = new TeamFilterInput();
        }

        public TeamFilterInput Filter { get; set; }
        public PagingHandler<TeamForViewDto> PagingHandler { get; set; }
    }
}