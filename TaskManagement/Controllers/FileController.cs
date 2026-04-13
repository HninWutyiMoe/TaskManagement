using BAL.IServiceFunction;
using Microsoft.AspNetCore.Mvc;
using MODEL.DTO;
using MODEL.DTOs;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("GetAllFiles")]
        public async Task<IActionResult> GetAllFiles()
        {
            try
            {
                var files = await _fileService.GetAllFiles();
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Files retrieved successfully.",
                    Status = ApiStatus.Successful,
                    Data = files.ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    Message = "An internal error occurred while retrieving files.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });

            }
        }

        [HttpGet("GetFileById/{fileId}")]
        public async Task<IActionResult> GetFileById(Guid fileId)
        {
            try
            {
                if (fileId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid file ID format.",
                        Status = ApiStatus.Error
                    });
                }
                var file = await _fileService.GetFilesById(fileId);
                if (file == null)
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = "File not found.",
                        Status = ApiStatus.Error
                    });
                }
                return Ok(
                    new ResponseModel
                    {
                        StatusCode = 200,
                        Message = "File retrieved successfully.",
                        Status = ApiStatus.Successful,
                        Data = file
                    }
                    );
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = "An error occurred while retrieving the file.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> UploaFile(IFormFile File, Guid taskDetailId)
        {
            try
            {
                if (File == null || File.Length == 0)
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Please provide a valid file",
                        Status = ApiStatus.Error
                    });

                if (File.Length > 5 * 1024 * 1024)
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Allowed file types: JPG, JPEG, PNG, PDF(max 5MB)",
                        Status = ApiStatus.Error
                    });

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

                var extension = Path.GetExtension(File.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid file type. Allowed: JPG, JPEG, PNG, PDF",
                        Status = ApiStatus.Error
                    });

                var allowedMimeTypes = new[]
                {
                "image/jpeg",
                "image/png",
                "application/pdf",
            };

                if (!allowedMimeTypes.Contains(File.ContentType))
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid file content type.",
                        Status = ApiStatus.Error
                    });

                var (message, createdFile, fileId) = await _fileService.CreateFile(File, taskDetailId);

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "File uploaded successfully.",
                    Status = ApiStatus.Successful,
                    Data = new { FileId = fileId, FileName = createdFile }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = "An error occurred while uploading the file.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpPut("UpdateFile")]
        public async Task<IActionResult> UpdateFile(IFormFile file, Guid fileId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Please provide a valid file",
                        Status = ApiStatus.Error
                    });

                var result = await _fileService.UpdateFile(fileId, file);

                if (!result.IsSuccess)
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = result.Message,
                        Status = ApiStatus.Error
                    });

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "File updated successfully.",
                    Status = ApiStatus.Successful,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = "An error occurred while updating the file.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("DeleteFile")]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            try
            {
                if(fileId == Guid.Empty)
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid file ID format.",
                        Status = ApiStatus.Error
                    });
                }
                var result = await _fileService.DeleteFile(fileId);

                if (!result.IsSuccess)
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = result.Message,
                        Status = ApiStatus.Error
                    });

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "File deleted successfully.",
                    Status = ApiStatus.Successful,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = "An error occurred while deleting the file.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("DeleteFilesByMultipleId")]
        public async Task<IActionResult> DeleteFileByMultiple(DeleteFileDTO req)
        {
            try
            {
                if (req.FileIds == null || !req.FileIds.Any())
                {
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = "Invalid file ID format.",
                        Status = ApiStatus.Error
                    });
                }
                var result = await _fileService.DeleteFileByMultiple(req);
                if (!result.success)
                    return BadRequest(new ResponseModel
                    {
                        StatusCode = 400,
                        Message = result.Message,
                        Status = ApiStatus.Error
                    });
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "Files deleted successfully.",
                    Status = ApiStatus.Successful,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = "An error occurred while deleting the files.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }


        [HttpGet("DownloadFile/{fileId}")]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            try
            {
                var result = await _fileService.DownloadFile(fileId);

                if (!result.success || string.IsNullOrEmpty(result.FilePath))
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = result.Message,
                        Status = ApiStatus.Error
                    });
                }

                if (!System.IO.File.Exists(result.FilePath))
                {
                    return NotFound(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = "File not found on server.",
                        Status = ApiStatus.Error
                    });
                }

                var fileName = string.IsNullOrEmpty(result.FileName) ? System.IO.Path.GetFileName(result.FilePath) : result.FileName;
                var contentType = string.IsNullOrEmpty(result.ContentType) ? "application/octet-stream" : result.ContentType;

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Message = "File downloaded successfully.",
                    Status = ApiStatus.Successful,
                    Data = new
                    {
                        FileName = fileName,
                        ContentType = contentType,
                        result.FilePath
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    Message = "An error occurred while downloading the file.",
                    Status = ApiStatus.Error,
                    Data = ex.Message
                });
            }
        }
    }
}

