using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace HxCore
{
    public class HxImagePicture
    {
        /// <summary>
        /// Image To Bytes
        /// </summary>
        /// <param name="image">Image Resource</param>
        /// <returns>Bytes</returns>
        public static byte[] ImageToByteArray(Image image)
        {
            if (image == null) { return Array.Empty<byte>(); }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Bytes(Array) To Image
        /// </summary>
        /// <param name="bytes">Bytes</param>
        /// <returns>Image Resource</returns>
        public static Image ByteArrayToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) {  return null; }

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }
        /// <summary>
        /// File To Image Resource
        /// </summary>
        /// <param name="filePath">File Full Name</param>
        /// <returns>Image Resource</returns>
        public static Image LoadImageFromFile(string filePath)
        {
            if (filePath.IsNullOrWhiteSpaceEx() == true || HxFile.FileExists(filePath) != true)
                return null;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return Image.FromStream(fs);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading image from file: {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// Image Resource To File
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        public static void SaveImageToFile(Image image, string filePath, System.Drawing.Imaging.ImageFormat format)
        {
            if (image == null || string.IsNullOrWhiteSpace(filePath))
                return;
            try
            {
                image.Save(filePath, format);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving image to file: {ex.Message}");
            }
        }

        public static string GetMD5Checksum(Image image)
        {
            byte[] bytes = ImageToByteArray(image);
            return HxString.GetMD5Checksum(bytes);
        }
        public static string GetMD5Checksum(byte[] bytes)
        {
            return HxString.GetSHA512Checksum(bytes);
        }
        public static string GetSHA1Checksum(byte[] bytes)
        {
            return HxString.GetSHA1Checksum(bytes);
        }
        public static string GetSHA256Checksum(byte[] bytes)
        {
            return HxString.GetSHA256Checksum(bytes);
        }
        public static string GetSHA512Checksum(byte[] bytes)
        {
            return HxString.GetSHA512Checksum(bytes);
        }
        public static string GetSHA1Checksum(Image image)
        {
            byte[] bytes = ImageToByteArray(image);
            return GetSHA1Checksum(bytes);
        }
        public static string GetSHA256Checksum(Image image)
        {
            byte[] bytes = ImageToByteArray(image);
            return GetSHA256Checksum(bytes);
        }

        public static string GetSHA512Checksum(Image image)
        {
            byte[] bytes = ImageToByteArray(image);
            return GetSHA512Checksum(bytes);
        }
        



        public static string GetImageFormatString(System.Drawing.Imaging.ImageFormat format)
        {
            if (format == System.Drawing.Imaging.ImageFormat.Jpeg)
                return "JPEG";
            if (format == System.Drawing.Imaging.ImageFormat.Png)
                return "PNG";
            if (format == System.Drawing.Imaging.ImageFormat.Bmp)
                return "BMP";
            if (format == System.Drawing.Imaging.ImageFormat.Gif)
                return "GIF";
            if (format == System.Drawing.Imaging.ImageFormat.Tiff)
                return "TIFF";
            if (format == System.Drawing.Imaging.ImageFormat.Icon)
                return "ICON";
            return "Unknown";
        }

        public static System.Drawing.Imaging.ImageFormat GetImageFormatFromString(string formatStr)
        {
            System.Drawing.Imaging.ImageFormat Result = null;

            if (!HxString.IsNullOrWhiteSpace(formatStr)) { return Result; }

            switch (formatStr.ToUpper())
            {
                case "JPEG":
                    Result = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case "PNG":
                    Result = System.Drawing.Imaging.ImageFormat.Png;
                    break;
                case "BMP":
                    Result = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                case "GIF":
                    Result = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
                case "TIFF":
                    Result = System.Drawing.Imaging.ImageFormat.Tiff;
                    break;
                case "ICON":
                    Result = System.Drawing.Imaging.ImageFormat.Icon;
                    break;
                default:
                    Result = null;
                    break;
            }
            return Result;
        }
    }
}
