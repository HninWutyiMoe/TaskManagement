using BAL.IServiceFunction;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using MODEL.DTO;
using MODEL.Eneities;
using REPOSITORY.UnitOfWork;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ServiceFunction
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private const long MaxFileSize = 5 * 1024 * 1024;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };

        public async Task<IEnumerable<FileDTO>> GetAllFiles()
        {
            var files = await _unitOfWork.UploadedFile.GetAll();
            var taskDetails = await _unitOfWork.TaskDetail.GetAll();
            var users = await _unitOfWork.User.GetAll();

            if (files == null || taskDetails == null || users == null)
            {
                return new List<FileDTO>();
            }
            var fileList = from f in files
                           join td in taskDetails on f.TaskDetailId equals td.TaskDetailId into fd
                           from taskDetail in fd.DefaultIfEmpty()
                           join u in users on (taskDetail != null ? taskDetail.UserId : Guid.Empty) equals u.UserId into tu
                           from user in tu.DefaultIfEmpty()
                           select new FileDTO
                           {
                               FileId = f.FileId,
                               FileUrl = f.FileUrl,
                               Filename=f.FileName,
                               ContentType=f.ContentType,
                               AssignDetails = new List<AssignDetails>
                               {
                                   new AssignDetails
                                   {
                                        TaskDetailId = taskDetail != null ? taskDetail.TaskDetailId : Guid.Empty,
                                        TaskId = taskDetail?.TaskId,
                                        LineNumber = taskDetail?.LineNumber,
                                        ItemTitle= taskDetail?.ItemTitle,
                                        UserId = taskDetail?.UserId,
                                        UserName = user != null ? user.UserName : null,

                                   }
                               },
                               CreatedBy = f.CreatedBy,
                               CreatedAt = f.CreatedAt,
                               UpdatedAt = f.UpdatedAt
                           };
            return fileList.ToList();
        }
        public async Task<FileDTO> GetFilesById(Guid Id)
        {
            var files = await _unitOfWork.UploadedFile.GetByCondition(f => f.FileId == Id);
            var f = files.FirstOrDefault();
            if (f == null)
                return null;

            var taskDetails = await _unitOfWork.TaskDetail.GetByCondition(td => td.TaskDetailId == f.TaskDetailId);
            var td = taskDetails.FirstOrDefault();
            if (td == null)
                return new FileDTO
                {
                    FileId = f.FileId,
                    FileUrl = f.FileUrl,
                    Filename=f.FileName,
                    ContentType=f.ContentType,
                    AssignDetails = new List<AssignDetails>(),
                    CreatedBy = f.CreatedBy,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                };

            var users = await _unitOfWork.User.GetByCondition(u => u.UserId == td.UserId);
            var u = users.FirstOrDefault();

            return new FileDTO
            {
                FileId = f.FileId,
                FileUrl = f.FileUrl,
                ContentType=f.ContentType,
                AssignDetails = new List<AssignDetails>
                {
                    new AssignDetails
                    {
                        TaskDetailId = td.TaskDetailId,
                        TaskId = td.TaskId,
                        LineNumber = td.LineNumber,
                        ItemTitle= td.ItemTitle,
                        UserId = td.UserId,
                        UserName = u != null ? u.UserName : null,
                    }
                },
                CreatedBy = f.CreatedBy,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt
            };
        }

        public async Task<(bool IsSuccess, string Message, Guid FileId)> CreateFile(IFormFile file, Guid taskDetailId)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var entity = new UploadedFile
            {
                FileId = Guid.NewGuid(),
                TaskDetailId = taskDetailId,
                FileUrl = Path.Combine("uploads", fileName).Replace("\\", "/"), 
                FileName = fileName,
                ContentType = file.ContentType,
                CreatedBy = "admin",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UploadedFile.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return (true, "File uploaded successfully", entity.FileId);
        }
        public async Task<UploadedFile?> GetFileById(Guid fileId)
        {
            return (await _unitOfWork.UploadedFile
                .GetByCondition(f => f.FileId == fileId))
                .FirstOrDefault();
        }

        public async Task<(bool IsSuccess, string Message)> UpdateFile(Guid fileId, IFormFile file)
        {
            var entity = (await _unitOfWork.UploadedFile
                .GetByCondition(x => x.FileId == fileId))
                .FirstOrDefault();

            if (entity == null)
                return (false, "File not found");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            if (!string.IsNullOrEmpty(entity.FileUrl))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.FileUrl);
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            var newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var newFullPath = Path.Combine(uploadsFolder, newFileName);

            using (var stream = new FileStream(newFullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            entity.FileUrl = Path.Combine("uploads", newFileName).Replace("\\", "/"); 
            entity.FileName = newFileName;
            entity.ContentType = file.ContentType;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.UploadedFile.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return (true, "File updated successfully");
        }

        public async Task<(bool IsSuccess, string Message)> DeleteFile(Guid fileId)
        {
            try
            {
                if (fileId == Guid.Empty)
                    return (false, "Invalid FileId");

                var existingFile = await _unitOfWork.UploadedFile
                    .GetByCondition(x => x.FileId == fileId);

                var entity = existingFile.FirstOrDefault();

                if (entity == null)
                    return (false, "File not found");

                _unitOfWork.UploadedFile.Delete(entity);
                await _unitOfWork.SaveChangesAsync();

                return (true, "File deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting file: {ex.Message}");
            }
        }

        public async Task<(bool success, string Message)> DeleteFileByMultiple(DeleteFileDTO req)
        {
            try
            {
                var existingFiles = _unitOfWork.UploadedFile
                    .GetByExp(x => req.FileIds!.Contains(x.FileId))
                    .ToList();

                if (!existingFiles.Any())
                    return (false, "No files found for the given IDs");

                _unitOfWork.UploadedFile.DeleteMultiple(existingFiles);

                await _unitOfWork.SaveChangesAsync();

                return (true, "Files deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting files: {ex.Message}");
            }
        }
        public async Task<(bool success, string Message, string? FilePath, string? FileName, string? ContentType)> DownloadFile(Guid fileId)
        {
            try
            {
                var entity = (await _unitOfWork.UploadedFile
                    .GetByCondition(x => x.FileId == fileId))
                    .FirstOrDefault();

                if (entity == null)
                    return (false, "File not found", null, null, null);

                if (string.IsNullOrEmpty(entity.FileUrl))
                    return (false, "FileUrl is empty", null, null, null);

                var fullPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    entity.FileUrl.TrimStart('/', '\\')
                );

                if (!File.Exists(fullPath))
                    return (false, $"File not found on server: {fullPath}", null, null, null);

                return (
                    true,
                    "Success",
                    fullPath,
                    entity.FileName,
                    entity.ContentType
                );
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, null, null);
            }
        }
    }

}