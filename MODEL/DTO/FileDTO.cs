namespace MODEL.DTO
{
    public class FileDTO
    {
        public Guid FileId { get; set; }
        public string? FileUrl { get; set; }
        public string? Filename { get; set; }
        public List<AssignDetails>? AssignDetails { get; set; }
        public string? ContentType { get; set; }
        public string? CreatedBy { get; set; } 
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 
    }
    //public class UploadedFileDTO
    //{
    //    public Guid FileId { get; set; }
    //    public IFormFile? File { get; set; }
    //    public string? UpdatedBy { get; set; }
    //}

    public class DeleteFileDTO
    {
        public List<Guid>? FileIds { get; set; }
    }
    //public class FileCreateDTO
    //{
    //    public IFormFile File { get; set; } = default!;


    //}
    public class FileDownloadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
    }
    public class AssignDetails
    {
        public Guid TaskDetailId { get; set; }
        public Guid? TaskId { get; set; }
        public string? LineNumber { get; set; }
        public string? ItemTitle { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }

    }
}
