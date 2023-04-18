namespace Vekalat.Application.Common.InfraServices
{
    public interface IPasswordHashService
    {
        string EncodePasswordMD5(string pass);
    }
}