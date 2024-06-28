using Application.ViewModels;
using Application.ViewModels.UserVMs;
using Microsoft.AspNetCore.Http;

namespace Application.IServices
{
    public interface IFileService
    {
        Task<FileUploadResult> UploadFileToDOAsync(IFormFile fileUpload);
        Task<List<UserVM>> ConvertExcelFileToUser(IFormFile fileUpload);
    }
}
