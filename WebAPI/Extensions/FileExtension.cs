namespace WebAPI.Extensions
{
    public static class FileExtension
    {
        public static async Task<byte[]> ConvertToByteArrayAsync(this IFormFile formFile)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                await formFile.CopyToAsync(memory);
                return memory.ToArray();
            }
        }

        public static string CreateUniqueFileNameWithExtension(this string fileName, string uniqueString)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileExtension = Path.GetExtension(fileName);
            return string.Concat(fileNameWithoutExtension + "-", uniqueString, fileExtension);
        }
    }
}
