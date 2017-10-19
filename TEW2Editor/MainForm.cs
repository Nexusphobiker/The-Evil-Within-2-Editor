using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zlib;

namespace TEW2Editor
{
    public partial class MainForm : Form
    {
        private PTR loadedPTR = null;
        private string ptrPath = "";
        private string pkrPath = "";
        public MainForm()
        {
            InitializeComponent();
            fileTreeView.AfterExpand += fileTreeView_LoadPartial;
            fileTreeView.AfterSelect += fileTreeView_ShowPreview;
        }
        #region fileTreeViewEvents
        private void fileTreeView_ShowPreview(object sender, TreeViewEventArgs eventargs)
        {
            splitContainerMainForm.Panel2.Controls.Clear();
            string path = fileTreeView.SelectedNode.FullPath.Replace('\\', '/');
            foreach(var file in loadedPTR.index)
            {
                if(path.Length == file.path.Length)
                {
                    if(path == file.path)
                    {
                        Control temp = Preview.GetControl(file,pkrPath);
                        if (temp != null)
                        {
                            splitContainerMainForm.Panel2.Controls.Add(temp);
                        }
                    }
                }
            }
        }

        private void fileTreeView_LoadPartial(object sender, TreeViewEventArgs eventargs)
        {
            Console.WriteLine("partial load " + eventargs.Node.FullPath);
            string[] path = eventargs.Node.FullPath.Split('\\');
            bool isPart;
            foreach (var ptrfile in loadedPTR.index)
            {
                isPart = true;
                string[] filepath = ptrfile.path.Split('/');
                int i = 0;
                while (i < path.Length)
                {
                    if (filepath[i] != path[i])
                    {
                        isPart = false;
                        break;
                    }
                    i++;
                }
                if (i < filepath.Length && isPart)
                {
                    if (eventargs.Node.Nodes.Find(filepath[i], true).Length == 0)
                    {
                        if (i < filepath.Length - 1)
                        {
                            eventargs.Node.Nodes.Add(filepath[i], filepath[i]).Nodes.Add("TEMP", "You shouldnt see this");
                        }
                        else
                        {
                            eventargs.Node.Nodes.Add(filepath[i], filepath[i]);
                        }
                    }
                    else
                    {
                        //Because some directories contain files with the same name
                        if (i >= filepath.Length - 1)
                        {
                            eventargs.Node.Nodes.Add(filepath[i], filepath[i]);
                        }
                    }
                }
            }
            eventargs.Node.Nodes.RemoveByKey("TEMP");
        }
        #endregion

        #region ToolStripMenu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ".ptr files (*.ptr) |*.ptr| .pkr files (*.pkr)|*.pkr";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("No file selected.");
                return;
            }
            Console.WriteLine("Opening " + openFileDialog.FileName);
            
            switch (Path.GetExtension(openFileDialog.FileName))
            {
                case ".ptr":
                    Console.WriteLine("Seeking according .pkr file...");
                    ptrPath = openFileDialog.FileName;
                    pkrPath = Path.GetDirectoryName(ptrPath).Replace("ptr", string.Empty) + Path.GetFileNameWithoutExtension(ptrPath) + ".pkr";
                    if (!File.Exists(pkrPath))
                    {
                        Console.WriteLine(" pkr:" + pkrPath);
                        Console.WriteLine(".pkr file not found. Make sure to copy use the same folder structure like the game");
                        return;
                    }
                    break;
                case ".pkr":
                    Console.WriteLine("Seeking according .ptr file...");
                    pkrPath = openFileDialog.FileName;
                    ptrPath = Path.GetDirectoryName(pkrPath) + "\\ptr\\" + Path.GetFileNameWithoutExtension(pkrPath) + ".ptr";
                    if (!File.Exists(ptrPath))
                    {
                        Console.WriteLine(" ptr:" + ptrPath);
                        Console.WriteLine(".ptr file not found. Make sure to copy use the same folder structure like the game");
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("Unknown file extension. That shouldnt happen");
                    return;
            }
            Console.WriteLine("ptr:" + ptrPath + " pkr:" + pkrPath);
            Console.WriteLine("Loading .ptr ...");
            byte[] ptrFile = File.ReadAllBytes(ptrPath);
            Console.WriteLine(".ptr loaded " + ptrFile.Length + " bytes");

            //Load deflated file
            MemoryStream ptrFileStream = new MemoryStream(ptrFile);
            ptrFileStream.Position += 16;
            DeflateStream deflateFileStream = new DeflateStream(ptrFileStream, CompressionMode.Decompress);
            MemoryStream mStream = new MemoryStream();
            deflateFileStream.CopyTo(mStream);
            //close the streams and parse deflated ptr
            deflateFileStream.Close();
            ptrFileStream.Close();
            PTR ptr = new PTR(mStream);
            loadedPTR = ptr;
            mStream.Close();
            Console.WriteLine("Loading finished. Populating treeView");
            fileTreeView.Nodes.Clear();
            foreach(var ptrfile in ptr.index)
            {
                Console.WriteLine(ptrfile.path);
                string[] pathArr = ptrfile.path.Split('/');
                if (fileTreeView.Nodes.Find(pathArr[0], false).Length == 0)
                {
                    if (pathArr.Length > 1)
                    {
                        //The TEMP nodes are added to make the nodes expandable
                        fileTreeView.Nodes.Add(pathArr[0], pathArr[0]).Nodes.Add("TEMP", "loading...");
                    }
                    else
                    {
                        fileTreeView.Nodes.Add(pathArr[0], pathArr[0]);
                    }
                }
            }
            Console.WriteLine(ptr.index.Count);
            Console.WriteLine("Done");
            this.Text = Path.GetFileNameWithoutExtension(ptrPath);
        }

        

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(loadedPTR == null)
            {
                Console.WriteLine("No file loaded. Please load a file before extraction.");
                return;
            }
                

