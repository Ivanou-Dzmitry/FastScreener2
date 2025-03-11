namespace FastScreener2
{
    partial class FS2MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelBottom = new Panel();
            labelDebug = new Label();
            panelDragBottomR = new Panel();
            panelDragBottomL = new Panel();
            panelDragTop = new Panel();
            txtbNumber = new TextBox();
            txtbName = new TextBox();
            button2 = new Button();
            button1 = new Button();
            btnFrame = new Button();
            btnArrowType = new Button();
            btnSettings = new Button();
            btnScreen = new Button();
            buttonMainMenu = new Button();
            buttonMinimizeForm = new Button();
            buttonCloseForm = new Button();
            panelDragTopR = new Panel();
            panelDragTopL = new Panel();
            panelDragLeft = new Panel();
            chbSave = new CheckBox();
            chbArrow = new CheckBox();
            chbFrame = new CheckBox();
            chbNumbers = new CheckBox();
            chbGuides = new CheckBox();
            panelRight = new Panel();
            rangeTrackBar = new VerticalRangeTrackBar();
            panelScreenArea = new Panel();
            pnlBarTop = new Panel();
            pnlBarBottom = new Panel();
            contextMenuMain = new ContextMenuStrip(components);
            mitSize01 = new ToolStripMenuItem();
            mitSize02 = new ToolStripMenuItem();
            mitSize03 = new ToolStripMenuItem();
            mitSize04 = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            mitTakeScreen = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            mitArrow = new ToolStripMenuItem();
            mitFrame = new ToolStripMenuItem();
            mitGuidlines = new ToolStripMenuItem();
            mitNumber = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            mitSaveFile = new ToolStripMenuItem();
            mitOpenFolder = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            mitSettings = new ToolStripMenuItem();
            mitHelp = new ToolStripMenuItem();
            mitExit = new ToolStripMenuItem();
            toolTipFS = new ToolTip(components);
            panelBottom.SuspendLayout();
            panelDragTop.SuspendLayout();
            panelDragLeft.SuspendLayout();
            panelRight.SuspendLayout();
            panelScreenArea.SuspendLayout();
            contextMenuMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.Transparent;
            panelBottom.Controls.Add(labelDebug);
            panelBottom.Controls.Add(panelDragBottomR);
            panelBottom.Controls.Add(panelDragBottomL);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Font = new Font("Inter", 9F);
            panelBottom.Location = new Point(0, 334);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(650, 32);
            panelBottom.TabIndex = 0;
            // 
            // labelDebug
            // 
            labelDebug.BackColor = Color.Transparent;
            labelDebug.Dock = DockStyle.Fill;
            labelDebug.Font = new Font("Inter", 9F);
            labelDebug.ForeColor = Color.DimGray;
            labelDebug.Location = new Point(60, 0);
            labelDebug.MaximumSize = new Size(0, 60);
            labelDebug.MinimumSize = new Size(240, 35);
            labelDebug.Name = "labelDebug";
            labelDebug.Size = new Size(530, 35);
            labelDebug.TabIndex = 0;
            labelDebug.Text = "Info Label, Info Label, Info Label, Info Label,Info Label";
            labelDebug.TextAlign = ContentAlignment.TopCenter;
            labelDebug.Click += labelDebug_Click;
            // 
            // panelDragBottomR
            // 
            panelDragBottomR.BackColor = Color.SlateGray;
            panelDragBottomR.Dock = DockStyle.Right;
            panelDragBottomR.Location = new Point(590, 0);
            panelDragBottomR.Name = "panelDragBottomR";
            panelDragBottomR.Size = new Size(60, 32);
            panelDragBottomR.TabIndex = 2;
            // 
            // panelDragBottomL
            // 
            panelDragBottomL.BackColor = Color.SlateGray;
            panelDragBottomL.Dock = DockStyle.Left;
            panelDragBottomL.Location = new Point(0, 0);
            panelDragBottomL.Name = "panelDragBottomL";
            panelDragBottomL.Size = new Size(60, 32);
            panelDragBottomL.TabIndex = 1;
            // 
            // panelDragTop
            // 
            panelDragTop.BackColor = Color.SlateGray;
            panelDragTop.Controls.Add(txtbNumber);
            panelDragTop.Controls.Add(txtbName);
            panelDragTop.Controls.Add(button2);
            panelDragTop.Controls.Add(button1);
            panelDragTop.Controls.Add(btnFrame);
            panelDragTop.Controls.Add(btnArrowType);
            panelDragTop.Controls.Add(btnSettings);
            panelDragTop.Controls.Add(btnScreen);
            panelDragTop.Controls.Add(buttonMainMenu);
            panelDragTop.Controls.Add(buttonMinimizeForm);
            panelDragTop.Controls.Add(buttonCloseForm);
            panelDragTop.Controls.Add(panelDragTopR);
            panelDragTop.Controls.Add(panelDragTopL);
            panelDragTop.Dock = DockStyle.Top;
            panelDragTop.Location = new Point(0, 0);
            panelDragTop.Name = "panelDragTop";
            panelDragTop.Size = new Size(650, 32);
            panelDragTop.TabIndex = 1;
            // 
            // txtbNumber
            // 
            txtbNumber.BorderStyle = BorderStyle.FixedSingle;
            txtbNumber.Dock = DockStyle.Left;
            txtbNumber.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtbNumber.Location = new Point(462, 0);
            txtbNumber.MaxLength = 4;
            txtbNumber.Name = "txtbNumber";
            txtbNumber.PlaceholderText = "Num";
            txtbNumber.Size = new Size(33, 25);
            txtbNumber.TabIndex = 16;
            txtbNumber.WordWrap = false;
            // 
            // txtbName
            // 
            txtbName.BorderStyle = BorderStyle.FixedSingle;
            txtbName.Dock = DockStyle.Left;
            txtbName.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtbName.Location = new Point(224, 0);
            txtbName.MaxLength = 32;
            txtbName.Name = "txtbName";
            txtbName.PlaceholderText = "File name (32 symbols)";
            txtbName.Size = new Size(238, 25);
            txtbName.TabIndex = 15;
            txtbName.WordWrap = false;
            // 
            // button2
            // 
            button2.BackColor = Color.DimGray;
            button2.Dock = DockStyle.Left;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(192, 0);
            button2.Name = "button2";
            button2.Size = new Size(32, 32);
            button2.TabIndex = 12;
            button2.Text = ">";
            toolTipFS.SetToolTip(button2, "Next resulution");
            button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.DimGray;
            button1.Dock = DockStyle.Left;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(160, 0);
            button1.Name = "button1";
            button1.Size = new Size(32, 32);
            button1.TabIndex = 11;
            button1.Text = "<";
            toolTipFS.SetToolTip(button1, "Previous resolution");
            button1.UseVisualStyleBackColor = false;
            // 
            // btnFrame
            // 
            btnFrame.BackColor = Color.DimGray;
            btnFrame.Dock = DockStyle.Left;
            btnFrame.FlatAppearance.BorderSize = 0;
            btnFrame.FlatStyle = FlatStyle.Flat;
            btnFrame.Image = FS2Resources.frame_unlocked_icon;
            btnFrame.Location = new Point(128, 0);
            btnFrame.Name = "btnFrame";
            btnFrame.Size = new Size(32, 32);
            btnFrame.TabIndex = 10;
            toolTipFS.SetToolTip(btnFrame, "Frame type");
            btnFrame.UseVisualStyleBackColor = false;
            // 
            // btnArrowType
            // 
            btnArrowType.BackColor = Color.DimGray;
            btnArrowType.Dock = DockStyle.Left;
            btnArrowType.FlatAppearance.BorderSize = 0;
            btnArrowType.FlatStyle = FlatStyle.Flat;
            btnArrowType.Image = FS2Resources.arrow_type01_icon;
            btnArrowType.Location = new Point(96, 0);
            btnArrowType.Name = "btnArrowType";
            btnArrowType.Size = new Size(32, 32);
            btnArrowType.TabIndex = 3;
            toolTipFS.SetToolTip(btnArrowType, "Arrow direction");
            btnArrowType.UseVisualStyleBackColor = false;
            btnArrowType.Click += btnArrowType_Click;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.DimGray;
            btnSettings.Dock = DockStyle.Right;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Image = FS2Resources.settings_icon;
            btnSettings.Location = new Point(522, 0);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(32, 32);
            btnSettings.TabIndex = 9;
            btnSettings.Text = "_";
            toolTipFS.SetToolTip(btnSettings, "Settings");
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnScreen
            // 
            btnScreen.BackColor = Color.DimGray;
            btnScreen.Dock = DockStyle.Left;
            btnScreen.FlatAppearance.BorderSize = 0;
            btnScreen.FlatStyle = FlatStyle.Flat;
            btnScreen.Image = FS2Resources.screen_icon;
            btnScreen.Location = new Point(64, 0);
            btnScreen.Name = "btnScreen";
            btnScreen.Size = new Size(32, 32);
            btnScreen.TabIndex = 2;
            toolTipFS.SetToolTip(btnScreen, "Screenshot");
            btnScreen.UseVisualStyleBackColor = false;
            btnScreen.Click += btnScreen_Click;
            // 
            // buttonMainMenu
            // 
            buttonMainMenu.BackColor = Color.DimGray;
            buttonMainMenu.Dock = DockStyle.Left;
            buttonMainMenu.FlatAppearance.BorderSize = 0;
            buttonMainMenu.FlatStyle = FlatStyle.Flat;
            buttonMainMenu.Image = FS2Resources.menu_icon;
            buttonMainMenu.Location = new Point(32, 0);
            buttonMainMenu.Name = "buttonMainMenu";
            buttonMainMenu.Size = new Size(32, 32);
            buttonMainMenu.TabIndex = 1;
            toolTipFS.SetToolTip(buttonMainMenu, "Main menu");
            buttonMainMenu.UseVisualStyleBackColor = false;
            buttonMainMenu.Click += buttonMainMenu_Click;
            // 
            // buttonMinimizeForm
            // 
            buttonMinimizeForm.BackColor = Color.DimGray;
            buttonMinimizeForm.Dock = DockStyle.Right;
            buttonMinimizeForm.FlatAppearance.BorderSize = 0;
            buttonMinimizeForm.FlatStyle = FlatStyle.Flat;
            buttonMinimizeForm.Image = FS2Resources.minimize_icon;
            buttonMinimizeForm.Location = new Point(554, 0);
            buttonMinimizeForm.Name = "buttonMinimizeForm";
            buttonMinimizeForm.Size = new Size(32, 32);
            buttonMinimizeForm.TabIndex = 7;
            toolTipFS.SetToolTip(buttonMinimizeForm, "Minimize");
            buttonMinimizeForm.UseVisualStyleBackColor = false;
            buttonMinimizeForm.Click += buttonMinimizeForm_Click;
            // 
            // buttonCloseForm
            // 
            buttonCloseForm.BackColor = Color.DimGray;
            buttonCloseForm.Dock = DockStyle.Right;
            buttonCloseForm.FlatAppearance.BorderSize = 0;
            buttonCloseForm.FlatStyle = FlatStyle.Flat;
            buttonCloseForm.Image = FS2Resources.close_icon;
            buttonCloseForm.Location = new Point(586, 0);
            buttonCloseForm.Name = "buttonCloseForm";
            buttonCloseForm.Size = new Size(32, 32);
            buttonCloseForm.TabIndex = 8;
            toolTipFS.SetToolTip(buttonCloseForm, "Close");
            buttonCloseForm.UseVisualStyleBackColor = false;
            buttonCloseForm.Click += buttonCloseForm_Click;
            buttonCloseForm.MouseEnter += buttonCloseForm_MouseEnter;
            buttonCloseForm.MouseLeave += buttonCloseForm_MouseLeave;
            // 
            // panelDragTopR
            // 
            panelDragTopR.BackColor = Color.SlateGray;
            panelDragTopR.Dock = DockStyle.Right;
            panelDragTopR.Location = new Point(618, 0);
            panelDragTopR.Name = "panelDragTopR";
            panelDragTopR.Size = new Size(32, 32);
            panelDragTopR.TabIndex = 13;
            // 
            // panelDragTopL
            // 
            panelDragTopL.BackColor = Color.SlateGray;
            panelDragTopL.Dock = DockStyle.Left;
            panelDragTopL.Location = new Point(0, 0);
            panelDragTopL.Name = "panelDragTopL";
            panelDragTopL.Size = new Size(32, 32);
            panelDragTopL.TabIndex = 14;
            // 
            // panelDragLeft
            // 
            panelDragLeft.BackColor = Color.SlateGray;
            panelDragLeft.Controls.Add(chbSave);
            panelDragLeft.Controls.Add(chbArrow);
            panelDragLeft.Controls.Add(chbFrame);
            panelDragLeft.Controls.Add(chbNumbers);
            panelDragLeft.Controls.Add(chbGuides);
            panelDragLeft.Dock = DockStyle.Left;
            panelDragLeft.Location = new Point(0, 32);
            panelDragLeft.Name = "panelDragLeft";
            panelDragLeft.Size = new Size(32, 302);
            panelDragLeft.TabIndex = 2;
            // 
            // chbSave
            // 
            chbSave.Appearance = Appearance.Button;
            chbSave.BackColor = Color.DimGray;
            chbSave.Dock = DockStyle.Bottom;
            chbSave.FlatAppearance.BorderSize = 0;
            chbSave.FlatStyle = FlatStyle.Flat;
            chbSave.Image = FS2Resources.save_icon;
            chbSave.Location = new Point(0, 142);
            chbSave.Name = "chbSave";
            chbSave.Size = new Size(32, 32);
            chbSave.TabIndex = 8;
            toolTipFS.SetToolTip(chbSave, "Save to file toggle");
            chbSave.UseVisualStyleBackColor = false;
            chbSave.Click += chbSave_Click;
            // 
            // chbArrow
            // 
            chbArrow.Appearance = Appearance.Button;
            chbArrow.BackColor = Color.DimGray;
            chbArrow.Dock = DockStyle.Bottom;
            chbArrow.FlatAppearance.BorderSize = 0;
            chbArrow.FlatStyle = FlatStyle.Flat;
            chbArrow.Image = FS2Resources.arrow_icon;
            chbArrow.Location = new Point(0, 174);
            chbArrow.Name = "chbArrow";
            chbArrow.Size = new Size(32, 32);
            chbArrow.TabIndex = 7;
            toolTipFS.SetToolTip(chbArrow, "Arrow toggle");
            chbArrow.UseVisualStyleBackColor = false;
            chbArrow.Click += chbArrow_Click;
            // 
            // chbFrame
            // 
            chbFrame.Appearance = Appearance.Button;
            chbFrame.BackColor = Color.DimGray;
            chbFrame.Dock = DockStyle.Bottom;
            chbFrame.FlatAppearance.BorderSize = 0;
            chbFrame.FlatStyle = FlatStyle.Flat;
            chbFrame.Image = FS2Resources.frame_icon;
            chbFrame.Location = new Point(0, 206);
            chbFrame.Name = "chbFrame";
            chbFrame.Size = new Size(32, 32);
            chbFrame.TabIndex = 6;
            toolTipFS.SetToolTip(chbFrame, "Frame toggle");
            chbFrame.UseVisualStyleBackColor = false;
            chbFrame.Click += chbFrame_Click;
            // 
            // chbNumbers
            // 
            chbNumbers.Appearance = Appearance.Button;
            chbNumbers.BackColor = Color.DimGray;
            chbNumbers.Dock = DockStyle.Bottom;
            chbNumbers.FlatAppearance.BorderSize = 0;
            chbNumbers.FlatStyle = FlatStyle.Flat;
            chbNumbers.Image = FS2Resources.number_icon;
            chbNumbers.Location = new Point(0, 238);
            chbNumbers.Name = "chbNumbers";
            chbNumbers.Size = new Size(32, 32);
            chbNumbers.TabIndex = 5;
            toolTipFS.SetToolTip(chbNumbers, "Numbers toggle");
            chbNumbers.UseVisualStyleBackColor = false;
            chbNumbers.Click += chbNumbers_Click;
            // 
            // chbGuides
            // 
            chbGuides.Appearance = Appearance.Button;
            chbGuides.BackColor = Color.DimGray;
            chbGuides.Dock = DockStyle.Bottom;
            chbGuides.FlatAppearance.BorderSize = 0;
            chbGuides.FlatStyle = FlatStyle.Flat;
            chbGuides.Image = FS2Resources.guides_icon;
            chbGuides.Location = new Point(0, 270);
            chbGuides.Name = "chbGuides";
            chbGuides.Size = new Size(32, 32);
            chbGuides.TabIndex = 4;
            toolTipFS.SetToolTip(chbGuides, "Guides toggle");
            chbGuides.UseVisualStyleBackColor = false;
            chbGuides.CheckedChanged += chbGuides_CheckedChanged;
            chbGuides.Click += chbGuides_Click;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.Transparent;
            panelRight.Controls.Add(rangeTrackBar);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(618, 32);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(32, 302);
            panelRight.TabIndex = 3;
            // 
            // rangeTrackBar
            // 
            rangeTrackBar.BackColor = Color.DimGray;
            rangeTrackBar.Dock = DockStyle.Fill;
            rangeTrackBar.Location = new Point(0, 0);
            rangeTrackBar.LowerValue = 0;
            rangeTrackBar.Maximum = 100;
            rangeTrackBar.Minimum = 0;
            rangeTrackBar.Name = "rangeTrackBar";
            rangeTrackBar.RangeColor = Color.Gray;
            rangeTrackBar.Size = new Size(32, 302);
            rangeTrackBar.TabIndex = 0;
            rangeTrackBar.Text = "verticalRangeTrackBar1";
            rangeTrackBar.ThumbBorderColor = Color.Black;
            rangeTrackBar.ThumbColor = Color.White;
            toolTipFS.SetToolTip(rangeTrackBar, "Frame size");
            rangeTrackBar.TrackColor = Color.Orange;
            rangeTrackBar.UpperValue = 100;
            rangeTrackBar.MouseMove += rangeTrackBar_MouseMove_1;
            // 
            // panelScreenArea
            // 
            panelScreenArea.BorderStyle = BorderStyle.FixedSingle;
            panelScreenArea.Controls.Add(pnlBarTop);
            panelScreenArea.Controls.Add(pnlBarBottom);
            panelScreenArea.Dock = DockStyle.Fill;
            panelScreenArea.Location = new Point(32, 32);
            panelScreenArea.Name = "panelScreenArea";
            panelScreenArea.Size = new Size(586, 302);
            panelScreenArea.TabIndex = 4;
            // 
            // pnlBarTop
            // 
            pnlBarTop.BackColor = Color.Black;
            pnlBarTop.Dock = DockStyle.Top;
            pnlBarTop.Location = new Point(0, 0);
            pnlBarTop.Name = "pnlBarTop";
            pnlBarTop.Size = new Size(584, 0);
            pnlBarTop.TabIndex = 1;
            // 
            // pnlBarBottom
            // 
            pnlBarBottom.BackColor = Color.Black;
            pnlBarBottom.Dock = DockStyle.Bottom;
            pnlBarBottom.Location = new Point(0, 300);
            pnlBarBottom.Name = "pnlBarBottom";
            pnlBarBottom.Size = new Size(584, 0);
            pnlBarBottom.TabIndex = 0;
            // 
            // contextMenuMain
            // 
            contextMenuMain.Items.AddRange(new ToolStripItem[] { mitSize01, mitSize02, mitSize03, mitSize04, toolStripMenuItem1, mitTakeScreen, toolStripMenuItem3, mitArrow, mitFrame, mitGuidlines, mitNumber, toolStripMenuItem2, mitSaveFile, mitOpenFolder, toolStripMenuItem4, mitSettings, mitHelp, mitExit });
            contextMenuMain.Name = "contextMenuMain";
            contextMenuMain.Size = new Size(192, 336);
            // 
            // mitSize01
            // 
            mitSize01.Name = "mitSize01";
            mitSize01.Size = new Size(191, 22);
            mitSize01.Text = "Size1";
            // 
            // mitSize02
            // 
            mitSize02.Name = "mitSize02";
            mitSize02.Size = new Size(191, 22);
            mitSize02.Text = "Size2";
            // 
            // mitSize03
            // 
            mitSize03.Name = "mitSize03";
            mitSize03.Size = new Size(191, 22);
            mitSize03.Text = "Size3";
            // 
            // mitSize04
            // 
            mitSize04.Name = "mitSize04";
            mitSize04.Size = new Size(191, 22);
            mitSize04.Text = "Size4";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(188, 6);
            // 
            // mitTakeScreen
            // 
            mitTakeScreen.Image = FS2Resources.screen_icon;
            mitTakeScreen.Name = "mitTakeScreen";
            mitTakeScreen.ShortcutKeys = Keys.F4;
            mitTakeScreen.Size = new Size(191, 22);
            mitTakeScreen.Text = "Screen";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(188, 6);
            // 
            // mitArrow
            // 
            mitArrow.Name = "mitArrow";
            mitArrow.Size = new Size(191, 22);
            mitArrow.Text = "Arrow";
            mitArrow.Click += mitArrow_Click;
            // 
            // mitFrame
            // 
            mitFrame.Name = "mitFrame";
            mitFrame.Size = new Size(191, 22);
            mitFrame.Text = "Frame";
            mitFrame.Click += mitFrame_Click;
            // 
            // mitGuidlines
            // 
            mitGuidlines.Name = "mitGuidlines";
            mitGuidlines.Size = new Size(191, 22);
            mitGuidlines.Text = "Guidlines";
            mitGuidlines.Click += mitGuidlines_Click;
            // 
            // mitNumber
            // 
            mitNumber.Name = "mitNumber";
            mitNumber.Size = new Size(191, 22);
            mitNumber.Text = "Number";
            mitNumber.Click += mitNumber_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(188, 6);
            // 
            // mitSaveFile
            // 
            mitSaveFile.Name = "mitSaveFile";
            mitSaveFile.Size = new Size(191, 22);
            mitSaveFile.Text = "Save to File";
            mitSaveFile.Click += mitSaveFile_Click;
            // 
            // mitOpenFolder
            // 
            mitOpenFolder.Name = "mitOpenFolder";
            mitOpenFolder.Size = new Size(191, 22);
            mitOpenFolder.Text = "Open Folder with Files";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(188, 6);
            // 
            // mitSettings
            // 
            mitSettings.Image = FS2Resources.settings_icon;
            mitSettings.Name = "mitSettings";
            mitSettings.Size = new Size(191, 22);
            mitSettings.Text = "Settings";
            mitSettings.Click += mitSettings_Click;
            // 
            // mitHelp
            // 
            mitHelp.Name = "mitHelp";
            mitHelp.Size = new Size(191, 22);
            mitHelp.Text = "Help";
            mitHelp.Click += mitHelp_Click;
            // 
            // mitExit
            // 
            mitExit.Name = "mitExit";
            mitExit.ShortcutKeys = Keys.Alt | Keys.F4;
            mitExit.Size = new Size(191, 22);
            mitExit.Text = "Exit";
            mitExit.Click += buttonCloseForm_Click;
            // 
            // FS2MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 366);
            Controls.Add(panelScreenArea);
            Controls.Add(panelRight);
            Controls.Add(panelDragLeft);
            Controls.Add(panelDragTop);
            Controls.Add(panelBottom);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FS2MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FastScreener 2";
            TopMost = true;
            Shown += FS2MainForm_Shown;
            panelBottom.ResumeLayout(false);
            panelDragTop.ResumeLayout(false);
            panelDragTop.PerformLayout();
            panelDragLeft.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            panelScreenArea.ResumeLayout(false);
            contextMenuMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBottom;
        private Panel panelDragTop;
        private Panel panelDragLeft;
        private Panel panelRight;
        private Panel panelScreenArea;
        private ContextMenuStrip contextMenuMain;
        private ToolStripMenuItem mitSize01;
        private ToolStripMenuItem mitSize02;
        private ToolStripMenuItem mitSize03;
        private ToolStripMenuItem mitSize04;
        private ToolStripMenuItem mitTakeScreen;
        private Label labelDebug;
        private Button buttonCloseForm;
        private Button buttonMinimizeForm;
        private Button buttonMainMenu;
        private ToolStripSeparator toolStripMenuItem1;
        private Button btnScreen;
        private Button btnArrowType;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem mitHelp;
        private ToolStripMenuItem mitExit;
        private ToolStripMenuItem mitSettings;
        private ToolStripMenuItem mitArrow;
        private ToolStripMenuItem mitNumber;
        private ToolStripMenuItem mitFrame;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem mitGuidlines;
        private ToolStripMenuItem mitSaveFile;
        private ToolStripMenuItem mitOpenFolder;
        private ToolStripSeparator toolStripMenuItem4;
        private CheckBox chbGuides;
        private CheckBox chbNumbers;
        private CheckBox chbFrame;
        private Button btnSettings;
        private CheckBox chbArrow;
        private Panel panelDragBottomL;
        private Panel panelDragBottomR;
        private Button btnFrame;
        private Button button2;
        private Button button1;
        private Panel panelDragTopR;
        private CheckBox chbSave;
        private VerticalRangeTrackBar rangeTrackBar;
        private Panel panelDragTopL;
        private Panel pnlBarTop;
        private Panel pnlBarBottom;
        private ToolTip toolTipFS;
        private TextBox txtbName;
        private TextBox txtbNumber;
    }
}
