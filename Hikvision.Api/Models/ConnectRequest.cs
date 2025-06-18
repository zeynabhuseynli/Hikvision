namespace Hikvision.Api.Models;
public class ConnectRequest
{
    public string IpAddress { get; set; } 
    public ushort Port { get; set; } = 8000;
    public string Username { get; set; } 
    public string Password { get; set; } 
}
