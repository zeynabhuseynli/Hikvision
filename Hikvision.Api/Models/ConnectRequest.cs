namespace Hikvision.Api.Models
{
    public class ConnectRequest
    {
        public string Ip { get; set; } = default!;
        public ushort Port { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
