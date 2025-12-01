using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore
{
    internal class HxPcInfomationTypes
    {
    }
    /*
    public class HxPoint { public int X { get; set; } public int Y { get; set; } }
    public class HxSizeDetail { public int Height { get; set; } public int Width { get; set; } }
    */
    public class HxRectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //public HxScreenRectangle() { }

        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;

        public HxRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}, Width:{Width}, Height:{Height}";
        }

    }

    /// <summary>
    /// 시스템 제품 정보 (모델명, UUID 등)를 담을 클래스
    /// </summary>
    public class HxHardwareSystemInfo
    {
        [Description("시스템 모델명")]
        public string Name { get; set; }

        [Description("제조사")]
        public string Vendor { get; set; }

        [Description("버전")]
        public string Version { get; set; }

        [Description("식별 번호 (SKU Number 등)")]
        public string IdentifyingNumber { get; set; }

        [Description("UUID (고유 ID)")]
        public string UUID { get; set; }
    }

    public class HxHardwareCpuInfo
    {
        [Description("제품명")]
        public string Name { get; set; }
        [Description("프로세서ID")]
        public string ProcessorId { get; set; }
        [Description("제조사")]
        public string Manufacturer { get; set; }
        [Description("시리얼 번호")]
        public string SerialNumber { get; set; }
        [Description("고유ID")]
        public string UniqueId { get; set; }

        [Description("코어 수")]
        public int NumberOfCores { get; set; }
        [Description("최대 클럭 속도")]
        public string MaxClockSpeed { get; set; }
    }

    public class HxHardwareDiskDriveInfo
    {
        [Description("모델명")]
        public string Model { get; set; }
        [Description("시리얼 번호")]
        public string SerialNumber { get; set; }
        [Description("인터페이스")]
        public string InterfaceType { get; set; }
        [Description("용량 - byte")]
        public ulong SizeByte { get; set; }
        public string SizeGB => SizeByte > 0 ? HxString.FormatNumber(Math.Round((double)(SizeByte / (1024 * 1024 * 1024)), 2)) + "GB" : string.Empty;
    }

    public class HxHardwareDiskVolumeInfo
    {
        public string DeviceID { get; set; } // 예: "C:"
        public string VolumeSerialNumber { get; set; } // 예: "1A2B3C4D"
        public ulong FreeSpaceBytes { get; set; }
        public ulong TotalSizeBytes { get; set; }

        public string FreeSpaceGB => HxString.FormatNumber(Math.Round((double)FreeSpaceBytes / (1024 * 1024 * 1024), 2)) + " GB";
        public string TotalSizeGB => HxString.FormatNumber(Math.Round((double)TotalSizeBytes / (1024 * 1024 * 1024), 2)) + " GB";
    }
    /// <summary>
    /// 물리 디스크 파티션 정보를 담을 클래스
    /// (C: 드라이브 여부와 관계없이 모든 파티션)
    /// </summary>
    public class HxHardwarePartitionInfo
    {
        [Description("장치 ID")]
        public string DeviceID { get; set; } // 예: "Disk #0, Partition #1"

        [Description("물리 디스크 인덱스")]
        public uint DiskIndex { get; set; } // 예: 0

        [Description("파티션 인덱스")]
        public uint Index { get; set; } // 예: 1

        [Description("파티션 유형")]
        public string Type { get; set; } // 예: "GPT: Basic Data Partition"

        [Description("부팅 가능 여부")]
        public bool Bootable { get; set; }

        [Description("파티션 크기 (byte)")]
        public ulong Size { get; set; }

        // 편의 속성 (GB)
        public string SizeGB =>
            Size > 0 ? HxString.FormatNumber(Math.Round((double)Size / (1024 * 1024 * 1024), 2)) + " GB" : "0 GB";
    }

    /// <summary>
    /// 마더보드 정보를 담을 클래스
    /// </summary>
    public class HxHardwareBoardInfo
    {
        [Description("제조사")]
        public string Manufacturer { get; set; }

        [Description("제품명")]
        public string Product { get; set; }
        [Description("모델명")]
        public string Model { get; set; }

        [Description("시리얼 번호")]
        public string SerialNumber { get; set; }
    }

    /// <summary>
    /// 네트워크 어댑터 정보를 담을 클래스
    /// </summary>
    public class HxHardwareNetworkInfo
    {   //https://learn.microsoft.com/ko-kr/windows/win32/cimwin32prov/win32-networkadapterconfiguration
        [Description("설명 (이름)")]
        public string Description { get; set; }

        [Description("MAC 주소")]
        public string MacAddress { get; set; }

        [Description("IP 주소 (IPv4, IPv6)")]
        public string[] IpAddresses { get; set; }
        [Description("TCP/IP가 활성화 여부?")]
        public bool IPEnabled { get; set; }

        // IP 주소 배열을 쉼표로 구분된 문자열로 반환 (편의 속성)
        public string IpAddressString => IpAddresses != null ? string.Join(", ", IpAddresses) : string.Empty;
    }

    /// <summary>
    /// 물리적 메모리(RAM 스틱) 정보를 담을 클래스
    /// </summary>
    public class HxHardwarePhysicalMemory
    {
        [Description("슬롯 위치")]
        public string DeviceLocator { get; set; }

        [Description("제조사")]
        public string Manufacturer { get; set; }

        [Description("속도 (MHz)")]
        public uint Speed { get; set; }

        [Description("용량 (byte)")]
        public ulong CapacityBytes { get; set; }

        // 편의 속성 (GB)
        public string CapacityGB =>
            CapacityBytes > 0 ? HxString.FormatNumber(Math.Round((double)CapacityBytes / (1024 * 1024 * 1024), 2)) + " GB" : "0 GB";
    }

    /// <summary>
    /// BIOS 정보를 담을 클래스
    /// </summary>
    public class HxHardwareBiosInfo
    {
        [Description("제조사")]
        public string Manufacturer { get; set; }

        [Description("시리얼 번호 (SMBIOS)")]
        public string SerialNumber { get; set; }

        [Description("BIOS 버전 (SMBIOS)")]
        public string SMBIOSBIOSVersion { get; set; }

        [Description("BIOS 버전 (전체)")]
        public string Version { get; set; }
    }

    /// <summary>
    /// 운영체제(OS) 정보를 담을 클래스
    /// </summary>
    public class HxOsInfo
    {
        [Description("OS 이름")]
        public string Caption { get; set; }

        [Description("버전")]
        public string Version { get; set; }

        [Description("빌드 번호")]
        public string BuildNumber { get; set; }

        [Description("아키텍처 (32/64비트)")]
        public string OsArchitecture { get; set; }

        [Description("설치 날짜")]
        public DateTime InstallDate { get; set; }

        [Description("마지막 부팅 시간")]
        public DateTime LastBootUpTime { get; set; }
    }

    /// <summary>
    /// 모니터 정보를 담을 클래스
    /// </summary>
    public class HxHardwareMonitorInfo
    {
        [Description("이름")]
        public string Name { get; set; }

        [Description("장치 ID")]
        public string DeviceID { get; set; }

        [Description("PnP 장치 ID")]
        public string PNPDeviceID { get; set; }

        [Description("모니터 제조사")]
        public string MonitorManufacturer { get; set; }

        [Description("모니터 유형")]
        public string MonitorType { get; set; }
    }

    /// <summary>
    /// 비디오 컨트롤러(그래픽 카드)의 현재 디스플레이 설정
    /// </summary>
    public class HxHardwareVideoController
    {
        [Description("장치 이름")]
        public string Name { get; set; }

        [Description("현재 수평 해상도")]
        public uint CurrentHorizontalResolution { get; set; }

        [Description("현재 수직 해상도")]
        public uint CurrentVerticalResolution { get; set; }

        [Description("현재 재생률 (Hz)")]
        public uint CurrentRefreshRate { get; set; }

        // 편의 속성
        public string Resolution => $"{CurrentHorizontalResolution} x {CurrentVerticalResolution}";
    }

    /// <summary>
    /// GDI/User32 API를 통해 가져온 상세 모니터 정보
    /// </summary>
    public class HxHardwareScreenInfo
    {
        [Description("장치 이름 (\\.\\DISPLAY1)")]
        public string DeviceName { get; set; }

        [Description("해상도 (Bounds)")]
        public HxRectangle Bounds { get; set; }

        [Description("작업 영역 (WorkingArea)")]
        public HxRectangle WorkingArea { get; set; }

        [Description("주 모니터 여부")]
        public bool IsPrimary { get; set; }

        [Description("현재 재생률 (Hz)")]
        public int RefreshRate { get; set; }

        [Description("확대/축소 배율 (%) (96DPI=100%)")]
        public uint ScalingPercent { get; set; }

        [Description("디스플레이 방향")]
        public string Orientation { get; set; }

        // 편의 속성
        public string Resolution => $"{Bounds.Width} x {Bounds.Height}";
    }

    public class HxSoftwareInfo
    {
        // 제품 ID (레지스트리 키 이름, 종종 GUID)
        public string ProductID { get; set; }

        // 1. 제품명
        public string DisplayName { get; set; }

        // 2. 게시자
        public string Publisher { get; set; }

        // 3. 설치 일시 (DateTime?으로 변환)
        public DateTime? InstallDate { get; set; }

        // 4. 크기 (KB 단위)
        public long? EstimatedSizeKB { get; set; }

        // 5. 버전
        public string DisplayVersion { get; set; }

        // 6. 설치 폴더
        public string InstallLocation { get; set; }

        // 7. 실행 파일 전체 경로 (메인 실행 파일 경로)
        public string MainExecutableFullPath { get; set; }
        // 8. 실행 파일 크기 (메인 실행 파일 크기)
        public long MainExecutableFileSize { get; set; }
        public string MainExecutableFileSizeKB => MainExecutableFileSize > 0 ? HxString.FormatNumber(Math.Round((double)MainExecutableFileSize / (1024), 2)) + " KB" : "0 KB";

        public override string ToString()
        {
            return $"[ {DisplayName} ]\n" +
                   $"  - Version: {DisplayVersion}\n" +
                   $"  - ProductID: {ProductID}\n" +
                   $"  - Publisher: {Publisher}\n" +
                   $"  - InstallDate: {InstallDate?.ToString("yyyy-MM-dd")}\n" +
                   $"  - Location: {InstallLocation}\n" +
                   $"  - Size(KB): {MainExecutableFileSizeKB}\n" +
                   $"  - Executable: {MainExecutableFullPath}\n"
                   ;
        }
    }

    public class HxPcInfomations
    {
        public string MachineId { get; set; } //= HxWin32Fingerprint.GetUniqueMachineId();
        public string SystemUUID { get; set; } //= HxWin32HardwareSearcher.GetSystemUUID();
        public string VolumnID { get; set; } //= HxUtils.GetUserVolumeId();
        public string SystemChecksum => HxCrypt.Md5(SystemUUID + VolumnID);

        public string UserAgent { get; set; } //= HxUtils.GetOSCustomUserAgent();
        public string MachineName { get; set; } //= HxUtils.GetOSMachineName();
        public string DomainName { get; set; } //= HxUtils.GetOSUserDomainName();
        public string UserName { get; set; } //= HxUtils.GetOSUserName();
        public string WorkgroupName { get; set; } //= HxUtils.GetUserWorkgroup();
        public string LocalIpAddress { get; set; } //= HxUtils.GetUserHostAddress();
        public string LocalMacAddress { get; set; } //= HxUtils.GetUserMacAddress();
        public string GlobalIpAddress { get; set; } //= HxUtils.GetUserGlobalAddress(true);
        public string InternalIpAddress { get; set; } //= HxNet.GetUserGlobalAddress(@"http://gw.htenc.co.kr/api/ip/");


        public HxHardwareSystemInfo SystemInfo { get; set; } //= HxWin32HardwareSearcher.GetSystemInfo();
        public HxOsInfo OSInfo { get; set; } //= HxWin32HardwareSearcher.GetOsInfo();

        //public IPAddress LocalIpAdress { get; set; } = HxNet.GetLocalIPAddress();

        public HxHardwareCpuInfo CpuInfo { get; set; } //= HxWin32HardwareSearcher.GetCpuInfo();
        public IEnumerable<HxHardwareDiskDriveInfo> DiskDriveInfos { get; set; } //= HxWin32HardwareSearcher.GetDiskDriveInfos();
        public IEnumerable<HxHardwareDiskVolumeInfo> VolumeInfos { get; set; } //= HxWin32HardwareSearcher.GetDiskVolumneInfos();
        public IEnumerable<HxHardwarePartitionInfo> PartitionInfos { get; set; } //= HxWin32HardwareSearcher.GetPartitionInfos();
        public HxHardwareBoardInfo BoardInfo { get; set; } //= HxWin32HardwareSearcher.GetBoardInfo();
        public IEnumerable<HxHardwareNetworkInfo> NetworkInfos { get; set; } //= HxWin32HardwareSearcher.GetNetworkInfos();
        public IEnumerable<HxHardwarePhysicalMemory> Memories { get; set; } //= HxWin32HardwareSearcher.GetMemoryInfos();
        public HxHardwareBiosInfo BiosInfo { get; set; } //= HxWin32HardwareSearcher.GetBiosInfo();
        public IEnumerable<HxHardwareMonitorInfo> MonitorInfos { get; set; } //= HxWin32HardwareSearcher.GetMonitorInfos();
        public IEnumerable<HxHardwareVideoController> VideoInfos { get; set; } //= HxWin32HardwareSearcher.GetVideoControllerInfos();
        public IEnumerable<HxHardwareScreenInfo> ScreenInfos { get; set; } //= HxWin32HardwareSearcher.GetScreenInfos();

        public IEnumerable<HxSoftwareInfo> SoftwareInfos { get; set; } //= HxWin32SoftwareSearcher.GetInstalledSoftware();
        
    }
}
