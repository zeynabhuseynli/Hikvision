using Hikvision.Api.Models;
using Hikvision.Api.SDK;
using System.Runtime.InteropServices;
using static Hikvision.Api.SDK.HCNetSDK;

namespace Hikvision.Api.Services;
public class HikvisionService : IHikvisionService, IDisposable
{
    private bool _initialized = false;
    private static int _userID = -1;
    private static bool _isConnected = false;

    public HikvisionService()
    {
        _initialized = HCNetSDK.NET_DVR_Init();
        if (_initialized)
        {
            HCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
        }
        else
        {
            throw new InvalidOperationException("SDK başlatma uğursuz oldu.");
        }
    }

    public ApiResponse<object> HikConnection(ConnectRequest request)
    {
        // Log input parameters for debugging
        System.Diagnostics.Debug.WriteLine($"Connecting to {request.IpAddress}:{request.Port} with username {request.Username}");

        // Initialize login information
        NET_DVR_USER_LOGIN_INFO loginInfo = new NET_DVR_USER_LOGIN_INFO
        {
            sDeviceAddress = request.IpAddress,
            wPort = request.Port,
            sUserName = request.Username,
            sPassword = request.Password,
            byUseTransport = 1, // Use TCP
            byRes = new byte[119]
        };
        NET_DVR_DEVICEINFO_V40 deviceInfo = new NET_DVR_DEVICEINFO_V40
        {
            byRes = new byte[255]
        };

        // Attempt to log in with V40
        _userID = HCNetSDK.NET_DVR_Login_V40(ref loginInfo, ref deviceInfo);

        if (_userID >= 0)
        {
            IntPtr buffer = Marshal.AllocHGlobal(1024);
            try
            {
                uint bytesReturned;
                bool result = HCNetSDK.NET_DVR_GetDVRConfig(_userID, HCNetSDK.NET_DVR_GET_DEVICECFG_V40, -1, buffer, 1024, out bytesReturned);
                if (result)
                {
                    _isConnected = true;
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Connected successfully and config retrieved",
                        Data = new { UserID = _userID }
                    };
                }
                else
                {
                    uint errorCode = HCNetSDK.NET_DVR_GetLastError();
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Failed to retrieve config: {HelperService.GetErrorMessage(errorCode)}",
                        ErrorCode = errorCode.ToString()
                    };
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        else
        {
            uint errorCode = HCNetSDK.NET_DVR_GetLastError();
            // Fallback to V30 if V40 fails
            NET_DVR_DEVICEINFO_V30 deviceInfoV30 = new NET_DVR_DEVICEINFO_V30();
            _userID = HCNetSDK.NET_DVR_Login_V30(request.IpAddress, request.Port, request.Username, request.Password, ref deviceInfoV30);
            if (_userID >= 0)
            {
                _isConnected = true;
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Connected successfully using V30",
                    Data = new { UserID = _userID }
                };
            }
            else
            {
                errorCode = HCNetSDK.NET_DVR_GetLastError();
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Login failed: {HelperService.GetErrorMessage(errorCode)}",
                    ErrorCode = errorCode.ToString()
                };
            }
        }
    }

    public void Dispose()
    {
        if (_initialized)
        {
            HCNetSDK.NET_DVR_Cleanup();
        }
    }
}
