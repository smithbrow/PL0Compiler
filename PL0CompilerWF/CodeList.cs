using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PL0CompilerWF
{
    public partial class CodeList : Form
    {
        public CodeList()
        {
            InitializeComponent();
        }

        public void setCodeText(string code)
        {
            this.textBoxCode.Text = "\r\n" + code;
        }
    }
}
