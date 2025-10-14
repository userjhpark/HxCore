using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    partial class HxUtils
    {
        #region File 처리 관련
        public static string FileReadAllTextToString(string fileName)
        {
            return HxFile.FileReadAllTextToString(fileName);
        }
        public static string FileReadAllTextToString(string fileName, Encoding encoding)
        {
            return HxFile.FileReadAllTextToString(fileName, encoding);
        }
        #endregion

        #region File / JSON 처리
        public static T FileReadAllTextToDeserializeObject<T>(string fileName)
        {
            string strJson = FileReadAllTextToString(fileName);
            if (strJson.IsNullOrWhiteSpaceEx() != true)
            {
                return JsonConvert.DeserializeObject<T>(strJson);
            }
            return default;
        }
        #endregion

        #region File/Image Checksum
        public static string GetMD5Checksum(string filePath)
        {
            return HxFile.GetFileMD5Checksum(filePath);
        }
        public static string GetMD5Checksum(System.Drawing.Image image)
        {
            return HxImagePicture.GetMD5Checksum(image);
        }
        public static string GetMD5Checksum(byte[] bytes)
        {
            return HxImagePicture.GetMD5Checksum(bytes);
        }
        #endregion

        #region Image
        public static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            return HxImagePicture.ImageToByteArray(image);
        }
        #endregion
    }
}
