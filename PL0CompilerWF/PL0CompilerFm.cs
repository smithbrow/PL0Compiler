using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PL0CompilerLibrary;
using System.Diagnostics;
using System.Threading;

namespace PL0CompilerWF
{
    public partial class PL0CompilerFM : Form
    {
        static public bool FileCreated { get; set; }

        static public string FilePath { get; set; }

        private PL0Compiler Compiler;

        private ErrorList errList = new ErrorList();

        private CodeList codeList = new CodeList();

        public PL0CompilerFM()
        {
            InitializeComponent();
            textBoxContent.Visible = false;
            FileCreated = false;
            FilePath = null;
        }

        private void PL0Compiler_Load(object sender, EventArgs e)
        {

        }

        #region TextBoxOpration

        private void NewPL0Program(object sender, EventArgs e)
        {
            if (FilePath == null)
            {
                NewFileDlg NewFile = new NewFileDlg();
                if (NewFile.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxContent.Visible = true;
                    OpenFile();
                    setFormTitle();
                    
                    this.textBoxContent.Focus();
                    this.textBoxContent.AcceptsTab = true;
                    this.textBoxContent.SelectionLength = 0;
                    this.toolStripStatusLabelLine.Visible = true;
                    this.toolStripStatusLabelLineNu.Visible = true;
                    this.toolStripStatusLabelLineNu.Text = "1";
                    this.toolStripStatusLabelRaw.Visible = true;
                    this.toolStripStatusLabelRawNu.Visible = true;
                    this.toolStripStatusLabelRawNu.Text = "1";
                }

                NewFile.Close();
            }
            else
            {
                string msg = "请先关闭当前文件！";
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnOpenFile(object sender, EventArgs e)
        {
            if (FilePath == null)
            {
                OpenFileDialog OFDlg = new OpenFileDialog();
                OFDlg.Filter = "PL程序文件 (*.pl)|*.pl|文本文件 (*.txt)|*.txt|所有文件|*.*";
                if (OFDlg.ShowDialog() == DialogResult.OK)
                {
                    FilePath = OFDlg.FileName;
                    this.textBoxContent.Visible = true;
                    OpenFile();
                    setFormTitle();
                    
                    this.textBoxContent.Focus();
                    this.textBoxContent.AcceptsTab = true;
                    this.textBoxContent.SelectionLength = 0;
                    this.toolStripStatusLabelLine.Visible = true;
                    this.toolStripStatusLabelLineNu.Visible = true;
                    this.toolStripStatusLabelLineNu.Text = "1";
                    this.toolStripStatusLabelRaw.Visible = true;
                    this.toolStripStatusLabelRawNu.Visible = true;
                    this.toolStripStatusLabelRawNu.Text = "1";
                }
            }
            else
            {
                string msg = "请先关闭当前文件！";
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnSaveFile(object sender, EventArgs e)
        {
            if (FilePath != null)
            {
                SaveFile();
            }
        }

        private void OnSaveAsFile(object sender, EventArgs e)
        {
            if (FilePath != null)
            {
                SaveFileDialog SFDlg = new SaveFileDialog();
                SFDlg.Filter = "PL程序文件 (*.pl)|*.pl|文本文件 (*.txt)|*.txt";
                if (SFDlg.ShowDialog() == DialogResult.OK)
                {
                    FilePath = SFDlg.FileName;
                    SaveFile();
                    setFormTitle();
                }
            }
        }

        private void OnCloseFile(object sender, EventArgs e)
        {
            if (FilePath != null)
            {
                if (textBoxContent.Text != File.ReadAllText(FilePath))
                {
                    string sMsg = "是否保存对文档的更改？";
                    DialogResult dr = MessageBox.Show(sMsg, this.Text,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFile();
                        Compiler = null;
                        if (errList != null)
                            errList.Close();
                        if (codeList != null)
                            codeList.Close();
                        
                    }
                    else if (dr == DialogResult.No)
                    {
                        textBoxContent.Clear();
                        textBoxContent.ClearUndoAndRedo();
                        textBoxContent.AcceptsTab = false;
                        textBoxContent.Visible = false;
                        FilePath = null;
                        Compiler = null;
                        if (errList != null)
                            errList.Close();
                        if (codeList != null)
                            codeList.Close();
                    }
                }
                else
                {
                    textBoxContent.Clear();
                    textBoxContent.ClearUndoAndRedo();
                    textBoxContent.AcceptsTab = false;
                    textBoxContent.Visible = false;
                    FilePath = null;
                    Compiler = null;
                    if (errList != null)
                        errList.Close();
                    if (codeList != null)
                        codeList.Close();
                }
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            Close();
        }

        private void OnClosingForm(object sender, FormClosingEventArgs e)
        {
            if (FilePath != null)
            {
                if (textBoxContent.Text != File.ReadAllText(FilePath))
                {
                    string sMsg = "是否保存对文档的更改？";
                    DialogResult dr = MessageBox.Show(sMsg, this.Text,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        SaveFile();
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void OpenFile()
        {
            try
            {
                textBoxContent.Clear();
                textBoxContent.Text = File.ReadAllText(FilePath);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "出现异常", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void setFormTitle()
        {
            FileInfo fi = new FileInfo(FilePath);
            this.Text = fi.Name + " - PL0Compiler";
        }

        private void SaveFile()
        {
            try
            {
                File.WriteAllText(FilePath, this.textBoxContent.Text);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "出现异常", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnUndo(object sender, EventArgs e)
        {
            textBoxContent.Undo();
            Focuslocation();
        }

        private void OnRedo(object sender, EventArgs e)
        {
            textBoxContent.Redo();
            Focuslocation();
        }

        private void OnCopy(object sender, EventArgs e)
        {
            textBoxContent.Copy();
        }

        private void OnPaste(object sender, EventArgs e)
        {
            string text = Clipboard.GetText();
            textBoxContent.Paste(text);
            Focuslocation();
        }

        private void OnCut(object sender, EventArgs e)
        {
            textBoxContent.Cut();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            int start = textBoxContent.SelectionStart;
            int len = textBoxContent.SelectionLength;
            textBoxContent.Text = textBoxContent.Text.Remove(start, len);
            textBoxContent.Focus();
            textBoxContent.SelectionStart = start;
            textBoxContent.SelectionLength = 0;
        }

        private void OnSelectAll(object sender, EventArgs e)
        {
            textBoxContent.SelectAll();
        }

        private void OnFocusLoc(object sender, EventArgs e)
        {
            Focuslocation();
        }

        private void Focuslocation()
        {
            string sub = textBoxContent.Text.Substring(0, textBoxContent.SelectionStart);
            string[] subs = sub.Split('\n');
            this.toolStripStatusLabelLineNu.Text = subs.Length.ToString();
            int l = 0;
            byte[] subsByte;
            for (int i = 0; i < subs.Length - 1; i++)
            {
                subsByte = Encoding.GetEncoding("GBK").GetBytes(subs[i]);
                l +=  subsByte.Length + 1;
            }
            byte[] subByte = Encoding.GetEncoding("GBK").GetBytes(sub);
            int r = subByte.Length - l + 1;
            this.toolStripStatusLabelRawNu.Text = r.ToString();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                OnSelectAll(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.O)
            {
                OnOpenFile(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.Z)
            {
                OnUndo(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.Y)
            {
                OnRedo(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.X)
            {
                OnCut(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                OnCopy(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.V)
            {
                OnPaste(sender, e);
                return;
            }

            if (e.Control && e.KeyCode == Keys.N)
            {
                NewPL0Program(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                OnSaveFile(sender, e);
            }

            if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                OnSaveAsFile(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.D)
            {
                OnDelete(sender, e);
            }

            if (e.Control && e.KeyCode == Keys.W)
            {
                OnCloseFile(sender, e);
            }
            
            if ( e.KeyCode == Keys.Tab)
            {
                textBoxContent.SelectedText = "    ";
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.F7)
            {
                OnCompile(sender, e);
            }

            if (e.KeyCode == Keys.F6)
            {
                OnBuild(sender, e);
            }

            if (e.KeyCode == Keys.F5)
            {
                OnExecute(sender, e);
            }

            if (e.KeyCode == Keys.Left|| e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                string sub = textBoxContent.Text;
                string[] subs = sub.Split('\n');
                int l = Convert.ToInt32(this.toolStripStatusLabelLineNu.Text);
                int r = Convert.ToInt32(this.toolStripStatusLabelRawNu.Text);
                if (e.KeyCode == Keys.Left)
                {
                    if (r > 1)
                    {
                        string c = sub[textBoxContent.SelectionStart-1].ToString();
                        byte[] cByte = Encoding.GetEncoding("GBK").GetBytes(c);
                        r = r - cByte.Length;
                        this.toolStripStatusLabelRawNu.Text = r.ToString();
                    }
                    else if (r == 1 && l>1)
                    {
                        l--;
                        byte[] subByte = Encoding.GetEncoding("GBK").GetBytes(subs[l - 1]);
                        r = subByte.Length;
                        this.toolStripStatusLabelLineNu.Text = l.ToString();
                        this.toolStripStatusLabelRawNu.Text = r.ToString();
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    byte[] subByte = Encoding.GetEncoding("GBK").GetBytes(subs[l - 1]);
                    if (r < subByte.Length)
                    {
                        string c = sub[textBoxContent.SelectionStart].ToString();
                        byte[] cByte = Encoding.GetEncoding("GBK").GetBytes(c);
                        r = r + cByte.Length;
                        this.toolStripStatusLabelRawNu.Text = r.ToString();
                    }
                    else if (r == subByte.Length && l < subs.Length)
                    {
                        l++;
                        r = 1;
                        this.toolStripStatusLabelLineNu.Text = l.ToString();
                        this.toolStripStatusLabelRawNu.Text = r.ToString();
                    }
                    else if (l == subs.Length)
                    {
                        if (r < subByte.Length + 1)
                        {
                            string c = sub[textBoxContent.SelectionStart].ToString();
                            byte[] cByte = Encoding.GetEncoding("GBK").GetBytes(c);
                            r = r + cByte.Length;
                            this.toolStripStatusLabelRawNu.Text = r.ToString();
                        }
                    }

                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (l > 1)
                    {
                        l--;
                        this.toolStripStatusLabelLineNu.Text = l.ToString();
                        byte[] subByte = Encoding.GetEncoding("GBK").GetBytes(subs[l - 1]);
                        if (r > subByte.Length)
                        {
                            r = subByte.Length;
                            this.toolStripStatusLabelRawNu.Text = r.ToString();
                        }
                    }

                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (l < subs.Length-1)
                    {
                        l++;
                        this.toolStripStatusLabelLineNu.Text = l.ToString();
                        byte[] subByte = Encoding.GetEncoding("GBK").GetBytes(subs[l - 1]);
                        if (r > subByte.Length)
                        {
                            r = subByte.Length;
                            this.toolStripStatusLabelRawNu.Text = r.ToString();
                        }
                    }
                    else if (l == subs.Length - 1)
                    {
                        l++;
                        this.toolStripStatusLabelLineNu.Text = l.ToString();
                        byte[] subByte = Encoding.GetEncoding("GBK").GetBytes(subs[l - 1]);
                        if (r > subByte.Length + 1)
                        {
                            r = subByte.Length + 1;
                            this.toolStripStatusLabelRawNu.Text = r.ToString();
                        }
                    }
                }
            }
            /*if (e.KeyCode == Keys.Enter)
            {
                int l = Convert.ToInt32(this.toolStripStatusLabelLineNu.Text);
                l++;
                this.toolStripStatusLabelLineNu.Text = l.ToString();
                this.toolStripStatusLabelRawNu.Text = "1";
            }

            if (e.KeyCode == Keys.Back)
            {
                string sub = textBoxContent.Text;
                string[] subs = sub.Split('\n');
                int line = Convert.ToInt32(this.toolStripStatusLabelLineNu.Text);
                int raw = Convert.ToInt32(this.toolStripStatusLabelRawNu.Text);
                if (raw == 1 && line > 1)
                {
                    line--;
                    raw = subs[line - 1].Length;
                    this.toolStripStatusLabelLineNu.Text = line.ToString();
                    this.toolStripStatusLabelRawNu.Text = raw.ToString();

                }
                else if (raw > 1)
                {
                    raw--;
                    this.toolStripStatusLabelRawNu.Text = raw.ToString();
                }
            }*/

        }

        private void OnTextChange(object sender, EventArgs e)
        {
            Focuslocation();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            //Focuslocation();
        }
        #endregion

        #region Compiler

        private void OnCompile(object sender, EventArgs e)
        {
            if (FilePath != null)
            {
                this.SaveFile();
                
                this.Compiler = new PL0Compiler(FilePath);
                Compiler.compile();

                if (!errList.Created)
                {
                    errList = new ErrorList();
                    errList.Show();
                }
                errList.setErrText(Compiler.displayErr());
                
               
                
            }
            else
            {
                string msg = "请新建或打开一个PL程序文件！";
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnBuild(object sender, EventArgs e)
        {
            if (FilePath != null)
            {
                this.SaveFile();
              
                this.Compiler = new PL0Compiler(FilePath);
                Compiler.compile();
                if (Compiler.EH.errItem.Count == 0)
                {
                    if (!codeList.Created)
                    {
                        codeList = new CodeList();
                        codeList.Show();
                    }
                    this.pCodeFile();
                    codeList.setCodeText(Compiler.listCode());
                }
                else
                {
                    if (!errList.Created)
                    {
                        errList = new ErrorList();
                        errList.Show();
                    }
                    errList.setErrText(Compiler.displayErr());
                }
            }
            else
            {
                string msg = "请新建或打开一个PL程序文件！";
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void OnExecute(object sender, EventArgs e)
        {
            if (FilePath != null)
            {
                this.SaveFile();

                this.Compiler = new PL0Compiler(FilePath);
                Compiler.compile();
                if (Compiler.EH.errItem.Count == 0)
                {
                    string CodeFile = pCodeFile();
                    if (CodeFile != "")
                    {
                        string curPath = Directory.GetCurrentDirectory();
                        
                        Process pInte = new Process();
                        pInte.StartInfo.FileName = curPath + "\\PL0CodeInterpreter.exe";
                        pInte.StartInfo.Arguments = CodeFile;
                        pInte.Start();
                    }                    

                }
                else
                {
                    if (!errList.Created)
                    {
                        errList = new ErrorList();
                        errList.Show();
                    }
                    errList.setErrText(Compiler.displayErr());
                }
            }
            else
            {
                string msg = "请新建或打开一个PL程序文件！";
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string pCodeFile()
        {
            try
            {
                FileInfo fi = new FileInfo(FilePath);
                int lp = fi.Name.LastIndexOf('.');
                string CodeFile = fi.DirectoryName + "\\" + fi.Name.Substring(0, lp) + ".pCode";
                StreamWriter cwr = File.CreateText(CodeFile);
                
                instruction ins = new instruction();
                for (int i = 0; i < Compiler.PCG.code.Count; i++)
                {
                    ins = Compiler.PCG.code[i];
                    string temp = ((int)ins.oprc).ToString() + " " + ins.fa.ToString() + " " + ins.la.ToString();
                    cwr.WriteLine(temp);
                }
                cwr.Close();
                return CodeFile;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            } 
            finally
            {
               
            }

        }

        #endregion

    }
}
