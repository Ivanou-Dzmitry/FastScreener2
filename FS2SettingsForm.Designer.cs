namespace FastScreener2
{
    partial class FS2SettingsForm
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
            panel1 = new Panel();
            labelSetDebug = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pgSettings
            // 
            pgSettings.Location = new Point(124, 0);
            pgSettings.Name = "pgSettings";
            pgSettings.Size = new Size(409, 353);
            pgSettings.TabIndex = 0;
            // 
            // lboxSetCat
            // 
            lboxSetCat.BorderStyle = BorderStyle.None;
            lboxSetCat.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lboxSetCat.FormattingEnabled = true;
            lboxSetCat.ItemHeight = 25;
            lboxSetCat.Items.AddRange(new object[] { "Arrow", "Frame", "Guides", "Numbers", "Sizes" });
            lboxSetCat.Location = new Point(0, 0);
            lboxSetCat.Name = "lboxSetCat";
            lboxSetCat.Size = new Size(120, 350);
            lboxSetCat.TabIndex = 1;
            lboxSetCat.Click += lboxSetCat_Click;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(451, 17);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Silver;
            panel1.Controls.Add(labelSetDebug);
            panel1.Controls.Add(btnOK);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 356);
            panel1.Name = "panel1";
            panel1.Size = new Size(534, 55);
            panel1.TabIndex = 3;
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
            // FS2SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 411);
            Controls.Add(panel1);
            Controls.Add(lboxSetCat);
            Controls.Add(pgSettings);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FS2SettingsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            TopMost = true;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid pgSettings;
        private ListBox lboxSetCat;
        private Button btnOK;
        private Panel panel1;
        private Label labelSetDebug;
    }
}