using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEW2Editor
{
    public class PTR
    {
        public List<PTRFile> index;
        public PTR(Stream stream)
        {
            if (stream.CanSeek == false)
            {
                throw (new Exception("The PTR class needs to be able to seek the Stream to skip a lot of unused data by this tool."));
            }
            //Reset stream
            stream.Seek(0, SeekOrigin.Begin);

            byte[] intBuff = new byte[4];
            stream.Read(intBuff, 0, intBuff.Length);
            Int32 primaryIndexCount = BitConverter.ToInt32(intBuff,0);
            //Skip
            stream.Read(intBuff, 0, intBuff.Length);
            stream.Read(intBuff, 0, intBuff.Length);
            stream.Read(intBuff, 0, intBuff.Length);

            stream.Read(intBuff, 0, intBuff.Length);
            Int32 secondaryIndexCount = BitConverter.ToInt32(intBuff, 0);

            //Skip the first two indexes because they are not needed
            stream.Position += (4 * primaryIndexCount)+(4*secondaryIndexCount);

            stream.Read(intBuff, 0, intBuff.Length);
            Int32 pathSectionLength = BitConverter.ToInt32(intBuff, 0);

            stream.Read(intBuff, 0, intBuff.Length);
            Int32 pathCount = BitConverter.ToInt32(intBuff, 0);

            //Read all pathes to the array
            string[] pathList = new string[pathCount];
            int i = 0;
            while(i < pathList.Length)
            {
                pathList[i] = ReadString(stream);
                i++;
            }

            index = new List<PTRFile>();

            //Read actual file index
            while (primaryIndexCount > 0) { 
                
                stream.Read(intBuff, 0, intBuff.Length);
                Int32 pathIndex = BitConverter.ToInt32(intBuff, 0);

                stream.Read(intBuff, 0, intBuff.Length);
                Int32 pkrOffset = BitConverter.ToInt32(intBuff, 0);

                stream.Read(intBuff, 0, intBuff.Length);
                Int32 sizeB = BitConverter.ToInt32(intBuff, 0);

                stream.Read(intBuff, 0, intBuff.Length);
                Int32 sizeA = BitConverter.ToInt32(intBuff, 0);

                PTRFile temp = new PTRFile();
                temp.path = pathList[pathIndex];
                temp.pkrOffset = pkrOffset;
                temp.size = sizeA;//sizeB;
                temp.sizeZipped = sizeB;//sizeA;

                index.Add(temp);
                primaryIndexCount--;
            }
            
            //The caller has to close the handle
        }

        private string ReadString(Stream stream)
        {
            byte i = (byte)stream.ReadByte();
            string ret = "";
            while (i != 0){
                ret += (char)i;
                i = (byte)stream.ReadByte();
            }
            return ret;
        }

        public partial class PTRFile
        {
            public string path;
            public Int32 pkrOffset;
            public Int32 size;
            public Int32 sizeZipped;
        }
    }
}
