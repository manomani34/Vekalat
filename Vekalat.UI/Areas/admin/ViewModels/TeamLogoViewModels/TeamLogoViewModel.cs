using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.TeamLogoFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.TeamLogoViewModels
{
    public class TeamLogoViewModel
    {
        public TeamLogoViewModel()
        {
            Filter = new TeamLogoFilterInput();
        }
        public TeamLogoFilterInput Filter { get; set; }
        public PagingHandler<TeamLogoForViewDto> PagingHandler { get; set; }
    }
}