            Console.WriteLine("Extracting selected file or directory...");
            FolderBrowserDialog folderbrowserdialog = new FolderBrowserDialog();
            if(folderbrowserdialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("No folder chosen.");
                return;
            }

            string[] path = fileTreeView.SelectedNode.FullPath.Split('\\');
            Console.WriteLine("folder:" + folderbrowserdialog.SelectedPath);
            Console.WriteLine("start extracting " + path);
            List<string> extractedFiles = new List<string>();
            foreach (var file in loadedPTR.index)
            {
                string[] filePath = file.path.Split('/');
                if (filePath.Length >= path.Length)
                {
                    bool isExtractionTarget = true;
                    int i = 0;
                    while (i < path.Length)
                    {
                        if(path[i] != filePath[i])
                        {
                            isExtractionTarget = false;
                            break;
                        }
                        i++;
                    }
                    if (isExtractionTarget)
                    {
                        Console.WriteLine("Extracting " + file.path);
                        if (!extractedFiles.Contains(file.path))
                        {
                            Extract.ToDisk(folderbrowserdialog.SelectedPath, pkrPath, file);
                            extractedFiles.Add(file.path);
                        }
                        else
                        {
                            int fileCount = 0;
                            PTR.PTRFile temp = file;
                            while (extractedFiles.Contains(temp.path + fileCount))
                            {
                                fileCount++;
                            }
                            temp.path = temp.path + fileCount;
                            Extract.ToDisk(folderbrowserdialog.SelectedPath, pkrPath, temp);
                            extractedFiles.Add(temp.path);
                        }
                    }
                }
            }
            Console.WriteLine("Done extracting");
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedPTR == null)
            {
                Console.WriteLine("No file loaded. Please load a file before extraction.");
                return;
            }

            Console.WriteLine("Extracting selected file or directory...");
            FolderBrowserDialog folderbrowserdialog = new FolderBrowserDialog();
            if (folderbrowserdialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("No folder chosen.");
                return;
            }
            Console.WriteLine("folder:" + folderbrowserdialog.SelectedPath);
            Console.WriteLine("start extracting all...");
            List<string> extractedFiles = new List<string>();
            foreach (var file in loadedPTR.index)
            {
                Console.WriteLine("Extracting " + file.path);
                if (!extractedFiles.Contains(file.path))
                {
                    Extract.ToDisk(folderbrowserdialog.SelectedPath, pkrPath, file);
                    extractedFiles.Add(file.path);
                }
                else
                {
                    int fileCount = 0;
                    PTR.PTRFile temp = file;
                    while (extractedFiles.Contains(temp.path+fileCount))
                    {
                        fileCount++;
                    }
                    temp.path = temp.path + fileCount;
                    Extract.ToDisk(folderbrowserdialog.SelectedPath, pkrPath, temp);
                    extractedFiles.Add(temp.path);
                }
            }
            Console.WriteLine("Done");
        }
        #endregion
    }
}
