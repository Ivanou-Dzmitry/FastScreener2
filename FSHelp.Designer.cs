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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formFSHelp));
            pnlFSHelpHead = new Panel();
            labelHelpHeader = new Label();
            picboxHelp = new PictureBox();
            btnCloseHelp = new Button();
            richTextBoxHelp = new RichTextBox();
            panelMainHelp = new Panel();
            pnlFSHelpHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picboxHelp).BeginInit();
            panelMainHelp.SuspendLayout();
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
            pnlFSHelpHead.Margin = new Padding(4);
            pnlFSHelpHead.Name = "pnlFSHelpHead";
            pnlFSHelpHead.Size = new Size(600, 52);
            pnlFSHelpHead.TabIndex = 0;
            // 
            // labelHelpHeader
            // 
            labelHelpHeader.Dock = DockStyle.Left;
            labelHelpHeader.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelHelpHeader.Location = new Point(46, 0);
            labelHelpHeader.Margin = new Padding(4, 0, 4, 0);
            labelHelpHeader.Name = "labelHelpHeader";
            labelHelpHeader.Size = new Size(64, 52);
            labelHelpHeader.TabIndex = 4;
            labelHelpHeader.Text = "Help";
            labelHelpHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picboxHelp
            // 
            picboxHelp.Dock = DockStyle.Left;
            picboxHelp.Image = FS2Resources.help_icon;
            picboxHelp.Location = new Point(0, 0);
            picboxHelp.Margin = new Padding(4);
            picboxHelp.Name = "picboxHelp";
            picboxHelp.Size = new Size(46, 52);
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
            btnCloseHelp.Location = new Point(554, 0);
            btnCloseHelp.Margin = new Padding(4);
            btnCloseHelp.Name = "btnCloseHelp";
            btnCloseHelp.Size = new Size(46, 52);
            btnCloseHelp.TabIndex = 1;
            btnCloseHelp.UseVisualStyleBackColor = true;
            btnCloseHelp.Click += btnCloseHelp_Click;
            // 
            // richTextBoxHelp
            // 
            richTextBoxHelp.BorderStyle = BorderStyle.None;
            richTextBoxHelp.Dock = DockStyle.Fill;
            richTextBoxHelp.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            richTextBoxHelp.Location = new Point(20, 0);
            richTextBoxHelp.Margin = new Padding(4);
            richTextBoxHelp.Name = "richTextBoxHelp";
            richTextBoxHelp.ReadOnly = true;
            richTextBoxHelp.Size = new Size(580, 698);
            richTextBoxHelp.TabIndex = 1;
            richTextBoxHelp.Text = "";
            // 
            // panelMainHelp
            // 
            panelMainHelp.Controls.Add(richTextBoxHelp);
            panelMainHelp.Dock = DockStyle.Fill;
            panelMainHelp.Location = new Point(0, 52);
            panelMainHelp.Name = "panelMainHelp";
            panelMainHelp.Padding = new Padding(20, 0, 0, 0);
            panelMainHelp.Size = new Size(600, 698);
            panelMainHelp.TabIndex = 2;
            // 
            // formFSHelp
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(600, 750);
            Controls.Add(panelMainHelp);
            Controls.Add(pnlFSHelpHead);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "formFSHelp";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Help";
            TopMost = true;
            Shown += formFSHelp_Shown;
            pnlFSHelpHead.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picboxHelp).EndInit();
            panelMainHelp.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlFSHelpHead;
        private Button btnCloseHelp;
        private RichTextBox richTextBoxHelp;
        private PictureBox picboxHelp;
        private Label labelHelpHeader;
        private Panel panelMainHelp;
    }
}