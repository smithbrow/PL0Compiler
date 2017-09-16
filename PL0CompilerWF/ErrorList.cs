using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PL0CompilerLibrary;

namespace PL0CompilerWF
{
    public partial class ErrorList : Form
    {


        public ErrorList()
        {
            InitializeComponent();
        }

        public void setErrText(string text)
        {
            this.textBoxError.Text = "\r\n"+text;
        }

    }
}
