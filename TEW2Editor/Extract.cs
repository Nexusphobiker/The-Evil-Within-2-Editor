using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEW2Editor
{
    public static class Extract
    {
        public static void ToDisk(string targetDirectory, string pkrFilePath, PTR.PTRFile ptrfile)
        {
            Stream pkrFileHandle = File.OpenRead(pkrFilePath);
            byte[] buff = new byte[ptrfile.sizeZipped];
            pkrFileHandle.Position = ptrfile.pkrOffset;
            pkrFileHandle.Read(buff, 0, buff.Length);
            pkrFileHandle.Close();
            string path = targetDirectory + '\\' + Path.GetDirectoryName(ptrfile.path.Replace('/','\\'));
            Directory.CreateDirectory(path);
            Stream fileStream = File.Create(path + '\\' + Path.GetFileName(ptrfile.path));
            MemoryStream memoryStream = new MemoryStream(buff);
            //Check if the file needs to be deflated
            if (buff[buff.Length - 1] == 0xFF && buff[buff.Length - 2] == 0xFF && buff[buff.Length - 3] == 0x00 && buff[buff.Length - 4] == 0x00)
            {
                DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
                deflateStream.CopyTo(fileStream);
                deflateStream.Close();
            }
            else
            {
                memoryStream.CopyTo(fileStream);
            }
            fileStream.Close();
            memoryStream.Close();
        }

        public static byte[] ToMem(string pkrFilePath, PTR.PTRFile ptrfile)
        {
            byte[] ret = new byte[1];
            Stream pkrFileHandle = File.OpenRead(pkrFilePath);
            byte[] buff = new byte[ptrfile.sizeZipped];
            pkrFileHandle.Position = ptrfile.pkrOffset;
            pkrFileHandle.Read(buff, 0, buff.Length);
            pkrFileHandle.Close();
            MemoryStream memoryStream = new MemoryStream(buff);
            MemoryStream returnStream = new MemoryStream();
            //Check if the file needs to be deflated
            if (buff[buff.Length - 1] == 0xFF && buff[buff.Length - 2] == 0xFF && buff[buff.Length - 3] == 0x00 && buff[buff.Length - 4] == 0x00)
            {
                DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
                deflateStream.CopyTo(returnStream);
                deflateStream.Close();
                ret = returnStream.ToArray();
            }
            else
            {
                ret = memoryStream.ToArray();
            }
            returnStream.Close();
            memoryStream.Close();
            return ret;
        }
    }
}
