using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq; // .Cast<T>(), .FirstOrDefault()를 위해 필요
using System.Management; // System.Management NuGet 패키지 필요
using System.Security.Cryptography; // SHA256를 위해 필요
using System.Text; // Encoding.UTF8을 위해 필요

namespace HxCore.Win
{

    public class HxWin32Fingerprint
    {
        /// <summary>
        /// 1~3순위 WMI 하드웨어 ID를 조합하여
        /// 이 PC의 고유한 SHA256 해시 ID를 생성합니다.
        /// </summary>
        public static string GetUniqueMachineId()
        {
            // 1. 신뢰도 순으로 WMI 값 조회
            // (WMI 쿼리 횟수를 줄이기 위해 각 클래스별로 한 번만 쿼리)

            string uuid = string.Empty;
            string baseBoardSerial = string.Empty;
            string processorId = string.Empty;

            try
            {
                // 1순위: SMBIOS UUID
                uuid = GetWmiValue("Win32_ComputerSystemProduct", "UUID");

                // 2순위: 마더보드 시리얼
                baseBoardSerial = GetWmiValue("Win32_BaseBoard", "SerialNumber");

                // 3순위: CPU ID
                processorId = GetWmiValue("Win32_Processor", "ProcessorId");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI 조회 중 오류 발생: {ex.Message}");
                // WMI 접근이 불가능한 경우(예: 권한 문제)에 대비
            }

            // 2. 유효한 ID만 필터링하여 리스트에 추가
            List<string> validIdComponents = new List<string>();

            if (IsValidHardwareId(uuid))
            {
                validIdComponents.Add(uuid);
            }
            if (IsValidHardwareId(baseBoardSerial))
            {
                validIdComponents.Add(baseBoardSerial);
            }
            if (IsValidHardwareId(processorId))
            {
                validIdComponents.Add(processorId);
            }

            // 3. 유효한 ID가 하나도 없는 경우
            if (validIdComponents.Count == 0)
            {
                //Debug.WriteLine("경고: 유효한 하드웨어 ID를 하나도 찾지 못했습니다.");
                // (대안: MAC 주소나 MachineGuid를 사용하거나, null 반환)
                string strVolumeId = HxWin32HardwareSearcher.GetDiskVolumneInfos()?.FirstOrDefault()?.VolumeSerialNumber;
                if (strVolumeId.IsNullOrWhiteSpaceEx() != true)
                {
                    validIdComponents.Add(strVolumeId);
                }
                string strBiosId = HxWin32HardwareSearcher.GetBiosInfo()?.SerialNumber;
                if(strBiosId.IsNullOrWhiteSpaceEx() != true)
                {
                    validIdComponents.Add(strBiosId);
                }
                string strBoardId = HxWin32HardwareSearcher.GetBoardInfo()?.SerialNumber;
                if (strBoardId.IsNullOrWhiteSpaceEx() != true)
                {
                    validIdComponents.Add(strBoardId);
                }
            }

            if(validIdComponents.Count == 0)
            {
                return null;
            }

            // 4. 유효한 ID들을 일관된 순서로 조합
            string rawId = string.Join("-", validIdComponents);
            Debug.WriteLine($"Raw ID String: {rawId}");

            // 5. SHA256으로 해시하여 최종 ID 생성
            return ComputeSha256Hash(rawId);
        }

        /// <summary>
        /// 지정된 WMI 클래스와 속성에서 첫 번째 값을 가져옵니다.
        /// </summary>
        private static string GetWmiValue(string wmiClass, string wmiProperty)
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher($"SELECT {wmiProperty} FROM {wmiClass}");

                ManagementObject mo = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                return mo?[wmiProperty]?.ToString()?.Trim();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"WMI GetValue 오류 ({wmiClass}.{wmiProperty}): {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// WMI에서 가져온 값이 유효한지 검사합니다.
        /// (null, 빈 값, 일반적인 기본값/오류 값 필터링)
        /// </summary>
        private static bool IsValidHardwareId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            // 일반적인 무효(Invalid) 값 필터링
            string upperId = id.ToUpper();
            if (upperId.Contains("TO BE FILLED") ||
                upperId.Contains("NOT SPECIFIED") ||
                upperId.Contains("DEFAULT") ||
                upperId.Contains("NONE"))
            {
                return false;
            }

            // 모두 '0' 또는 'F'로 채워진 UUID/시리얼 필터링
            // (예: 00000000-0000-0000-0000-000000000000)
            if (upperId.Replace("0", "").Replace("-", "").Length == 0)
            {
                return false;
            }
            if (upperId.Replace("F", "").Replace("-", "").Length == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 입력 문자열의 SHA256 해시 값을 계산합니다.
        /// </summary>
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // 바이트 배열로 변환
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // 바이트 배열을 16진수 문자열로 변환
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // 2자리 소문자 16진수
                }
                return builder.ToString();
            }
        }
    }
}
