using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Vekalat.Application.Common.InfraServices
{
    public interface IFileSaver
    {
        Task<string> SaveImageToServer(IFormFile img, string folderName);
        void ImageToThumbnail(string imageName, string folderName);

        Task DeleteImageFromServer(string imageName, string folderName);
    }
}