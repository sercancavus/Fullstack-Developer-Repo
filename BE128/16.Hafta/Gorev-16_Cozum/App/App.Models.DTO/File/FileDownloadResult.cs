namespace App.Models.DTO.File
{
    public class FileDownloadResult
    {
        public string Name { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] Content { get; set; } = null!;
    }
}