using AryanShop.Application.Common.Helpers;
using AutoMapper;
using FluentValidation;
using Vekalat.Application.Common;
using Vekalat.Application.Common.Dto;
using Vekalat.Application.Common.Helpers;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Entities;
using Vekalat.Core.Errors;
using Vekalat.Core.Localization;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
namespace Vekalat.Application.Features
{
    public class AccountFeature
    {
        #region CQRS

        #region Login

        public class LoginCommand : IRequest<LoginResultDto>
        {
            public LoginDto Login { get; set; }
        }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
        {
            private readonly IAccountRepository _repository;
            private readonly IPasswordHashService _hashService;
            private readonly IMapper _mapper;

            public LoginCommandHandler(IAccountRepository repository, IPasswordHashService hashService, IMapper mapper)
            {
                _repository = repository;
                _hashService = hashService;
                _mapper = mapper;
            }

            public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                request.Login.Username = ExtensionHelper.ToLatingDigit(request.Login.Username);
                request.Login.Password = ExtensionHelper.ToLatingDigit(request.Login.Password);
                request.Login.Password = _hashService.EncodePasswordMD5(request.Login.Password);

                var user = await _repository.UsernameExist(request.Login.Username, request.Login.Password, cancellationToken);
                if (user == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));
                return _mapper.Map<LoginResultDto>(user);
            }

        }

        #endregion

        #region Register 

        public class RegisterCommand : IRequest
        {
            public RegisterDto RegisterDto { get; set; }
        }


        public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
        {
            private readonly IAccountRepository _repository;


            public RegisterCommandHandler(IAccountRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                if (await _repository.IsUsernameExist(request.RegisterDto.Email, cancellationToken))
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.EmailExist), Messages.EmailExist));
                }
                try
                {

                    //await _repository.Register(request.RegisterDto, cancellationToken);
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>(nameof(Messages.ExceptionHappened), Messages.ExceptionHappened));
                }
            }

        }

        #endregion


        //#region SendPassword

        //public class SendPassword : IRequest<User>
        //{
        //    public LoginDto Login { get; set; }
        //}


        //public class SendPasswordCommandHandler : IRequestHandler<SendPassword, User>
        //{
        //    private readonly IAccountRepository _repository;
        //    private readonly IMapper _mapper;
        //    public SendPasswordCommandHandler(IAccountRepository repository, IMapper mapper)
        //    {
        //        _repository = repository;
        //        _mapper = mapper;
        //    }
        //    public async Task<User> Handle(SendPassword request, CancellationToken cancellationToken)
        //    {
        //        var author = await _repository.GetUserByMobil(request.Login, cancellationToken);
        //        if (author == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>("User Not Found", "کاربر یافت نشد"));
        //        string pass = System.DateTime.Now.Second.ToString() + System.DateTime.Now.Day.ToString() + author.Mobil.Substring(5, 3);
        //        if (pass.Length > 8) pass = pass.Substring(0, 8);
        //        author.Password = pass;
        //        await _repository.SetPassword(author.Id, pass, cancellationToken);
        //        return author;
        //    }
        //}
        //#endregion

        //#region ChangePassword

        //public class ChangePasswordCommand : IRequest
        //{
        //    public ChangePasswordDto ChangePasswordDto { get; set; }
        //}


        //public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
        //{
        //    private readonly IAccountRepository _repository;

        //    public ChangePasswordCommandHandler(IAccountRepository repository)
        //    {
        //        _repository = repository;
        //    }


        //    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        //    {
        //        var userExist = await _repository.UserExistForChangePassword(request.ChangePasswordDto, cancellationToken);
        //        if (!userExist) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>("User Not Found", "کاربر یافت نشد"));
        //        var changePassResult = await _repository.ChangePassword(request.ChangePasswordDto, cancellationToken);
        //        if (!changePassResult) throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Error", "خطایی رخ داده است"));
        //        return Unit.Value;
        //    }

        //}

        //#endregion

        //#region User Profile

        //public class UserProfileQuery : IRequest<UserProfileDto>
        //{
        //    public int UserId { get; set; }
        //    public APIRequestMetadata APIRequestMetadata { get; set; }
        //}


        //public class UserProfileQueryHandler : IRequestHandler<UserProfileQuery, UserProfileDto>
        //{
        //    private readonly IAccountRepository _repository;
        //    private readonly IMapper _mapper;


        //    public UserProfileQueryHandler(IAccountRepository repository, IMapper mapper)
        //    {
        //        _repository = repository;
        //        _mapper = mapper;
        //    }

        //    public async Task<UserProfileDto> Handle(UserProfileQuery query, CancellationToken cancellationToken)
        //    {
        //        var author = await _repository.GetUserProfile(query.UserId, cancellationToken);
        //        if (author == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>("User Not Found", "کاربر یافت نشد"));
        //        var uuu = _mapper.Map<User, UserProfileDto>(author);
        //        return uuu;
        //    }
        //}
        //#endregion

        #endregion


        #region Mapper And Repo

        public interface IAccountRepository
        {
            Task<User> UsernameExist(string email, string password, CancellationToken cancellationToken);
            Task<bool> IsUsernameExist(string username, CancellationToken cancellationToken);
        }
        public class AccountProfile : Profile
        {
            public AccountProfile()
            {
                CreateMap<User, LoginResultDto>()
                    .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));
                CreateMap<UserProfileDto, User>();
                CreateMap<UserProfileDto, User>().ReverseMap();

            }
        }

        #endregion


        #region Serialization

        public class RegisterDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string RePassword { get; set; }
        }

        public class UserProfileDto
        {
            public string Mobil { get; set; }
            public string Password { get; set; }
        }
        public class ChangePasswordDto
        {
            public int UserId { get; set; }
            public string CurrentPassword { get; set; }
            public string Password { get; set; }

            [Compare("Password")]
            public string RePassword { get; set; }
        }
        public class LoginDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        #endregion

    }
}
