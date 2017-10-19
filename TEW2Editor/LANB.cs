using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEW2Editor
{
    public class LANB
    {
        public List<Entry> entrys;
        public LANB(Stream stream)
        {
            //Reset stream and skip the first 12 bytes
            stream.Seek(12, SeekOrigin.Begin);
            entrys = new List<Entry>();
            byte[] intBuff = new byte[4];
            while (stream.Position < stream.Length)
            {
                //Skip 4
                stream.Read(intBuff, 0, intBuff.Length);

                //Read name
                stream.Read(intBuff, 0, intBuff.Length);
                Int32 nameLen = BitConverter.ToInt32(intBuff, 0);
                byte[] nameBuff = new byte[nameLen];
                stream.Read(nameBuff, 0, nameBuff.Length);

                //Read text
                stream.Read(intBuff, 0, intBuff.Length);
                Int32 textLen = BitConverter.ToInt32(intBuff, 0);
                byte[] textBuff = new byte[textLen];
                stream.Read(textBuff, 0, textBuff.Length);

                //Add
                Entry temp = new Entry();
                temp.name = Encoding.UTF8.GetString(nameBuff); //ENCODING <---------------- UTF8 seems to work for german strings but im not sure if it is the right encoding
                temp.text = Encoding.UTF8.GetString(textBuff);
                entrys.Add(temp);
            }
        }

        public partial class Entry
        {
            public string name;
            public string text;
        }
    }
}
