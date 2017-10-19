using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zlib;
using System.Threading;
using System.Windows.Forms;

namespace TEW2Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread MainFormThread = new Thread(() => {
                Application.Run(new MainForm());
            });
            MainFormThread.SetApartmentState(ApartmentState.STA);
            MainFormThread.Start();
            Console.WriteLine("MainForm started");
            //PTR test = new PTR(File.Open("dump", FileMode.Open));
            /*foreach(var a in test.index)
            {
                if (!a.encrypted)
                {
                    Console.WriteLine(a.path + " " + a.pkrOffset.ToString("X"));
                }
            }*/


            //MemStream example
            /*Stream pkrHandle = File.OpenRead(@"E:\Steam\steamapps\common\TheEvilWithin2\base\st00_main.pkr");
            PTR.PTRFile ptrFile = test.index[0];
            byte[] buff = new byte[ptrFile.sizeZipped];

            pkrHandle.Position = ptrFile.pkrOffset;
            pkrHandle.Read(buff, 0, buff.Length);
            pkrHandle.Flush();
            MemoryStream buffStream = new MemoryStream(buff);
            DeflateStream zStream = new DeflateStream(buffStream, CompressionMode.Decompress, CompressionLevel.Default);
            byte[] outBuff = new byte[ptrFile.sizeZipped];
            zStream.Read(outBuff, 0, outBuff.Length);
            File.WriteAllBytes("secondary", outBuff);*/
            //Console.WriteLine("Done");
            //Console.ReadKey();
        }
    }
}
