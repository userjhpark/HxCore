/*
using LibreHardwareMonitor;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore
{
    public class HxPcHardwareLibre
    {
        public HxPcHardwareLibre()
        {
        }

        /// <summary>
        /// LHM의 모든 하드웨어와 센서를 업데이트하기 위한 헬퍼 클래스
        /// </summary>
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update(); // 센서 값 업데이트
                foreach (IHardware subHardware in hardware.SubHardware)
                    subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        private static HxHardwareCpuInfo GetCpuInfo()
        {
            HxHardwareCpuInfo Result = null;

            // LHM Computer 객체 초기화
            Computer computer = new Computer
            {
                IsCpuEnabled = true // CPU 정보만 활성화
            };

            if (computer == null) { return Result; }

            try
            {
                computer.Open(); // 하드웨어 스캔 시작
                computer.Accept(new UpdateVisitor()); // 센서 값 1회 업데이트
                if(computer.IsCpuEnabled != true) {  return Result; }

                IEnumerable<IHardware> list = computer.Hardware.Where(h => h.HardwareType == HardwareType.Cpu);
                if(list == null || list.Any() != true) { return Result; }

                // CPU 하드웨어 찾기 (보통 1개)
                foreach (IHardware hardware in list)
                {
                    var item = new HxHardwareCpuInfo();

                    // [1] 제품명 (LHM이 더 자세한 이름을 제공할 수 있음)
                    item.Name = hardware.Name;
                    if(item.Name.IsNullOrWhiteSpaceEx() == true) { continue; }

                    // [2] 고유ID (LHM에서 제공 불가)
                    item.ProcessorId = null; // WMI로만 가능

                    // [3] 제조사 (LHM에서 제공 불가)
                    item.Manufacturer = null; // WMI로만 가능

                    // [4] 코어 수 (LHM은 보통 '논리' 코어(스레드) 수를 감지)
                    // 'Core #' 이름을 가진 클럭 센서의 개수를 셉니다.
                    int coreCount = hardware.Sensors.Count(s =>
                        s.SensorType == SensorType.Clock && s.Name.StartsWith("Core #"));

                    item.NumberOfCores = coreCount > 0 ? coreCount : 0;

                    // [5] 최대 클럭 속도 (LHM은 '관측된' 최대 속도를 제공)
                    // "Core Clock" 센서 중 가장 높은 Max 값을 찾습니다.
                    ISensor clockSensor = hardware.Sensors
                        .Where(s => s.SensorType == SensorType.Clock && s.Name.Contains("Core"))
                        .OrderByDescending(s => s.Max)
                        .FirstOrDefault();

                    if (clockSensor != null && clockSensor.Max.HasValue)
                    {
                        // LHM은 MHz 단위의 float? 값으로 제공
                        item.MaxClockSpeed = $"{Math.Round(clockSensor.Max.Value, 0)} MHz";
                    }
                    else
                    {
                        // WMI의 'MaxClockSpeed'는 정격 속도라 LHM 값과 다릅니다.
                        item.MaxClockSpeed = "N/A";
                    }

                    //Debug.WriteLine($"{hardware.Parent}");

                    Result = item;
                    if(Result != null)
                    {
                        break;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                computer?.Close();
            }
            return Result;
        }

        /// <summary>
        /// LibreHardwareMonitor를 사용하여 저장 장치 정보 조회 (제한적)
        /// </summary>
        public static IEnumerable<HxHardwareDiskDriveInfo> GetDiskDriveInfo()
        {
            List<HxHardwareDiskDriveInfo> Result = new List<HxHardwareDiskDriveInfo>();
            Computer computer = new Computer
            {
                IsStorageEnabled = true // 저장 장치 활성화
            };

            try
            {
                computer.Open();
                computer.Accept(new UpdateVisitor());

                foreach (IHardware hardware in computer.Hardware.Where(h => h.HardwareType == HardwareType.Storage))
                {
                    var info = new HxHardwareDiskDriveInfo
                    {
                        // [1] 모델명 (LHM이 제공)
                        Model = hardware.Name,

                        // [2] 시리얼 번호 (LHM 제공 불가)
                        SerialNumber = "N/A (LHM)",

                        // [3] 인터페이스 (LHM 제공 불가)
                        InterfaceType = "N/A (LHM)",

                        // [4] 용량 (LHM 제공 불가 - LHM은 센서(온도, 사용률)에 집중)
                        SizeByte = 0
                    };

                    // (참고) LHM은 총용량 대신 '사용된 공간' 센서를 제공합니다.
                    // 예: hardware.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Used Space");

                    Result.Add(info);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LHM 디스크 정보 조회 오류: {ex.Message}");
            }
            finally
            {
                computer.Close();
            }

            return Result?.ToArray();
        }
    }
}
*/