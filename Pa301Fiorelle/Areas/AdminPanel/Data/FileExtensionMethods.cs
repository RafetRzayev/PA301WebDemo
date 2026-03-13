namespace Pa301Fiorelle.Areas.AdminPanel.Data
{
    public static class FileExtensionMethods
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool IsValidSize(this IFormFile file, long maxSizeInMb)
        {
            return file.Length <= maxSizeInMb * 1024 * 1024;
        }

        public static async Task<string> SaveFileAsync(this IFormFile file, string folderPath)
        {
            var unicalFileName = Path.GetFileNameWithoutExtension(file.FileName) + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, unicalFileName);
    
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
    
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
    
            return unicalFileName;
        }
    }
}
