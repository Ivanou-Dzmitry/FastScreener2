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
            labelHelpHeader = new Label();
            picboxHelp = new PictureBox();
            btnCloseHelp = new Button();
            richTextBoxHelp = new RichTextBox();
            pnlFSHelpHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picboxHelp).BeginInit();
            SuspendLayout();
            // 
            // pnlFSHelpHead
            // 
            pnlFSHelpHead.BackColor = Color.YellowGreen;
            pnlFSHelpHead.Controls.Add(labelHelpHeader);
            pnlFSHelpHead.Controls.Add(picboxHelp);
            pnlFSHelpHead.Controls.Add(btnCloseHelp);
            pnlFSHelpHead.Dock = DockStyle.Top;
            pnlFSHelpHead.Location = new Point(0, 0);
            pnlFSHelpHead.Name = "pnlFSHelpHead";
            pnlFSHelpHead.Size = new Size(400, 32);
            pnlFSHelpHead.TabIndex = 0;
            // 
            // labelHelpHeader
            // 
            labelHelpHeader.Dock = DockStyle.Left;
            labelHelpHeader.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelHelpHeader.Location = new Point(32, 0);
            labelHelpHeader.Name = "labelHelpHeader";
            labelHelpHeader.Size = new Size(45, 32);
            labelHelpHeader.TabIndex = 4;
            labelHelpHeader.Text = "Help";
            labelHelpHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picboxHelp
            // 
            picboxHelp.Dock = DockStyle.Left;
            picboxHelp.Image = FS2Resources.help_icon;
            picboxHelp.Location = new Point(0, 0);
            picboxHelp.Name = "picboxHelp";
            picboxHelp.Size = new Size(32, 32);
            picboxHelp.SizeMode = PictureBoxSizeMode.CenterImage;
            picboxHelp.TabIndex = 3;
            picboxHelp.TabStop = false;
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
            // richTextBoxHelp
            // 
            richTextBoxHelp.BorderStyle = BorderStyle.None;
            richTextBoxHelp.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            richTextBoxHelp.Location = new Point(6, 32);
            richTextBoxHelp.Name = "richTextBoxHelp";
            richTextBoxHelp.ReadOnly = true;
            richTextBoxHelp.Size = new Size(387, 412);
            richTextBoxHelp.TabIndex = 1;
            richTextBoxHelp.Text = "";
            // 
            // formFSHelp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 450);
            Controls.Add(richTextBoxHelp);
            Controls.Add(pnlFSHelpHead);
            FormBorderStyle = FormBorderStyle.None;
            Name = "formFSHelp";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Form1";
            TopMost = true;
            pnlFSHelpHead.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picboxHelp).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlFSHelpHead;
        private Button btnCloseHelp;
        private RichTextBox richTextBoxHelp;
        private PictureBox picboxHelp;
        private Label labelHelpHeader;
    }
}