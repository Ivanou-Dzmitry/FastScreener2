namespace FastScreener2
{
    partial class formFSHelp
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
            pnlFSHelpHead = new Panel();
            btnCloseHelp = new Button();
            richTextBox1 = new RichTextBox();
            pnlFSHelpHead.SuspendLayout();
            SuspendLayout();
            // 
            // pnlFSHelpHead
            // 
            pnlFSHelpHead.BackColor = Color.SlateGray;
            pnlFSHelpHead.Controls.Add(btnCloseHelp);
            pnlFSHelpHead.Dock = DockStyle.Top;
            pnlFSHelpHead.Location = new Point(0, 0);
            pnlFSHelpHead.Name = "pnlFSHelpHead";
            pnlFSHelpHead.Size = new Size(400, 32);
            pnlFSHelpHead.TabIndex = 0;
            // 
            // btnCloseHelp
            // 
            btnCloseHelp.Dock = DockStyle.Right;
            btnCloseHelp.FlatAppearance.BorderSize = 0;
            btnCloseHelp.FlatStyle = FlatStyle.Flat;
            btnCloseHelp.Image = FS2Resources.close_icon;
            btnCloseHelp.Location = new Point(368, 0);
            btnCloseHelp.Name = "btnCloseHelp";
            btnCloseHelp.Size = new Size(32, 32);
            btnCloseHelp.TabIndex = 1;
            btnCloseHelp.UseVisualStyleBackColor = true;
            btnCloseHelp.Click += btnCloseHelp_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(0, 32);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(400, 418);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // formFSHelp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 450);
            Controls.Add(richTextBox1);
            Controls.Add(pnlFSHelpHead);
            FormBorderStyle = FormBorderStyle.None;
            Name = "formFSHelp";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Form1";
            pnlFSHelpHead.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlFSHelpHead;
        private Button btnCloseHelp;
        private RichTextBox richTextBox1;
    }
}