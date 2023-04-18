using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Entities;
using Application.Common.Dto.Paging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Vekalat.Core.Common;
using System;

namespace Vekalat.Application.Features
{
    public class UserFeature
    {
        #region CQRS

        #region Edit User

        public class EditUserCommand : IRequest
        {
            public int UserId { get; set; }
            public EditUserInfoDto EditUserInfoDto { get; set; }
            public IFormFile ImageFile { get; set; }
        }
        public class EditUserCommandHandler : IRequestHandler<EditUserCommand>
        {
            private readonly IUserRepository _repository;
            private readonly IFileSaver _fileSaver;

            public EditUserCommandHandler(IUserRepository repository, IFileSaver fileSaver)
            {
                _repository = repository;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(EditUserCommand request, CancellationToken cancellationToken)
            {
                //if (request.ImageFile != null)
                //{
                //    await _fileSaver.DeleteImageFromServer(request.User.MemerLogo, "pdf");
                //    request.User.MemerLogo = await _fileSaver.SaveImageToServer(request.ImageFile, "pdf");
                //}
                try
                {
                    var result = await _repository.EditUserInfo(request.UserId, request.EditUserInfoDto, cancellationToken);

                    if (!result) throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));

                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }
            }
        }

        #endregion

        #region Delete User

        public class DeleteUserCommand : IRequest
        {
            public int Id { get; set; }
        }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
        {
            private readonly IUserRepository _repository;
            private readonly IFileSaver _fileSaver;

            public DeleteUserCommandHandler(IUserRepository repository, IFileSaver fileSaver)
            {
                _repository = repository;
                _fileSaver = fileSaver;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {

                var item = await _repository.GetById(request.Id);
                if (item == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>("Not Found", "یافت نشد"));

                //if (item.Logo != null)
                //{
                //    await _fileSaver.DeleteImageFromServer(item.Logo, "pdf");
                //}

                try
                {
                    await _repository.Delete(request.Id);
                    await _repository.SaveChangesAsync();
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Error", "خطای ارتباط با سرور"));
                }

            }
        }

        #endregion

        #region Get User By UserType

        public class GetUserByUserTypeQuery : IRequest<List<UserDto>>
        {
            public int UserType { get; set; }
        }
        public class GetUserByUserIdQueryHandler : IRequestHandler<GetUserByUserTypeQuery, List<UserDto>>
        {

            private readonly IUserRepository _repository;
            private readonly IMapper _mapper;

            public GetUserByUserIdQueryHandler(IUserRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<UserDto>> Handle(GetUserByUserTypeQuery request, CancellationToken cancellationToken)
            {

                var user = await _repository.GetByUserType(request.UserType);
                return _mapper.Map<List<UserDto>>(user);
            }
        }
        #endregion

        #region Get User

        public class GetUserQuery : IRequest<User>
        {
            public int UserId { get; set; }
        }
        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
        {

            private readonly IUserRepository _repository;

            public GetUserQueryHandler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetById(request.UserId);
            }
        }
        #endregion

        #region check Memeber Exist
        public class DoesUserExistsQuery : IRequest<bool>
        {
            public string Mobile { get; set; }
        }
        public class DoesUserExistsQueryHandler : IRequestHandler<DoesUserExistsQuery, bool>
        {
            private readonly IUserRepository _repo;

            public DoesUserExistsQueryHandler(IUserRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(DoesUserExistsQuery request, CancellationToken cancellationToken)
            {
                return await _repo.DoesUserExist(request.Mobile);
            }
        }
        #endregion

        #region Create User

        public class CreateUserCommand : IRequest
        {
            public UserCreatDto UserCreatDto { get; set; }
            //public IFormFile ImageFile { get; set; }

        }
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
        {
            private readonly IUserRepository _repository;
            private readonly IFileSaver _fileSaver;
            private readonly IMapper _mapper;

            public CreateUserCommandHandler(IUserRepository repository, IMapper mapper, IFileSaver fileSaver)
            {
                _repository = repository;
                _fileSaver = fileSaver;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                //if (request.ImageFile != null)
                //{
                //    request.UserDto.MemerLogo = await _fileSaver.SaveImageToServer(request.ImageFile, "pdf");
                //}
                try
                {
                    var user = _mapper.Map<User>(request.UserCreatDto);
                    await _repository.InsertNew(user);
                    await _repository.SaveChangesAsync();
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }
            }
        }

        #endregion

        #region Get User For Admin

        public class GetUserForAdminQuery : IRequest<PagingHandler<UserDto>>
        {
            public string Search { get; set; }
            public int PageId { get; set; }

        }
        public class GetUserForAdminQueryHandler : IRequestHandler<GetUserForAdminQuery, PagingHandler<UserDto>>
        {

            private readonly IUserRepository _repository;
            private readonly IPagerService<UserDto, UserDto> _pager;

            public GetUserForAdminQueryHandler(IUserRepository repository, IPagerService<UserDto, UserDto> pager)
            {
                _repository = repository;
                _pager = pager;
            }

            public async Task<PagingHandler<UserDto>> Handle(GetUserForAdminQuery request, CancellationToken cancellationToken)
            {
                const int take = 10;

                var item = await Task.FromResult(_repository.GetUserForAdmin(request.Search));

                var pager = _pager.PageBuilder(item.Count(), request.PageId, take);
                return await _pager.SetItems(item, pager);
            }
        }

        #endregion

        #region Get Get Top User List

        public class GetTopUserListQuery : IRequest<List<UserDto>>
        {

        }
        public class GetTopUserListQueryHandler : IRequestHandler<GetTopUserListQuery, List<UserDto>>
        {
            private readonly IUserRepository _repository;

            public GetTopUserListQueryHandler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<UserDto>> Handle(GetTopUserListQuery request, CancellationToken cancellationToken)
            {
                var items = await Task.FromResult(_repository.GetTopUserList());
                return items.ToList();
            }
        }

        #endregion

        #region Get User List
        public class GetUserListQuery : IRequest<PagingHandler<UserForViewDto>>
        {
            public UserFilterInput UserFilterInput { get; set; }
        }

        public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PagingHandler<UserForViewDto>>
        {
            private readonly IUserRepository _repository;
            private readonly IPagerService<UserForViewDto, User> _pager;
            public GetUserListQueryHandler(IUserRepository userRepository, IPagerService<UserForViewDto, User> pager)
            {
                _repository = userRepository;
                _pager = pager;
            }
            public async Task<PagingHandler<UserForViewDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
            {
                var users = await Task.FromResult(_repository.GetAllWithFilter(request.UserFilterInput));
                var pager = _pager.PageBuilder(users.Count(), request.UserFilterInput.PageId, request.UserFilterInput.Take);
                return await _pager.SetItemsMapper(users, pager);
            }
        }
        #endregion

        #endregion


        #region Mapper And Repo

        public interface IUserRepository : IRepository<User>
        {
            IQueryable<User> GetAllWithFilter(UserFilterInput filterInput);
            Task<User> GetUserByCurrentPassword(ChangePasswordDto passwordDto, CancellationToken cancellationToken);

            IQueryable<UserDto> GetUserForAdmin(string search);
            IQueryable<UserDto> GetUserList();
            IQueryable<UserDto> GetTopUserList();
            Task<bool> DoesUserExist(string mobile);
            Task<List<User>> GetByUserType(int userType);
            Task<bool> EditUserInfo(int userId
                , EditUserInfoDto EditUserInfoDto
                , CancellationToken cancellationToken);

        }

        public class UserProfile : Profile
        {
            public UserProfile()
            {

                CreateMap<UserCreatDto, User>();
                CreateMap<UserCreatDto, User>().ReverseMap();

                CreateMap<EditUserInfoDto, User>();
                CreateMap<EditUserInfoDto, User>().ReverseMap();

                CreateMap<UserDto, User>();
                CreateMap<UserDto, User>().ReverseMap();


                CreateMap<UserForViewDto, User>();
                CreateMap<UserForViewDto, User>().ReverseMap();

            }
        }

        #endregion


        #region Serialization

        public class UserDto
        {
            public int Userid { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Mobil { get; set; }
        }

        public class UserCreatDto
        {
            public int UserId { get; set; }
            public int Userid { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string Tel { get; set; }
            public string PostCode { get; set; }
            public string Email { get; set; }
            public string Mobil { get; set; }
            public int? cityid { get; set; }
            public int? ostanid { get; set; }
            public string Title { get; set; }
            public string RegisterDate { get; set; }
            public string PassWord { get; set; }
        }

        public class EditUserInfoDto
        {
            public int UserId { get; set; }
            [Display(Name = "نام نماینده")]
            [MaxLength(50)]
            [Required(ErrorMessage = "نباید بدون مقدار باشد!")]
            public string FirstName { get; set; }
            [Display(Name = "نام خانوادگی نماینده")]
            [MaxLength(50)]
            [Required(ErrorMessage = "نباید بدون مقدار باشد!")]
            public string LastName { get; set; }
            [Display(Name = "آدرس")]
            [MaxLength(150)]
            [Required(ErrorMessage = "نباید بدون مقدار باشد!")]
            public string Address { get; set; }
            [Display(Name = "تلفن")]
            [MaxLength(11)]
            [Required(ErrorMessage = "نباید بدون مقدار باشد!")]
            public string Tel { get; set; }
            [Display(Name = "کد پستی")]
            [MaxLength(10)]
            [Required(ErrorMessage = "نباید بدون مقدار باشد!")]
            public string PostCode { get; set; }
            [Display(Name = "ایمیل")]
            public string Email { get; set; }
            [Display(Name = "شهر")]
            public int? cityid { get; set; }
            [Display(Name = "استان")]
            public int? ostanid { get; set; }
            [Display(Name = "عنوان حقوقی")]
            [MaxLength(100)]
            [Required(ErrorMessage = "نباید بدون مقدار باشد!")]
            public string Title { get; set; }
        }


        public class CreateOrEditUserDto : Entity
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class UserForViewDto : Entity
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Mobil { get; set; }
            public DateTime? CreationTime { get; set; }
        }

        public class UserFilterInput : PaginationInput
        {
            public string SearchFilter { get; set; } = string.Empty;
            public int RoleTypeFilter { get; set; } = -1;
        }
        public class ChangePasswordDto
        {
            public int UserId { get; set; }

            //[Display(Name = "کلمه عبور فعلی")]
            //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
            //[StringLength(30, MinimumLength = 5, ErrorMessage = "کلمه عبور میتواند بین 5 تا 30 کارکتر باشد")]
            public string CurrentPassword { get; set; }

            //[Display(Name = "کلمه عبور")]
            //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
            //[StringLength(30, MinimumLength = 5, ErrorMessage = "کلمه عبور میتواند بین 5 تا 30 کارکتر باشد")]
            public string Password { get; set; }

            //[Display(Name = "تکرار کلمه عبور")]
            //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
            //[Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
            //[StringLength(30, MinimumLength = 5, ErrorMessage = "کلمه عبور میتواند بین 5 تا 30 کارکتر باشد")]
            public string RePassword { get; set; }
        }

        #endregion


    }
}
