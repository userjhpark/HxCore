using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    public class HxWinPcInfomations : HxPcInfomations
    {
        public HxWinPcInfomations()
        {
            MachineId = HxWin32Fingerprint.GetUniqueMachineId();
            SystemUUID = HxWin32HardwareSearcher.GetSystemUUID();
            VolumnID = HxUtils.GetUserVolumeId();
            UserAgent = HxUtils.GetOSCustomUserAgent();
            MachineName = HxUtils.GetOSMachineName();
            DomainName = HxUtils.GetOSUserDomainName();
            UserName = HxUtils.GetOSUserName();
            WorkgroupName = HxUtils.GetUserWorkgroup();
            LocalIpAddress = HxUtils.GetUserHostAddress();
            LocalMacAddress = HxUtils.GetUserMacAddress();
            GlobalIpAddress = HxUtils.GetUserGlobalAddress(true);
            InternalIpAddress = HxNet.GetUserGlobalAddress(@"http://gw.htenc.co.kr/api/ip/");
            SystemInfo = HxWin32HardwareSearcher.GetSystemInfo();
            OSInfo = HxWin32HardwareSearcher.GetOsInfo();
            CpuInfo = HxWin32HardwareSearcher.GetCpuInfo();
            DiskDriveInfos = HxWin32HardwareSearcher.GetDiskDriveInfos();
            VolumeInfos = HxWin32HardwareSearcher.GetDiskVolumneInfos();
            PartitionInfos = HxWin32HardwareSearcher.GetPartitionInfos();
            BoardInfo = HxWin32HardwareSearcher.GetBoardInfo();
            NetworkInfos = HxWin32HardwareSearcher.GetNetworkInfos();
            Memories = HxWin32HardwareSearcher.GetMemoryInfos();
            BiosInfo = HxWin32HardwareSearcher.GetBiosInfo();
            MonitorInfos = HxWin32HardwareSearcher.GetMonitorInfos();
            VideoInfos = HxWin32HardwareSearcher.GetVideoControllerInfos();
            ScreenInfos = HxWin32HardwareSearcher.GetScreenInfos();
            SoftwareInfos = HxWin32SoftwareSearcher.GetInstalledSoftware();
        }
    }
}
