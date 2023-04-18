using Application.Common.Dto.Paging;
using static Vekalat.Application.Features.UserFeature;

namespace Vekalat.UI.Areas.admin.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Filter = new UserFilterInput();
        }

        public UserFilterInput Filter { get; set; }
        public PagingHandler<UserForViewDto> PagingHandler { get; set; }

    }
}
