namespace PL0CompilerWF
{
    partial class NewFileDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelRemind = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelLoca = new System.Windows.Forms.Label();
            this.textBoxLoca = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonConcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelRemind
            // 
            this.labelRemind.AutoSize = true;
            this.labelRemind.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelRemind.Location = new System.Drawing.Point(57, 30);
            this.labelRemind.Name = "labelRemind";
            this.labelRemind.Size = new System.Drawing.Size(172, 21);
            this.labelRemind.TabIndex = 0;
            this.labelRemind.Text = "您将创建PL\\0程序文件";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelName.Location = new System.Drawing.Point(57, 96);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(65, 20);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "文件名：";
            // 
            // textBoxName
            // 
            this.textBoxName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxName.Location = new System.Drawing.Point(128, 97);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(180, 25);
            this.textBoxName.TabIndex = 2;
            this.textBoxName.Text = "New.pl";
            // 
            // labelLoca
            // 
            this.labelLoca.AutoSize = true;
            this.labelLoca.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLoca.Location = new System.Drawing.Point(61, 147);
            this.labelLoca.Name = "labelLoca";
            this.labelLoca.Size = new System.Drawing.Size(59, 20);
            this.labelLoca.TabIndex = 3;
            this.labelLoca.Text = "位  置：";
            // 
            // textBoxLoca
            // 
            this.textBoxLoca.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLoca.Location = new System.Drawing.Point(128, 146);
            this.textBoxLoca.Name = "textBoxLoca";
            this.textBoxLoca.Size = new System.Drawing.Size(180, 25);
            this.textBoxLoca.TabIndex = 4;
            // 
            // buttonScan
            // 
            this.buttonScan.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonScan.Location = new System.Drawing.Point(315, 148);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(65, 23);
            this.buttonScan.TabIndex = 5;
            this.buttonScan.Text = "浏 览";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOK.Location = new System.Drawing.Point(128, 217);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(65, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "确 定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonConcel
            // 
            this.buttonConcel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonConcel.Location = new System.Drawing.Point(243, 217);
            this.buttonConcel.Name = "buttonConcel";
            this.buttonConcel.Size = new System.Drawing.Size(65, 23);
            this.buttonConcel.TabIndex = 7;
            this.buttonConcel.Text = "取 消";
            this.buttonConcel.UseVisualStyleBackColor = true;
            this.buttonConcel.Click += new System.EventHandler(this.buttonConcel_Click);
            // 
            // NewFileDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 289);
            this.Controls.Add(this.buttonConcel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.textBoxLoca);
            this.Controls.Add(this.labelLoca);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelRemind);
            this.Name = "NewFileDlg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRemind;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelLoca;
        private System.Windows.Forms.TextBox textBoxLoca;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonConcel;
    }
}