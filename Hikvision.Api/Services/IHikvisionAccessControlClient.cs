using Hikvision.Api.Models;

namespace Hikvision.Api.Services;
public interface IHikvisionAccessControlClient : IDisposable
{
    void Initialize();
    void Connect(string ip, ushort port, string username, string password);
    void Disconnect();
    int GetLastError();
    IEnumerable<UserDto> GetUsers();
    void CreateUser(string username, string password, string role);
}
