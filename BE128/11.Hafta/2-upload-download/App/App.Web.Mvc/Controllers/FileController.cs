using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
    public class FileController : Controller
    {
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile uploadedFile) // IFormFile -> yüklenecek dosyayı ifade eder
        {

            // 1) 
            //string path = "C:\\BE128";

            // 2)
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullFilePath = Path.Combine(path, uploadedFile.FileName);

            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                uploadedFile.CopyTo(fileStream); // yüklenen dosyanın byte'larını parça parça stream'e kopyala
                fileStream.Flush(); // önbellekte olan byte'ları stream'e yaz
                fileStream.Close(); // stream'i kapat
            }

            return View();
        }

        [HttpGet]
        public IActionResult FileList()
        {
            // dosya yolu
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // bu dosya yolundaki dosyaların listesini al
            var files = Directory.GetFiles(path);

            var fileNames = files.Select(Path.GetFileName);

            // dosya adlarını view'a gönder
            return View(fileNames);
        }
        [HttpGet]
        public IActionResult Download(string fileName)
        {
            // dosya yolu
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(path)) // klasör yoksa
            {
                return NotFound();
            }

            string fullFilePath = Path.Combine(path, fileName);

            if (!System.IO.File.Exists(fullFilePath)) // dosya yoksa
            {
                return NotFound();
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fullFilePath);

            // ilk parametre indirilecek olan dosyanın byte'ları
            // ikinci parametre mime-type, content-type
            // üçüncü parametre -> indiren kişi hangi isimde indirsin.

            return File(fileBytes, "image/jpeg", "download.jpg" );
        }
    }
}
