namespace MODEL.DTO
{
    public class TaskHeaderDTO
    {
        public Guid TaskId { get; set; }
        public Guid? AssignToDepartmentId { get; set; }
        public string? AssignedTo { get; set; }
        public string? TaskCode { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 
    }
    public class CreateTaskHeaderDTO
    {
        public Guid? AssignToDepartmentId { get; set; }
        public string? TaskCode { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public enum TaskStatus 
    {
        NotStarted = 1,
        InProgress = 2,
        Finished1 = 3,
    }
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
    }
}
