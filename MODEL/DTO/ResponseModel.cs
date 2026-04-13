namespace MODEL.DTOs
{
    public class ResponseModel
    {
        public string? Message { get; set; }
        public ApiStatus Status { get; set; }
        public object? Data { get; set; }
        public int StatusCode { get; set; }
    }
    public enum ApiStatus
    {
        Successful = 0,
        Error = 1,
        SystemError = 2
    }
}
