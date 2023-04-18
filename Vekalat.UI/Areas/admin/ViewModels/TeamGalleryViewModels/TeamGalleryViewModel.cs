using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.TeamGalleryFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.TeamGalleryViewModels
{
    public class TeamGalleryViewModel
    {
        public TeamGalleryViewModel()
        {
            Filter = new TeamGalleryFilterInput();
        }
        public TeamGalleryFilterInput Filter { get; set; }
        public PagingHandler<TeamGalleryForViewDto> PagingHandler { get; set; }
    }
}