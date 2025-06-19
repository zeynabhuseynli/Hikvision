using Hikvision.Api.Models;
using Hikvision.Api.SDK;
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
        try
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
                // Connection successful with V40
                _isConnected = true;
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Connected successfully using V40",
                    Data = new
                    {
                        UserID = _userID,
                        DeviceInfo = new
                        {
                            SerialNumber = System.Text.Encoding.Default.GetString(deviceInfo.struDeviceV30.sSerialNumber).TrimEnd('\0'),
                            DeviceType = deviceInfo.struDeviceV30.wDevType,
                            ChannelCount = deviceInfo.struDeviceV30.byChanNum,
                            StartChannel = deviceInfo.struDeviceV30.byStartChan
                        }
                    }
                };
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
                        Data = new
                        {
                            UserID = _userID,
                            DeviceInfo = new
                            {
                                SerialNumber = System.Text.Encoding.Default.GetString(deviceInfoV30.sSerialNumber).TrimEnd('\0'),
                                DeviceType = deviceInfoV30.wDevType,
                                ChannelCount = deviceInfoV30.byChanNum,
                                StartChannel = deviceInfoV30.byStartChan
                            }
                        }
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
        catch (Exception ex)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = $"Connection error: {ex.Message}",
                ErrorCode = "EXCEPTION"
            };
        }
    }

    public ApiResponse<object> Disconnect()
    {
        try
        {
            if (_userID >= 0)
            {
                bool result = HCNetSDK.NET_DVR_Logout(_userID);
                _userID = -1;
                _isConnected = false;

                return new ApiResponse<object>
                {
                    Success = result,
                    Message = result ? "Disconnected successfully" : "Disconnect failed",
                    Data = null
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Already disconnected",
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = $"Disconnect error: {ex.Message}",
                ErrorCode = "EXCEPTION"
            };
        }
    }

    public void Dispose()
    {
        if (_userID >= 0)
        {
            HCNetSDK.NET_DVR_Logout(_userID);
            _userID = -1;
            _isConnected = false;
        }

        if (_initialized)
        {
            HCNetSDK.NET_DVR_Cleanup();
        }
    }
}

