namespace FastScreener2
{
    partial class formFS2Settings
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
            pgSettings = new PropertyGrid();
            lboxSetCat = new ListBox();
            btnOK = new Button();
            pnlSetBottom = new Panel();
            labelSetDebug = new Label();
            pnlSetHeader = new Panel();
            labelSetHeader = new Label();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            pnlSetBottom.SuspendLayout();
            pnlSetHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pgSettings
            // 
            pgSettings.Location = new Point(124, 32);
            pgSettings.Name = "pgSettings";
            pgSettings.Size = new Size(409, 315);
            pgSettings.TabIndex = 0;
            // 
            // lboxSetCat
            // 
            lboxSetCat.BackColor = Color.Silver;
            lboxSetCat.BorderStyle = BorderStyle.None;
            lboxSetCat.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lboxSetCat.ItemHeight = 17;
            lboxSetCat.Items.AddRange(new object[] { "Arrow", "Bar", "Frame", "Guides", "Numbers", "Sizes" });
            lboxSetCat.Location = new Point(0, 32);
            lboxSetCat.Name = "lboxSetCat";
            lboxSetCat.Size = new Size(120, 323);
            lboxSetCat.TabIndex = 1;
            lboxSetCat.Click += lboxSetCat_Click;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(456, 16);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(64, 32);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // pnlSetBottom
            // 
            pnlSetBottom.BackColor = Color.SlateGray;
            pnlSetBottom.Controls.Add(labelSetDebug);
            pnlSetBottom.Controls.Add(btnOK);
            pnlSetBottom.Dock = DockStyle.Bottom;
            pnlSetBottom.Location = new Point(0, 347);
            pnlSetBottom.Name = "pnlSetBottom";
            pnlSetBottom.Size = new Size(534, 64);
            pnlSetBottom.TabIndex = 3;
            // 
            // labelSetDebug
            // 
            labelSetDebug.AutoSize = true;
            labelSetDebug.Location = new Point(8, 17);
            labelSetDebug.Name = "labelSetDebug";
            labelSetDebug.Size = new Size(38, 15);
            labelSetDebug.TabIndex = 3;
            labelSetDebug.Text = "label1";
            // 
            // pnlSetHeader
            // 
            pnlSetHeader.BackColor = Color.SteelBlue;
            pnlSetHeader.Controls.Add(labelSetHeader);
            pnlSetHeader.Controls.Add(button1);
            pnlSetHeader.Controls.Add(pictureBox1);
            pnlSetHeader.Dock = DockStyle.Top;
            pnlSetHeader.Location = new Point(0, 0);
            pnlSetHeader.Name = "pnlSetHeader";
            pnlSetHeader.Size = new Size(534, 32);
            pnlSetHeader.TabIndex = 4;
            // 
            // labelSetHeader
            // 
            labelSetHeader.Dock = DockStyle.Left;
            labelSetHeader.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelSetHeader.Location = new Point(32, 0);
            labelSetHeader.Name = "labelSetHeader";
            labelSetHeader.Size = new Size(128, 32);
            labelSetHeader.TabIndex = 1;
            labelSetHeader.Text = "Settings";
            labelSetHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Image = FS2Resources.close_icon;
            button1.Location = new Point(502, 0);
            button1.Name = "button1";
            button1.Size = new Size(32, 32);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnOK_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = FS2Resources.settings_icon;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(32, 32);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // formFS2Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 411);
            Controls.Add(pnlSetHeader);
            Controls.Add(pnlSetBottom);
            Controls.Add(lboxSetCat);
            Controls.Add(pgSettings);
            FormBorderStyle = FormBorderStyle.None;
            Name = "formFS2Settings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            TopMost = true;
            pnlSetBottom.ResumeLayout(false);
            pnlSetBottom.PerformLayout();
            pnlSetHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid pgSettings;
        private ListBox lboxSetCat;
        private Button btnOK;
        private Panel pnlSetBottom;
        private Label labelSetDebug;
        private Panel pnlSetHeader;
        private Button button1;
        private Label labelSetHeader;
        private PictureBox pictureBox1;
    }
}