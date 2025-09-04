using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HxCore.Web
{
    public struct HxPostFileRec
    {
        public string Name;
        public string LocalFileName;
        public long LocalFileLength;
        public string LocalDirName;
        public string FileName;
        public string FileType;
        public string FileSavePath;
        public string FileSaveName;

        public HxPostFileRec(System.Net.Http.MultipartFileData file)
        {
            this.Name = file.Headers.ContentDisposition.Name;
            this.LocalFileName = file.LocalFileName;
            if (File.Exists(this.LocalFileName))
            {
                FileInfo fi = new FileInfo(this.LocalFileName);
                this.LocalFileLength = fi.Length;
                this.LocalDirName = fi.DirectoryName;
            }
            else
            {
                this.LocalFileLength = -1;
                this.LocalDirName = null;
            }
            this.FileName = file.Headers.ContentDisposition.FileName;
            this.FileType = file.Headers.ContentType.MediaType;

            this.FileSavePath = null;
            this.FileSaveName = null;
        }

    }
}
