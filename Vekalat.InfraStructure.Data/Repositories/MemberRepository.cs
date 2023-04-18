using Vekalat.Core.Entities;
using Vekalat.Application.Features;
using Vekalat.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.UserFeature;
using Vekalat.Application.Enums;

namespace Ehda.InfraStructure.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly VekalatDataContext _context;
        public UserRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

       

        public IQueryable<UserFeature.UserDto> GetUserForAdmin(string search)
        {
            var result = _context.Users.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(s => EF.Functions.Like(s.Firstname, $"%{search}%"));
            }
            return result.OrderByDescending(c => c.Id).Select(c => new UserFeature.UserDto()
            {
                Userid = c.Id,
                FirstName = c.Firstname,
                LastName = c.Lastname,
                Mobil = c.Mobil,
            });
        }

        public IQueryable<UserFeature.UserDto> GetUserList()
        {
            var result = _context.Users.AsNoTracking().AsQueryable();
            return result.OrderByDescending(c => c.Id).Select(c => new UserFeature.UserDto()
            {
                Userid = c.Id,
                FirstName = c.Firstname,
                LastName = c.Lastname,
                Mobil = c.Mobil,
            });
        }

        public IQueryable<UserFeature.UserDto> GetTopUserList()
        {
            var result = _context.Users.AsNoTracking().AsQueryable();
            return result.OrderByDescending(c => c.Id).Take(6).Select(c => new UserFeature.UserDto()
            {
                Userid = c.Id,
                FirstName = c.Firstname,
                LastName = c.Lastname,
                Mobil = c.Mobil,
            });
        }

        public async Task<bool> DoesUserExist(string mobile)
        {
            try
            {
                var result = await _context.Users.FirstOrDefaultAsync(u => u.Mobil == mobile);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> EditUserInfo(int userId, UserFeature.EditUserInfoDto EditUserInfoDto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return false;

                user.Firstname = EditUserInfoDto.FirstName;
                user.Lastname = EditUserInfoDto.LastName;
                //user.ostanid = EditUserInfoDto.ostanid;
                //user.cityid = EditUserInfoDto.cityid;
                user.Address = EditUserInfoDto.Address;
                user.Tel = EditUserInfoDto.Tel;
                user.PostCode = EditUserInfoDto.PostCode;
                _context.Update(user);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<User>> GetByUserType(int userType)
        {
            return await _context.Users.Where(c => c.UserType == (UserType)userType).ToListAsync();
        }

        public IQueryable<User> GetAllWithFilter(UserFilterInput filterInput)
        {
            var items = _context.Users
               .OrderByDescending(e => e.Id)
            .AsQueryable();

            if (!string.IsNullOrEmpty(filterInput.SearchFilter))
                items = items.Where(e => e.Email.Contains(filterInput.SearchFilter));
            return items;
        }

        public async Task<User> GetUserByCurrentPassword(ChangePasswordDto passwordDto, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(c => c.Id == passwordDto.UserId && c.Password == passwordDto.CurrentPassword, cancellationToken);
        }
    }
}
