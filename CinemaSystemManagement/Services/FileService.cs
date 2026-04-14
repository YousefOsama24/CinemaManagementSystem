using Microsoft.AspNetCore.Http;

namespace CinemaSystemManagement.Services
{
    public enum ImgType
    {
        Main,
        Sub,
        Actor
    }

    public class FileService
    {
        public string Upload(IFormFile file, ImgType type)
        {
            string folder = type switch
            {
                ImgType.Main => "images/movies",
                ImgType.Sub => "images/movies/sub",
                _ => "images/actors"
            };

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string path = Path.Combine(folderPath, fileName);

            using (var stream = System.IO.File.Create(path))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        public void Delete(string fileName, ImgType type)
        {
            string folder = type switch
            {
                ImgType.Main => "images/movies",
                ImgType.Sub => "images/movies/sub",
                _ => "images/actors"
            };

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}