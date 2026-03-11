//using HidSharp.Utility;
//using LibreHardwareMonitor.Hardware.Motherboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    public class HxWin32HardwareSearcher
    {
        /// <summary>
        /// 시스템의 UUID (SMBIOS)를 가져옵니다. 
        /// PC 고유 ID 생성에 가장 신뢰도 높은 값입니다.
        /// </summary>
        public static string GetSystemUUID()
        {
            string Result = string.Empty;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT UUID FROM Win32_ComputerSystemProduct");

                if (searcher == null) { return string.Empty; }

                ManagementObject mo = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                if (mo != null)
                {
                    Result = mo["UUID"]?.ToString()?.Trim();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI UUID 조회 오류: {ex.Message}");
            }

            //Debug.WriteLine($"시스템 UUID: {uuid}");
            return Result;
        }

        /// <summary>
        /// WMI (Win32_ComputerSystemProduct)를 사용하여
        /// 시스템 제품 정보(모델명, UUID) 조회
        /// (시스템에 이 정보는 하나만 존재)
        /// </summary>
        public static HxHardwareSystemInfo GetSystemInfo()
        {
            HxHardwareSystemInfo Result = null;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT Name, IdentifyingNumber, UUID, Vendor, Version FROM Win32_ComputerSystemProduct");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                // 시스템 제품 정보는 하나만 존재합니다.
                ManagementObject mo = list.Cast<ManagementObject>().FirstOrDefault();

                if (mo != null)
                {
                    Result = new HxHardwareSystemInfo
                    {
                        Name = mo["Name"]?.ToString()?.Trim(),
                        Vendor = mo["Vendor"]?.ToString()?.Trim(),
                        Version = mo["Version"]?.ToString()?.Trim(),
                        IdentifyingNumber = mo["IdentifyingNumber"]?.ToString()?.Trim(),
                        UUID = mo["UUID"]?.ToString()?.Trim() // PC 고유 ID 생성에 가장 중요
                    };

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine("--- 시스템 제품 정보 ---");
                    //Debug.WriteLine($"모델명 (Name): {Result.Name}");
                    //Debug.WriteLine($"제조사 (Vendor): {Result.Vendor}");
                    //Debug.WriteLine($"버전 (Version): {Result.Version}");
                    //Debug.WriteLine($"식별번호 (SKU): {Result.IdentifyingNumber}");
                    //Debug.WriteLine($"UUID: {Result.UUID}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 시스템 제품 정보 조회 오류: {ex.Message}");
            }

            return Result;
        }


        public static HxHardwareCpuInfo GetCpuInfo()
        {
            HxHardwareCpuInfo Result = null;
            //Debug.WriteLine("--- CPU 정보 ---");
            try
            {
                // WMI 쿼리 실행
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

                if (searcher == null) { return Result; }

                ManagementObjectCollection list = searcher.Get();
                if(list == null || list.Count <= 0) { return Result; }

                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    // 속성 값 가져오기
                    string name = mo["Name"]?.ToString();
                    if (name.IsNullOrWhiteSpaceEx() == true) { continue; }

                    string processorId = mo["ProcessorId"]?.ToString(); // 고유 ID
                    string manufacturer = mo["Manufacturer"]?.ToString();
                    int cores = (mo["NumberOfCores"] ?? 0).ToIntEx(); //NumberOfLogicalProcessors
                    string maxClockSpeed = mo["MaxClockSpeed"]?.ToString(); // MHz
                    string serialNumber = mo["SerialNumber"]?.ToString();
                    string uniqueId = mo["UniqueId"]?.ToString();

                    /*
                    foreach(PropertyData p in mo.Properties)
                    {
                        Debug.WriteLine($"{p.Name.ToStringEx()} : {p.Value.ToStringEx()}");
                    }
                    */

                    Result = new HxHardwareCpuInfo { Name = name, ProcessorId = processorId, Manufacturer = manufacturer, NumberOfCores = cores, MaxClockSpeed = maxClockSpeed, SerialNumber = serialNumber, UniqueId = uniqueId };
                    if(Result != null)
                    {
                        break;
                    }
                    /*
                    Console.WriteLine($"이름: {name}");
                    Console.WriteLine($"제조사: {manufacturer}");
                    Console.WriteLine($"ProcessorId: {processorId}"); // PC 고유 ID 생성에 유용
                    Console.WriteLine($"코어 수: {cores}");
                    Console.WriteLine($"최대 클럭 속도: {maxClockSpeed} MHz");
                    Console.WriteLine(new string('-', 20));
                    */
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"오류 발생: {ex.Message}");
            }
            return Result;
        }

        public static IEnumerable<HxHardwareDiskDriveInfo> GetDiskDriveInfos()
        {
            //Debug.WriteLine("--- 디스크 드라이브 정보 ---");
            List<HxHardwareDiskDriveInfo> Result = null;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                if (searcher == null) { return Result; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return Result; }

                Result = new List<HxHardwareDiskDriveInfo>();
                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    ulong size = 0;
                    if (mo["Size"] != null)
                    {
                        size = (ulong)mo["Size"];
                    }

                    if (size <= 0 || (mo["Model"].ToStringEx().IsNullOrWhiteSpaceEx() == true && mo["SerialNumber"].ToStringEx().IsNullOrWhiteSpaceEx() == true )) { continue; }

                    foreach(PropertyData p in mo.Properties) { Debug.WriteLine($"{p.Name.ToStringEx()} : {p.Value.ToStringEx()}"); }

                    Result.Add(new HxHardwareDiskDriveInfo
                    {
                        Model = mo["Model"]?.ToString()?.Trim(),
                        SerialNumber = mo["SerialNumber"]?.ToString()?.Trim(), // 시리얼 (고유ID용)
                        InterfaceType = mo["InterfaceType"]?.ToString()?.Trim(),
                        SizeByte = size // Byte 단위 용량
                    });

                    /*
                    double sizeInGB = Math.Round((double)sizeInBytes / (1024 * 1024 * 1024), 2);

                    Console.WriteLine($"모델명: {model}");
                    Console.WriteLine($"시리얼 번호: {serialNumber}"); // PC 고유 ID 생성에 유용
                    Console.WriteLine($"인터페이스: {interfaceType}");
                    Console.WriteLine($"용량: {sizeInGB} GB");
                    Console.WriteLine(new string('-', 20));
                    */
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"오류 발생: {ex.Message}");
            }
            return Result?.ToArray();
        }

        /// <summary>
        /// 로컬 논리 드라이브(C:, D: 등)의 볼륨 ID와 용량 정보 조회
        /// (DriveType = 3 : 로컬 고정 디스크만)
        /// </summary>
        public static IEnumerable<HxHardwareDiskVolumeInfo> GetDiskVolumneInfos(int? driveTypeNo = 3)
        {
            List<HxHardwareDiskVolumeInfo> Result = null;
            try
            {
                string strQueryString = "SELECT DeviceID, VolumeSerialNumber, FreeSpace, Size, DriveType FROM Win32_LogicalDisk";
                if(driveTypeNo.IsNullOrWhiteSpaceEx() == false)
                {
                    strQueryString += $" WHERE DriveType = {driveTypeNo}";
                }
                // 로컬 고정 디스크(DriveType=3)만 쿼리합니다.
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(strQueryString);

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                Result = new List<HxHardwareDiskVolumeInfo>();
                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    ulong freeSpace = 0;
                    if (mo["FreeSpace"] != null)
                    {
                        freeSpace = (ulong)mo["FreeSpace"];
                    }

                    ulong totalSize = 0;
                    if (mo["Size"] != null)
                    {
                        totalSize = (ulong)mo["Size"];
                    }

                    var info = new HxHardwareDiskVolumeInfo
                    {
                        DeviceID = mo["DeviceID"]?.ToString(), // 예: "C:"
                        VolumeSerialNumber = mo["VolumeSerialNumber"]?.ToString(), // 예: "A0B1C2D3"
                        FreeSpaceBytes = freeSpace,
                        TotalSizeBytes = totalSize
                    };

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine($"--- 논리 드라이브: {info.DeviceID} ---");
                    //Debug.WriteLine($"볼륨 ID (Serial): {info.VolumeSerialNumber}");
                    //Debug.WriteLine($"전체 용량: {info.TotalSizeGB} ({info.TotalSizeBytes} Bytes)");
                    //Debug.WriteLine($"남은 용량 (Free): {info.FreeSpaceGB} ({info.FreeSpaceBytes} Bytes)");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 논리 디스크 정보 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }

        /// <summary>
        /// WMI (Win32_DiskPartition)를 사용하여
        /// 모든 물리 디스크 파티션 정보 조회
        /// (시스템 예약, 복구 파티션 등 숨겨진 파티션 포함)
        /// </summary>
        public static IEnumerable<HxHardwarePartitionInfo> GetPartitionInfos()
        {
            List<HxHardwarePartitionInfo> Result = new List<HxHardwarePartitionInfo>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT DeviceID, DiskIndex, Index, Type, Size, Bootable FROM Win32_DiskPartition");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    var info = new HxHardwarePartitionInfo
                    {
                        DeviceID = mo["DeviceID"]?.ToString()?.Trim(),
                        DiskIndex = (uint)(mo["DiskIndex"] ?? 0),
                        Index = (uint)(mo["Index"] ?? 0),
                        Type = mo["Type"]?.ToString()?.Trim(),
                        Bootable = (bool)(mo["Bootable"] ?? false),
                        Size = (ulong)(mo["Size"] ?? 0)
                    };

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    Debug.WriteLine($"--- 파티션 정보 ---");
                    Debug.WriteLine($"장치 ID: {info.DeviceID}");
                    Debug.WriteLine($"유형: {info.Type}");
                    Debug.WriteLine($"크기: {info.SizeGB}");
                    Debug.WriteLine($"부팅 가능: {info.Bootable}");
                    Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 파티션 정보 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }

        /// <summary>
        /// WMI (Win32_BaseBoard)를 사용하여 마더보드 정보 조회
        /// </summary>
        public static IEnumerable<HxHardwareBoardInfo> GetBoardInfos()
        {
            List<HxHardwareBoardInfo> Result = null;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT Manufacturer, Product, Model, SerialNumber FROM Win32_BaseBoard");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                Result = new List<HxHardwareBoardInfo>();
                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    var info = new HxHardwareBoardInfo
                    {
                        Manufacturer = mo["Manufacturer"]?.ToString()?.Trim(),
                        Product = mo["Product"]?.ToString()?.Trim(),
                        Model = mo["Model"]?.ToString()?.Trim(),
                        SerialNumber = mo["SerialNumber"]?.ToString()?.Trim() // 고유 ID 생성에 핵심
                    };

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine("--- 마더보드 정보 ---");
                    //Debug.WriteLine($"제조사: {info.Manufacturer}");
                    //Debug.WriteLine($"모델명: {info.Product}");
                    //Debug.WriteLine($"시리얼 번호: {info.SerialNumber}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 마더보드 정보 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }
        public static HxHardwareBoardInfo GetBoardInfo()
        {
            return GetBoardInfos()?.FirstOrDefault();
        }
        public static void GetNetworkAdapterInfo()
        {
            Console.WriteLine("--- 네트워크 어댑터 정보 (Win32_NetworkAdapterConfiguration) ---");
            try
            {
                // IP가 활성화된 어댑터만 조회
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE");

                foreach (ManagementObject mo in searcher.Get())
                {
                    Console.WriteLine($"설명: {mo["Description"]?.ToString()}");
                    Console.WriteLine($"MAC 주소: {mo["MACAddress"]?.ToString()}"); // 고유 ID

                    // IP 주소는 배열(string[])로 반환됩니다.
                    string[] ipAddresses = (string[])mo["IPAddress"];
                    if (ipAddresses != null && ipAddresses.Length > 0)
                    {
                        Console.WriteLine($"IP 주소: {string.Join(", ", ipAddresses)}");
                    }
                    Console.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"네트워크 정보 조회 오류: {ex.Message}");
            }
        }

        public static void GetOperatingSystemInfo()
        {
            Console.WriteLine("--- 운영체제 정보 (Win32_OperatingSystem) ---");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementObject mo in searcher.Get())
                {
                    Console.WriteLine($"이름 (Caption): {mo["Caption"]?.ToString()}");
                    Console.WriteLine($"버전: {mo["Version"]?.ToString()}");
                    Console.WriteLine($"아키텍처: {mo["OSArchitecture"]?.ToString()}");
                    Console.WriteLine($"빌드 번호: {mo["BuildNumber"]?.ToString()}");
                    Console.WriteLine($"설치 날짜: {FormatWmiDate(mo["InstallDate"]?.ToString())}");
                    Console.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OS 정보 조회 오류: {ex.Message}");
            }
        }

        // WMI 날짜 형식(yyyymmddHHMMSS.mmmmm...)을 DateTime으로 변환하는 도우미 메서드
        private static DateTime FormatWmiDate(string wmiDate)
        {
            if (string.IsNullOrEmpty(wmiDate) || wmiDate.Length < 14)
            {
                return DateTime.MinValue;
            }
            // yyyymmddHHMMSS
            return ManagementDateTimeConverter.ToDateTime(wmiDate);
        }

        /// <summary>
        /// WMI (Win32_NetworkAdapterConfiguration)를 사용하여 
        /// IP가 활성화된 네트워크 어댑터 정보 조회
        /// </summary>
        public static IEnumerable<HxHardwareNetworkInfo> GetNetworkInfos(bool isIpEnabledOny = true)
        {
            List<HxHardwareNetworkInfo> Result = new List<HxHardwareNetworkInfo>();
            try
            {
                string strQueryString = "SELECT Description, MACAddress, IPAddress, IPSubnet, DefaultIPGateway, DNSServerSearchOrder, DHCPEnabled, DHCPServer, IPEnabled FROM Win32_NetworkAdapterConfiguration";
                if (isIpEnabledOny == true)
                {
                    strQueryString += "  WHERE IPEnabled = TRUE";
                }
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(strQueryString);

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    var info = new HxHardwareNetworkInfo
                    {
                        Description = mo["Description"]?.ToString()?.Trim(),
                        MacAddress = mo["MACAddress"]?.ToString()?.Trim(),
                        IpAddresses = (string[])mo["IPAddress"] // IP 주소는 문자열 배열로 반환됨
                    };

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine("--- 활성 네트워크 어댑터 ---");
                    //Debug.WriteLine($"설명: {info.Description}");
                    //Debug.WriteLine($"MAC 주소: {info.MacAddress}");
                    //Debug.WriteLine($"IP 주소: {info.IpAddressString}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 네트워크 정보 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }
        /// <summary>
        /// WMI (Win32_PhysicalMemory)를 사용하여
        /// 개별 물리 RAM 스틱 정보 조회
        /// </summary>
        public static IEnumerable<HxHardwarePhysicalMemory> GetMemoryInfos()
        {
            List<HxHardwarePhysicalMemory> Result = null;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT DeviceLocator, Manufacturer, Capacity, Speed FROM Win32_PhysicalMemory");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                Result =  new List<HxHardwarePhysicalMemory>();
                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    var info = new HxHardwarePhysicalMemory
                    {
                        DeviceLocator = mo["DeviceLocator"]?.ToString()?.Trim(),
                        Manufacturer = mo["Manufacturer"]?.ToString()?.Trim(),
                        Speed = (uint)(mo["Speed"] ?? 0),
                        CapacityBytes = (ulong)(mo["Capacity"] ?? 0)
                    };

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine("--- 물리 메모리 슬롯 ---");
                    //Debug.WriteLine($"슬롯: {info.DeviceLocator}");
                    //Debug.WriteLine($"제조사: {info.Manufacturer}");
                    //Debug.WriteLine($"속도: {info.Speed} MHz");
                    //Debug.WriteLine($"용량: {info.CapacityGB}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 물리 메모리 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }
        public static HxHardwarePhysicalMemory GetMemoryInfo()
        {
            HxHardwarePhysicalMemory Result = null;
            
            // 집계(Aggregation)를 위한 임시 리스트
            
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT DeviceLocator, Manufacturer, Capacity, Speed FROM Win32_PhysicalMemory");

                if (searcher == null) { return null; }

                IEnumerable<HxHardwarePhysicalMemory> list = GetMemoryInfos();
                if (list == null || list.Any() != true) { return Result; }
                
                List<string> locators = new List<string>();
                List<string> manufacturers = new List<string>();
                List<uint> speeds = new List<uint>();
                ulong totalCapacity = 0;

                // 1. WMI 결과를 순회하며 값 수집 및 합산
                foreach (HxHardwarePhysicalMemory mo in list.Cast<HxHardwarePhysicalMemory>())
                {
                    if (mo == null || mo.DeviceLocator.IsNullOrWhiteSpaceEx() == true) { continue; }
                    // 문자열 속성 수집
                    locators.Add(mo.DeviceLocator);
                    manufacturers.Add(mo.Manufacturer);

                    // 숫자 속성 수집 (첫 번째 값을 사용하기 위함)
                    speeds.Add(mo.Speed);

                    // 용량 속성 합산 (SUM)
                    totalCapacity += mo.CapacityBytes;
                }

                // 2. 수집된 값으로 단일 '요약' 객체 생성
                Result = new HxHardwarePhysicalMemory
                {
                    // 요청대로 문자열은 ','로 결합 (null/empty 값 제외)
                    DeviceLocator = string.Join(", ", locators.Where(s => !string.IsNullOrEmpty(s))),

                    // 제조사는 중복을 제거하고 결합
                    Manufacturer = string.Join(", ", manufacturers.Where(s => !string.IsNullOrEmpty(s))),

                    // 용량은 합계
                    CapacityBytes = totalCapacity,

                    // 속도는 숫자이므로 첫 번째 감지된 값을 사용
                    Speed = speeds.Min()
                };

                // --- (콘솔 출력 예시) ---
                //Debug.WriteLine("--- (요약) 총 물리 메모리 정보 ---");
                //Debug.WriteLine($"총 용량: {Result.CapacityGB}");
                //Debug.WriteLine($"슬롯 위치: {Result.DeviceLocator}");
                //Debug.WriteLine($"제조사: {Result.Manufacturer}");
                //Debug.WriteLine($"속도 (첫번째): {Result.Speed} MHz");
                //Debug.WriteLine(new string('-', 20));

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 물리 메모리 요약 조회 오류: {ex.Message}");
            }

            return Result;
        }

        /// <summary>
        /// WMI (Win32_BIOS)를 사용하여 BIOS 정보 조회
        /// (시스템에 BIOS 정보는 하나만 존재)
        /// </summary>
        public static HxHardwareBiosInfo GetBiosInfo()
        {
            HxHardwareBiosInfo Result = null;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT Manufacturer, SerialNumber, SMBIOSBIOSVersion, Version FROM Win32_BIOS");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                // BIOS 정보는 시스템에 하나만 존재합니다.
                ManagementObject mo = list.Cast<ManagementObject>().FirstOrDefault();

                if (mo != null)
                {
                    Result = new HxHardwareBiosInfo
                    {
                        Manufacturer = mo["Manufacturer"]?.ToString()?.Trim(),
                        SerialNumber = mo["SerialNumber"]?.ToString()?.Trim(), // 고유 ID 생성에 사용 가능
                        SMBIOSBIOSVersion = mo["SMBIOSBIOSVersion"]?.ToString()?.Trim(),
                        Version = mo["Version"]?.ToString()?.Trim()
                    };

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine("--- BIOS 정보 ---");
                    //Debug.WriteLine($"제조사: {Result.Manufacturer}");
                    //Debug.WriteLine($"시리얼 번호: {Result.SerialNumber}");
                    //Debug.WriteLine($"BIOS 버전 (SMBIOS): {Result.SMBIOSBIOSVersion}");
                    //Debug.WriteLine($"BIOS 버전 (Full): {Result.Version}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI BIOS 정보 조회 오류: {ex.Message}");
            }

            return Result;
        }

        /// <summary>
        /// WMI (Win32_DesktopMonitor)를 사용하여
        /// PC에 연결된 모니터 정보 조회
        /// </summary>
        public static IEnumerable<HxHardwareMonitorInfo> GetMonitorInfos()
        {
            List<HxHardwareMonitorInfo> Result = new List<HxHardwareMonitorInfo>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT Name, DeviceID, PNPDeviceID, MonitorManufacturer, MonitorType FROM Win32_DesktopMonitor");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    var info = new HxHardwareMonitorInfo
                    {
                        Name = mo["Name"]?.ToString()?.Trim(),
                        DeviceID = mo["DeviceID"]?.ToString()?.Trim(),
                        PNPDeviceID = mo["PNPDeviceID"]?.ToString()?.Trim(),
                        MonitorManufacturer = mo["MonitorManufacturer"]?.ToString()?.Trim(),
                        MonitorType = mo["MonitorType"]?.ToString()?.Trim()
                    };

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine("--- 모니터 정보 ---");
                    //Debug.WriteLine($"이름: {info.Name}");
                    //Debug.WriteLine($"장치 ID: {info.DeviceID}");
                    //Debug.WriteLine($"PnP ID: {info.PNPDeviceID}");
                    //Debug.WriteLine($"제조사: {info.MonitorManufacturer}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 모니터 정보 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }

        /// <summary>
        /// WMI (Win32_VideoController)를 사용하여
        /// 현재 해상도 및 재생률 정보 조회
        /// </summary>
        public static IEnumerable<HxHardwareVideoController> GetVideoControllerInfos()
        {
            List<HxHardwareVideoController> Result = new List<HxHardwareVideoController>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT Name, CurrentHorizontalResolution, CurrentVerticalResolution, CurrentRefreshRate FROM Win32_VideoController");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    // 해상도 값이 0이 아닌, 실제 활성화된 컨트롤러 정보만 필터링
                    uint hRes = (uint)(mo["CurrentHorizontalResolution"] ?? 0);
                    if (hRes > 0)
                    {
                        var info = new HxHardwareVideoController
                        {
                            Name = mo["Name"]?.ToString()?.Trim(),
                            CurrentHorizontalResolution = hRes,
                            CurrentVerticalResolution = (uint)(mo["CurrentVerticalResolution"] ?? 0),
                            CurrentRefreshRate = (uint)(mo["CurrentRefreshRate"] ?? 0)
                        };

                        Result.Add(info);

                        // --- (콘솔 출력 예시) ---
                        //Debug.WriteLine("--- 비디오 컨트롤러 (해상도/재생률) ---");
                        //Debug.WriteLine($"이름: {info.Name}");
                        //Debug.WriteLine($"해상도: {info.Resolution}");
                        //Debug.WriteLine($"재생률: {info.CurrentRefreshRate} Hz");
                        //Debug.WriteLine(new string('-', 20));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 비디오 컨트롤러 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }

        /// <summary>
        /// System.Windows.Forms.Screen과 GDI/User32 API를 결합하여
        /// 모든 '활성화된' 모니터의 상세 정보(재생률, 방향, 배율 포함)를 가져옵니다.
        /// </summary>
        public static IEnumerable<HxHardwareScreenInfo> GetScreenInfos()
        {
            List<HxHardwareScreenInfo> Result = new List<HxHardwareScreenInfo>();

            try
            {
                // .NET 래퍼를 통해 '활성화된' 모든 모니터를 순회
                // (WMI의 Win32_DesktopMonitor보다 정확함)
                foreach (var screen in System.Windows.Forms.Screen.AllScreens)
                {
                    var info = new HxHardwareScreenInfo
                    {
                        DeviceName = screen.DeviceName,
                        Bounds = new HxRectangle(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height),
                        //Bounds = screen.Bounds,
                        WorkingArea = new HxRectangle(screen.WorkingArea.X, screen.WorkingArea.Y, screen.WorkingArea.Width, screen.WorkingArea.Height),
                        //WorkingArea = screen.WorkingArea,
                        IsPrimary = screen.Primary
                    };
                    
                    // --- 1. 재생률(RefreshRate) 및 방향(Orientation) ---
                    HxWin32HardwareScreen.DEVMODE devMode = new HxWin32HardwareScreen.DEVMODE();
                    devMode.dmSize = (short)Marshal.SizeOf(devMode);

                    // P/Invoke: EnumDisplaySettings (screen.DeviceName으로 현재 설정 조회)
                    if (HxWin32HardwareScreen.EnumDisplaySettings(screen.DeviceName,
                        HxWin32HardwareScreen.ENUM_CURRENT_SETTINGS, ref devMode))
                    {
                        info.RefreshRate = devMode.dmDisplayFrequency;

                        switch (devMode.dmDisplayOrientation)
                        {
                            case 0: info.Orientation = "가로 (0도)"; break;
                            case 1: info.Orientation = "세로 (90도)"; break;
                            case 2: info.Orientation = "가로 (180도)"; break;
                            case 3: info.Orientation = "세로 (270도)"; break;
                            default: info.Orientation = "알 수 없음"; break;
                        }
                    }

                    // --- 2. 확대/축소 배율 (DPI) ---
                    try
                    {
                        // P/Invoke: MonitorFromPoint (모니터 핸들 가져오기)
                        IntPtr hMonitor = HxWin32HardwareScreen.MonitorFromPoint(
                            screen.Bounds.Location, HxWin32HardwareScreen.MONITOR_DEFAULTTOPRIMARY);

                        // P/Invoke: GetDpiForMonitor (모니터별 DPI 조회)
                        if (HxWin32HardwareScreen.GetDpiForMonitor(hMonitor,
                            HxWin32HardwareScreen.MonitorDpiType.MDT_EFFECTIVE_DPI,
                            out uint dpiX, out uint dpiY) == 0) // S_OK
                        {
                            // 96 DPI = 100% (기본값)
                            info.ScalingPercent = (uint)Math.Round(dpiX / 96.0 * 100.0);
                        }
                    }
                    catch (Exception)
                    {
                        info.ScalingPercent = 100; // API 호출 실패 시 100%로 가정
                    }

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine($"--- 모니터 (GDI): {info.DeviceName} ---");
                    //Debug.WriteLine($"해상도: {info.Resolution}");
                    //Debug.WriteLine($"재생률: {info.RefreshRate} Hz");
                    //Debug.WriteLine($"배율: {info.ScalingPercent} %");
                    //Debug.WriteLine($"방향: {info.Orientation}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GDI 모니터 정보 조회 오류: {ex.Message}");
            }

            return Result.ToArray();
        }

        /// <summary>
        /// WMI (Win32_OperatingSystem)를 사용하여 OS 정보 조회
        /// </summary>
        protected static IEnumerable<HxOsInfo> GetOsInfos()
        {
            List<HxOsInfo> Result = null;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT Caption, Version, BuildNumber, OSArchitecture, InstallDate, LastBootUpTime FROM Win32_OperatingSystem");

                if (searcher == null) { return null; }

                ManagementObjectCollection list = searcher.Get();
                if (list == null || list.Count <= 0) { return null; }

                Result =  new List<HxOsInfo>();
                // OS 정보는 보통 단일 인스턴스입니다.
                foreach (ManagementObject mo in list.Cast<ManagementObject>())
                {
                    var info = new HxOsInfo
                    {
                        Caption = mo["Caption"]?.ToString()?.Trim(),
                        Version = mo["Version"]?.ToString()?.Trim(),
                        BuildNumber = mo["BuildNumber"]?.ToString()?.Trim(),
                        OsArchitecture = mo["OSArchitecture"]?.ToString()?.Trim(),

                        // WMI 날짜(DMTF 형식)를 .NET DateTime으로 변환
                        InstallDate = FormatWmiDate(mo["InstallDate"]?.ToString()),
                        LastBootUpTime = FormatWmiDate(mo["LastBootUpTime"]?.ToString())
                    };

                    Result.Add(info);

                    // --- (콘솔 출력 예시) ---
                    //Debug.WriteLine("--- 운영체제 정보 ---");
                    //Debug.WriteLine($"이름: {info.Caption}");
                    //Debug.WriteLine($"버전: {info.Version} (Build {info.BuildNumber})");
                    //Debug.WriteLine($"아키텍처: {info.OsArchitecture}");
                    //Debug.WriteLine($"설치 날짜: {info.InstallDate}");
                    //Debug.WriteLine($"마지막 부팅: {info.LastBootUpTime}");
                    //Debug.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI OS 정보 조회 오류: {ex.Message}");
            }

            return Result?.ToArray();
        }

        public static HxOsInfo GetOsInfo()
        {
            return GetOsInfos()?.FirstOrDefault();
        }

        private static void GetPhysicalMemoryInfo()
        {
            //Console.WriteLine("--- 개별 물리 메모리 정보 (Win32_PhysicalMemory) ---");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

                ulong totalCapacity = 0;

                foreach (ManagementObject mo in searcher.Get())
                {
                    string bankLabel = mo["BankLabel"]?.ToString();
                    string deviceLocator = mo["DeviceLocator"]?.ToString(); // 예: "DIMM 0"
                    string manufacturer = mo["Manufacturer"]?.ToString();
                    string partNumber = mo["PartNumber"]?.ToString();
                    string speed = mo["Speed"]?.ToString(); // MHz

                    ulong capacity = (ulong)mo["Capacity"]; // Bytes
                    double capacityGB = Math.Round((double)capacity / (1024 * 1024 * 1024), 2);
                    totalCapacity += capacity;

                    Console.WriteLine($"슬롯 (DeviceLocator): {deviceLocator}");
                    Console.WriteLine($"뱅크 레이블: {bankLabel}");
                    Console.WriteLine($"제조사: {manufacturer}");
                    Console.WriteLine($"파트 번호: {partNumber}");
                    Console.WriteLine($"속도: {speed} MHz");
                    Console.WriteLine($"용량: {capacityGB} GB");
                    Console.WriteLine(new string('-', 20));
                }

                double totalCapacityGB = Math.Round((double)totalCapacity / (1024 * 1024 * 1024), 2);
                Console.WriteLine($"==> 총 물리적 설치 용량: {totalCapacityGB} GB");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"물리 메모리 정보 조회 오류: {ex.Message}");
            }
        }

        private static void GetOperatingSystemMemoryInfo()
        {
            //Console.WriteLine("--- OS 기준 메모리 정보 (Win32_OperatingSystem) ---");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

                foreach (ManagementObject mo in searcher.Get())
                {
                    // 값들이 Kilobytes 단위로 반환됩니다.
                    ulong totalVisibleKB = (ulong)mo["TotalVisibleMemorySize"];
                    ulong freePhysicalKB = (ulong)mo["FreePhysicalMemory"];
                    ulong usedKB = totalVisibleKB - freePhysicalKB;

                    // GB로 변환 (1024 * 1024)
                    double totalVisibleGB = Math.Round((double)totalVisibleKB / (1024 * 1024), 2);
                    double freePhysicalGB = Math.Round((double)freePhysicalKB / (1024 * 1024), 2);
                    double usedGB = Math.Round((double)usedKB / (1024 * 1024), 2);

                    Console.WriteLine($"OS 인식 총 메모리: {totalVisibleGB} GB ({totalVisibleKB} KB)");
                    Console.WriteLine($"사용 가능 메모리: {freePhysicalGB} GB ({freePhysicalKB} KB)");
                    Console.WriteLine($"현재 사용 중 메모리: {usedGB} GB ({usedKB} KB)");
                    Console.WriteLine(new string('-', 20));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OS 메모리 정보 조회 오류: {ex.Message}");
            }
        }
    }
}