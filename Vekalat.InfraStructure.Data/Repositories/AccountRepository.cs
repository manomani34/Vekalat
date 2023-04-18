using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.AccountFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly VekalatDataContext _context;
        public AccountRepository(VekalatDataContext context)
        {
            _context = context;
        }

        //public async Task<User> UserNameExist(LoginDto login, CancellationToken cancellationToken)
        //{
        //    return await _context.Users.SingleOrDefaultAsync(u => u.Mobil == login.UserName && u.Password == login.Password,cancellationToken);
        //}

        //public async Task<User> GetUserByMobil(LoginDto login, CancellationToken cancellationToken)
        //{
        //    return await _context.Users.Where(u => u.Mobil == login.UserName).SingleOrDefaultAsync(cancellationToken);
        //}


        //public async Task<User> GetUserProfile(int authorId, CancellationToken cancellationToken)
        //{
        //    return await _context.Users.Where(u => u.Userid == authorId).Select(u => new User()
        //    {
        //        Userid  = u.Userid,
        //        FirstName = u.FirstName + " " + u.LastName,
        //    }).SingleOrDefaultAsync(cancellationToken);
        //}

        public async Task<bool> UserExistForChangePassword(ChangePasswordDto changePasswordDto
            , CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(u =>
                u.Id == changePasswordDto.UserId && u.Password == changePasswordDto.CurrentPassword, cancellationToken);
        }
        public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            try
            {
                var author = await _context.Users.SingleOrDefaultAsync(u => u.Id == changePasswordDto.UserId && u.Password == changePasswordDto.CurrentPassword, cancellationToken);
                author.Password = changePasswordDto.Password;
                _context.Update(author);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SetPassword(int Id, string NewPassword, CancellationToken cancellationToken)
        {
            try
            {
                var author = await _context.Users.SingleOrDefaultAsync(u => u.Id == Id, cancellationToken);
                author.Password = NewPassword;
                _context.Update(author);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> GetUserProfile(int authorId, CancellationToken cancellationToken)
        {
            return await _context.Users.Where(u => u.Id == authorId)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsUsernameExist(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(c => c.Email == username, cancellationToken);
        }

        public async Task<User> UsernameExist(string email, string password, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(c => c.Mobil == email && c.Password == password, cancellationToken);
        }
    }
}
