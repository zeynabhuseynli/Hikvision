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

    public void Dispose()
    {
        Disconnect();
        GC.SuppressFinalize(this);
    }

    ~HikvisionAccessControlClient() => Disconnect();
}
