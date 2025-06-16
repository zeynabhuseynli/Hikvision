namespace Hikvision.Api.Models
{
    public class ErrorResponse
    {
        public string Error { get; set; } = default!;
        public int Code { get; set; }
    }
}
