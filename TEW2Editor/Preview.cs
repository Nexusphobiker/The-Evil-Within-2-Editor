using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEW2Editor
{
    public class Preview
    {
        public static Control GetControl(PTR.PTRFile ptrfile, string pkrPath)
        {
            string fileExtension = Path.GetExtension(ptrfile.path);
            switch (fileExtension)
            {
                case ".png":
                    PictureBox picboxprev = new PictureBox();
                    picboxprev.Dock = DockStyle.Fill;
                    byte[] pngdata = Extract.ToMem(pkrPath, ptrfile);
                    Image tempIMG;
                    using (MemoryStream stream = new MemoryStream(pngdata))
                    {
                        tempIMG = Image.FromStream(stream);
                    }
                    picboxprev.Image = tempIMG;
                    picboxprev.SizeMode = PictureBoxSizeMode.Zoom;
                    return picboxprev;
                case ".lanb":
                    RichTextBox lanbprev = new RichTextBox();
                    lanbprev.Dock = DockStyle.Fill;
                    byte[] lanbdata = Extract.ToMem(pkrPath, ptrfile);
                    LANB tempLANB;
                    using (MemoryStream stream = new MemoryStream(lanbdata))
                    {
                        tempLANB = new LANB(stream);
                    }
                    foreach(var entry in tempLANB.entrys)
                    {
                        lanbprev.Text = lanbprev.Text + "NAME:" + entry.name + " TEXT:" + entry.text + '\n';
                    }
                    return lanbprev;
                case ".decl":
                    RichTextBox declprev = new RichTextBox();
                    declprev.Dock = DockStyle.Fill;
                    byte[] decldata = Extract.ToMem(pkrPath, ptrfile);
                    declprev.Text = Encoding.Default.GetString(decldata); //formatting would be nice probably
                    declprev.ScrollBars = RichTextBoxScrollBars.Both;
                    return declprev;
                case ".bdecl":
                    RichTextBox bdeclprev = new RichTextBox();
                    bdeclprev.Dock = DockStyle.Fill;
                    byte[] bdecldata = Extract.ToMem(pkrPath, ptrfile);
                    BDECL tempBDECL;
                    using(MemoryStream stream = new MemoryStream(bdecldata))
                    {
                        tempBDECL = new BDECL(stream);
                    }
                    bdeclprev.Text = tempBDECL.data;
                    bdeclprev.ScrollBars = RichTextBoxScrollBars.Both;
                    return bdeclprev;
                default:
                    return null;
            }
        }
    }
}
