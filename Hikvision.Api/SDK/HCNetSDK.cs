using System.Runtime.InteropServices;

namespace Hikvision.Api.SDK;
public static class HCNetSDK
{
    private const string DllName = "HCNetSDK.dll";

    // Basic SDK functions
    [DllImport(DllName)]
    public static extern bool NET_DVR_Cleanup();

    [DllImport(DllName)]
    public static extern bool NET_DVR_Init();

    [DllImport(DllName)]
    public static extern bool NET_DVR_SetLogToFile(int bLogEnable, string strLogDir, bool bAutoDel);

    [DllImport(DllName)]
    public static extern int NET_DVR_Login_V40(ref NET_DVR_USER_LOGIN_INFO pLoginInfo, ref NET_DVR_DEVICEINFO_V40 lpDeviceInfo);

    [DllImport(DllName)]
    public static extern int NET_DVR_Login_V30(string sDVRIP, int wDVRPort, string sUserName, string sPassword, ref NET_DVR_DEVICEINFO_V30 lpDeviceInfo);

    [DllImport(DllName)]
    public static extern bool NET_DVR_Logout(int lUserID);

    [DllImport(DllName)]
    public static extern uint NET_DVR_GetLastError();

    // Constants
    public const int NET_DVR_GET_USERCFG = 1006;
    public const int NET_DVR_GET_USERCFG_V30 = 1048;
    public const int NET_DVR_SET_USERCFG_V30 = 1049;
    public const int NET_DVR_GET_DEVICECFG_V40 = 3801;

    // Error codes
    public const int NET_DVR_NOERROR = 0;
    public const int NET_DVR_PASSWORD_ERROR = 1;
    public const int NET_DVR_NOENOUGHPRI = 2;
    public const int NET_DVR_NOINIT = 3;
    public const int NET_DVR_CHANNEL_ERROR = 4;
    public const int NET_DVR_OVER_MAXLINK = 5;
    public const int NET_DVR_VERSIONNOMATCH = 6;
    public const int NET_DVR_NETWORK_FAIL_CONNECT = 7;
    public const int NET_DVR_NETWORK_SEND_ERROR = 8;
    public const int NET_DVR_NETWORK_RECV_ERROR = 9;
    public const int NET_DVR_NETWORK_RECV_TIMEOUT = 10;
    public const int NET_DVR_NETWORK_ERROR = 11;
    public const int NET_DVR_ORDER_ERROR = 12;
    public const int NET_DVR_OPERNOPERMIT = 13;
    public const int NET_DVR_COMMANDTIMEOUT = 14;
    public const int NET_DVR_ERRORSERIALPORT = 15;
    public const int NET_DVR_ERRORALARMPORT = 16;

    // User rights constants
    public const byte USERRIGHTS_PLAYBACK = 1;
    public const byte USERRIGHTS_PTZ = 2;
    public const byte USERRIGHTS_BACKUP = 4;
    public const byte USERRIGHTS_CONFIG = 8;
    public const byte USERRIGHTS_SHUTDOWNREBOOT = 16;
    public const byte USERRIGHTS_LOCALPLAYBACK = 32;
    public const byte USERRIGHTS_REMOTECFG = 64;
    public const byte USERRIGHTS_LOCALCFG = 128;

    // Structures
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct NET_DVR_USER_LOGIN_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string sDeviceAddress;
        public byte byUseTransport;
        public ushort wPort;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sUserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string sPassword;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 119)]
        public byte[] byRes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NET_DVR_DEVICEINFO_V40
    {
        public NET_DVR_DEVICEINFO_V30 struDeviceV30;
        public byte bySupport;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public byte[] byRes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NET_DVR_DEVICEINFO_V30
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] sSerialNumber;
        public byte byAlarmInPortNum;
        public byte byAlarmOutPortNum;
        public byte byDiskNum;
        public byte byDVRType;
        public byte byChanNum;
        public byte byStartChan;
        public byte byAudioChanNum;
        public byte byIPChanNum;
        public byte byZeroChanNum;
        public byte byMainProto;
        public byte bySubProto;
        public byte bySupport;
        public byte bySupport1;
        public byte bySupport2;
        public ushort wDevType;
        public byte bySupport3;
        public byte byMultiStreamProto;
        public byte byStartDChan;
        public byte byStartDTalkChan;
        public byte byHighDChanNum;
        public byte bySupport4;
        public byte byLanguageType;
        public byte byVoiceInChanNum;
        public byte byStartVoiceInChanNo;
        public byte bySupport5;
        public byte bySupport6;
        public byte byMirrorChanNum;
        public ushort wStartMirrorChanNo;
        public byte bySupport7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] byRes2;
    }

}


