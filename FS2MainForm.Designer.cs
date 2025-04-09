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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FS2MainForm));
            panelBottom = new Panel();
            labelDebug = new BlurOutlineLabel();
            panelDragBottomR = new Panel();
            panelDragBottomL = new Panel();
            panelDragTop = new Panel();
            txtbName = new TextBox();
            splitter1 = new Splitter();
            btnNextRes = new Button();
            btnFrameType = new Button();
            btnArrowType = new Button();
            btnSettings = new Button();
            btnScreen = new Button();
            buttonMainMenu = new Button();
            buttonMinimizeForm = new Button();
            buttonCloseForm = new Button();
            panelDragTopR = new Panel();
            panelDragTopL = new Panel();
            panelDragLeft = new Panel();
            chbArrow = new CheckBox();
            chbFrame = new CheckBox();
            chbNumbers = new CheckBox();
            chbText = new CheckBox();
            chbSave = new CheckBox();
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
            mitMax = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            mitTakeScreen = new ToolStripMenuItem();
            mitFulscreen = new ToolStripMenuItem();
            mitClear = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            mitArrow = new ToolStripMenuItem();
            mitFrame = new ToolStripMenuItem();
            mitNumber = new ToolStripMenuItem();
            mitText = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            mitGuidlines = new ToolStripMenuItem();
            mitSaveFile = new ToolStripMenuItem();
            mitOpenFolder = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            mitShowInfo = new ToolStripMenuItem();
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
            panelBottom.Location = new Point(0, 372);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(619, 35);
            panelBottom.TabIndex = 0;
            // 
            // labelDebug
            // 
            labelDebug.BlurAmount = 2;
            labelDebug.Dock = DockStyle.Fill;
            labelDebug.Font = new Font("Inter", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelDebug.ForeColor = Color.WhiteSmoke;
            labelDebug.Location = new Point(57, 0);
            labelDebug.Name = "labelDebug";
            labelDebug.OutlineColor = Color.Black;
            labelDebug.OutlineWidth = 2F;
            labelDebug.Size = new Size(505, 35);
            labelDebug.TabIndex = 2;
            labelDebug.Text = "labelDebug with info";
            // 
            // panelDragBottomR
            // 
            panelDragBottomR.BackColor = Color.SlateGray;
            panelDragBottomR.Dock = DockStyle.Right;
            panelDragBottomR.Location = new Point(562, 0);
            panelDragBottomR.Name = "panelDragBottomR";
            panelDragBottomR.Size = new Size(57, 35);
            panelDragBottomR.TabIndex = 2;
            // 
            // panelDragBottomL
            // 
            panelDragBottomL.BackColor = Color.SlateGray;
            panelDragBottomL.Dock = DockStyle.Left;
            panelDragBottomL.Location = new Point(0, 0);
            panelDragBottomL.Name = "panelDragBottomL";
            panelDragBottomL.Size = new Size(57, 35);
            panelDragBottomL.TabIndex = 1;
            // 
            // panelDragTop
            // 
            panelDragTop.BackColor = Color.SlateGray;
            panelDragTop.Controls.Add(txtbName);
            panelDragTop.Controls.Add(splitter1);
            panelDragTop.Controls.Add(btnNextRes);
            panelDragTop.Controls.Add(btnFrameType);
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
            panelDragTop.Size = new Size(619, 35);
            panelDragTop.TabIndex = 1;
            // 
            // txtbName
            // 
            txtbName.BackColor = Color.AliceBlue;
            txtbName.BorderStyle = BorderStyle.FixedSingle;
            txtbName.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtbName.Location = new Point(189, 5);
            txtbName.MaxLength = 35;
            txtbName.Name = "txtbName";
            txtbName.PlaceholderText = "File name (35 symbols, optional)";
            txtbName.Size = new Size(285, 25);
            txtbName.TabIndex = 6;
            toolTipFS.SetToolTip(txtbName, "File name");
            txtbName.WordWrap = false;
            // 
            // splitter1
            // 
            splitter1.BackColor = Color.SlateGray;
            splitter1.Location = new Point(186, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(4, 35);
            splitter1.TabIndex = 4;
            splitter1.TabStop = false;
            // 
            // btnNextRes
            // 
            btnNextRes.BackColor = Color.Gray;
            btnNextRes.Dock = DockStyle.Left;
            btnNextRes.FlatAppearance.BorderSize = 0;
            btnNextRes.FlatStyle = FlatStyle.Flat;
            btnNextRes.Image = FS2Resources.res_cycle_icon;
            btnNextRes.Location = new Point(155, 0);
            btnNextRes.Name = "btnNextRes";
            btnNextRes.Size = new Size(31, 35);
            btnNextRes.TabIndex = 5;
            toolTipFS.SetToolTip(btnNextRes, "Resolution cycle (Ctrl+Right arrow)");
            btnNextRes.UseVisualStyleBackColor = false;
            btnNextRes.Click += btnNextRes_Click;
            // 
            // btnFrameType
            // 
            btnFrameType.BackColor = Color.Gray;
            btnFrameType.Dock = DockStyle.Left;
            btnFrameType.FlatAppearance.BorderSize = 0;
            btnFrameType.FlatStyle = FlatStyle.Flat;
            btnFrameType.Image = FS2Resources.frame_unlocked_icon;
            btnFrameType.Location = new Point(124, 0);
            btnFrameType.Name = "btnFrameType";
            btnFrameType.Size = new Size(31, 35);
            btnFrameType.TabIndex = 4;
            toolTipFS.SetToolTip(btnFrameType, "Frame type: free or fixed  (Ctrl+Down arrow)");
            btnFrameType.UseVisualStyleBackColor = false;
            btnFrameType.Click += btnFrame_Click;
            // 
            // btnArrowType
            // 
            btnArrowType.BackColor = Color.Gray;
            btnArrowType.Dock = DockStyle.Left;
            btnArrowType.FlatAppearance.BorderSize = 0;
            btnArrowType.FlatStyle = FlatStyle.Flat;
            btnArrowType.Image = FS2Resources.arrow_type01_icon;
            btnArrowType.Location = new Point(93, 0);
            btnArrowType.Name = "btnArrowType";
            btnArrowType.Size = new Size(31, 35);
            btnArrowType.TabIndex = 3;
            toolTipFS.SetToolTip(btnArrowType, "Arrow direction (Ctrl+Up arrow)");
            btnArrowType.UseVisualStyleBackColor = false;
            btnArrowType.Click += btnArrowType_Click;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.Gray;
            btnSettings.Dock = DockStyle.Right;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Image = FS2Resources.settings_icon;
            btnSettings.Location = new Point(495, 0);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(31, 35);
            btnSettings.TabIndex = 14;
            toolTipFS.SetToolTip(btnSettings, "Settings");
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnScreen
            // 
            btnScreen.BackColor = Color.Gray;
            btnScreen.Dock = DockStyle.Left;
            btnScreen.FlatAppearance.BorderSize = 0;
            btnScreen.FlatStyle = FlatStyle.Flat;
            btnScreen.Image = FS2Resources.screen_icon;
            btnScreen.Location = new Point(62, 0);
            btnScreen.Name = "btnScreen";
            btnScreen.Size = new Size(31, 35);
            btnScreen.TabIndex = 2;
            toolTipFS.SetToolTip(btnScreen, "Take a screenshot (F4)");
            btnScreen.UseVisualStyleBackColor = false;
            btnScreen.Click += btnScreen_Click;
            // 
            // buttonMainMenu
            // 
            buttonMainMenu.BackColor = Color.Gray;
            buttonMainMenu.Dock = DockStyle.Left;
            buttonMainMenu.FlatAppearance.BorderSize = 0;
            buttonMainMenu.FlatStyle = FlatStyle.Flat;
            buttonMainMenu.Image = FS2Resources.menu_icon;
            buttonMainMenu.Location = new Point(31, 0);
            buttonMainMenu.Name = "buttonMainMenu";
            buttonMainMenu.Size = new Size(31, 35);
            buttonMainMenu.TabIndex = 1;
            toolTipFS.SetToolTip(buttonMainMenu, "Main menu");
            buttonMainMenu.UseVisualStyleBackColor = false;
            buttonMainMenu.Click += buttonMainMenu_Click;
            // 
            // buttonMinimizeForm
            // 
            buttonMinimizeForm.BackColor = Color.Gray;
            buttonMinimizeForm.Dock = DockStyle.Right;
            buttonMinimizeForm.FlatAppearance.BorderSize = 0;
            buttonMinimizeForm.FlatStyle = FlatStyle.Flat;
            buttonMinimizeForm.Image = FS2Resources.minimize_icon;
            buttonMinimizeForm.Location = new Point(526, 0);
            buttonMinimizeForm.Name = "buttonMinimizeForm";
            buttonMinimizeForm.Size = new Size(31, 35);
            buttonMinimizeForm.TabIndex = 15;
            toolTipFS.SetToolTip(buttonMinimizeForm, "Minimize");
            buttonMinimizeForm.UseVisualStyleBackColor = false;
            buttonMinimizeForm.Click += buttonMinimizeForm_Click;
            // 
            // buttonCloseForm
            // 
            buttonCloseForm.BackColor = Color.Gray;
            buttonCloseForm.Dock = DockStyle.Right;
            buttonCloseForm.FlatAppearance.BorderSize = 0;
            buttonCloseForm.FlatStyle = FlatStyle.Flat;
            buttonCloseForm.Image = FS2Resources.close_icon;
            buttonCloseForm.Location = new Point(557, 0);
            buttonCloseForm.Name = "buttonCloseForm";
            buttonCloseForm.Size = new Size(31, 35);
            buttonCloseForm.TabIndex = 16;
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
            panelDragTopR.Location = new Point(588, 0);
            panelDragTopR.Name = "panelDragTopR";
            panelDragTopR.Size = new Size(31, 35);
            panelDragTopR.TabIndex = 13;
            // 
            // panelDragTopL
            // 
            panelDragTopL.BackColor = Color.SlateGray;
            panelDragTopL.Dock = DockStyle.Left;
            panelDragTopL.Location = new Point(0, 0);
            panelDragTopL.Name = "panelDragTopL";
            panelDragTopL.Size = new Size(31, 35);
            panelDragTopL.TabIndex = 14;
            // 
            // panelDragLeft
            // 
            panelDragLeft.BackColor = Color.SlateGray;
            panelDragLeft.Controls.Add(chbArrow);
            panelDragLeft.Controls.Add(chbFrame);
            panelDragLeft.Controls.Add(chbNumbers);
            panelDragLeft.Controls.Add(chbText);
            panelDragLeft.Controls.Add(chbSave);
            panelDragLeft.Controls.Add(chbGuides);
            panelDragLeft.Dock = DockStyle.Left;
            panelDragLeft.Location = new Point(0, 35);
            panelDragLeft.Name = "panelDragLeft";
            panelDragLeft.Size = new Size(31, 337);
            panelDragLeft.TabIndex = 2;
            // 
            // chbArrow
            // 
            chbArrow.Appearance = Appearance.Button;
            chbArrow.BackColor = Color.Gray;
            chbArrow.Dock = DockStyle.Bottom;
            chbArrow.FlatAppearance.BorderSize = 0;
            chbArrow.FlatStyle = FlatStyle.Flat;
            chbArrow.Image = FS2Resources.arrow_icon;
            chbArrow.Location = new Point(0, 127);
            chbArrow.Name = "chbArrow";
            chbArrow.Size = new Size(31, 35);
            chbArrow.TabIndex = 11;
            toolTipFS.SetToolTip(chbArrow, "Arrow");
            chbArrow.UseVisualStyleBackColor = false;
            chbArrow.Click += chbArrow_Click;
            // 
            // chbFrame
            // 
            chbFrame.Appearance = Appearance.Button;
            chbFrame.BackColor = Color.Gray;
            chbFrame.Dock = DockStyle.Bottom;
            chbFrame.FlatAppearance.BorderSize = 0;
            chbFrame.FlatStyle = FlatStyle.Flat;
            chbFrame.Image = FS2Resources.frame_icon;
            chbFrame.Location = new Point(0, 162);
            chbFrame.Name = "chbFrame";
            chbFrame.Size = new Size(31, 35);
            chbFrame.TabIndex = 10;
            toolTipFS.SetToolTip(chbFrame, "Frame");
            chbFrame.UseVisualStyleBackColor = false;
            chbFrame.Click += chbFrame_Click;
            // 
            // chbNumbers
            // 
            chbNumbers.Appearance = Appearance.Button;
            chbNumbers.BackColor = Color.Gray;
            chbNumbers.Dock = DockStyle.Bottom;
            chbNumbers.FlatAppearance.BorderSize = 0;
            chbNumbers.FlatStyle = FlatStyle.Flat;
            chbNumbers.Image = FS2Resources.number_icon;
            chbNumbers.Location = new Point(0, 197);
            chbNumbers.Name = "chbNumbers";
            chbNumbers.Size = new Size(31, 35);
            chbNumbers.TabIndex = 9;
            toolTipFS.SetToolTip(chbNumbers, "Numbers");
            chbNumbers.UseVisualStyleBackColor = false;
            chbNumbers.Click += chbNumbers_Click;
            // 
            // chbText
            // 
            chbText.Appearance = Appearance.Button;
            chbText.BackColor = Color.Gray;
            chbText.Dock = DockStyle.Bottom;
            chbText.FlatAppearance.BorderSize = 0;
            chbText.FlatStyle = FlatStyle.Flat;
            chbText.Image = FS2Resources.text_icon;
            chbText.Location = new Point(0, 232);
            chbText.Name = "chbText";
            chbText.Size = new Size(31, 35);
            chbText.TabIndex = 13;
            toolTipFS.SetToolTip(chbText, "Text");
            chbText.UseVisualStyleBackColor = false;
            chbText.Click += chbText_Click;
            // 
            // chbSave
            // 
            chbSave.Appearance = Appearance.Button;
            chbSave.BackColor = Color.Gray;
            chbSave.Dock = DockStyle.Bottom;
            chbSave.FlatAppearance.BorderSize = 0;
            chbSave.FlatStyle = FlatStyle.Flat;
            chbSave.Image = FS2Resources.save_icon;
            chbSave.Location = new Point(0, 267);
            chbSave.Name = "chbSave";
            chbSave.Size = new Size(31, 35);
            chbSave.TabIndex = 12;
            toolTipFS.SetToolTip(chbSave, "Save to file");
            chbSave.UseVisualStyleBackColor = false;
            chbSave.Click += chbSave_Click;
            // 
            // chbGuides
            // 
            chbGuides.Appearance = Appearance.Button;
            chbGuides.BackColor = Color.Gray;
            chbGuides.Dock = DockStyle.Bottom;
            chbGuides.FlatAppearance.BorderSize = 0;
            chbGuides.FlatStyle = FlatStyle.Flat;
            chbGuides.Image = FS2Resources.guides_icon;
            chbGuides.Location = new Point(0, 302);
            chbGuides.Name = "chbGuides";
            chbGuides.Size = new Size(31, 35);
            chbGuides.TabIndex = 8;
            toolTipFS.SetToolTip(chbGuides, "Guides");
            chbGuides.UseVisualStyleBackColor = false;
            chbGuides.Click += chbGuides_Click;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.Transparent;
            panelRight.Controls.Add(rangeTrackBar);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(588, 35);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(31, 337);
            panelRight.TabIndex = 3;
            // 
            // rangeTrackBar
            // 
            rangeTrackBar.BackColor = Color.Gray;
            rangeTrackBar.Dock = DockStyle.Fill;
            rangeTrackBar.Location = new Point(0, 0);
            rangeTrackBar.LowerValue = 0;
            rangeTrackBar.Maximum = 100;
            rangeTrackBar.Minimum = 0;
            rangeTrackBar.Name = "rangeTrackBar";
            rangeTrackBar.RangeColor = Color.DimGray;
            rangeTrackBar.Size = new Size(31, 337);
            rangeTrackBar.TabIndex = 13;
            rangeTrackBar.Text = "verticalRangeTrackBar1";
            rangeTrackBar.ThumbBorderColor = Color.Transparent;
            rangeTrackBar.ThumbColor = Color.Black;
            toolTipFS.SetToolTip(rangeTrackBar, "Bar size");
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
            panelScreenArea.Location = new Point(31, 35);
            panelScreenArea.Name = "panelScreenArea";
            panelScreenArea.Size = new Size(557, 337);
            panelScreenArea.TabIndex = 4;
            panelScreenArea.Paint += panelScreenArea_Paint;
            // 
            // pnlBarTop
            // 
            pnlBarTop.BackColor = Color.Black;
            pnlBarTop.Dock = DockStyle.Top;
            pnlBarTop.Location = new Point(0, 0);
            pnlBarTop.Name = "pnlBarTop";
            pnlBarTop.Size = new Size(555, 0);
            pnlBarTop.TabIndex = 1;
            // 
            // pnlBarBottom
            // 
            pnlBarBottom.BackColor = Color.Black;
            pnlBarBottom.Dock = DockStyle.Bottom;
            pnlBarBottom.Location = new Point(0, 335);
            pnlBarBottom.Name = "pnlBarBottom";
            pnlBarBottom.Size = new Size(555, 0);
            pnlBarBottom.TabIndex = 0;
            // 
            // contextMenuMain
            // 
            contextMenuMain.ImageScalingSize = new Size(24, 24);
            contextMenuMain.Items.AddRange(new ToolStripItem[] { mitSize01, mitSize02, mitSize03, mitSize04, mitMax, toolStripMenuItem1, mitTakeScreen, mitFulscreen, mitClear, toolStripMenuItem3, mitArrow, mitFrame, mitNumber, mitText, toolStripMenuItem2, mitGuidlines, mitSaveFile, mitOpenFolder, toolStripMenuItem4, mitShowInfo, mitSettings, mitHelp, mitExit });
            contextMenuMain.Name = "contextMenuMain";
            contextMenuMain.Size = new Size(206, 526);
            // 
            // mitSize01
            // 
            mitSize01.AutoSize = false;
            mitSize01.Name = "mitSize01";
            mitSize01.ShortcutKeys = Keys.Alt | Keys.D1;
            mitSize01.Size = new Size(205, 25);
            mitSize01.Text = "Size 1";
            mitSize01.Click += mitSize01_Click;
            // 
            // mitSize02
            // 
            mitSize02.AutoSize = false;
            mitSize02.Name = "mitSize02";
            mitSize02.ShortcutKeys = Keys.Alt | Keys.D2;
            mitSize02.Size = new Size(205, 25);
            mitSize02.Text = "Size 2";
            mitSize02.Click += mitSize02_Click;
            // 
            // mitSize03
            // 
            mitSize03.AutoSize = false;
            mitSize03.Name = "mitSize03";
            mitSize03.ShortcutKeys = Keys.Alt | Keys.D3;
            mitSize03.Size = new Size(205, 25);
            mitSize03.Text = "Size 3";
            mitSize03.Click += mitSize03_Click;
            // 
            // mitSize04
            // 
            mitSize04.AutoSize = false;
            mitSize04.Name = "mitSize04";
            mitSize04.ShortcutKeys = Keys.Alt | Keys.D4;
            mitSize04.Size = new Size(205, 25);
            mitSize04.Text = "Size 4";
            mitSize04.Click += mitSize04_Click;
            // 
            // mitMax
            // 
            mitMax.Name = "mitMax";
            mitMax.Size = new Size(205, 30);
            mitMax.Text = "Max";
            mitMax.ToolTipText = "Zoom to current monitor size";
            mitMax.Click += mitMax_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.AutoSize = false;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(202, 5);
            // 
            // mitTakeScreen
            // 
            mitTakeScreen.AutoSize = false;
            mitTakeScreen.Image = FS2Resources.screen_icon;
            mitTakeScreen.Name = "mitTakeScreen";
            mitTakeScreen.ShortcutKeys = Keys.F4;
            mitTakeScreen.Size = new Size(205, 25);
            mitTakeScreen.Text = "Screenshot";
            // 
            // mitFulscreen
            // 
            mitFulscreen.AutoSize = false;
            mitFulscreen.Name = "mitFulscreen";
            mitFulscreen.ShortcutKeys = Keys.Alt | Keys.D5;
            mitFulscreen.Size = new Size(205, 25);
            mitFulscreen.Text = "Fullscreen";
            mitFulscreen.ToolTipText = "Current screen without taskbar";
            mitFulscreen.Click += mitFulscreen_Click;
            // 
            // mitClear
            // 
            mitClear.AutoSize = false;
            mitClear.Name = "mitClear";
            mitClear.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Z;
            mitClear.Size = new Size(205, 25);
            mitClear.Text = "Clear";
            mitClear.ToolTipText = "Clear screenshot area";
            mitClear.Click += mitClear_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.AutoSize = false;
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(202, 5);
            // 
            // mitArrow
            // 
            mitArrow.AutoSize = false;
            mitArrow.Name = "mitArrow";
            mitArrow.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
            mitArrow.Size = new Size(205, 25);
            mitArrow.Text = "Arrow";
            mitArrow.Click += mitArrow_Click;
            // 
            // mitFrame
            // 
            mitFrame.AutoSize = false;
            mitFrame.Name = "mitFrame";
            mitFrame.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F;
            mitFrame.Size = new Size(205, 25);
            mitFrame.Text = "Frame";
            mitFrame.Click += mitFrame_Click;
            // 
            // mitNumber
            // 
            mitNumber.AutoSize = false;
            mitNumber.Name = "mitNumber";
            mitNumber.ShortcutKeys = Keys.Control | Keys.Shift | Keys.N;
            mitNumber.Size = new Size(205, 25);
            mitNumber.Text = "Number";
            mitNumber.Click += mitNumber_Click;
            // 
            // mitText
            // 
            mitText.AutoSize = false;
            mitText.Name = "mitText";
            mitText.ShortcutKeys = Keys.Control | Keys.Shift | Keys.T;
            mitText.Size = new Size(205, 25);
            mitText.Text = "Text";
            mitText.Click += mitText_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.AutoSize = false;
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(202, 5);
            // 
            // mitGuidlines
            // 
            mitGuidlines.AutoSize = false;
            mitGuidlines.Name = "mitGuidlines";
            mitGuidlines.ShortcutKeys = Keys.Control | Keys.Shift | Keys.G;
            mitGuidlines.Size = new Size(205, 25);
            mitGuidlines.Text = "Guidlines";
            mitGuidlines.Click += mitGuidlines_Click;
            // 
            // mitSaveFile
            // 
            mitSaveFile.AutoSize = false;
            mitSaveFile.Name = "mitSaveFile";
            mitSaveFile.Size = new Size(205, 25);
            mitSaveFile.Text = "Save to File";
            mitSaveFile.Click += mitSaveFile_Click;
            // 
            // mitOpenFolder
            // 
            mitOpenFolder.AutoSize = false;
            mitOpenFolder.Name = "mitOpenFolder";
            mitOpenFolder.Size = new Size(205, 25);
            mitOpenFolder.Text = "Open Folder with Files";
            mitOpenFolder.Click += mitOpenFolder_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.AutoSize = false;
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(202, 5);
            // 
            // mitShowInfo
            // 
            mitShowInfo.AutoSize = false;
            mitShowInfo.Name = "mitShowInfo";
            mitShowInfo.Size = new Size(205, 25);
            mitShowInfo.Text = "Show Info";
            mitShowInfo.ToolTipText = "Show information";
            mitShowInfo.Click += mitShowInfo_Click;
            // 
            // mitSettings
            // 
            mitSettings.AutoSize = false;
            mitSettings.Image = FS2Resources.settings_icon;
            mitSettings.Name = "mitSettings";
            mitSettings.Size = new Size(205, 25);
            mitSettings.Text = "Settings";
            mitSettings.Click += mitSettings_Click;
            // 
            // mitHelp
            // 
            mitHelp.AutoSize = false;
            mitHelp.Image = FS2Resources.help_icon;
            mitHelp.Name = "mitHelp";
            mitHelp.ShortcutKeys = Keys.F1;
            mitHelp.Size = new Size(205, 25);
            mitHelp.Text = "Help";
            mitHelp.Click += mitHelp_Click;
            // 
            // mitExit
            // 
            mitExit.AutoSize = false;
            mitExit.Name = "mitExit";
            mitExit.ShortcutKeys = Keys.Alt | Keys.F4;
            mitExit.Size = new Size(205, 25);
            mitExit.Text = "Exit";
            mitExit.Click += buttonCloseForm_Click;
            // 
            // FS2MainForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(619, 407);
            Controls.Add(panelScreenArea);
            Controls.Add(panelRight);
            Controls.Add(panelDragLeft);
            Controls.Add(panelDragTop);
            Controls.Add(panelBottom);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "FS2MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FastScreener 2";
            TopMost = true;
            Shown += FS2MainForm_Shown;
            Move += FS2MainForm_Move;
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
        private Button btnFrameType;
        private Button btnNextRes;
        private Panel panelDragTopR;
        private CheckBox chbSave;
        private VerticalRangeTrackBar rangeTrackBar;
        private Panel panelDragTopL;
        private Panel pnlBarTop;
        private Panel pnlBarBottom;
        private ToolTip toolTipFS;
        private TextBox txtbName;
        private BlurOutlineLabel labelDebug;
        private ToolStripMenuItem mitClear;
        private Splitter splitter1;
        private ToolStripMenuItem mitFulscreen;
        private CheckBox chbText;
        private ToolStripMenuItem mitText;
        private ToolStripMenuItem mitShowInfo;
        private ToolStripMenuItem mitMax;
    }
}
