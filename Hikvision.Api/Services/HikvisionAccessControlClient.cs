using Hikvision.Api.Models;
using Hikvision.Api.SDK;

namespace Hikvision.Api.Services;
public class HikvisionAccessControlClient : IHikvisionAccessControlClient
{
    private int _userId = -1;
    private bool _initialized;

    public void Initialize()
    {
        if (_initialized) return;
        if (!HCNetSDK.NET_DVR_Init())
            throw new InvalidOperationException("HCNetSDK initialization failed.");
        _initialized = true;
    }

    public void Connect(string ip, ushort port, string username, string password)
    {
        if (!_initialized)
            throw new InvalidOperationException("Initialize must be called before Connect.");

        var deviceInfo = new HCNetSDK.NET_DVR_DEVICEINFO_V30();
        _userId = HCNetSDK.NET_DVR_Login_V30(ip, port, username, password, ref deviceInfo);
        if (_userId < 0)
        {
            var err = HCNetSDK.NET_DVR_GetLastError();
            throw new InvalidOperationException($"Login failed with error code {err}.");
        }
    }

    public void Disconnect()
    {
        if (_userId >= 0)
        {
            HCNetSDK.NET_DVR_Logout(_userId);
            _userId = -1;
        }
        if (_initialized)
        {
            HCNetSDK.NET_DVR_Cleanup();
            _initialized = false;
        }
    }

    public int GetLastError() => HCNetSDK.NET_DVR_GetLastError();

    public IEnumerable<UserDto> GetUsers()
    {
        if (_userId < 0)
            throw new InvalidOperationException("Not connected.");
        // TODO: call actual HCNetSDK user query methods; stubbed list for example
        return new List<UserDto>
        {
            new UserDto { Id = 1, Username = "operator", Role = "Operator" },
            new UserDto { Id = 2, Username = "admin", Role = "Admin" }
        };
    }

    public void CreateUser(string username, string password, string role)
    {
        if (_userId < 0)
            throw new InvalidOperationException("Not connected.");
        // TODO: invoke HCNetSDK.NET_DVR_SetUserInfo or similar
        var success = true; // replace with actual SDK call result
        if (!success)
        {
            var err = HCNetSDK.NET_DVR_GetLastError();
            throw new InvalidOperationException($"Create user failed with code {err}.");
        }
    }

    public void Dispose()
    {
        Disconnect();
        GC.SuppressFinalize(this);
    }

    ~HikvisionAccessControlClient() => Disconnect();
}
