using Amazon.S3;
using Amazon.S3.Model;
using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.UserVMs;
using ClosedXML;
using ClosedXML.Excel;
using Domain.Entities;
using System.Globalization;
using System.Net;
using WebAPI.Extensions;

namespace WebAPI.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<UserVM>> ConvertExcelFileToUser(IFormFile fileUpload)
        {
            XLWorkbook workbook;
            List<UserVM> users = new List<UserVM>();
            using (var stream = new MemoryStream())
            {
                await fileUpload.CopyToAsync(stream);
                workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var currentRow = 2;
                while (!string.IsNullOrEmpty(worksheet.Cell(currentRow, 1).GetValue<string>()))
                {
                    users.Add(new UserVM()
                    {
                        FullName = worksheet.Cell(currentRow, 1).GetValue<string>(),
                        Birthday = DateTime.ParseExact(worksheet.Cell(currentRow, 2).GetValue<string>(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Sex = worksheet.Cell(currentRow, 3).GetValue<int>(),
                        BirthPlace = worksheet.Cell(currentRow, 4).GetValue<string>(),
                        HomeTown = worksheet.Cell(currentRow, 5).GetValue<string>(),
                        NationName = worksheet.Cell(currentRow, 6).GetValue<string>(),
                        IdentityNumber = worksheet.Cell(currentRow, 7).GetValue<string>(),
                        Issue = DateTime.ParseExact(worksheet.Cell(currentRow, 8).GetValue<string>(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        PlaceOfIssue = worksheet.Cell(currentRow, 9).GetValue<string>(),
                        AccountEmail = worksheet.Cell(currentRow, 10).GetValue<string>(),
                        PhoneNumber = worksheet.Cell(currentRow, 11).GetValue<string>(),
                        OfficePhoneNumber = worksheet.Cell(currentRow, 12).GetValue<string>(),
                        PermanentAddress = worksheet.Cell(currentRow, 13).GetValue<string>(),
                        CurrentResidence = worksheet.Cell(currentRow, 14).GetValue<string>(),
                        Unit = worksheet.Cell(currentRow, 15).GetValue<string>(),
                        DepartmentId = Guid.Parse(worksheet.Cell(currentRow, 16).GetValue<string>()),
                        Title = worksheet.Cell(currentRow, 17).GetValue<string>(),
                        Position = worksheet.Cell(currentRow, 18).GetValue<string>(),
                        Degree = worksheet.Cell(currentRow, 19).GetValue<string>(),
                        AcademicRank = worksheet.Cell(currentRow, 20).GetValue<string>(),
                        TaxCode = worksheet.Cell(currentRow, 21).GetValue<string>(),
                        BankAccountNumber = worksheet.Cell(currentRow, 22).GetValue<string>(),
                        Bank = worksheet.Cell(currentRow, 23).GetValue<string>(),
                    });
                    currentRow++;
                }

                return users;
            }
        }

        public async Task<FileUploadResult> UploadFileToDOAsync(IFormFile fileUpload)
        {
            string accessKey = _configuration.GetValue<string>("DigitalOcean:AccessKey")!;
            string secretKey = _configuration.GetValue<string>("DigitalOcean:SecretKey")!;
            string bucketName = _configuration.GetValue<string>("DigitalOcean:BucketName")!;
            string fileName = fileUpload.FileName.CreateUniqueFileNameWithExtension(DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = _configuration.GetValue<string>("DigitalOcean:ServiceURL")!;

            AmazonS3Client client = new AmazonS3Client(accessKey, secretKey, config);
            PutObjectResponse response;

            FileUploadResult result = new FileUploadResult
            {
                IsSuccess = false,
                FileName = fileName,
            };

            using (var stream = new MemoryStream())
            {
                await fileUpload.CopyToAsync(stream);
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    InputStream = stream,
                    CannedACL = S3CannedACL.PublicRead,
                };
                response = await client.PutObjectAsync(request);
                stream.Close();
            }

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                result.FileLink = $"https://{bucketName}.sgp1.cdn.digitaloceanspaces.com/{fileName}";
                result.IsSuccess = true;
            }

            return result;
        }
    }
}
