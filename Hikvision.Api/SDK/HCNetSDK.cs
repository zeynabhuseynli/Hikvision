using System.Runtime.InteropServices;

namespace Hikvision.Api.SDK;
public static class HCNetSDK
{
    private const string DllName = "HCNetSDK.dll";

    [DllImport(DllName)] internal static extern bool NET_DVR_Init();
    [DllImport(DllName)] internal static extern bool NET_DVR_Cleanup();
    [DllImport(DllName)]
    internal static extern int NET_DVR_Login_V30(
        string sDVRIP,
        ushort wDVRPort,
        string sUserName,
        string sPassword,
        ref NET_DVR_DEVICEINFO_V30 lpDeviceInfo);
    [DllImport(DllName)] internal static extern bool NET_DVR_Logout(int lUserID);
    [DllImport(DllName)] internal static extern int NET_DVR_GetLastError();

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct NET_DVR_DEVICEINFO_V30
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)] internal string sSerialNumber;
        internal byte byAlarmInPortNum; internal byte byAlarmOutPortNum;
        internal byte byDiskNum; internal byte byDVRType;
        internal byte byChanNum; internal byte byStartChan;
        internal byte byAudioChanNum; internal byte byIPChanNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)] internal byte[] byRes;
    }
}
