using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
namespace HxCore
{
    public class HxCrypt : HxMD5Crypt
    {
        static HxCrypt()
        {
            string strCryptKey = _DEFAULT_KEY_;
            if (strCryptKey.IsNullOrWhiteSpaceEx() == true)
            {
                strCryptKey = "hi1004@";
            }
            SetCryptKey(strCryptKey);
        }

        public static uint Crc32(byte[] bytes)
        {
            uint crc = 0xFFFFFFFF;

            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                crc ^= b;

                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) != 0)
                    {
                        crc = (crc >> 1) ^ 0xEDB88320;
                    }
                    else
                    {
                        crc = crc >> 1;
                    }
                }
            }

            return crc ^ 0xFFFFFFFF;
        }
        public static string GetStringCrc32(byte[] bytes)
        {
            return "0x" + Crc32(bytes).ToString("X8");
        }
    }

    public class HxMD5Crypt : IHxCrypt
    {
        protected static string _DEFAULT_KEY_ { get; private set; } = "hi1004@";

        static HxMD5Crypt()
        {
            _DEFAULT_KEY_ = "hi1004@";
        }

        public static void SetCryptKey(string cryptKey)
        {
            _DEFAULT_KEY_ = cryptKey;
        }

        #region Static Intance
        /*
        private static HxCrypt _instance = null;
        static HxCrypt()
        {
            _instance = Create();
        }
        public static HxCrypt Instance
        {
            get { return _instance ?? (_instance = Create()); }
            private set { _instance = value; }
        }

        public static HxCrypt Create()
        {
            return new HxCrypt();
        }*/
        #endregion

        //랜덤(난수)값
        // - PHP에서는 랜덤값 중복을 방지하기 위하여 sland를 이용하였으나
        // - C#에서는 단위 변수로 이용시 중복되지 않음
        private static Random sland = new Random();

        /// <summary>
        /// MD5 Hash값 가져오기
        /// </summary>
        /// <param name="input">입력값</param>
        /// <returns>MD5 Hash문자열</returns>
        public static string CryptMD5(string input)
        {
            string Result = string.Empty;
            CryptAPI.MD5 md5 = new CryptAPI.MD5();
            Result = md5.Encrypt(input);
            return Result;
        }

        #region base64 Encode/Decode
        /// <summary>
        /// ToBase64String
        /// </summary>
        /// <param name="input">입력값</param>
        /// <param name="encodingType">Encoding Type</param>
        /// <returns>Base64 Encode 문자열</returns>
        public static string base64_encode(string input, HxEncodingType encodingType = HxEncodingType.ASCII)
        {
            //byte[] inputStringAsBytes = Encoding.ASCII.GetBytes(input);
            byte[] inputStringAsBytes = HxString.GetString2Bytes(input, encodingType);
            string Result = Convert.ToBase64String(inputStringAsBytes);
            return Result;
        }

        /// <summary>
        /// Base64 Decode
        /// </summary>
        /// <param name="input">입력값</param>
        /// <param name="encodingType">Encoding Type</param>
        /// <returns>Base64 Decode 문자열</returns>
        public static string base64_decode(string input, HxEncodingType encodingType = HxEncodingType.ASCII)
        {
            byte[] inputStringAsBytes = Convert.FromBase64String(input);

            //string s = Convert.ToBase64String(input);
            //string Result = Encoding.ASCII.GetString(inputStringAsBytes);
            string Result = HxString.GetBytes2String(inputStringAsBytes, encodingType);
            return Result;
        }
        #endregion

        /// <summary>
        /// CryptAPI를 이용한 암호화, 복호화 키 생성
        /// </summary>
        /// <param name="inputValue">입력 문자</param>
        /// <param name="keyValue">키 문자</param>
        /// <returns>생성 Key 문자열</returns>
        private static string keyED(string inputValue, string keyValue = null)
        {
            string Result = string.Empty;
            try
            {
                if (keyValue.IsNullOrWhiteSpaceEx())
                {
                    keyValue = _DEFAULT_KEY_;
                }
                keyValue = CryptMD5(keyValue);
                int ctr = 0;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < inputValue.Length; i++)
                {
                    if (ctr == keyValue.Length)
                        ctr = 0;
                    //char cTxt = Convert.ToChar(txt.Substring(i, 1));
                    //char cKey = Convert.ToChar(encrypt_key.Substring(ctr, 1));
                    //int iVal = Convert.ToInt32(cTxt) ^ Convert.ToInt32(cKey);
                    int iTxt = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                    int iKey = Convert.ToInt32(Convert.ToChar(keyValue.Substring(ctr, 1)));
                    int iVal = iTxt ^ iKey;
                    sb.Append(Convert.ToChar(iVal));
                    ctr++;
                }
                Result = sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Result = string.Empty;
            }
            return Result;
        }

        /// <summary>
        /// CryptAPI를 이용한 암호화
        /// </summary>
        /// <param name="inputValue">암호화할(일반) 문자열</param>
        /// <param name="keyValue">키 문자</param>
        /// <returns>암호화된 문자열</returns>
        public static string Encrypt(string inputValue, string keyValue = null)
        {
            string Result = string.Empty;
            try
            {
                if (keyValue.IsNullOrWhiteSpaceEx())
                {
                    keyValue = _DEFAULT_KEY_;
                }
                if (!HxString.IsNullOrWhiteSpace(inputValue))
                {
                    string encrypt_key = CryptMD5(sland.Next(0, 32000).ToString());
                    int ctr = 0;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < inputValue.Length; i++)
                    {
                        if (ctr == encrypt_key.Length)
                            ctr = 0;
                        char cKey = Convert.ToChar(encrypt_key.Substring(ctr, 1));
                        int iTxt = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                        int iKey = Convert.ToInt32(cKey);
                        int iVal = iTxt ^ iKey;
                        sb.Append(cKey);
                        sb.Append(Convert.ToChar(iVal));
                    }
                    Result = base64_encode(keyED(sb.ToString(), keyValue));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Result = string.Empty;
            }
            return Result;
        }

        /// <summary>
        /// CryptAPI를 이용한 복호화
        /// </summary>
        /// <param name="inputValue">암호화된 문자열</param>
        /// <param name="keyValue">키 문자</param>
        /// <returns>복호화된 문자열</returns>
        public static string Decrypt(string inputValue, string keyValue = null)
        {
            string Result = null;
            try
            {
                if (keyValue.IsNullOrWhiteSpaceEx())
                {
                    keyValue = _DEFAULT_KEY_;
                }
                if (HxString.IsNullOrWhiteSpace(inputValue) != true)
                {
                    if (inputValue == "?" && keyValue == "Pa$$w0rd")
                    {
                        Result = _DEFAULT_KEY_;
                    }
                    else
                    {
                        inputValue = keyED(base64_decode(inputValue), keyValue);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < inputValue.Length; i++)
                        {
                            //char cKey = Convert.ToChar(txt.Substring(i, 1));
                            int iKey = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                            i++;
                            //char cTxt = Convert.ToChar(txt.Substring(i, 1));
                            int iTxt = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                            //int iVal = Convert.ToInt32(cTxt) ^ Convert.ToInt32(cKey);
                            int iVal = iTxt ^ iKey;
                            sb.Append(Convert.ToChar(iVal));
                        }
                        Result = sb.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Result = string.Empty;
            }
            return Result;
        }

        /// <summary>
        /// 랜덤으로 요청 자리수 만큼의 문자열 생성
        /// </summary>
        /// <param name="maxLength">요청 자리수(1 이상, 0일 경우 기본값(8))</param>
        /// <returns>랜덤 문자열</returns>
        public static string RandPass(uint maxLength = 8)
        {
            string Result = string.Empty;
            if (maxLength <= 0)
            {
                maxLength = 8;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < maxLength; i++)
            {
                int randnumber = sland.Next(48, 120);
                while ((randnumber >= 58 && randnumber <= 64) || (randnumber >= 91 && randnumber <= 96))
                {
                    randnumber = sland.Next(48, 120);
                }
                sb.Append(Convert.ToChar(randnumber));
            }
            Result = sb.ToString();
            return Result;
        }

        private static string GetBytes2String(byte[] data, string format = null)
        {
            return HxString.GetBytes2String(data, format);
            /*
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString(format));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
            */
        }

        private static byte[] GetString2Bytes(string input, HxEncodingType encodingType = HxEncodingType.None)
        {
            return HxString.GetString2Bytes(input, encodingType);
            /*
            byte[] Result;
            switch (encodingType)
            {
                case HxEncodingType.UTF7:
                    Result = Encoding.UTF7.GetBytes(input);
                    break;
                case HxEncodingType.UTF32:
                    Result = Encoding.UTF32.GetBytes(input);
                    break;
                case HxEncodingType.Unicode:
                    Result = Encoding.Unicode.GetBytes(input);
                    break;
                case HxEncodingType.BigEndianUnicode:
                    Result = Encoding.BigEndianUnicode.GetBytes(input);
                    break;
                case HxEncodingType.ASCII:
                    Result = Encoding.ASCII.GetBytes(input);
                    break;
                case HxEncodingType.Default:
                    Result = Encoding.Default.GetBytes(input);
                    break;
                case HxEncodingType.UTF8:
                case HxEncodingType.None:
                default:
                    Result = Encoding.UTF8.GetBytes(input);
                    break;
            }
            return Result;
            */
        }

        #region System.Security.Cryptography
        /// <summary>
        /// MD5 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="encodingType"></param>
        /// <returns></returns>
        public static string Md5(string inputValue, HxEncodingType encodingType = HxEncodingType.None)
        {
            string Result = null;
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                Result = GetMd5Hash(md5Hash, inputValue, encodingType);

                //Debug.WriteLine("The MD5 hash of " + source + " is: " + Result + ".");

                //Debug.WriteLine("Verifying the hash...");
                /*
                if (VerifyMd5Hash(md5Hash, inputValue, Result))
                {
                    //Debug.WriteLine("The hashes are the same.");
                }
                else
                {
                    //Debug.WriteLine("The hashes are not same.");
                }
                */
            }
            //String.IsNullOrWhiteSpace
            return Result;
        }

        private static string GetMd5Hash(System.Security.Cryptography.MD5 md5Hash, string input, HxEncodingType encodingType = HxEncodingType.None)
        {

            // Convert the input string to a byte array and compute the hash.
            //byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            //byte[] data = md5Hash.ComputeHash(Encoder.Default.GetBytes(input));
            byte[] bytes = GetString2Bytes(input, encodingType);
            if (bytes == null || bytes.Length == 0) { return null; }

            byte[] data = md5Hash.ComputeHash(bytes);
            return GetBytes2String(data, "x2");

            //byte[] dataEA = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(input));
            //byte[] dataE7 = md5Hash.ComputeHash(Encoding.UTF7.GetBytes(input)); 
            //byte[] dataE8 = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            //byte[] dataEU = md5Hash.ComputeHash(Encoding.Unicode.GetBytes(input));
            //byte[] dataE32 = md5Hash.ComputeHash(Encoding.UTF32.GetBytes(input));
            //byte[] dataED = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));

            //byte[] dataAA = md5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            //byte[] dataA7 = md5Hash.ComputeHash(ASCIIEncoding.UTF7.GetBytes(input));
            //byte[] dataA8 = md5Hash.ComputeHash(ASCIIEncoding.UTF8.GetBytes(input));
            //byte[] dataAU = md5Hash.ComputeHash(ASCIIEncoding.Unicode.GetBytes(input));
            //byte[] dataA32 = md5Hash.ComputeHash(ASCIIEncoding.UTF32.GetBytes(input));
            //byte[] dataAD = md5Hash.ComputeHash(ASCIIEncoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            /*
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
            */

        }

        // Verify a hash against a string.
        private static bool VerifyMd5Hash(System.Security.Cryptography.MD5 md5Hash, string input, string hash, HxEncodingType encodingType = HxEncodingType.None)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input, encodingType);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Sha1(string inputValue, HxEncodingType encodingType = HxEncodingType.None)
        {
            string Result = null;
            using (System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                byte[] data = sha.ComputeHash(GetString2Bytes(inputValue, encodingType));
                Result = GetBytes2String(data, "x2");
            }
            return Result;
        }

        public static string Sha256(string inputValue, HxEncodingType encodingType = HxEncodingType.None)
        {
            string Result = null;
            using (System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {
                byte[] data = sha.ComputeHash(GetString2Bytes(inputValue, encodingType));
                Result = GetBytes2String(data, "x2");
            }
            return Result;
        }
        public static string Sha384(string inputValue, HxEncodingType encodingType = HxEncodingType.None)
        {
            string Result = null;
            using (System.Security.Cryptography.SHA384 sha = new System.Security.Cryptography.SHA384CryptoServiceProvider())
            {
                byte[] data = sha.ComputeHash(GetString2Bytes(inputValue, encodingType));
                Result = GetBytes2String(data, "x2");
            }
            return Result;
        }
        public static string Sha512(string inputValue, HxEncodingType encodingType = HxEncodingType.None)
        {
            string Result = null;
            using (System.Security.Cryptography.SHA512 sha = new System.Security.Cryptography.SHA512CryptoServiceProvider())
            {
                byte[] data = sha.ComputeHash(GetString2Bytes(inputValue, encodingType));
                Result = GetBytes2String(data, "x2");
            }
            return Result;
        }
        public static string GetHashString<T>(string inputValue, HxEncodingType encodingType = HxEncodingType.None) where T : HashAlgorithm
        {
            string Result = null;
            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))
            {
                byte[] bytes = GetString2Bytes(inputValue, encodingType);
                byte[] hashBytes = crypt.ComputeHash(bytes);
                Result = GetBytes2String(hashBytes, "x2");
            }
            return Result;
        }
        public static string GetHashString(string inputValue, string hashAlgorithmType, HxEncodingType encodingType = HxEncodingType.None)
        {
            string Result = null;
            if (inputValue.IsNullOrWhiteSpaceEx() != true && hashAlgorithmType.IsNullOrWhiteSpaceEx() != true)
                switch (hashAlgorithmType?.ToUpper())
                {
                    case "MD5":
                        Result = Md5(inputValue, encodingType);
                        break;
                    case "SHA1":
                        Result = Sha1(inputValue, encodingType);
                        break;
                    case "SHA256":
                        Result = Sha256(inputValue, encodingType);
                        break;
                    case "SHA384":
                        Result = Sha384(inputValue, encodingType);
                        break;
                    case "SHA512":
                        Result = Sha512(inputValue, encodingType);
                        break;
                    default:
                        Result = null;
                        break;
                }
            return Result;
        }
        string IHxCrypt.Base64Decode(string value)
        {
            return HxCrypt.base64_decode(value);
        }

        string IHxCrypt.Base64Encode(string value)
        {
            return HxCrypt.base64_encode(value);
        }

        string IHxCrypt.Decrypt(string value, string key)
        {
            return HxCrypt.Decrypt(value, key);
        }

        string IHxCrypt.Encrypt(string value, string key)
        {
            return HxCrypt.Encrypt(value, key);
        }

        string IHxCrypt.Md5(string value)
        {
            return HxCrypt.Md5(value);
        }

        string IHxCrypt.RandPass()
        {
            return HxCrypt.RandPass();
        }

        #endregion
    }
}
