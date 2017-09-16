using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PL0CompilerWF
{
    public partial class NewFileDlg : Form
    {
        
        public NewFileDlg()
        {
            InitializeComponent();
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog BDlg = new FolderBrowserDialog();

            if (BDlg.ShowDialog() == DialogResult.OK)
            {
                this.textBoxLoca.Text = BDlg.SelectedPath;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string path;
            if(textBoxLoca.Text.EndsWith("\\"))
                path = textBoxLoca.Text + textBoxName.Text;
            else
                path = textBoxLoca.Text +"\\"+ textBoxName.Text;
            try
            {
                if (!File.Exists(path))
                {
                    File.CreateText(path).Dispose();
                    
                    PL0CompilerFM.FilePath = path;
                    PL0CompilerFM.FileCreated = true;
                    this.DialogResult = DialogResult.OK;     
                }
                else
                {
                    string warning = "文件已存在，确定覆盖该文件吗？";
                    DialogResult ds =
                        MessageBox.Show(warning, "确定创建", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (ds == DialogResult.OK)
                    {
                        File.CreateText(path).Close();
                       
                        PL0CompilerFM.FilePath = path;
                        PL0CompilerFM.FileCreated = true;
                        this.DialogResult = DialogResult.OK;  
                    }

                }
                

            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.Message,"出现异常",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void buttonConcel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;  
        }


    }



}
