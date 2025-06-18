using Hikvision.Api.SDK;

namespace Hikvision.Api.Services;
public static class HelperService
{
    public static string GetErrorMessage(uint errorCode) => errorCode switch
    {
        HCNetSDK.NET_DVR_NOERROR => "Success",
        HCNetSDK.NET_DVR_PASSWORD_ERROR => "Incorrect password",
        HCNetSDK.NET_DVR_NOENOUGHPRI => "Insufficient permissions",
        HCNetSDK.NET_DVR_NOINIT => "SDK not initialized",
        HCNetSDK.NET_DVR_CHANNEL_ERROR => "Channel error",
        HCNetSDK.NET_DVR_OVER_MAXLINK => "Maximum number of connections exceeded",
        HCNetSDK.NET_DVR_VERSIONNOMATCH => "Version mismatch",
        HCNetSDK.NET_DVR_NETWORK_FAIL_CONNECT => "Failed to connect to the network",
        HCNetSDK.NET_DVR_OPERNOPERMIT => "Operation not permitted",
        HCNetSDK.NET_DVR_COMMANDTIMEOUT => "Command timed out",
        17 => "Parameter error - NULL value provided",
        _ => $"Unknown error: {errorCode}"
    };
}
