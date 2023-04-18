using Vekalat.Application.Common.InfraServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Vekalat.InfraStructure.Data.Repositories.InfraRepository
{
    public class FileSaver : IFileSaver
    {
        private readonly IRandomCodeGenerator _randomCodeGenerator;

        public FileSaver(IRandomCodeGenerator randomCodeGenerator)
        {
            _randomCodeGenerator = randomCodeGenerator;
        }

        public async Task DeleteImageFromServer(string imageName, string folderName)
        {
            try
            {
                if (imageName == null) return;

                var imagePath = $"wwwroot/{folderName}/";
                var path = Path.Combine(Directory.GetCurrentDirectory(), imagePath, imageName);
                if (File.Exists(path))
                    await Task.Run(() => { File.Delete(path); });

                var thumbnailPath = $"wwwroot/{folderName}/thumbnail/";
                var thumpPath = Path.Combine(Directory.GetCurrentDirectory(), thumbnailPath, imageName);
                if (File.Exists(thumpPath))
                    await Task.Run(() => { File.Delete(thumpPath); });
            }
            catch (Exception)
            {
                throw new Exception("faild to delete image from server");
            }
        }

        public void ImageToThumbnail(string imageName, string folderName)
        {
            if (imageName == null) return;

            var imagePath = $"wwwroot/{folderName}/";
            var thumbnailPath = $"wwwroot/{folderName}/thumbnail/";

            var path = Path.Combine(Directory.GetCurrentDirectory(), imagePath, imageName);

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), thumbnailPath));



            ImageConvertor thumbnail = new();
            string thumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                thumbnailPath, imageName);
            thumbnail.Image_resize(path, thumbPath, 480);


        }

        public async Task<string> SaveImageToServer(IFormFile img, string folderName)
        {

            var imagePath = $"wwwroot/{folderName}/";
            //var imagePath = $"wwwroot/images/{folderName}/{target}/";
            try
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), imagePath));
                var imageName = _randomCodeGenerator.GuidGenerator() + Path.GetExtension(img.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), imagePath, imageName);

                using var stream = new FileStream(path, FileMode.Create);
                await img.CopyToAsync(stream);

                return imageName;
            }
            catch (Exception)
            {
                throw new Exception("faild to save image on server");
            }
        }
    }
}