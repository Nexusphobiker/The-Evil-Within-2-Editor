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
        }
    }
}
