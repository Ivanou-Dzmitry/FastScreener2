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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formFS2Settings));
            pgSettings = new PropertyGrid();
            lboxSetCat = new ListBox();
            btnOK = new Button();
            pnlSetBottom = new Panel();
            lblProfile = new Label();
            cmbProfiles = new ComboBox();
            btnLoadProfile = new Button();
            btnSaveProfileAs = new Button();
            btnDeleteProfile = new Button();
            btnReset = new Button();
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
            pgSettings.Location = new Point(180, 54);
            pgSettings.Margin = new Padding(4);
            pgSettings.Name = "pgSettings";
            pgSettings.Size = new Size(564, 490);
            pgSettings.TabIndex = 0;
            // 
            // lboxSetCat
            // 
            lboxSetCat.BackColor = Color.Gainsboro;
            lboxSetCat.BorderStyle = BorderStyle.FixedSingle;
            lboxSetCat.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lboxSetCat.ItemHeight = 28;
            lboxSetCat.Items.AddRange(new object[] { "Appearance", "Arrow", "Bar", "File", "Frame", "Guides", "Numbers", "Sizes", "Watermark" });
            lboxSetCat.Location = new Point(20, 94);
            lboxSetCat.Margin = new Padding(4);
            lboxSetCat.Name = "lboxSetCat";
            lboxSetCat.Size = new Size(152, 450);
            lboxSetCat.Sorted = true;
            lboxSetCat.TabIndex = 1;
            lboxSetCat.Click += lboxSetCat_Click;
            // 
            // btnOK
            // 
            btnOK.BackColor = Color.WhiteSmoke;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Location = new Point(591, 70);
            btnOK.Margin = new Padding(4);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(153, 52);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // pnlSetBottom
            // 
            pnlSetBottom.BackColor = SystemColors.Control;
            pnlSetBottom.Controls.Add(lblProfile);
            pnlSetBottom.Controls.Add(cmbProfiles);
            pnlSetBottom.Controls.Add(btnLoadProfile);
            pnlSetBottom.Controls.Add(btnSaveProfileAs);
            pnlSetBottom.Controls.Add(btnDeleteProfile);
            pnlSetBottom.Controls.Add(btnReset);
            pnlSetBottom.Controls.Add(btnOK);
            pnlSetBottom.Dock = DockStyle.Bottom;
            pnlSetBottom.Location = new Point(0, 552);
            pnlSetBottom.Margin = new Padding(4);
            pnlSetBottom.Name = "pnlSetBottom";
            pnlSetBottom.Size = new Size(764, 164);
            pnlSetBottom.TabIndex = 3;
            // 
            // lblProfile
            // 
            lblProfile.AutoSize = true;
            lblProfile.Location = new Point(232, 32);
            lblProfile.Margin = new Padding(4);
            lblProfile.Name = "lblProfile";
            lblProfile.Size = new Size(66, 25);
            lblProfile.TabIndex = 4;
            lblProfile.Text = "Profile:";
            // 
            // cmbProfiles
            // 
            cmbProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProfiles.Location = new Point(300, 29);
            cmbProfiles.Margin = new Padding(4);
            cmbProfiles.Name = "cmbProfiles";
            cmbProfiles.Size = new Size(248, 33);
            cmbProfiles.TabIndex = 5;
            // 
            // btnLoadProfile
            // 
            btnLoadProfile.BackColor = Color.WhiteSmoke;
            btnLoadProfile.FlatStyle = FlatStyle.Flat;
            btnLoadProfile.Location = new Point(232, 70);
            btnLoadProfile.Margin = new Padding(4);
            btnLoadProfile.Name = "btnLoadProfile";
            btnLoadProfile.Size = new Size(100, 52);
            btnLoadProfile.TabIndex = 6;
            btnLoadProfile.Text = "Load";
            btnLoadProfile.UseVisualStyleBackColor = false;
            btnLoadProfile.Click += btnLoadProfile_Click;
            // 
            // btnSaveProfileAs
            // 
            btnSaveProfileAs.BackColor = Color.WhiteSmoke;
            btnSaveProfileAs.FlatStyle = FlatStyle.Flat;
            btnSaveProfileAs.Location = new Point(340, 70);
            btnSaveProfileAs.Margin = new Padding(4);
            btnSaveProfileAs.Name = "btnSaveProfileAs";
            btnSaveProfileAs.Size = new Size(100, 52);
            btnSaveProfileAs.TabIndex = 7;
            btnSaveProfileAs.Text = "Save as...";
            btnSaveProfileAs.UseVisualStyleBackColor = false;
            btnSaveProfileAs.Click += btnSaveProfileAs_Click;
            // 
            // btnDeleteProfile
            // 
            btnDeleteProfile.BackColor = Color.WhiteSmoke;
            btnDeleteProfile.FlatStyle = FlatStyle.Flat;
            btnDeleteProfile.Location = new Point(448, 70);
            btnDeleteProfile.Margin = new Padding(4);
            btnDeleteProfile.Name = "btnDeleteProfile";
            btnDeleteProfile.Size = new Size(100, 52);
            btnDeleteProfile.TabIndex = 8;
            btnDeleteProfile.Text = "Delete";
            btnDeleteProfile.UseVisualStyleBackColor = false;
            btnDeleteProfile.Click += btnDeleteProfile_Click;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.DimGray;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Location = new Point(20, 70);
            btnReset.Margin = new Padding(4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(153, 52);
            btnReset.TabIndex = 3;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // pnlSetHeader
            // 
            pnlSetHeader.BackColor = Color.SteelBlue;
            pnlSetHeader.Controls.Add(labelSetHeader);
            pnlSetHeader.Controls.Add(button1);
            pnlSetHeader.Controls.Add(pictureBox1);
            pnlSetHeader.Dock = DockStyle.Top;
            pnlSetHeader.Location = new Point(0, 0);
            pnlSetHeader.Margin = new Padding(4);
            pnlSetHeader.Name = "pnlSetHeader";
            pnlSetHeader.Size = new Size(764, 48);
            pnlSetHeader.TabIndex = 4;
            // 
            // labelSetHeader
            // 
            labelSetHeader.Dock = DockStyle.Left;
            labelSetHeader.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelSetHeader.Location = new Point(46, 0);
            labelSetHeader.Margin = new Padding(4, 0, 4, 0);
            labelSetHeader.Name = "labelSetHeader";
            labelSetHeader.Size = new Size(98, 48);
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
            button1.Location = new Point(718, 0);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(46, 48);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnOK_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = FS2Resources.settings_icon;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(46, 48);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // formFS2Settings
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(764, 716);
            Controls.Add(pnlSetHeader);
            Controls.Add(pnlSetBottom);
            Controls.Add(lboxSetCat);
            Controls.Add(pgSettings);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "formFS2Settings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            TopMost = true;
            Shown += formFS2Settings_Shown;
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
        private Panel pnlSetHeader;
        private Button button1;
        private Label labelSetHeader;
        private PictureBox pictureBox1;
        private Button btnReset;
        private Label lblProfile;
        private ComboBox cmbProfiles;
        private Button btnLoadProfile;
        private Button btnSaveProfileAs;
        private Button btnDeleteProfile;
    }
}