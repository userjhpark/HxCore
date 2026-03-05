using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win
{
    public class HxIniFile
    {
        string defaultFileName = "Config.ini";
        string ConfigFileFullName { get; set; }

        public HxIniFile(string path)
        {
            if (path.IsNullOrWhiteSpaceEx())
            {
                path = defaultFileName;
            }
            if (!File.Exists(path))
            {
                path = Path.Combine(HxUtils.AppBaseDir, path);
            }

            if (File.Exists(path))
            {
                ConfigFileFullName = path;
            }
        }


        /// <summary>
        /// ini 파일에 정보를 기록하기 위한 함수
        /// </summary>
        /// <param name="Section">섹션명</param>
        /// <param name="Key">키 명</param>
        /// <param name="Value">기록할 값</param>
        private void _IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.ConfigFileFullName);
        }
        /// <summary>
        /// ini 파일에 정보를 가져오기 위한 함수
        /// </summary>
        /// <param name="Section">섹션명</param>
        /// <param name="Key">키 명</param>
        /// <returns>가져온 값</returns>
        private string _IniReadValue(string Section, string Key)
        {
            StringBuilder sb = new StringBuilder(2000);
            int i = GetPrivateProfileString(Section, Key, "", sb, 2000, this.ConfigFileFullName);
            return sb.ToString().Trim();
        }

        /// <summary>
        /// 문자열 타입으로 값을 기록한다
        /// </summary>
        /// <param name="Section">섹션명</param>
        /// <param name="Key">키 명</param>
        /// <param name="Value">기록 할 문자열</param>
        public void SetString(string Section, string Key, string Value)
        {
            _IniWriteValue(Section, Key, Value.Trim());
        }
        /// <summary>
        /// 정수 타입으로 값을 기록한다
        /// </summary>
        /// <param name="Section">섹션명 </param>
        /// <param name="Key">키 명</param>
        /// <param name="Value">기록 할  정수값</param>
        /// 
        public void SetInteger(string Section, string Key, int Value)
        {
            _IniWriteValue(Section, Key, Value.ToString().Trim());
        }
        /// <summary>
        /// 논리 타입으로 값을 기록 한다.
        /// </summary>
        /// <param name="Section">섹션명</param>
        /// <param name="Key">키 명</param>
        /// <param name="Value">기록 할 논리 값</param>
        public void SetBoolean(string Section, string Key, bool Value)
        {
            _IniWriteValue(Section, Key, Value ? "1" : "0");
        }
        /// <summary>
        /// 논리 타입으로 값을 가져온다
        /// </summary>
        /// <param name="Section">섹션명</param>
        /// <param name="Key">키 값</param>
        /// <param name="def">기본값</param>
        /// <returns>가져온 논리값</returns>
        public bool GetBoolean(string Section, string Key, bool def)
        {
            bool temp = def;
            string stTemp = _IniReadValue(Section, Key);
            if (stTemp == "") return def;
            if (stTemp.Trim() == "1" || stTemp.Trim() == "true") return true;
            else return false;
        }
        /// <summary>
        /// 문자열로 값을 가져 온다
        /// </summary>
        /// <param name="Section">섹션명</param>
        /// <param name="Key">키 명</param>
        /// <returns>가져온 문자열</returns>
        public string GetString(string Section, string Key)
        {
            return _IniReadValue(Section, Key).Trim();
        }
        /// <summary>
        /// 정수 타입으로 값을 가져 온다
        /// </summary>
        /// <param name="Section">섹션명</param>
        /// <param name="Key">키 명</param>
        /// <param name="def">기본값</param>
        /// <returns>가져온 정수값</returns>
        public int GetInteger(string Section, string Key, int def = -1)
        {
            int temp = def;
            string stTemp = _IniReadValue(Section, Key);
            if (stTemp == "") return def;
            try
            {
                temp = int.Parse(stTemp.Trim());
            }
            catch (Exception)
            {
                return def;
            }
            return temp;
        }




        public static int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath)
        {
            return HxWin.GetPrivateProfileString(section, key, def, retVal, size, filePath);
        }
        public static long WritePrivateProfileString(string section, string key, string val, string filePath)
        {
            return HxWin.WritePrivateProfileString(section, key, val, filePath);
        }
    }
}
