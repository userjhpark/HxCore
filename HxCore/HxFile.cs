using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore
{
    public class HxFile
    {
        
        #region FIle 관련
        /// <summary>
        /// 실제 풀 경로(절대 경로)
        /// </summary>
        /// <param name="fileName">파일 경로</param>
        /// <returns></returns>
        public static string GetFileFullPath(string fileName)
        {
            string Result = null;
            if (fileName.IsNullOrWhiteSpaceEx() == true) return Result;

            if (File.Exists(fileName))
            {
                Result = Path.GetFullPath(fileName);
            }
            else if(IsFullName(fileName) == true)
            {
                Result = fileName;
            }
            return Result;
        }

        public static string GetStringReplace(string input, string oldValue = null, string newValue = null)
        {
            return HxString.GetFileDirReplace(input, oldValue, newValue);
        }
        // <summary>
        /// 입력된 파일에서 해당 폴더 경로만 추출
        /// </summary>
        /// <param name="fileName">파일명 또는 전체 파일 경로</param>
        /// <param name="bLastDirSCharRemove">마지막 폴더구분 기호(\) 생략 여부</param>
        /// <returns>폴더 경로</returns>
        public static string GetFileDirPath(string fileName, bool bLastDirSCharRemove = true)
        {
            string Result = string.Empty;
            if (fileName.IsNullOrWhiteSpaceEx() == true) return Result;

            try
            {
                Result = System.IO.Path.GetDirectoryName(fileName);
            }
            catch (Exception ex)
            {
                HxBase.DebugMessageOut(ex.Message);

                string[] tempStr = fileName.Split(new char[] { GetDirSeparatorChar() });
                for (int i = 0; i < tempStr.Length - 1; ++i)
                {
                    if (i > 0)
                    {
                        Result += GetDirSeparatorChar();
                    }
                    Result += tempStr[i];
                    // 경로의 끝에는 \ 문자 추가
                    if (bLastDirSCharRemove == true)
                    {
                        if (i < tempStr.Length - 2) Result += GetDirSeparatorChar();
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 입력된 파일에서 해당 파일명 추출
        ///     (확장자 포함)
        /// </summary>
        /// <param name="fileName">파일명 또는 전체 파일 경로</param>
        /// <returns>파일명</returns>
        public static string GetFileName(string fileName, bool isSafeFileName = false)
        {
            string Result = null;
            if (fileName.IsNullOrWhiteSpaceEx() == true) return Result;

            try
            {
                fileName = GetLongFileName(fileName);
                
                string[] tempStr = fileName.Split(new char[] { GetDirSeparatorChar(), '\\' });
                Result = tempStr[tempStr.Length - 1];
            }
            catch (Exception ex)
            {
                Result = System.IO.Path.GetFileName(fileName);
                HxBase.DebugMessageOut(ex.Message);
            }
            if (Result.IsNullOrWhiteSpaceEx() != true && isSafeFileName == true)
            {
                Result = GetSafeFileName(Result);
            }
            return Result;
        }

        public static string GetSafeFileName(string fileName, string replaceChar = null, bool bSpecialCharReplace = false)
        {
            string Result = GetFileName(fileName);
            if (Result.IsNullOrWhiteSpaceEx() != true)
            {
                
                if (bSpecialCharReplace == true)
                {
                    Result = HxString.GetSafeFileName(Result, true);
                }

                Result = Result.RegexReplaceEx(HxDefs._REGEX_BAD_NAME_PERTTERN_, replaceChar ?? string.Empty);
            }
            return Result;
        }

        public static string GetSafeDirName(string inputString, bool bOptionSpecialCharReplace = true, string replaceChar = null)
        {
            string Result = inputString;
            if (Result.IsNullOrWhiteSpaceEx() != true)
            {
                Result = HxString.GetSafeDirName(Result, bOptionSpecialCharReplace, replaceChar);
            }
            return Result;
        }

        /// <summary>
        /// 입력된 파일에서 확장자를 뺀 순수 파일명 추출
        ///     (확장자 제외)
        /// </summary>
        /// <param name="fileName">파일명 또는 파일 경로</param>
        /// <param name="bFindAllReplace">확장자 형식으로 끝나는 모든(유사) 문자열 제거 여부(기본값 : True - 기본 확장자만 제거)</param>
        /// <returns>파일명(확장자 제외)</returns>
        public static string GetFileNameWithOutExt(string fileName, bool bFindAllReplace = false)
        {
            string Result = string.Empty;
            if (fileName.IsNullOrWhiteSpaceEx() == true) return Result;

            try
            {
                fileName = GetLongFileName(fileName);
                Result = System.IO.Path.GetFileNameWithoutExtension(fileName);
                
            }
            catch (Exception ex)
            {
                HxBase.DebugMessageOut(ex.Message);
                string strFileName = GetFileName(fileName);
                /*
                string strFileExt = getFileNameExt(FileName);
                if (!strFileExt.StartsWith("."))
                {
                    strFileExt = "." + strFileExt;
                }*/
                Result = strFileName.Substring(0, strFileName.LastIndexOf('.'));
            }
            string pattern = @"(\.[0-9a-zA-Z]{1,4})+$";
            if (bFindAllReplace == true && Regex.IsMatch(Result, pattern))
            {
                //Result = System.IO.Path.GetFileNameWithoutExtension(Result);
                Result = Result.RegexReplaceEx(pattern, string.Empty);
            }
            return Result;
        }

        /// <summary>
        /// 입력된 파일에서 확장자 추출
        ///     ('.'삭제)
        /// </summary>
        /// <param name="fileName">파일명 또는 파일 경로</param>
        /// <returns>파일 확장자</returns>
        public static string GetFileNameExt(string fileName, bool isFileExtLower = true)
        {
            string Result = string.Empty;
            try
            {
                string strFileName = GetLongFileName(fileName);
                if (strFileName.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = System.IO.Path.GetExtension(strFileName);
                }
                else
                {
                    Result = strFileName;
                }
                int LastIndex = Result.LastIndexOf('.');
                if (LastIndex > -1)
                {
                    Result = Result.Substring(LastIndex + 1);
                }
            }
            catch (Exception ex)
            {
                HxBase.DebugMessageOut(ex.Message);
                int LastIndex = fileName.LastIndexOf('.');
                if (LastIndex > -1)
                {
                    Result = fileName.Substring(LastIndex + 1);
                }
            }
            if(Result.IsNullOrWhiteSpaceEx() != true && isFileExtLower == true)
            {
                Result = Result.ToLower();
            }
            return Result;
        }
        /// <summary>
        /// 입력된 파일에서 확장자 추출
        ///     ('.'삭제)
        /// </summary>
        /// <param name="fileName">파일명 또는 파일 경로</param>
        /// <returns>파일 확장자</returns>
        public static string GetFileExt(string fileName, bool isFileExtLower = true)
        {
            return GetFileNameExt(fileName, isFileExtLower);
        }
        public static string GetFileMimeType(string fileName)
        {
            string fileExt = GetFileNameExt(fileName);
            return GetMimeType(fileExt);
        }
        public static long GetFileSize(FileInfo fi)
        {
            long Result = 0;
            //FileInfo fi = new FileInfo(fullName);
            if (fi != null && fi.Exists)
            {
                Result = fi.Length;
            }
            return Result;
        }
        public static long GetFileSize(string fileName)
        {
            long Result = -1;
            if (fileName.IsNullOrWhiteSpaceEx() != true)
            {
                Result = 0;
                fileName = GetLongFileName(fileName);
                string fullName = GetFileFullPath(fileName);
                if (fullName.IsNullOrWhiteSpaceEx() != true)
                {
                    FileInfo fi = new FileInfo(fullName);
                    if (fi != null && fi.Exists)
                    {
                        Result = fi.Length;
                    }
                }
            }
            return Result;
        }
        /// <summary>
        /// 용량 단위 표기한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>문자방식 사이즈</returns>
        public static string GetSize2String(int size)
        {
            return GetSize2HumanSizeString(size);
        }
        /// <summary>
        /// 용량 단위 표기한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>문자방식 사이즈</returns>
        public static string GetSize2HumanString(long size)
        {
            return GetSize2HumanSizeString<long>(size);
        }
        /// <summary>
        /// 용량 단위 표기한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>문자방식 사이즈</returns>
        public static string GetSize2HumanSizeString(long? size)
        {
            if (size == null)
            {
                return "0 KB";
            }
            else
            {
                return GetSize2HumanSizeString<long>(size.ToLongEx());
            }
        }
        /// <summary>
        /// KB단위로 변경한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>사이즈 문자열</returns>
        public static string GetSize2KByteString(ulong size)
        {
            return GetSize2KByteString(size.ToLongEx());
        }
        /// <summary>
        /// KB단위로 변경한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>사이즈 문자열</returns>
        public static string GetSize2KByteString(long size)
        {
            decimal Result; //size.ToDecimalEx(0);
            if (size > 0)
            {
                Result = size / 1024m;
                Result = Math.Ceiling(Result);
            }
            else
            {
                Result = 0;
            }

            return Result.ToNumberStringEx("N0") + " KB";
        }
        /// <summary>
        /// KB단위로 변경한 문자방식 사이즈 (KB 단위 포함)
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>사이즈 문자열</returns>
        public static string GetSize2KByteString(long? size)
        {
            if(size == null)
            {
                return "O KB";
            }
            else
            {
                return GetSize2KByteString(size.ToLongEx());
            }
        }

        /// <summary>
        /// 용량 단위 표기한 문자방식 사이즈
        /// </summary>
        /// <typeparam name="T">입력 타입</typeparam>
        /// <param name="size">파일 크기</param>
        /// <returns>사이즈 문자열</returns>
        public static string GetSize2HumanSizeString<T>(T size)
        {
            if(size == null) return null;
            int i = 0;
            double real_size = Convert.ToDouble(size);
            string[] iec = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            string Result;
            try
            {
                //Result = string.Empty;

                while ((real_size / 1024) > 1)
                {
                    real_size = real_size / 1024;
                    i++;
                }
                if ((real_size / 1024) >= 1)
                {
                    real_size = real_size / 1024;
                    i++;
                }
                string strSize = real_size.ToString();
                int iDotPos = strSize.IndexOf('.');
                if (iDotPos > 0)
                    iDotPos += 4;
                else
                    iDotPos = strSize.Length;
                Result = strSize.Substring(0, iDotPos) + iec[i];
            }
            catch (Exception ex)
            {
                HxBase.DebugMessageOut(ex.Message);
                throw ex;
            }

            return Result;
        }

        /// <summary>
        /// 용량 단위 표기한 문자방식 사이즈
        /// </summary>
        /// <param name="size">파일 크기</param>
        /// <returns>문자방식 사이즈</returns>
        public static string GetSize2HumanSizeString(int size)
        {
            return GetSize2HumanSizeString<int>(size);
        }

        public static bool IsFullName(string path)
        {
            string pattern = HxDefs._REGEX_FILE_FULLNAME_START_PATTERN_;
            return path.IsRegexMatchEx(pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 파일 Exist
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns>존재 여부?</returns>
        public static bool IsFileExists(string path, bool isSafeName = false)
        {
            bool Result = false;
            string strFilePath = GetLongFileName(path);
            if (isSafeName == true)
            {
                strFilePath = GetSafeFileName(strFilePath);
            }
            try
            {
                Result = File.Exists(strFilePath);
            }
            catch (Exception ex)
            {
                HxBase.DebugMessageOut(ex.Message);
                //throw ex;
            }

            if (Result != true)
            {
                try
                {
                    FileInfo fi = new FileInfo(strFilePath);
                    Result = fi.Exists;
                }
                catch (Exception ex)
                {
                    HxBase.DebugMessageOut(ex.Message);
                }
            }

            return Result;
        }

        /// <summary>
        /// 파일 Exist
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns>존재 여부?</returns>
        public static bool FileExists(string path, bool isSafeName = false)
        {
            return IsFileExists(path, isSafeName);
        }

        /// <summary>
        /// 파일이 기존에 열려있는지 확인(다른 프로세서에서 사용 여부)
        /// </summary>
        /// <param name="file">FileInfo 정보</param>
        /// <returns>사용 여부</returns>
        public static bool IsFileLocked(System.IO.FileInfo file)
        {
            string strMsg = string.Empty;
            return IsFileLocked(file, out strMsg);
        }
        /// <summary>
        /// 파일이 기존에 열려있는지 확인(다른 프로세서에서 사용 여부)
        /// </summary>
        /// <param name="file">File 정보</param>
        /// <returns>사용 여부</returns>
        public static bool IsFileLocked(string fileName)
        {
            string strMsg = string.Empty;
            string strLongPath = GetLongFileName(fileName);
            return IsFileLocked(new FileInfo(strLongPath), out strMsg);
        }

        

        public static string GetLongFileName(string path, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            string Result = path;
            if (path.IsNullOrWhiteSpaceEx() == true) return Result;

            string strFilePath = GetStringReplace(path);
            if (Path.DirectorySeparatorChar == '\\' && strFilePath.IsNullOrWhiteSpaceEx() != true && HxString.GetByteCount(strFilePath, encodingType) >= HxDefs._FILE_MAX_PATH_)
            {
                Result = HxLongFile.GetWin32LongPath(strFilePath);
            }
            return Result;
        }
        public static string GetLongDirName(string path, HxEncodingType encodingType = HxEncodingType.UTF8)
        {
            string Result = path;
            if (path.IsNullOrWhiteSpaceEx() == true) return Result;

            string strDirPath = GetStringReplace(path);
            if (Path.DirectorySeparatorChar == '\\' && strDirPath.IsNullOrWhiteSpaceEx() != true && HxString.GetByteCount(strDirPath, encodingType) >= HxDefs._DIR_MAX_PATH_)
            {
                Result = HxLongDirectory.GetWin32LongPath(strDirPath);
            }
            return Result;
        }
        /// <summary>
        /// 파일이 기존에 열려있는지 확인(다른 프로세서에서 사용 여부)
        /// </summary>
        /// <param name="file">FileInfo 정보</param>
        /// <param name="message">오류 메세지</param>
        /// <returns>Lock 여부</returns>
        public static bool IsFileLocked(System.IO.FileInfo file, out string message)
        {
            message = string.Empty;
            System.IO.FileStream stream = null;
            try
            {
                if (FileExists(file.FullName))
                {
                    //stream = file.Open(System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                    string strLongPath = GetLongFileName(file.FullName);
                    stream = File.Open(strLongPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
            }
            catch (System.IO.IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                HxBase.DebugMessageOut(ex.Message);
                message = ex.Message;
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            //file is not locked
            return false;
        }

        public static string GetFileUniquePath(string fileFullName, HxFileOverwriteType overwriteType = HxFileOverwriteType.RenameSequence, string dateFormat = "yyyyMMddHHmmss")
        {
            string Result = null;

            //출처 : https://stackoverflow.com/questions/13049732/automatically-rename-a-file-if-it-already-exists-in-windows-way
            //string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            //string extension = Path.GetExtension(fullPath);
            //string path = Path.GetDirectoryName(fullPath);
            //string newFullPath = fullPath;

            //int count = 1;
            //while (File.Exists(newFullPath))
            //{
            //    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);

            //    newFullPath = Path.Combine(path, tempFileName + extension);
            //}
            //fileFullName = fileFullName;
            switch (overwriteType)
            {
                case HxFileOverwriteType.RenameDateTime:
                    Result = GetFileExistDateTimePath(fileFullName, dateFormat);
                    break;
                case HxFileOverwriteType.RenameDateMicroTime:
                    Result = GetFileExistDateTimePath(fileFullName, dateFormat + ".fff");
                    break;
                case HxFileOverwriteType.OverWrite:
                    Result = GetSafeFileName(fileFullName);
                    break;
                case HxFileOverwriteType.RenameSequence:
                case HxFileOverwriteType.None:
                default:
                    Result = GetFileExistSequencePath(fileFullName);
                    break;
            }

            return Result;
        }

        public static string GetFileExistSequencePath(string fileFullName)
        {
            //출처 : https://stackoverflow.com/questions/13049732/automatically-rename-a-file-if-it-already-exists-in-windows-way
            string Result = fileFullName;
            if (File.Exists(fileFullName))
            {
                string fileSafeName = GetSafeFileName(fileFullName);
                if(fileSafeName.IsNullOrWhiteSpaceEx() == true)
                {
                    fileSafeName = GetFileName(fileFullName);
                }
                string folder = Path.GetDirectoryName(fileFullName);
                string fileOnlyName = Path.GetFileNameWithoutExtension(fileSafeName);
                string extension = Path.GetExtension(fileSafeName);
                int number = 1;

                Match regex = Regex.Match(fileFullName, @"(.+) \((\d+)\)\.\w+");

                if (regex.Success)
                {
                    fileOnlyName = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    Result = Path.Combine(folder, string.Format("{0}_({1}){2}", fileOnlyName, number, extension));
                }
                while (File.Exists(Result));
            }
            return Result;
        }

        public static string GetFileExistDateTimePath(string fileFullName, string dateFormat = "yyyyMMddHHmmss.ffffff")
        {
            
            string Result = fileFullName;

            string fileSafeName = GetSafeFileName(fileFullName);

            string folder = Path.GetDirectoryName(fileFullName);
            string fileOnlyName = Path.GetFileNameWithoutExtension(fileSafeName);
            string extension = Path.GetExtension(fileSafeName);
            string newFullPath = Path.Combine(folder, fileOnlyName + extension);

            while (File.Exists(newFullPath))
            {
                if(dateFormat.IsNullOrWhiteSpaceEx() == true)
                {
                    dateFormat = "yyyyMMddHHmmss";
                }
                string tempFileName = string.Format("{0}_{1}", fileOnlyName, DateTime.Now.ToStringEx(dateFormat));
                newFullPath = Path.Combine(folder, tempFileName + extension);
                Result = GetFileExistDateTimePath(newFullPath, dateFormat);
            }
            return Result;

        }

        public static void FileDelete(string path, bool isThrowException = true)
        {
            if(path.IsNullOrWhiteSpaceEx() != true)
            {
                string strFileName = GetLongFileName(path);
                try
                {
                    if(FileExists(strFileName))
                        File.Delete(strFileName);
                }
                catch (DirectoryNotFoundException ex1)
                {
                    Debug.WriteLine(ex1);
                    if (isThrowException == true)
                        throw ex1;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    if(isThrowException == true)
                        throw ex;
                }
            }
        }

        public static string GetFileNameSuffixCustomAdd(string fileName, string customSuffixStr, string customStartStr, string customEndStr = "")
        {
            if (customSuffixStr.IsNullOrWhiteSpaceEx() == true) return fileName;

            string strFileNameOnly = HxFile.GetFileNameWithOutExt(fileName);
            string strFileExt = HxFile.GetFileNameExt(fileName);
            string strFileSuffix = $"{customStartStr}{customSuffixStr}{customEndStr}";
            string Result = $"{strFileNameOnly}{strFileSuffix}";
            if (Result.IsNullOrWhiteSpaceEx() == true) return fileName;

            if (strFileExt.IsNullOrWhiteSpaceEx() != true)
            {
                Result = $"{Result}.{strFileExt}";
            }
            return Result;
        }
        #endregion

        #region System.IO.Directory 관련 처리
        /// <summary>
        /// OS의 폴더 구분자
        /// </summary>
        /// <returns>폴더 구분자</returns>
        public static char GetDirSeparatorChar()
        {
            return Path.DirectorySeparatorChar;
            //return HxUtils.DirSeparatorChar;
        }
        public static char DirSeparatorChar
        {
            get { return GetDirSeparatorChar(); }
        }

        /// <summary>
        /// 폴더 목록 가져오기
        /// </summary>
        /// <param name="path">찾을 경로</param>
        /// <param name="searchPattern">찾을 패턴</param>
        /// <param name="isAllDirectories">하위 폴더 검색 여부</param>
        /// <returns>폴더 목록(List)</returns>
        public static List<string> GetDirectoryeList(string path, string searchPattern = "*", bool isAllDirectories = false)
        {
            List<string> Result = null;
            string[] arrDirectorys = GetDirectories(path, searchPattern, isAllDirectories);
            if (arrDirectorys == null || arrDirectorys.Length <= 0) return Result;

            Result = new List<string>();
            foreach (string strDir in arrDirectorys)
            {
                Result.Add(strDir);
            }
            return Result;
        }

        /// <summary>
        /// 파일 목록 가져오기
        /// </summary>
        /// <param name="path">찾을 경로</param>
        /// <param name="searchPattern">찾을 패턴</param>
        /// <param name="isAllDirectories">하위 폴더 검색 여부</param>
        /// <returns>파일 목록(List)</returns>
        public static List<string> GetFileList(string path, string searchPattern = "*.*", bool isAllDirectories = false)
        {
            List<string> Result = null;
            string[] arrFiles = GetFiles(path, searchPattern, isAllDirectories);
            if (arrFiles == null || arrFiles.Length <= 0) return Result;

            Result = new List<string>();
            foreach (string strFile in arrFiles)
            {
                Result.Add(strFile);
            }
            return Result;
        }
        /// <summary>
        /// 파일 목록 가져오기
        /// </summary>
        /// <param name="path">찾을 경로</param>
        /// <param name="isAllDirectories">하위 폴더 검색 여부</param>
        /// <param name="searchPattern">찾을 패턴</param>
        /// <returns>파일 목록(List)</returns>
        public static List<string> GetFileList(string path, bool isAllDirectories, string searchPattern = "*.*")
        {
            return GetFileList(path, searchPattern, isAllDirectories);
        }

        /// <summary>
        /// 폴더 목록 가져오기
        /// </summary>
        /// <param name="path">찾을 경로</param>
        /// <param name="searchPattern">찾을 패턴</param>
        /// <param name="isAllDirectories">하위 폴더 검색 여부</param>
        /// <returns>폴더 목록(Array)</returns>
        public static string[] GetDirectories(string path, string searchPattern = "*", bool isAllDirectories = false)
        {
            string[] Result = null;
            SearchOption searchOption = SearchOption.TopDirectoryOnly;
            if (isAllDirectories == true)
            {
                searchOption = SearchOption.AllDirectories;
            }
            if (DirectoryExists(path))
            {
                string strDirPath = GetLongDirName(path);
                Result = Directory.GetDirectories(strDirPath, searchPattern, searchOption);
            }
            return Result;
        }

        /// <summary>
        /// 파일 목록 가져오기
        /// </summary>
        /// <param name="path">찾을 경로</param>
        /// <param name="searchPattern">찾을 패턴</param>
        /// <param name="isAllDirectories">하위 폴더 검색 여부</param>
        /// <returns>파일 목록(Array)</returns>
        public static string[] GetFiles(string path, string searchPattern = "*.*", bool isAllDirectories = false)
        {
            string[] Result = null;
            SearchOption searchOption = SearchOption.TopDirectoryOnly;
            if (isAllDirectories == true)
            {
                searchOption = SearchOption.AllDirectories;
            }
            if (DirectoryExists(path))
            {
                string strDirPath = GetLongDirName(path);
                Result = Directory.GetFiles(strDirPath, searchPattern, searchOption);
            }
            //Directory.get
            return Result;

        }
        /// <summary>
        /// 폴더 정보 가져오기
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns>폴더 경로</returns>
        public static DirectoryInfo GetDirectoryInfo(string path)
        {
            DirectoryInfo Result = null;
            if (Directory.Exists(path))
            {
                Result = new DirectoryInfo(path);
            }
            //Directory.
            return Result;
        }
        /// <summary>
        /// 지정 폴더를 다른 위치로 복사
        /// </summary>
        /// <param name="sourceDirName">원본 경로</param>
        /// <param name="destDirName">사본 경로</param>
        /// <param name="copySubDirs">하위 폴더 포함 여부</param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// 폴더 Exist
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns>존재 여부?</returns>
        public static bool DirectoryExists(string path, bool isSafeName = false)
        {
            if (path.IsNullOrWhiteSpaceEx() != true)
            {
                string strDirPath = path;
                if (isSafeName == true)
                {
                    strDirPath = GetSafeDirName(strDirPath);
                }
                strDirPath = GetLongDirName(strDirPath);
                return Directory.Exists(strDirPath);
            }
            return false;
        }
        public static bool IsDirectoryExists(string path, bool isSafeName = false)
        {
            return DirectoryExists(path, isSafeName);
        }
        public static string DirectoryFullName(string path)
        {
            if (path.IsNullOrWhiteSpaceEx() != true)
            {
                string strDirPath = path;
                strDirPath = GetFileDirPath(strDirPath);
                strDirPath = GetLongDirName(strDirPath);
                if (DirectoryExists(strDirPath))
                {
                    return new DirectoryInfo(strDirPath)?.FullName;
                }
            }
            return path;
        }

        /// <summary>
        /// 폴더 생성
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns>폴더 정보</returns>
        public static DirectoryInfo DirectoryCreate(string path, bool isSafeName = false)
        {
            if (path.IsNullOrWhiteSpaceEx() != true)
            {
                string strDirPath = path;
                if (isSafeName == true)
                {
                    strDirPath = GetSafeDirName(strDirPath);
                }
                strDirPath = GetLongDirName(strDirPath);
                return Directory.CreateDirectory(strDirPath);
            }
            return null;
        }
        /// <summary>
        /// 폴더 삭제
        /// </summary>
        /// <param name="path">폴더 경로</param>
        /// <param name="recursive">하위 폴더 및 파일 삭제?</param>
        public static void DirectoryDelete(string path, bool recursive = true, bool isThrowException = true)
        {
            if(path.IsNullOrWhiteSpaceEx() != true)
            {
                try
                {
                    string strDirPath = GetLongDirName(path);
                    Directory.Delete(strDirPath, recursive);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    try
                    {
                        TryDirectoryDelete(path);
                    }
                    catch (Exception exTryDel)
                    {
                        Debug.WriteLine(exTryDel);
                        if(isThrowException) throw exTryDel;
                    }
                    //throw ex;
                }
            }
        }
        /// <summary>
        /// 폴더 삭제
        /// </summary>
        /// <param name="di">폴더 정보</param>
        /// <param name="recursive">하위 폴더 및 파일 삭제?</param>
        public static void DirectoryDelete(DirectoryInfo di, bool recursive = true)
        {
            if(di != null && di.Exists)
            {
                try
                {
                    di.Delete(recursive);
                }
                catch (Exception ex)
                {
                    DirectoryDelete(di.FullName, recursive);
                    Debug.WriteLine(ex);
                    //throw ex;
                }
            }
        }
        /// <summary>
        /// Depth-first recursive delete, with handling for descendant 
        /// directories open in Windows Explorer.
        /// </summary>
        public static void TryDirectoryDelete(string path, bool recursive = true)
        {
            //참조 : https://kesio.tistory.com/55
            //출처 : https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true/1703799#1703799


            //string[] a = Directory.GetDirectories(path,"*");
            //var b = Getdir
            //foreach (string directory in a)
            //{
                //TryDirectoryDelete(directory, recursive);
            //}

            
            if (HxString.IsNullOrWhiteSpace(path) != true && DirectoryExists(path))
            {
                string strLongPath = GetLongDirName(path);
                try
                {
                    List<string> liFiles = GetFileList(strLongPath, "*.*", recursive);
                    if (liFiles != null && liFiles.Count > 0)
                    {
                        foreach (var sFile in liFiles)
                        {
                            FileDelete(sFile, false);
                        }
                    }
                    Directory.Delete(strLongPath, recursive);
                }
                catch (DirectoryNotFoundException)
                {
                    return;
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(0);
                    Directory.Delete(strLongPath, recursive);
                    
                }
                catch (UnauthorizedAccessException)
                {
                    System.Threading.Thread.Sleep(0);
                    Directory.Delete(strLongPath, recursive);
                }
                finally
                {

                }
            }
        }

        public static List<string> GetDirFileLocked(string strDirRootPath, bool isAllDirectories = true, string searchPattern = "*.*")
        {
            List<string> Result = null;
            List<string> listFileAll = HxFile.GetFileList(strDirRootPath, isAllDirectories, searchPattern);
            int nFiles = (listFileAll?.Count).ToIntEx(0);
            if(nFiles > 0)
            {
                Result = new List<string>();
                foreach (string sFile in listFileAll)
                {
                    string strFileName = HxFile.GetLongFileName(sFile);
                    if (HxFile.FileExists(strFileName))
                    {
                        //fi.Delete();
                        if (HxFile.IsFileLocked(strFileName) == true)
                        {
                            Result.Add(strFileName);
                        }
                    }
                }
            }
            return Result;
        }

        #endregion

        #region File/Dir 사용자 정의 함수들
        /// <summary>
        /// LOCAL 파일명 가져오기 : URI형태일 경우 임시폴더 기준으로 이름 생성
        /// </summary>
        /// <param name="fileName">파일명 또는 URI</param>
        /// <param name="isNullToRandomName">NULL일경우 Random으로 생성 여부?</param>
        /// <returns>LOCAL 파일명</returns>
        public static string GetLocalFileName(string fileName, bool isNullToRandomName = false)
        {
            string Result = fileName;
            if (fileName.IsNullOrWhiteSpaceEx() != true && HxString.IsWebUri(fileName) == true)
            {
                Uri uri = new Uri(fileName);
                if(uri != null && uri.Segments != null && uri.Segments.Length > 0)
                {
                    string strFileName = uri.Segments[uri.Segments.Length - 1];
                    Result = Path.Combine(GetLocalTempDirectory(), strFileName);
                }
            }
            else if(fileName.IsNullOrWhiteSpaceEx() == true && isNullToRandomName == true)
            {
                Result = Path.Combine(GetLocalTempDirectory(), HxCrypt.RandPass());
            }
            return Result;
        }

        /// <summary>
        /// 로컬 사용자 정의 임시 폴더 경로
        /// </summary>
        /// <param name="isDirectoryCreate">폴더 생성 여부?</param>
        /// <returns>임시 폴더 경로</returns>
        public static string GetLocalTempDirectory(bool isDirectoryCreate = false)
        {
            string Result = Path.Combine(Path.GetTempPath(), HxDefs._TEMP_DIR_NAME_, DateTime.Now.ToDateStringEx());
            if (isDirectoryCreate == true)
            {
                HxFile.DirectoryCreate(Result);
            }
            return Result;
        }
        #endregion

        #region 파일 Checksum
        //출처 : https://stackoverflow.com/questions/13569406/how-should-i-compute-files-hashmd5-sha1-in-c-sharp

        /// <summary>
        /// Gets a hash of the file using SHA1.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(string filePath)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(filePath, sha1);
        }

        /// <summary>
        /// Gets a hash of the file using SHA1.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(Stream s)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(s, sha1);
        }

        /// <summary>
        /// Gets a hash of the file using MD5.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string filePath)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(filePath, md5);
        }

        

        /// <summary>
        /// Gets a hash of the file using MD5.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMD5Hash(Stream s)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(s, md5);
        }

        private static string GetHash(string filePath, HashAlgorithm hasher)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                return GetHash(fs, hasher);
        }

        private static string GetHash(Stream s, HashAlgorithm hasher)
        {
            var hash = hasher.ComputeHash(s);
            var hashStr = Convert.ToBase64String(hash);
            return hashStr.TrimEnd('=');
        }

        #endregion

        public static string MD5CheckSum(string filename)
        {//출처 : https://stackoverflow.com/questions/10520048/calculate-md5-checksum-for-a-file
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static string MD5CheckSum(FileInfo info)
        {//출처 : https://stackoverflow.com/questions/10520048/calculate-md5-checksum-for-a-file
            if (info.Exists)
            {
                using (var md5 = MD5.Create())
                {
                    string filename = info.FullName;
                    return MD5CheckSum(filename);
                }
            }
            return null;
        }

        public static string GetCheckSum<T>(string filename) where T : HashAlgorithm
        {//출처 : https://stackoverflow.com/questions/10520048/calculate-md5-checksum-for-a-file
            using (FileStream fStream = File.OpenRead(filename))
            {
                return GetHash<T>(fStream);
            }
        }
        public static string GetHash<T>(Stream stream) where T : HashAlgorithm
        {//출처 : https://stackoverflow.com/questions/10520048/calculate-md5-checksum-for-a-file
            StringBuilder sb = new StringBuilder();
            
            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))
            {
                byte[] hashBytes = crypt.ComputeHash(stream);
                foreach (byte bt in hashBytes)
                {
                    sb.Append(bt.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public static string GetCheckSum(string filename, string hashAlgorithmType = "MD5")
        {
            string Result = null;
            if(filename.IsNullOrWhiteSpaceEx() != true && File.Exists(filename) && hashAlgorithmType.IsNullOrWhiteSpaceEx() != true)
            switch (hashAlgorithmType?.ToUpper())
            {
                case "MD5":
                    Result = HxFile.GetCheckSum<MD5>(filename);
                    break;
                case "SHA1":
                    Result = HxFile.GetCheckSum<SHA1>(filename);
                    break;
                case "SHA256":
                    Result = HxFile.GetCheckSum<SHA256>(filename);
                    break;
                case "SHA384":
                    Result = HxFile.GetCheckSum<SHA384>(filename);
                    break;
                case "SHA512":
                    Result = HxFile.GetCheckSum<SHA512>(filename);
                    break;
                case "KEYEDHASHALGORITHM": //KeyedHashAlgorithm
                    Result = HxFile.GetCheckSum<KeyedHashAlgorithm>(filename);
                    break;
                default:
                    Result = null;
                    break;
            }
            return Result;
        }

        public static string GetFileMD5Checksum(string filePath)
        {
            if (HxString.IsNullOrWhiteSpace(filePath) == true || HxFile.IsFileExists(filePath) != true) { return string.Empty; }

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    //return Convert.ToHexStringLower(hash);
                    return HxString.ConvertBytesToHexStringLower(hash);
                }
            }
        }
        internal static string GetFileSHA1Checksum(string filePath)
        {
            if (HxString.IsNullOrWhiteSpace(filePath) == true || HxFile.IsFileExists(filePath) != true) { return string.Empty; }

            using (var md5 = System.Security.Cryptography.SHA1.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    //return Convert.ToHexStringLower(hash);
                    return HxString.ConvertBytesToHexStringLower(hash);
                }
            }
        }
        internal static string GetFileSHA256Checksum(string filePath)
        {
            if (HxString.IsNullOrWhiteSpace(filePath) == true || HxFile.IsFileExists(filePath) != true) { return string.Empty; }

            using (var md5 = System.Security.Cryptography.SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    //return Convert.ToHexStringLower(hash);
                    return HxString.ConvertBytesToHexStringLower(hash);
                }
            }
        }
        internal static string GetFileSHA512Checksum(string filePath)
        {
            if (HxString.IsNullOrWhiteSpace(filePath) == true || HxFile.IsFileExists(filePath) != true) { return string.Empty; }

            using (var md5 = System.Security.Cryptography.SHA512.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    //return Convert.ToHexStringLower(hash);
                    return HxString.ConvertBytesToHexStringLower(hash);
                }
            }
        }

        #region File 처리
        /// <summary>
        /// 파일 생성
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isSafeName"></param>
        public static string FileCreate(string fileName, bool isSafeName = false)
        {
            string Result = null;
            if (fileName.IsNullOrWhiteSpaceEx() != true)
            {
                string strFileName = fileName;
                if (isSafeName == true)
                {
                    strFileName = GetSafeFileName(strFileName);
                }
                strFileName = GetLongFileName(strFileName);

                string strDirPath = GetFileDirPath(strFileName);
                if (!Directory.Exists(strDirPath))
                {
                    HxFile.DirectoryCreate(strDirPath, true);
                }
                using (FileStream fs = File.Create(strFileName))
                {
                    //fs.WriteByte(0);
                }
                Result = strFileName;
            }
            return Result;
        }

        public static FileInfo FileCopyRenameToDir(FileInfo source, string targetDirPath, string fileName, HxFileOverwriteType overwriteType = HxFileOverwriteType.RenameSequence, bool bSourceRemove = false)
        {
            FileInfo Result = null;
            //File.Copy
            if (source != null && source.Exists)
            {
                try
                {
                    if (!Directory.Exists(targetDirPath))
                    {
                        HxFile.DirectoryCreate(targetDirPath, true);
                    }
                    string targetFullPath = Path.Combine(targetDirPath, fileName);
                    targetFullPath = HxFile.GetFileUniquePath(targetFullPath, overwriteType);
                    if(bSourceRemove == true)
                    {
                        source.MoveTo(targetFullPath);
                    }
                    if(source.Exists)
                    {
                        targetFullPath = HxFile.GetFileUniquePath(targetFullPath, overwriteType);
                        Result = source.CopyTo(targetFullPath);
                        if (Result != null && Result.Exists && bSourceRemove == true)
                        {
                            try
                            {
                                //FileDelete(source);
                                source.Delete();
                            }
                            catch
                            {
                                FileDelete(source.FullName, isThrowException: false);
                            }
                            finally
                            {
                                //Result.
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return Result;
        }



        public static FileInfo FileMoveToDir(FileInfo source, string targetDirPath, string fileName, HxFileOverwriteType overwriteType = HxFileOverwriteType.RenameSequence)
        {
            FileInfo Result = null;
            //File.Copy
            if (source != null && source.Exists)
            {
                try
                {
                    if (!Directory.Exists(targetDirPath))
                    {
                        HxFile.DirectoryCreate(targetDirPath, true);
                    }
                    string targetFullName = fileName.IsNullOrWhiteSpaceEx() != true ? Path.Combine(targetDirPath, fileName) : targetDirPath;
                    targetFullName = HxFile.GetLongFileName(targetFullName);
                    targetFullName = HxFile.GetFileUniquePath(targetFullName, overwriteType);
                    File.Move(source.FullName, targetFullName);
                    Result = new FileInfo(targetFullName);
                }
                catch (Exception ex)
                {
                    Result = null;
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return Result;
        }

        public static FileInfo FileMoveToDir(string sourceFileName, string targetDirPath, string fileName, HxFileOverwriteType overwriteType = HxFileOverwriteType.RenameSequence)
        {
            FileInfo Result = null;
            //File.Copy
            if (sourceFileName != null && HxFile.FileExists(sourceFileName) == true)
            {
                try
                {
                    
                    if(fileName.IsNullOrWhiteSpaceEx() == true)
                    {
                        fileName = GetFileName(sourceFileName);
                    }
                    if (!Directory.Exists(targetDirPath))
                    {
                        HxFile.DirectoryCreate(targetDirPath, true);
                    }
                    string targetFullName = Path.Combine(targetDirPath, fileName);
                    targetFullName = HxFile.GetFileUniquePath(targetFullName, overwriteType);

                    sourceFileName = HxFile.GetLongFileName(sourceFileName);
                    targetFullName = HxFile.GetLongFileName(targetFullName);
                    File.Move(sourceFileName, targetFullName);
                    Result = new FileInfo(targetFullName);
                }
                catch (Exception ex)
                {
                    Result = null;
                    Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return Result;
        }

        public static FileInfo FileMove(string sourceFileName, string targetFileName, HxFileOverwriteType overwriteType = HxFileOverwriteType.RenameSequence)
        {
            if (sourceFileName.IsNullOrWhiteSpaceEx() == true || targetFileName.IsNullOrWhiteSpaceEx() == true) return null;

            string strTargetDirPath = GetFileDirPath(targetFileName);
            string strTargetFileName = GetFileName(targetFileName);
            return FileMoveToDir(sourceFileName, strTargetDirPath, strTargetFileName, overwriteType);
        }

        #endregion
        
        public static string FileWriteFromBase64Type(string fileName, string inputBase64String, HxFileOverwriteType overwriteType = HxFileOverwriteType.RenameDateMicroTime) //, HxDataType dataType = HxDataType.None 
        {
            string Result = null;
            if(fileName.IsNullOrWhiteSpaceEx() == true || inputBase64String.IsNullOrWhiteSpaceEx() == true) { return Result; }

            string strSaveFileName = fileName;
            if (strSaveFileName.IsNullOrWhiteSpaceEx() != true)
            {
                if (overwriteType != HxFileOverwriteType.None)
                {
                    strSaveFileName = HxFile.GetFileUniquePath(strSaveFileName, overwriteType);
                }
            }

            byte[] bytes = HxString.GetBytesFromBase64Decode(inputBase64String);
            if (bytes != null && inputBase64String.IsNullOrWhiteSpaceEx() != true && bytes.Length > 0)
            {
                Result = string.Empty;
                try
                {
                    using (FileStream fls = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fls.Write(bytes, 0, bytes.Length);
                        fls.Close();
                        Result = fileName;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Result;
        }
        public static string GetFileReader(string fileName)
        {
            string Result = null;
            if (!fileName.IsNullOrWhiteSpaceEx() && File.Exists(fileName))
            {
                try
                {
                    using (StreamReader sr = new System.IO.StreamReader(fileName))
                    {
                        Result = sr.ReadToEnd();
                        sr.Close();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //throw ex;
                }
            }
            return Result;
        }


        #region MIME #출처 : https://raw.githubusercontent.com/samuelneff/MimeTypeMap/master/src/MimeTypes/MimeTypeMap.cs
        private static readonly Lazy<IDictionary<string, string>> _mappings = new Lazy<IDictionary<string, string>>(BuildMappings);

        private static IDictionary<string, string> BuildMappings()
        {
            //출처 : https://raw.githubusercontent.com/samuelneff/MimeTypeMap/master/src/MimeTypes/MimeTypeMap.cs
            IDictionary<string, string> mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                #region Big freaking list of mime types
            
                // maps both ways,
                // extension -> mime type
                //   and
                // mime type -> extension
                //
                // any mime types on left side not pre-loaded on right side, are added automatically
                // some mime types can map to multiple extensions, so to get a deterministic mapping,
                // add those to the dictionary specifcially
                //
                // combination of values from Windows 7 Registry and 
                // from C:\Windows\System32\inetsrv\config\applicationHost.config
                // some added, including .7z and .dat
                //
                // Some added based on http://www.iana.org/assignments/media-types/media-types.xhtml
                // which lists mime types, but not extensions
                //
                {".323", "text/h323"},
                {".3g2", "video/3gpp2"},
                {".3gp", "video/3gpp"},
                {".3gp2", "video/3gpp2"},
                {".3gpp", "video/3gpp"},
                {".7z", "application/x-7z-compressed"},
                {".aa", "audio/audible"},
                {".AAC", "audio/aac"},
                {".aaf", "application/octet-stream"},
                {".aax", "audio/vnd.audible.aax"},
                {".ac3", "audio/ac3"},
                {".aca", "application/octet-stream"},
                {".accda", "application/msaccess.addin"},
                {".accdb", "application/msaccess"},
                {".accdc", "application/msaccess.cab"},
                {".accde", "application/msaccess"},
                {".accdr", "application/msaccess.runtime"},
                {".accdt", "application/msaccess"},
                {".accdw", "application/msaccess.webapplication"},
                {".accft", "application/msaccess.ftemplate"},
                {".acx", "application/internet-property-stream"},
                {".AddIn", "text/xml"},
                {".ade", "application/msaccess"},
                {".adobebridge", "application/x-bridge-url"},
                {".adp", "application/msaccess"},
                {".ADT", "audio/vnd.dlna.adts"},
                {".ADTS", "audio/aac"},
                {".afm", "application/octet-stream"},
                {".ai", "application/postscript"},
                {".aif", "audio/aiff"},
                {".aifc", "audio/aiff"},
                {".aiff", "audio/aiff"},
                {".air", "application/vnd.adobe.air-application-installer-package+zip"},
                {".amc", "application/mpeg"},
                {".anx", "application/annodex"},
                {".apk", "application/vnd.android.package-archive" },
                {".application", "application/x-ms-application"},
                {".art", "image/x-jg"},
                {".asa", "application/xml"},
                {".asax", "application/xml"},
                {".ascx", "application/xml"},
                {".asd", "application/octet-stream"},
                {".asf", "video/x-ms-asf"},
                {".ashx", "application/xml"},
                {".asi", "application/octet-stream"},
                {".asm", "text/plain"},
                {".asmx", "application/xml"},
                {".aspx", "application/xml"},
                {".asr", "video/x-ms-asf"},
                {".asx", "video/x-ms-asf"},
                {".atom", "application/atom+xml"},
                {".au", "audio/basic"},
                {".avi", "video/x-msvideo"},
                {".axa", "audio/annodex"},
                {".axs", "application/olescript"},
                {".axv", "video/annodex"},
                {".bas", "text/plain"},
                {".bcpio", "application/x-bcpio"},
                {".bin", "application/octet-stream"},
                {".bmp", "image/bmp"},
                {".c", "text/plain"},
                {".cab", "application/octet-stream"},
                {".caf", "audio/x-caf"},
                {".calx", "application/vnd.ms-office.calx"},
                {".cat", "application/vnd.ms-pki.seccat"},
                {".cc", "text/plain"},
                {".cd", "text/plain"},
                {".cdda", "audio/aiff"},
                {".cdf", "application/x-cdf"},
                {".cer", "application/x-x509-ca-cert"},
                {".cfg", "text/plain"},
                {".chm", "application/octet-stream"},
                {".class", "application/x-java-applet"},
                {".clp", "application/x-msclip"},
                {".cmd", "text/plain"},
                {".cmx", "image/x-cmx"},
                {".cnf", "text/plain"},
                {".cod", "image/cis-cod"},
                {".config", "application/xml"},
                {".contact", "text/x-ms-contact"},
                {".coverage", "application/xml"},
                {".cpio", "application/x-cpio"},
                {".cpp", "text/plain"},
                {".crd", "application/x-mscardfile"},
                {".crl", "application/pkix-crl"},
                {".crt", "application/x-x509-ca-cert"},
                {".cs", "text/plain"},
                {".csdproj", "text/plain"},
                {".csh", "application/x-csh"},
                {".csproj", "text/plain"},
                {".css", "text/css"},
                {".csv", "text/csv"},
                {".cur", "application/octet-stream"},
                {".cxx", "text/plain"},
                {".dat", "application/octet-stream"},
                {".datasource", "application/xml"},
                {".dbproj", "text/plain"},
                {".dcr", "application/x-director"},
                {".def", "text/plain"},
                {".deploy", "application/octet-stream"},
                {".der", "application/x-x509-ca-cert"},
                {".dgml", "application/xml"},
                {".dib", "image/bmp"},
                {".dif", "video/x-dv"},
                {".dir", "application/x-director"},
                {".disco", "text/xml"},
                {".divx", "video/divx"},
                {".dll", "application/x-msdownload"},
                {".dll.config", "text/xml"},
                {".dlm", "text/dlm"},
                {".doc", "application/msword"},
                {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".dot", "application/msword"},
                {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
                {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
                {".dsp", "application/octet-stream"},
                {".dsw", "text/plain"},
                {".dtd", "text/xml"},
                {".dtsConfig", "text/xml"},
                {".dv", "video/x-dv"},
                {".dvi", "application/x-dvi"},
                {".dwf", "drawing/x-dwf"},
                {".dwg", "application/acad"},
                {".dwp", "application/octet-stream"},
                {".dxf", "application/x-dxf" },
                {".dxr", "application/x-director"},
                {".eml", "message/rfc822"},
                {".emz", "application/octet-stream"},
                {".eot", "application/vnd.ms-fontobject"},
                {".eps", "application/postscript"},
                {".etl", "application/etl"},
                {".etx", "text/x-setext"},
                {".evy", "application/envoy"},
                {".exe", "application/octet-stream"},
                {".exe.config", "text/xml"},
                {".fdf", "application/vnd.fdf"},
                {".fif", "application/fractals"},
                {".filters", "application/xml"},
                {".fla", "application/octet-stream"},
                {".flac", "audio/flac"},
                {".flr", "x-world/x-vrml"},
                {".flv", "video/x-flv"},
                {".fsscript", "application/fsharp-script"},
                {".fsx", "application/fsharp-script"},
                {".generictest", "application/xml"},
                {".gif", "image/gif"},
                {".gpx", "application/gpx+xml"},
                {".group", "text/x-ms-group"},
                {".gsm", "audio/x-gsm"},
                {".gtar", "application/x-gtar"},
                {".gz", "application/x-gzip"},
                {".h", "text/plain"},
                {".hdf", "application/x-hdf"},
                {".hdml", "text/x-hdml"},
                {".hhc", "application/x-oleobject"},
                {".hhk", "application/octet-stream"},
                {".hhp", "application/octet-stream"},
                {".hlp", "application/winhlp"},
                {".hpp", "text/plain"},
                {".hqx", "application/mac-binhex40"},
                {".hta", "application/hta"},
                {".htc", "text/x-component"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".htt", "text/webviewhtml"},
                {".hxa", "application/xml"},
                {".hxc", "application/xml"},
                {".hxd", "application/octet-stream"},
                {".hxe", "application/xml"},
                {".hxf", "application/xml"},
                {".hxh", "application/octet-stream"},
                {".hxi", "application/octet-stream"},
                {".hxk", "application/xml"},
                {".hxq", "application/octet-stream"},
                {".hxr", "application/octet-stream"},
                {".hxs", "application/octet-stream"},
                {".hxt", "text/html"},
                {".hxv", "application/xml"},
                {".hxw", "application/octet-stream"},
                {".hxx", "text/plain"},
                {".i", "text/plain"},
                {".ico", "image/x-icon"},
                {".ics", "application/octet-stream"},
                {".idl", "text/plain"},
                {".ief", "image/ief"},
                {".iii", "application/x-iphone"},
                {".inc", "text/plain"},
                {".inf", "application/octet-stream"},
                {".ini", "text/plain"},
                {".inl", "text/plain"},
                {".ins", "application/x-internet-signup"},
                {".ipa", "application/x-itunes-ipa"},
                {".ipg", "application/x-itunes-ipg"},
                {".ipproj", "text/plain"},
                {".ipsw", "application/x-itunes-ipsw"},
                {".iqy", "text/x-ms-iqy"},
                {".isp", "application/x-internet-signup"},
                {".ite", "application/x-itunes-ite"},
                {".itlp", "application/x-itunes-itlp"},
                {".itms", "application/x-itunes-itms"},
                {".itpc", "application/x-itunes-itpc"},
                {".IVF", "video/x-ivf"},
                {".jar", "application/java-archive"},
                {".java", "application/octet-stream"},
                {".jck", "application/liquidmotion"},
                {".jcz", "application/liquidmotion"},
                {".jfif", "image/pjpeg"},
                {".jnlp", "application/x-java-jnlp-file"},
                {".jpb", "application/octet-stream"},
                {".jpe", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".js", "application/javascript"},
                {".json", "application/json"},
                {".jsx", "text/jscript"},
                {".jsxbin", "text/plain"},
                {".latex", "application/x-latex"},
                {".library-ms", "application/windows-library+xml"},
                {".lit", "application/x-ms-reader"},
                {".loadtest", "application/xml"},
                {".lpk", "application/octet-stream"},
                {".lsf", "video/x-la-asf"},
                {".lst", "text/plain"},
                {".lsx", "video/x-la-asf"},
                {".lzh", "application/octet-stream"},
                {".m13", "application/x-msmediaview"},
                {".m14", "application/x-msmediaview"},
                {".m1v", "video/mpeg"},
                {".m2t", "video/vnd.dlna.mpeg-tts"},
                {".m2ts", "video/vnd.dlna.mpeg-tts"},
                {".m2v", "video/mpeg"},
                {".m3u", "audio/x-mpegurl"},
                {".m3u8", "audio/x-mpegurl"},
                {".m4a", "audio/m4a"},
                {".m4b", "audio/m4b"},
                {".m4p", "audio/m4p"},
                {".m4r", "audio/x-m4r"},
                {".m4v", "video/x-m4v"},
                {".mac", "image/x-macpaint"},
                {".mak", "text/plain"},
                {".man", "application/x-troff-man"},
                {".manifest", "application/x-ms-manifest"},
                {".map", "text/plain"},
                {".master", "application/xml"},
                {".mbox", "application/mbox"},
                {".mda", "application/msaccess"},
                {".mdb", "application/x-msaccess"},
                {".mde", "application/msaccess"},
                {".mdp", "application/octet-stream"},
                {".me", "application/x-troff-me"},
                {".mfp", "application/x-shockwave-flash"},
                {".mht", "message/rfc822"},
                {".mhtml", "message/rfc822"},
                {".mid", "audio/mid"},
                {".midi", "audio/mid"},
                {".mix", "application/octet-stream"},
                {".mk", "text/plain"},
                {".mk3d", "video/x-matroska-3d"},
                {".mka", "audio/x-matroska"},
                {".mkv", "video/x-matroska"},
                {".mmf", "application/x-smaf"},
                {".mno", "text/xml"},
                {".mny", "application/x-msmoney"},
                {".mod", "video/mpeg"},
                {".mov", "video/quicktime"},
                {".movie", "video/x-sgi-movie"},
                {".mp2", "video/mpeg"},
                {".mp2v", "video/mpeg"},
                {".mp3", "audio/mpeg"},
                {".mp4", "video/mp4"},
                {".mp4v", "video/mp4"},
                {".mpa", "video/mpeg"},
                {".mpe", "video/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpf", "application/vnd.ms-mediapackage"},
                {".mpg", "video/mpeg"},
                {".mpp", "application/vnd.ms-project"},
                {".mpv2", "video/mpeg"},
                {".mqv", "video/quicktime"},
                {".ms", "application/x-troff-ms"},
                {".msg", "application/vnd.ms-outlook"},
                {".msi", "application/octet-stream"},
                {".mso", "application/octet-stream"},
                {".mts", "video/vnd.dlna.mpeg-tts"},
                {".mtx", "application/xml"},
                {".mvb", "application/x-msmediaview"},
                {".mvc", "application/x-miva-compiled"},
                {".mxp", "application/x-mmxp"},
                {".nc", "application/x-netcdf"},
                {".nsc", "video/x-ms-asf"},
                {".nws", "message/rfc822"},
                {".ocx", "application/octet-stream"},
                {".oda", "application/oda"},
                {".odb", "application/vnd.oasis.opendocument.database"},
                {".odc", "application/vnd.oasis.opendocument.chart"},
                {".odf", "application/vnd.oasis.opendocument.formula"},
                {".odg", "application/vnd.oasis.opendocument.graphics"},
                {".odh", "text/plain"},
                {".odi", "application/vnd.oasis.opendocument.image"},
                {".odl", "text/plain"},
                {".odm", "application/vnd.oasis.opendocument.text-master"},
                {".odp", "application/vnd.oasis.opendocument.presentation"},
                {".ods", "application/vnd.oasis.opendocument.spreadsheet"},
                {".odt", "application/vnd.oasis.opendocument.text"},
                {".oga", "audio/ogg"},
                {".ogg", "audio/ogg"},
                {".ogv", "video/ogg"},
                {".ogx", "application/ogg"},
                {".one", "application/onenote"},
                {".onea", "application/onenote"},
                {".onepkg", "application/onenote"},
                {".onetmp", "application/onenote"},
                {".onetoc", "application/onenote"},
                {".onetoc2", "application/onenote"},
                {".opus", "audio/ogg"},
                {".orderedtest", "application/xml"},
                {".osdx", "application/opensearchdescription+xml"},
                {".otf", "application/font-sfnt"},
                {".otg", "application/vnd.oasis.opendocument.graphics-template"},
                {".oth", "application/vnd.oasis.opendocument.text-web"},
                {".otp", "application/vnd.oasis.opendocument.presentation-template"},
                {".ots", "application/vnd.oasis.opendocument.spreadsheet-template"},
                {".ott", "application/vnd.oasis.opendocument.text-template"},
                {".oxt", "application/vnd.openofficeorg.extension"},
                {".p10", "application/pkcs10"},
                {".p12", "application/x-pkcs12"},
                {".p7b", "application/x-pkcs7-certificates"},
                {".p7c", "application/pkcs7-mime"},
                {".p7m", "application/pkcs7-mime"},
                {".p7r", "application/x-pkcs7-certreqresp"},
                {".p7s", "application/pkcs7-signature"},
                {".pbm", "image/x-portable-bitmap"},
                {".pcast", "application/x-podcast"},
                {".pct", "image/pict"},
                {".pcx", "application/octet-stream"},
                {".pcz", "application/octet-stream"},
                {".pdf", "application/pdf"},
                {".pfb", "application/octet-stream"},
                {".pfm", "application/octet-stream"},
                {".pfx", "application/x-pkcs12"},
                {".pgm", "image/x-portable-graymap"},
                {".pic", "image/pict"},
                {".pict", "image/pict"},
                {".pkgdef", "text/plain"},
                {".pkgundef", "text/plain"},
                {".pko", "application/vnd.ms-pki.pko"},
                {".pls", "audio/scpls"},
                {".pma", "application/x-perfmon"},
                {".pmc", "application/x-perfmon"},
                {".pml", "application/x-perfmon"},
                {".pmr", "application/x-perfmon"},
                {".pmw", "application/x-perfmon"},
                {".png", "image/png"},
                {".pnm", "image/x-portable-anymap"},
                {".pnt", "image/x-macpaint"},
                {".pntg", "image/x-macpaint"},
                {".pnz", "image/png"},
                {".pot", "application/vnd.ms-powerpoint"},
                {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
                {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
                {".ppa", "application/vnd.ms-powerpoint"},
                {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
                {".ppm", "image/x-portable-pixmap"},
                {".pps", "application/vnd.ms-powerpoint"},
                {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
                {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {".prf", "application/pics-rules"},
                {".prm", "application/octet-stream"},
                {".prx", "application/octet-stream"},
                {".ps", "application/postscript"},
                {".psc1", "application/PowerShell"},
                {".psd", "application/octet-stream"},
                {".psess", "application/xml"},
                {".psm", "application/octet-stream"},
                {".psp", "application/octet-stream"},
                {".pst", "application/vnd.ms-outlook"},
                {".pub", "application/x-mspublisher"},
                {".pwz", "application/vnd.ms-powerpoint"},
                {".qht", "text/x-html-insertion"},
                {".qhtm", "text/x-html-insertion"},
                {".qt", "video/quicktime"},
                {".qti", "image/x-quicktime"},
                {".qtif", "image/x-quicktime"},
                {".qtl", "application/x-quicktimeplayer"},
                {".qxd", "application/octet-stream"},
                {".ra", "audio/x-pn-realaudio"},
                {".ram", "audio/x-pn-realaudio"},
                {".rar", "application/x-rar-compressed"},
                {".ras", "image/x-cmu-raster"},
                {".rat", "application/rat-file"},
                {".rc", "text/plain"},
                {".rc2", "text/plain"},
                {".rct", "text/plain"},
                {".rdlc", "application/xml"},
                {".reg", "text/plain"},
                {".resx", "application/xml"},
                {".rf", "image/vnd.rn-realflash"},
                {".rgb", "image/x-rgb"},
                {".rgs", "text/plain"},
                {".rm", "application/vnd.rn-realmedia"},
                {".rmi", "audio/mid"},
                {".rmp", "application/vnd.rn-rn_music_package"},
                {".roff", "application/x-troff"},
                {".rpm", "audio/x-pn-realaudio-plugin"},
                {".rqy", "text/x-ms-rqy"},
                {".rtf", "application/rtf"},
                {".rtx", "text/richtext"},
                {".rvt", "application/octet-stream" },
                {".ruleset", "application/xml"},
                {".s", "text/plain"},
                {".safariextz", "application/x-safari-safariextz"},
                {".scd", "application/x-msschedule"},
                {".scr", "text/plain"},
                {".sct", "text/scriptlet"},
                {".sd2", "audio/x-sd2"},
                {".sdp", "application/sdp"},
                {".sea", "application/octet-stream"},
                {".searchConnector-ms", "application/windows-search-connector+xml"},
                {".setpay", "application/set-payment-initiation"},
                {".setreg", "application/set-registration-initiation"},
                {".settings", "application/xml"},
                {".sgimb", "application/x-sgimb"},
                {".sgml", "text/sgml"},
                {".sh", "application/x-sh"},
                {".shar", "application/x-shar"},
                {".shtml", "text/html"},
                {".sit", "application/x-stuffit"},
                {".sitemap", "application/xml"},
                {".skin", "application/xml"},
                {".skp", "application/x-koan" },
                {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
                {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
                {".slk", "application/vnd.ms-excel"},
                {".sln", "text/plain"},
                {".slupkg-ms", "application/x-ms-license"},
                {".smd", "audio/x-smd"},
                {".smi", "application/octet-stream"},
                {".smx", "audio/x-smd"},
                {".smz", "audio/x-smd"},
                {".snd", "audio/basic"},
                {".snippet", "application/xml"},
                {".snp", "application/octet-stream"},
                {".sol", "text/plain"},
                {".sor", "text/plain"},
                {".spc", "application/x-pkcs7-certificates"},
                {".spl", "application/futuresplash"},
                {".spx", "audio/ogg"},
                {".src", "application/x-wais-source"},
                {".srf", "text/plain"},
                {".SSISDeploymentManifest", "text/xml"},
                {".ssm", "application/streamingmedia"},
                {".sst", "application/vnd.ms-pki.certstore"},
                {".stl", "application/vnd.ms-pki.stl"},
                {".sv4cpio", "application/x-sv4cpio"},
                {".sv4crc", "application/x-sv4crc"},
                {".svc", "application/xml"},
                {".svg", "image/svg+xml"},
                {".swf", "application/x-shockwave-flash"},
                {".step", "application/step"},
                {".stp", "application/step"},
                {".t", "application/x-troff"},
                {".tar", "application/x-tar"},
                {".tcl", "application/x-tcl"},
                {".testrunconfig", "application/xml"},
                {".testsettings", "application/xml"},
                {".tex", "application/x-tex"},
                {".texi", "application/x-texinfo"},
                {".texinfo", "application/x-texinfo"},
                {".tgz", "application/x-compressed"},
                {".thmx", "application/vnd.ms-officetheme"},
                {".thn", "application/octet-stream"},
                {".tif", "image/tiff"},
                {".tiff", "image/tiff"},
                {".tlh", "text/plain"},
                {".tli", "text/plain"},
                {".toc", "application/octet-stream"},
                {".tr", "application/x-troff"},
                {".trm", "application/x-msterminal"},
                {".trx", "application/xml"},
                {".ts", "video/vnd.dlna.mpeg-tts"},
                {".tsv", "text/tab-separated-values"},
                {".ttf", "application/font-sfnt"},
                {".tts", "video/vnd.dlna.mpeg-tts"},
                {".txt", "text/plain"},
                {".u32", "application/octet-stream"},
                {".uls", "text/iuls"},
                {".user", "text/plain"},
                {".ustar", "application/x-ustar"},
                {".vb", "text/plain"},
                {".vbdproj", "text/plain"},
                {".vbk", "video/mpeg"},
                {".vbproj", "text/plain"},
                {".vbs", "text/vbscript"},
                {".vcf", "text/x-vcard"},
                {".vcproj", "application/xml"},
                {".vcs", "text/plain"},
                {".vcxproj", "application/xml"},
                {".vddproj", "text/plain"},
                {".vdp", "text/plain"},
                {".vdproj", "text/plain"},
                {".vdx", "application/vnd.ms-visio.viewer"},
                {".vml", "text/xml"},
                {".vscontent", "application/xml"},
                {".vsct", "text/xml"},
                {".vsd", "application/vnd.visio"},
                {".vsi", "application/ms-vsi"},
                {".vsix", "application/vsix"},
                {".vsixlangpack", "text/xml"},
                {".vsixmanifest", "text/xml"},
                {".vsmdi", "application/xml"},
                {".vspscc", "text/plain"},
                {".vss", "application/vnd.visio"},
                {".vsscc", "text/plain"},
                {".vssettings", "text/xml"},
                {".vssscc", "text/plain"},
                {".vst", "application/vnd.visio"},
                {".vstemplate", "text/xml"},
                {".vsto", "application/x-ms-vsto"},
                {".vsw", "application/vnd.visio"},
                {".vsx", "application/vnd.visio"},
                {".vtt", "text/vtt"},
                {".vtx", "application/vnd.visio"},
                {".wasm", "application/wasm"},
                {".wav", "audio/wav"},
                {".wave", "audio/wav"},
                {".wax", "audio/x-ms-wax"},
                {".wbk", "application/msword"},
                {".wbmp", "image/vnd.wap.wbmp"},
                {".wcm", "application/vnd.ms-works"},
                {".wdb", "application/vnd.ms-works"},
                {".wdp", "image/vnd.ms-photo"},
                {".webarchive", "application/x-safari-webarchive"},
                {".webm", "video/webm"},
                {".webp", "image/webp"}, /* https://en.wikipedia.org/wiki/WebP */
                {".webtest", "application/xml"},
                {".wiq", "application/xml"},
                {".wiz", "application/msword"},
                {".wks", "application/vnd.ms-works"},
                {".WLMP", "application/wlmoviemaker"},
                {".wlpginstall", "application/x-wlpg-detect"},
                {".wlpginstall3", "application/x-wlpg3-detect"},
                {".wm", "video/x-ms-wm"},
                {".wma", "audio/x-ms-wma"},
                {".wmd", "application/x-ms-wmd"},
                {".wmf", "application/x-msmetafile"},
                {".wml", "text/vnd.wap.wml"},
                {".wmlc", "application/vnd.wap.wmlc"},
                {".wmls", "text/vnd.wap.wmlscript"},
                {".wmlsc", "application/vnd.wap.wmlscriptc"},
                {".wmp", "video/x-ms-wmp"},
                {".wmv", "video/x-ms-wmv"},
                {".wmx", "video/x-ms-wmx"},
                {".wmz", "application/x-ms-wmz"},
                {".woff", "application/font-woff"},
                {".woff2", "application/font-woff2"},
                {".wpl", "application/vnd.ms-wpl"},
                {".wps", "application/vnd.ms-works"},
                {".wri", "application/x-mswrite"},
                {".wrl", "x-world/x-vrml"},
                {".wrz", "x-world/x-vrml"},
                {".wsc", "text/scriptlet"},
                {".wsdl", "text/xml"},
                {".wvx", "video/x-ms-wvx"},
                {".x", "application/directx"},
                {".xaf", "x-world/x-vrml"},
                {".xaml", "application/xaml+xml"},
                {".xap", "application/x-silverlight-app"},
                {".xbap", "application/x-ms-xbap"},
                {".xbm", "image/x-xbitmap"},
                {".xdr", "text/plain"},
                {".xht", "application/xhtml+xml"},
                {".xhtml", "application/xhtml+xml"},
                {".xla", "application/vnd.ms-excel"},
                {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
                {".xlc", "application/vnd.ms-excel"},
                {".xld", "application/vnd.ms-excel"},
                {".xlk", "application/vnd.ms-excel"},
                {".xll", "application/vnd.ms-excel"},
                {".xlm", "application/vnd.ms-excel"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
                {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".xlt", "application/vnd.ms-excel"},
                {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
                {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
                {".xlw", "application/vnd.ms-excel"},
                {".xml", "text/xml"},
                {".xmp", "application/octet-stream" },
                {".xmta", "application/xml"},
                {".xof", "x-world/x-vrml"},
                {".XOML", "text/plain"},
                {".xpm", "image/x-xpixmap"},
                {".xps", "application/vnd.ms-xpsdocument"},
                {".xrm-ms", "text/xml"},
                {".xsc", "application/xml"},
                {".xsd", "text/xml"},
                {".xsf", "text/xml"},
                {".xsl", "text/xml"},
                {".xslt", "text/xml"},
                {".xsn", "application/octet-stream"},
                {".xss", "application/xml"},
                {".xspf", "application/xspf+xml"},
                {".xtp", "application/octet-stream"},
                {".xwd", "image/x-xwindowdump"},
                {".z", "application/x-compress"},
                {".zip", "application/zip"},

                {"application/fsharp-script", ".fsx"},
                {"application/msaccess", ".adp"},
                {"application/msword", ".doc"},
                {"application/octet-stream", ".bin"},
                {"application/onenote", ".one"},
                {"application/postscript", ".eps"},
                {"application/step", ".step"},
                {"application/vnd.ms-excel", ".xls"},
                {"application/vnd.ms-powerpoint", ".ppt"},
                {"application/vnd.ms-works", ".wks"},
                {"application/vnd.visio", ".vsd"},
                {"application/x-director", ".dir"},
                {"application/x-shockwave-flash", ".swf"},
                {"application/x-x509-ca-cert", ".cer"},
                {"application/x-zip-compressed", ".zip"},
                {"application/xhtml+xml", ".xhtml"},
                {"application/xml", ".xml"},  // anomoly, .xml -> text/xml, but application/xml -> many thingss, but all are xml, so safest is .xml
                {"audio/aac", ".AAC"},
                {"audio/aiff", ".aiff"},
                {"audio/basic", ".snd"},
                {"audio/mid", ".midi"},
                {"audio/wav", ".wav"},
                {"audio/x-m4a", ".m4a"},
                {"audio/x-mpegurl", ".m3u"},
                {"audio/x-pn-realaudio", ".ra"},
                {"audio/x-smd", ".smd"},
                {"image/bmp", ".bmp"},
                {"image/jpeg", ".jpg"},
                {"image/pict", ".pic"},
                {"image/png", ".png"}, //Defined in [RFC-2045], [RFC-2048]
                {"image/x-png", ".png"}, //See https://www.w3.org/TR/PNG/#A-Media-type :"It is recommended that implementations also recognize the media type "image/x-png"."
                {"image/tiff", ".tiff"},
                {"image/x-macpaint", ".mac"},
                {"image/x-quicktime", ".qti"},
                {"message/rfc822", ".eml"},
                {"text/html", ".html"},
                {"text/plain", ".txt"},
                {"text/scriptlet", ".wsc"},
                {"text/xml", ".xml"},
                {"video/3gpp", ".3gp"},
                {"video/3gpp2", ".3gp2"},
                {"video/mp4", ".mp4"},
                {"video/mpeg", ".mpg"},
                {"video/quicktime", ".mov"},
                {"video/vnd.dlna.mpeg-tts", ".m2t"},
                {"video/x-dv", ".dv"},
                {"video/x-la-asf", ".lsf"},
                {"video/x-ms-asf", ".asf"},
                {"x-world/x-vrml", ".xof"},

                #endregion
            };
            var cache = mappings.ToList(); // need ToList() to avoid modifying while still enumerating

            foreach (var mapping in cache)
            {
                if (!mappings.ContainsKey(mapping.Value))
                {
                    mappings.Add(mapping.Value, mapping.Key);
                }
            }
            return mappings;
        }

        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            return _mappings.Value.TryGetValue(extension, out string mime) ? mime : "application/octet-stream";
        }

        public static string GetExtension(string mimeType)
        {
            return GetExtension(mimeType, true);
        }

        public static string GetExtension(string mimeType, bool throwErrorIfNotFound)
        {
            if (mimeType == null)
            {
                throw new ArgumentNullException("mimeType");
            }

            if (mimeType.StartsWith("."))
            {
                throw new ArgumentException("Requested mime type is not valid: " + mimeType);
            }


            if (_mappings.Value.TryGetValue(mimeType, out string extension))
            {
                return extension;
            }
            if (throwErrorIfNotFound)
            {
                throw new ArgumentException("Requested mime type is not registered: " + mimeType);
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region MIME / System.Web.MimeMapping
        public static string GetMime(string file)
        {
            string Result = null;
            Result = MimeMapping.MimeUtility.GetMimeMapping(file);
            if (Result.IsNullOrWhiteSpaceEx())
            {
                Result = MimeMapping.MimeUtility.UnknownMimeType;
            }
            return Result;
        }
        public static IDictionary<string, string> MimeType { get => MimeMapping.MimeUtility.TypeMap; }
        #endregion

        #region TEXT 파일
        //https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/file-system/how-to-write-to-a-text-file
        public static void SetTextFileWriteAllLines(string fileName, string[] lines)
        {
            try
            {
                System.IO.File.WriteAllLines(fileName, lines);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static void SetTextFileWriteAllText(string fileName, string text)
        {
            try
            {
                System.IO.File.WriteAllText(fileName, text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static void SetTextFileWriteAllText(string fileName, string text, Encoding encoding)
        {
            try
            {
                System.IO.File.WriteAllText(fileName, text, encoding);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string SetAppendAllText(string fileName, string text, bool isSafeName = false)
        {
            string Result = null;
            if (fileName.IsNullOrWhiteSpaceEx() != true)
            {
                string strFileName = fileName;
                if (isSafeName == true)
                {
                    strFileName = GetSafeFileName(strFileName);
                }
                strFileName = GetLongFileName(strFileName);

                File.AppendAllText(strFileName, text);

                Result = strFileName;
            }
            return Result;
        }
        public static string SetAppendAllText(string fileName, string text, Encoding encoding, bool isSafeName = false)
        {
            string Result = null;
            if (fileName.IsNullOrWhiteSpaceEx() != true)
            {
                string strFileName = fileName;
                if (isSafeName == true)
                {
                    strFileName = GetSafeFileName(strFileName);
                }
                strFileName = GetLongFileName(strFileName);

                File.AppendAllText(strFileName, text, encoding);

                Result = strFileName;
            }
            return Result;
        }
        public static void SetTextFileStreamWriter(string fileName, string[] lines, bool bAppend = false)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, bAppend))
                {
                    foreach (string line in lines)
                    {
                        file.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        //https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/file-system/how-to-read-from-a-text-file
        public static string GetTextFileReadAllText(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    return System.IO.File.ReadAllText(fileName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public static string GetTextFileReadAllText(string fileName, Encoding encoding)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    return System.IO.File.ReadAllText(fileName, encoding);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        

        public static string[] GetTextFileReadAllLines(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    return System.IO.File.ReadAllLines(fileName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        public static List<string> GetTextFileStreamReader(string fileName)
        {
            List<string> Result = null;
            try
            {
                if (File.Exists(fileName))
                {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                    {
                        string line;
                        //int counter = 0;
                        Result = new List<string>();
                        while ((line = file.ReadLine()) != null)
                        {
                            Result.Add(line);
                            //counter++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public static string FileReadAllTextToString(string fileName)
        {
            return GetTextFileReadAllText(fileName);
        }
        public static string FileReadAllTextToString(string fileName, Encoding encoding)
        {
            return GetTextFileReadAllText(fileName, encoding);
        }

        public static string[] FileReadAllLinesToArray(string fileName)
        {
            return GetTextFileReadAllLines(fileName);
        }
        public static List<string> ReadStreamReaderLinesToList (string fileName)
        {
            return GetTextFileStreamReader(fileName);
        }
        #endregion

        public static void WriteAllBytes(string fileName, byte[] byteArray)
        {
            File.WriteAllBytes(fileName, byteArray);
        }
        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            //출처 : https://stackoverflow.com/questions/6397235/write-bytes-to-file
            try
            {
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
        public static byte[] StringToByteArray(string hex)
        {
            return HxString.StringToByteArray(hex);
        }
        public static FileStream ReadFileStream(string fileName)
        {
            FileStream Result = null;

            Result = File.OpenRead(fileName);

            return Result;
        }
        public static Stream ReadStream(string fileName)
        {
            Stream Result = null;

            Result = (File.OpenRead(fileName) as Stream);

            return Result;
        }
        public static byte[] ReadAllBytes(string fileName)
        {
            byte[] Result = null;

            Result = File.ReadAllBytes(fileName);

            return Result;
        }
        
        public static string ReadAllToBase64Encode(string fileName, Base64FormattingOptions options = Base64FormattingOptions.InsertLineBreaks)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    byte[] bytes = File.ReadAllBytes(fileName);
                    return HxString.GetByteToBase64Encode(bytes, options);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            
            return null;
        }
        public static string ReadAllToBase64Encode(FileInfo file)
        {
            if(file != null && file.Exists)
            {
                return ReadAllToBase64Encode(file.FullName);
            }
            return null;
        }

        public static string GetFileToBase64Encode(string fileName) //, Base64FormattingOptions options = Base64FormattingOptions.None
        {
            string Result = null;
            if(fileName.IsNullOrWhiteSpaceEx() != true && File.Exists(fileName))
            {
                using (FileStream inputFile = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    Result = GetStreamToBase64Encode(inputFile);
                }
            }
            return Result;
        }
        public static string GetStreamToBase64Encode(Stream stream, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes, options);
        }

        public static IEnumerable<string> ConvertToBase64StringInBlocks(string filename, int blockSize)
        {
            byte[] buffer = new byte[blockSize];

            using (var file = File.OpenRead(filename))
            {

                while (true)
                {
                    int n = file.Read(buffer, 0, buffer.Length);

                    if (n == 0) // Exactly read to end of file in previous read, so we're already done.
                    {
                        break;
                    }
                    else
                    {
                        yield return Convert.ToBase64String(buffer, 0, n);
                    }
                }
            }
        }


        public static System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        public static void SaveFileStream(string path, Stream stream)
        {
            if (path.IsNullOrWhiteSpaceEx() == true || stream == null) return;

            try
            {
                string strFileName = GetLongFileName(path);
                using (var fileStream = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                    fileStream?.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        #region 어셈블리 특성 접근자

        internal static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        internal static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        internal static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        internal static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        internal static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        internal static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        internal static string AssemblyFileVersion
        {
            get
            {
                System.Reflection.Assembly assembly = Assembly.GetExecutingAssembly();
                if (assembly != null)
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    return fvi.FileVersion;
                }
                return null;
            }
        }
        #endregion

        
    }

    public struct HxCustomDataFileRec
    {
        public int? PostRowNo { get; private set; }
        public string PostKeyName { get; private set; }
        public FileInfo SaveFileInfo { get; private set; }
        public string FileName { get; private set; }
        public string FileType { get; private set; }
        public string FileSaveName { get; private set; }
        public string FileDirPath { get; private set; }
        public string FileFullName { get; private set; }
        public long? FileSize { get; private set; }
        public bool? IsFileMove { get; private set; }
        public HxCustomDataFileRec(bool bInit = false)
        {
            PostRowNo = int.MinValue;
            PostKeyName = null;
            SaveFileInfo = null;
            FileName = null;
            FileType = null;
            FileSaveName = null;
            FileDirPath = null;
            FileFullName = null;
            FileSize = null;
            IsFileMove = null;
            if(bInit == true)
            {
                FileSize = 0;
                IsFileMove = false;
            }
        }
        public HxCustomDataFileRec(DataTable postData, string name, string rowNumberColumnName, string targetPath, string rootPath = null, string prefix = null, HxFileOverwriteType overwriteType = HxFileOverwriteType.RenameSequence, bool bSourceRemove = true)
            : this()
        {
            if (postData != null && postData.Columns.Contains(rowNumberColumnName) && postData.Rows.Count > 0)
            {
                PostRowNo = postData.GetSingleLastValueEx<int>(name, rowNumberColumnName);
                DataRow row = postData.Select(string.Format("{0} = {1}", rowNumberColumnName, PostRowNo.ToIntEx())).LastOrDefault();
                if (PostRowNo != null && PostRowNo > int.MinValue && row != null)
                {
                    PostKeyName = row["name"].ToStringEx().Replace("[]", string.Empty); ;
                    FileSize = long.MinValue;
                    DataTable dt = row.Table;
                    if (dt.Columns.Count > 0 && dt.Columns.Contains("value"))
                    {
                        string fileTemp = row["value"].ToStringEx();

                        FileName = row["file_name"].ToString();
                        FileType = row["file_type"].ToStringEx();
                        //file_size = row["file_size"].ToConvertEx<long>();
                        FileSaveName = prefix.IsNullOrWhiteSpaceEx() ? FileName : string.Format("{0}{1}", prefix, FileName);
                        FileDirPath = rootPath;
                        //long fileSize = row["file_size"].ToConvertEx<long>();


                        //string saveDirPath = Path.Combine(this.StoragePath, comp_reg_num);
                        SaveFileInfo = HxFile.FileCopyRenameToDir(new FileInfo(fileTemp), targetPath, FileSaveName, overwriteType, bSourceRemove);
                        if (SaveFileInfo != null && SaveFileInfo.Exists)
                        {
                            targetPath = SaveFileInfo.DirectoryName;
                            if (!rootPath.IsNullOrWhiteSpaceEx())
                            {
                                targetPath = targetPath.RegexReplaceEx("^(" + rootPath.Replace(@"\", @"\\") + ")", string.Empty);
                            }

                            //fileName = fileName;
                            //file_type = file_type;
                            FileSize = SaveFileInfo.Length;
                            FileSaveName = SaveFileInfo.Name;
                            FileDirPath = targetPath;
                            FileFullName = SaveFileInfo.FullName;
                            IsFileMove = true;
                        }
                    }
                }
            }
        }

    }
}
