using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace HxCore.Win
{

    public class HxWin32SoftwareSearcher
    {
        public static IEnumerable<HxSoftwareInfo> GetInstalledSoftware()
        {
            List<HxSoftwareInfo> softwareList = new List<HxSoftwareInfo>();
            // HKEY_CURRENT_USER (현재 사용자)
            ProcessRegistryHive(Registry.CurrentUser, new[] { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" }, softwareList);

            string[] registryPaths = {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };
            // HKEY_LOCAL_MACHINE (모든 사용자)
            ProcessRegistryHive(Registry.LocalMachine, registryPaths, softwareList);

            return softwareList?.ToArray();
        }

        private static void ProcessRegistryHive(RegistryKey hive, string[] keyPaths, List<HxSoftwareInfo> softwareList)
        {
            foreach (string keyPath in keyPaths)
            {
                using (RegistryKey key = hive.OpenSubKey(keyPath))
                {
                    if (key == null) continue;

                    foreach (string subKeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                        {
                            if (subKey == null) continue;

                            // DisplayName이 없거나 SystemComponent가 1이면 (예: 핫픽스) 건너뜁니다.
                            var displayName = subKey.GetValue("DisplayName") as string;
                            var systemComponent = subKey.GetValue("SystemComponent");

                            if (!string.IsNullOrEmpty(displayName) && systemComponent == null)
                            {
                                var info = new HxSoftwareInfo
                                {
                                    ProductID = subKeyName, // 7. GUID (또는 키 이름)
                                    DisplayName = displayName, // 1. 제품명
                                    Publisher = subKey.GetValue("Publisher") as string, // 2. 게시자
                                    DisplayVersion = subKey.GetValue("DisplayVersion") as string, // 6. 버전
                                    InstallLocation = subKey.GetValue("InstallLocation") as string, // 8. 설치 폴더

                                    // 3. 설치 일시 (yyyyMMdd 형식)
                                    InstallDate = ParseInstallDate(subKey.GetValue("InstallDate") as string),

                                    // 5. 크기 (KB)
                                    EstimatedSizeKB = ParseSize(subKey.GetValue("EstimatedSize")),

                                    // 9. 설치 파일 (메인 실행 파일)
                                    MainExecutableFullPath = ParseDisplayIconPath(subKey.GetValue("DisplayIcon") as string),
                                    MainExecutableFileSize = 0
                                };
                                if(HxFile.IsFileExists(info.MainExecutableFullPath) == true)
                                {
                                    info.MainExecutableFileSize = HxFile.GetFileSize(info.MainExecutableFullPath);
                                }
                                if (
                                    info.ProductID.IsNullOrWhiteSpaceEx() != true && info.DisplayName.IsNullOrWhiteSpaceEx() != true 
                                    && softwareList.Where(r => r.ProductID == info.ProductID && r.DisplayName == info.DisplayName).Count() <= 0
                                    )
                                {
                                    softwareList.Add(info);
                                }
                                else
                                {
                                    //Debug.WriteLine(info.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        // "yyyyMMdd" 형식의 문자열을 DateTime?으로 변환
        private static DateTime? ParseInstallDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString)) return null;

            if (DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            return null;
        }

        // object(DWORD 또는 QWORD)를 long?으로 변환
        private static long? ParseSize(object sizeObj)
        {
            if (sizeObj == null) return null;
            if (sizeObj is int valInt) return (long)valInt;
            if (sizeObj is long valLong) return valLong;

            // 가끔 문자열로 저장되는 경우
            if (long.TryParse(sizeObj.ToString(), out long result)) return result;

            return null;
        }

        // "C:\Path\To\App.exe,0" 형식에서 실행 파일 경로만 추출
        private static string ParseDisplayIconPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            path = path.Trim('\"'); // 양쪽 따옴표 제거

            int commaIndex = path.LastIndexOf(',');
            if (commaIndex != -1)
            {
                // 쉼표 뒤가 숫자(아이콘 인덱스)인지 확인
                if (int.TryParse(path.Substring(commaIndex + 1), out _))
                {
                    path = path.Substring(0, commaIndex);
                }
            }

            // 실제 파일이 존재하는지, 확장자가 .exe인지 확인하여 신뢰도 향상
            if (File.Exists(path) && path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            // DisplayIcon이 .exe가 아닌 .ico 파일 등을 가리킬 수 있으므로
            // 유효하지 않으면 null 반환 (또는 파싱된 경로를 그대로 반환)
            return null;
        }
    }
}
