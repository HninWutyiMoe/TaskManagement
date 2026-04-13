using Microsoft.AspNetCore.Http;
using MODEL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServiceFunction
{
    public interface IFileService
    {
        public Task<IEnumerable<FileDTO>> GetAllFiles();
        public Task<FileDTO> GetFilesById(Guid Id);
        Task<(bool IsSuccess, string Message, Guid FileId)> CreateFile(IFormFile file, Guid taskDetailId);

        Task<(bool IsSuccess, string Message)> UpdateFile(Guid fileId, IFormFile file);

        Task<(bool IsSuccess, string Message)> DeleteFile(Guid fileId);
        Task<(bool success, string Message)> DeleteFileByMultiple(DeleteFileDTO req);
        Task<(bool success, string Message, string? FilePath, string? FileName, string? ContentType)> DownloadFile(Guid fileId);
    }
}
