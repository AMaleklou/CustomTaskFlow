namespace CustomTaskFlow.Api.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null) 
            => new()
             {
                 Success = true,
                 Message = message,
                 Data = data,
             };

        public static ApiResponse<T> ErrorResponse(List<string> errors, string? message = null)
            => new()
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors,
            };
    }
}
