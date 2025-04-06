using System.Drawing.Imaging;
using static FastScreener2.FSUtils;
using static FastScreener2.FS2SettingsManager;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace FastScreener2
{
    public partial class FS2MainForm : Form
    {
        //alpha color to remove 
        private Color ALPHA_KEY_COLOR = Color.FromArgb(255, 1, 0, 1);

        //for scaling
        public static float scalingFactor;

        // Create the Keyboard Hook
        KeyboardHook keyboardHook = new KeyboardHook();

        // Create the Mouse Hook
        MouseHook mouseHook = new MouseHook();

        private int frameSize = 32; //offset
        private const int snapMargin = 8; // Distance in pixels to trigger snapping

        public static int clickInArrowCount = 0;
        public static int clickInFrameCount = 0;
        public static int clickInRes = 0;

        static Point relativePoint; //first click point
        public static Rectangle currentRectangle;
        private Point startPoint;
        private bool isDrawing;
        private bool isLineDrawing;
        public static int numbering = 1; //for numbers

        private Button lastPressedButton; // Store the last pressed button

        public static bool isReseted = false;

        //for file
        private string stringURL = "";

        private string fileName = "";

        public static FS2MainForm Instance { get; private set; }
        private const int WM_DPICHANGED = 0x02E0;

        [DllImport("user32.dll")]
        private static extern int GetDpiForWindow(IntPtr hwnd);

        public FS2MainForm()
        {
            Instance = this;  // Store the reference when the form is created
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.Dpi;

            //set transparent form
            this.BackColor = ALPHA_KEY_COLOR;
            this.TransparencyKey = ALPHA_KEY_COLOR;

            Rectangle virtScreenRect = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            foreach (Screen screen in Screen.AllScreens)
                virtScreenRect = Rectangle.Union(virtScreenRect, screen.Bounds);

            //Get virtual screen size
            virtScreenWidth = virtScreenRect.Width;
            virtScreenHeight = virtScreenRect.Height;

            FS2SettingsManager.Load();

            //get scaling
            scalingFactor = GetScalingFactor(this);

            //frame upd
            frameSize = Convert.ToInt32(frameSize * scalingFactor);

            FormResizer(FS2SettingsManager.startResW, FS2SettingsManager.startResH);

            //load UI values Checked true/false
            mitArrow.Checked = FS2SettingsManager.drawArrows;
            chbArrow.Checked = FS2SettingsManager.drawArrows;
            ArrowPicUpdater(FS2SettingsManager.arrowType);

            //frame
            mitFrame.Checked = FS2SettingsManager.drawFrame;
            chbFrame.Checked = FS2SettingsManager.drawFrame;
            FramePicUpdater(FS2SettingsManager.frameType);

            //guides
            mitGuidlines.Checked = FS2SettingsManager.drawGuides;
            chbGuides.Checked = FS2SettingsManager.drawGuides;

            //number
            mitNumber.Checked = FS2SettingsManager.drawNumber;
            chbNumbers.Checked = FS2SettingsManager.drawNumber;

            //file
            mitSaveFile.Checked = FS2SettingsManager.saveToFile;
            chbSave.Checked = FS2SettingsManager.saveToFile;

            //center panel
            panelScreenArea.BackColor = Color.Transparent;

            FSUtils utils = new FSUtils();

            // Attach the same event handlers to all 4 panels
            utils.AttachDragEvents(panelDragBottomL);
            utils.AttachDragEvents(panelDragBottomR);
            utils.AttachDragEvents(panelDragTop);
            utils.AttachDragEvents(panelDragLeft);
            utils.AttachDragEvents(panelDragTopR);
            utils.AttachDragEvents(panelDragTopL);

            // Capture the events
            mouseHook.MiddleButtonDown += new MouseHook.MouseHookCallback(mouseHook_MMB);
            mouseHook.MiddleButtonUp += new MouseHook.MouseHookCallback(mouseHook_MouseUp);
            mouseHook.MouseMove += new MouseHook.MouseHookCallback(mouseHook_MouseMove);

            //Installing the Mouse Hooks
            mouseHook.Install();

            // Capture the events
            keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);

            //Installing the Keyboard Hooks
            keyboardHook.Install();

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            PanelSize();

            NameFieldPos();

            ShowInfo("start");

            MenuItemUpdate();

            this.KeyPreview = true;
            contextMenuMain.Focus();

            //buttons
            SetControlImage(btnSettings, "settings_icon");
            SetControlImage(buttonMinimizeForm, "minimize_icon");
            SetControlImage(buttonCloseForm, "close_icon");
            SetControlImage(btnNextRes, "res_cycle_icon");
            SetControlImage(btnScreen, "screen_icon");
            SetControlImage(buttonMainMenu, "menu_icon");

            //checkboxes
            SetControlImage(chbSave, "save_icon");
            SetControlImage(chbNumbers, "number_icon");
            SetControlImage(chbFrame, "frame_icon");
            SetControlImage(chbArrow, "arrow_icon");
            SetControlImage(chbGuides, "guides_icon");
        }

        private void SetControlImage(Control control, string resourceName)
        {
            // Determine the size of the icon based on scaling factor
            int iconSize = scalingFactor switch
            {
                1 => 16,
                1.5f => 32,
                2 => 48,
                _ => 16 // Default to 16px if scalingFactor is unexpected
            };

            Debug.WriteLine($"Icon size: {iconSize}"); // Debugging line to check size

            byte[] svgData = (byte[])SVGres.ResourceManager.GetObject(resourceName);

            if (svgData == null)
                return;

            // Load and render the SVG into a Bitmap
            Bitmap finalImage = SvgHelper.LoadSvgFromResources(svgData, iconSize, iconSize);

            if (finalImage != null)
            {
                // Assign the generated Bitmap to the appropriate control
                if (control is Button button)
                {
                    button.Image = finalImage;
                    button.ImageAlign = ContentAlignment.MiddleCenter;
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.Image = finalImage;
                    checkBox.ImageAlign = ContentAlignment.MiddleCenter;
                }
            }
        }

        private void NameFieldPos()
        {
            int panelHeight = panelDragTop.Height / 2;
            int fieldHeight = txtbName.Height / 2;

            txtbName.Top = panelHeight - fieldHeight;

            txtbName.Left = splitter1.Left + splitter1.Width;
        }

        private void PanelSize()
        {
            panelBottom.Height = frameSize;
            panelDragTop.Height = frameSize;

            panelDragLeft.Width = frameSize;
            panelRight.Width = frameSize;

            buttonCloseForm.Width = frameSize;
            buttonCloseForm.Height = frameSize;

            panelDragTopR.Width = frameSize;
            panelDragTopR.Height = frameSize;

            panelDragTopL.Width = frameSize;
            panelDragTopL.Height = frameSize;

            panelDragBottomL.Width = frameSize * 2;
            panelDragBottomL.Height = frameSize;

            panelDragBottomR.Width = frameSize * 2;
            panelDragBottomR.Height = frameSize;
        }


        //on dpi change
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DPICHANGED)
            {
                RestartApplication();
            }

            base.WndProc(ref m);
        }

        private void RestartApplication()
        {
            Process.Start(Application.ExecutablePath); // Start a new instance
            Application.Exit(); // Close the current instance
        }

        public float GetScalingFactor(Form form)
        {
            int dpi = GetDpiForWindow(form.Handle);
            return dpi / 96f; // Base DPI is 96
        }

        public void ShowInfo(string type)
        {
            string leftTopPos = "Pos X:" + this.Location.X.ToString() + ", Y:" + this.Location.Y.ToString();

            string screenArea = "";

            string panelW = panelScreenArea.Width.ToString();
            string panelH = panelScreenArea.Height.ToString();

            string panelWS = (panelScreenArea.Width / scalingFactor).ToString();
            string panelHS = (panelScreenArea.Height / scalingFactor).ToString();

            if (scalingFactor == 1)
            {
                screenArea = "Size W:" + panelW + ", H:" + panelH;
            }
            else
            {
                screenArea = $"Size W: {panelW} ({panelWS}), H: {panelH} ({panelHS})";
            }

            string name = "FastScreener 2.0";
            string scale = "Scaling: " + scalingFactor;

            string frameSize = "";

            if (rangeTrackBar != null)
            {
                frameSize = "Bar bottom: " + pnlBarBottom.Height + ", top: " + pnlBarTop.Height;
            }


            string saveFile = "";

            if (FS2SettingsManager.saveToFile)
            {
                saveFile = "to file (" + fileFormat + ") and clipboard";
            }
            else
            {
                saveFile = "to clipboard";
            }

            if (type == "drag")
            {
                labelDebug.Text = leftTopPos + " | " + screenArea + " | " + scale;
            }

            if (type == "start")
            {
                labelDebug.Text = name + " | " + screenArea + " | " + scale;
            }

            if (type == "capture")
            {
                labelDebug.Text = "Captured to " + saveFile + " | " + screenArea + " | " + scale;
            }

            if (type == "frame")
            {
                labelDebug.Text = screenArea + " | " + frameSize;
                ResizeBar();
            }

            if (type == "reset")
            {
                labelDebug.Text = "The settings were reset to default";
            }

            if (type == "clear")
            {
                labelDebug.Text = "The screenshot area has been cleared";

            }

            if (type == "fullscreen")
            {
                labelDebug.Text = "A screenshot of the current screen has been saved " + saveFile;

            }
        }

        public void SwapPanelsIfNeeded()
        {
            if (this.Left < 0)
            {
                panelDragLeft.Dock = DockStyle.Right;
                panelRight.Dock = DockStyle.Left;
            }
            else
            {
                panelDragLeft.Dock = DockStyle.Left;
                panelRight.Dock = DockStyle.Right;
            }

            if (this.Top < 0)
            {
                panelDragTop.Dock = DockStyle.Bottom;
                panelBottom.Dock = DockStyle.Top;
            }
            else
            {
                panelDragTop.Dock = DockStyle.Top;
                panelBottom.Dock = DockStyle.Bottom;
            }
        }

        private void SetFileName()
        {           
            string currentTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            fileName = !string.IsNullOrEmpty(txtbName.Text) ? $"{txtbName.Text}.{fileFormat}" : $"{currentTime}_screenshot.{fileFormat}";

        }


        private void CaptureScreen()
        {
            int bitmapWidth = panelScreenArea.Width;
            int bitmapHeight = panelScreenArea.Height;

            panelScreenArea.BorderStyle = BorderStyle.None;
            bool guideIsOn = drawGuides;

            if (guideIsOn)
            {
                RenderGuides(new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle), panelScreenArea, ALPHA_KEY_COLOR);
            }

            SetFileName();

            // Create bitmap and capture graphics
            using (Bitmap captureBitmap = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb))
            {
                Rectangle captureRectangle = new Rectangle(this.Location.X + frameSize, this.Location.Y + frameSize, bitmapWidth, bitmapHeight);

                using (Graphics captureGraphics = Graphics.FromImage(captureBitmap))
                {
                    captureGraphics.CopyFromScreen(captureRectangle.Location, Point.Empty, captureRectangle.Size);
                }

                // Save file if needed
                if (saveToFile)
                {
                    SaveToFile(captureBitmap);
                }

                SetScaledBitmapToClipboard(captureBitmap, scalingFactor);
            }

            panelScreenArea.BorderStyle = BorderStyle.FixedSingle;

            // Clear objects to free memory
            panelScreenArea.Invalidate();
            drawnRectangles.Clear();
            currentRectangle = new Rectangle(startPoint, new Size(0, 0));
            drawnArrows.Clear();
            drawnTexts.Clear();

            ShowInfo("capture");

            numbering = 1; // Reset numbering

            if (guideIsOn)
            {
                this.Refresh();
                RenderGuides(new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle), panelScreenArea, guideColor);
            }

            LogScreenshot(DateTime.Now.ToString("yyyy-MM-dd"), bitmapWidth, bitmapHeight, fileName);
        }

        private void SaveToFile(Bitmap captureBitmap)
        {            
            string appExeDir = Directory.GetCurrentDirectory();
            string directoryPath = Path.Combine(appExeDir, SUBPATH);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);


            // Set file extension and ImageCodecInfo
            ImageCodecInfo codec = null;
            ImageFormat imageFormat = ImageFormat.Png; // default

            //select format
            if (fileFormat == "jpg")
            {
                imageFormat = ImageFormat.Jpeg;
                codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
            }
            else
            {
                imageFormat = ImageFormat.Png;
            }

            stringURL = Path.Combine(directoryPath, fileName);

            try
            {
                // Scale down the image
                int newWidth = (int)(captureBitmap.Width / scalingFactor);
                int newHeight = (int)(captureBitmap.Height / scalingFactor);

                using (Bitmap scaledBitmap = new Bitmap(newWidth, newHeight))
                using (Graphics g = Graphics.FromImage(scaledBitmap))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.Clear(Color.Transparent);

                    g.DrawImage(captureBitmap, 0, 0, newWidth, newHeight);

                    // Save the scaled image
                    if (imageFormat == ImageFormat.Jpeg && codec != null)
                    {
                        // Create Encoder parameters for JPEG quality
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, fileQuality);
                        scaledBitmap.Save(stringURL, codec, encoderParams);
                    }
                    else
                    {
                        // Save normally (PNG, BMP, GIF)
                        scaledBitmap.Save(stringURL, imageFormat);
                    }
                }
            }
            catch
            {
                MessageBox.Show($"Can't save screenshot to file! Path: {stringURL}", "FastScreener Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        void SetScaledBitmapToClipboard(Bitmap originalBitmap, float scalingFactor)
        {
            // Calculate new dimensions
            int scaledWidth = (int)(originalBitmap.Width / scalingFactor);
            int scaledHeight = (int)(originalBitmap.Height / scalingFactor);

            // Create a new bitmap with scaled dimensions
            Bitmap scaledBitmap = new Bitmap(scaledWidth, scaledHeight);

            // Draw the original bitmap onto the scaled bitmap
            using (Graphics g = Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalBitmap, 0, 0, scaledWidth, scaledHeight);
            }

            // Set the scaled bitmap to the clipboard
            Clipboard.SetImage(scaledBitmap);

            // Dispose of the scaled bitmap if no longer needed
            scaledBitmap.Dispose();
        }

        //hook keys
        private void keyboardHook_KeyDown(KeyboardHook.VKeys key)
        {
            if (key == KeyboardHook.VKeys.F4)
            {
                CaptureScreen();
            }

            if (key == KeyboardHook.VKeys.F1)
            {
                mitHelp_Click(this, EventArgs.Empty);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.D1)) // Alt + 1
            {
                mitSize01_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            if (keyData == (Keys.Alt | Keys.D2)) // Alt + 2
            {
                mitSize02_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            if (keyData == (Keys.Alt | Keys.D3)) // Alt + 3
            {
                mitSize03_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            if (keyData == (Keys.Alt | Keys.D4)) // Alt + 4
            {
                mitSize04_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            if (keyData == (Keys.Alt | Keys.D5)) // Alt + 5
            {
                mitFulscreen_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            

            //cycle
            if (keyData == (Keys.Control | Keys.Right))
            {
                btnNextRes_Click(this, EventArgs.Empty);
                btnNextRes.BackColor = Color.WhiteSmoke;
                lastPressedButton = btnNextRes; // Store button reference
                return true; // Mark as handled
            }

            //arrow
            if (keyData == (Keys.Control | Keys.Up))
            {
                btnArrowType_Click(this, EventArgs.Empty);
                btnArrowType.BackColor = Color.WhiteSmoke;
                lastPressedButton = btnArrowType; // Store button reference
                return true; // Mark as handled
            }

            //arrow
            if (keyData == (Keys.Control | Keys.Down))
            {
                btnFrame_Click(this, EventArgs.Empty);
                btnFrameType.BackColor = Color.WhiteSmoke;
                lastPressedButton = btnFrameType; // Store button reference
                return true; // Mark as handled
            }


            if (keyData == (Keys.Control | Keys.Shift | Keys.A))
            {
                mitArrow_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                mitSaveFile_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.F))
            {
                mitFrame_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.G))
            {
                mitGuidlines_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.N))
            {
                mitNumber_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.C))
            {
                mitClear_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Detect when any key is released
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            // Reset color when key is released
            if (lastPressedButton != null)
            {
                lastPressedButton.BackColor = Color.DimGray; // Default color
                lastPressedButton = null; // Clear stored button
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            keyboardHook.KeyDown -= new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            keyboardHook.Uninstall();

            mouseHook.MiddleButtonDown -= new MouseHook.MouseHookCallback(mouseHook_MMB);
            mouseHook.MiddleButtonUp -= new MouseHook.MouseHookCallback(mouseHook_MouseUp);
            mouseHook.MouseMove -= new MouseHook.MouseHookCallback(mouseHook_MouseMove);

            mouseHook.Uninstall();

            //scaling
            int scaledW = Convert.ToInt32(panelScreenArea.Width / scalingFactor);
            int scaledH = Convert.ToInt32(panelScreenArea.Height / scalingFactor);

            string res = scaledW + "," + scaledH;
            FS2SettingsManager.SetSetting("res_on_close", res);
            FS2SettingsManager.Save();
        }


        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonMinimizeForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FS2MainForm_Shown(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
            this.Focus();
            this.TopMost = true;
        }

        private void buttonMainMenu_Click(object sender, EventArgs e)
        {
            contextMenuMain.Show(Cursor.Position.X, Cursor.Position.Y);
        }

        private void btnScreen_Click(object sender, EventArgs e)
        {
            CaptureScreen();
        }

        private void btnArrowType_Click(object sender, EventArgs e)
        {
            clickInArrowCount++;

            ArrowPicUpdater(clickInArrowCount);
        }

        private void ArrowPicUpdater(int number)
        {

            if (clickInArrowCount > 4)
            {
                clickInArrowCount = 1;
                number = 1;
            }

            switch (number)
            {
                case 1:
                    btnArrowType.Image = FS2Resources.arrow_type01_icon;
                    FS2SettingsManager.arrowType = 1; clickInArrowCount = 1;
                    SetControlImage(btnArrowType, "arrow_type01_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "1");
                    FS2SettingsManager.Save();
                    break;
                case 2:
                    btnArrowType.Image = FS2Resources.arrow_type02_icon;
                    FS2SettingsManager.arrowType = 2; clickInArrowCount = 2;
                    SetControlImage(btnArrowType, "arrow_type02_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "2");
                    FS2SettingsManager.Save();
                    break;
                case 3:
                    btnArrowType.Image = FS2Resources.arrow_type03_icon;
                    FS2SettingsManager.arrowType = 3; clickInArrowCount = 3;
                    SetControlImage(btnArrowType, "arrow_type03_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "3");
                    FS2SettingsManager.Save();
                    break;
                case 4:
                    btnArrowType.Image = FS2Resources.arrow_type04_icon;
                    FS2SettingsManager.arrowType = 4; clickInArrowCount = 4;
                    SetControlImage(btnArrowType, "arrow_type04_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "4");
                    FS2SettingsManager.Save();
                    break;

                default:
                    break;
            }
        }


        private void btnFrame_Click(object sender, EventArgs e)
        {
            clickInFrameCount++;
            FramePicUpdater(clickInFrameCount);
        }

        private void FramePicUpdater(int number)
        {
            if (clickInFrameCount > 2)
            {
                clickInFrameCount = 1;
                number = 1;
            }

            switch (number)
            {
                case 1:
                    btnFrameType.Image = FS2Resources.frame_unlocked_icon;
                    frameType = 1; clickInFrameCount = 1;
                    SetControlImage(btnFrameType, "frame_unlocked_icon");
                    SetSetting("frame_type", "1");
                    Save();
                    break;
                case 2:
                    btnFrameType.Image = FS2Resources.frame_locked_icon;
                    SetControlImage(btnFrameType, "frame_locked_icon");
                    frameType = 2; clickInFrameCount = 2;
                    SetSetting("frame_type", "2");
                    Save();
                    break;
                default:
                    break;
            }
        }

        private void mitSettings_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Form2 class            
            formFS2Settings settingsForm = new formFS2Settings();

            settingsForm.ShowDialog();

            MenuItemUpdate();

            if (drawGuides == true)
            {
                this.Refresh();
            }

            pnlBarTop.BackColor = barColor;
            pnlBarBottom.BackColor = barColor;

            panelScreenArea.Invalidate();

            chbArrow.Checked = drawArrows;
            chbFrame.Checked = drawFrame;
            chbGuides.Checked = drawGuides;
            chbNumbers.Checked = drawNumber;
            chbSave.Checked = saveToFile;

            if (isReseted)
            {
                ShowInfo("reset");
                isReseted = false;
            }

        }

        private void ToggleStatus(
        ToolStripMenuItem menuItem,
        ref bool statusFlag,
        string onMessage,
        string offMessage,
        string settingsKey,
        Control targetControl = null,
        bool? controlState = null)
        {
            if (menuItem.CheckState == CheckState.Checked)
            {
                menuItem.CheckState = CheckState.Unchecked;
                statusFlag = false;
                labelDebug.Text = offMessage;

                FS2SettingsManager.SetSetting(settingsKey, "false");

                // Handle optional control state update
                if (targetControl != null && controlState.HasValue)
                {
                    switch (targetControl)
                    {
                        case Button button:
                            button.Enabled = controlState.Value;
                            break;
                        case CheckBox checkBox:
                            checkBox.Checked = controlState.Value;
                            break;
                    }
                }
            }
            else
            {
                menuItem.CheckState = CheckState.Checked;
                statusFlag = true;
                labelDebug.Text = onMessage;

                FS2SettingsManager.SetSetting(settingsKey, "true");

                if (targetControl != null && controlState.HasValue)
                {
                    switch (targetControl)
                    {
                        case Button button:
                            button.Enabled = !controlState.Value;
                            break;
                        case CheckBox checkBox:
                            checkBox.Checked = !controlState.Value;
                            break;
                    }
                }
            }

            FS2SettingsManager.Save();
        }

        //ARROW
        private void mitArrow_Click(object sender, EventArgs e)
        {
            DrawArrowStatus();
        }

        private void DrawArrowStatus()
        {
            ToggleStatus(mitArrow, ref FS2SettingsManager.drawArrows, "Arrows turned ON", "Arrows turned OFF", "draw_arrows", chbArrow, false);
        }

        private void chbArrow_Click(object sender, EventArgs e)
        {
            DrawArrowStatus();
        }

        //FRAME
        private void mitFrame_Click(object sender, EventArgs e)
        {
            DrawFrameStatus();
        }

        private void DrawFrameStatus()
        {
            ToggleStatus(mitFrame, ref FS2SettingsManager.drawFrame, "Frame turned ON", "Frame turned OFF", "draw_frame", chbFrame, false);
        }

        //GUIDES
        private void mitGuidlines_Click(object sender, EventArgs e)
        {
            DrawGuideStatus();
        }

        private void chbGuides_Click(object sender, EventArgs e)
        {
            DrawGuideStatus();

            //paint rect
            PaintEventArgs paintRect = new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle);

            if (drawGuides == false)
            {
                //this.Refresh();
                RenderGuides(paintRect, panelScreenArea, ALPHA_KEY_COLOR);
            }
            else
            {
                RenderGuides(paintRect, panelScreenArea, guideColor);
            }

        }

        private void DrawGuideStatus()
        {
            ToggleStatus(mitGuidlines, ref FS2SettingsManager.drawGuides, "Guides turned ON", "Guides turned OFF", "draw_guidlines", chbGuides, false);
        }

        private void chbGuides_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chbFrame_Click(object sender, EventArgs e)
        {
            DrawFrameStatus();
        }

        //NUMBER
        private void mitNumber_Click(object sender, EventArgs e)
        {
            DrawNumberStatus();
        }

        private void chbNumbers_Click(object sender, EventArgs e)
        {
            DrawNumberStatus();
        }

        private void DrawNumberStatus()
        {
            ToggleStatus(mitNumber, ref FS2SettingsManager.drawNumber, "Numbers turned ON", "Numbers turned OFF", "draw_number", chbNumbers, false);
        }


        private void chbSave_Click(object sender, EventArgs e)
        {
            SaveToFileStatus();
        }

        private void mitSaveFile_Click(object sender, EventArgs e)
        {
            SaveToFileStatus();
        }

        private void SaveToFileStatus()
        {
            ToggleStatus(mitSaveFile, ref FS2SettingsManager.saveToFile, "Save to file turned ON", "Save to file turned OFF", "save_to_file", chbSave, false);
        }


        //hook mouse MMB
        private void mouseHook_MMB(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            Panel usedPanel = panelScreenArea;

            //paint rect
            PaintEventArgs paintRect = new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle);

            // important point
            relativePoint = usedPanel.PointToClient(Cursor.Position);

            //draw free Frame
            if (FS2SettingsManager.drawFrame && FS2SettingsManager.frameType == 1)
            {
                startPoint = new Point(relativePoint.X, relativePoint.Y);
                currentRectangle = new Rectangle(startPoint, new Size(0, 0));
                isDrawing = true;
            }

            //fixed
            if (drawFrame && frameType == 2)
            {
                //scale fixed frame
                int width = (int)(frameWidth * scalingFactor);
                int height = (int)(frameHeight * scalingFactor);

                startPoint = new Point(relativePoint.X - width / 2, relativePoint.Y - height / 2);
                currentRectangle = new Rectangle(startPoint, new Size(width, height));
                isDrawing = true;

                drawnRectangles.Add(currentRectangle);
            }

            isLineDrawing = true;


            //draw Arrow
            if (FS2SettingsManager.drawArrows && isLineDrawing)
            {
                SetArrow(relativePoint, FS2SettingsManager.arrowColor);
                RenderArrows(paintRect);
            }

            //draw Number
            if (FS2SettingsManager.drawNumber)
            {
                AddNumber(numbering.ToString(), relativePoint);
                RenderNumbers(paintRect);
                numbering++;
            }

        }

        // Mouse Middle Button Up (End drawing)
        private void mouseHook_MouseUp(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            isDrawing = false;
            isLineDrawing = false;

            int width = 0;
            int height = 0;

            // Calculate the final rectangle - Free type
            if (FS2SettingsManager.frameType == 1)
            {
                width = relativePoint.X - startPoint.X;
                height = relativePoint.Y - startPoint.Y;
            }

            // Create and add the rectangle
            Rectangle newRectangle = new Rectangle(
                    Math.Min(startPoint.X, relativePoint.X),
                    Math.Min(startPoint.Y, relativePoint.Y),
                    Math.Abs(width),
                    Math.Abs(height)
                );


            if (drawFrame)
            {
                drawnRectangles.Add(newRectangle);
            }

            panelScreenArea.Invalidate();

            this.Activate();
        }

        private void mouseHook_MouseMove(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            isLineDrawing = false;

            if (isDrawing)
            {
                // important point
                relativePoint = panelScreenArea.PointToClient(Cursor.Position);

                int width = 0;
                int height = 0;

                if (frameType == 1)
                {
                    width = relativePoint.X - startPoint.X;
                    height = relativePoint.Y - startPoint.Y;

                    currentRectangle = new Rectangle(startPoint.X, startPoint.Y, width, height);
                }

                panelScreenArea.Invalidate();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            mitSettings_Click(sender, e);
        }

        private void buttonCloseForm_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Brown;
        }

        private void buttonCloseForm_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.DimGray;
        }

        private void mitHelp_Click(object sender, EventArgs e)
        {
            formFSHelp helpForm = new formFSHelp();
            helpForm.ShowDialog();
        }


        private void rangeTrackBar_MouseMove_1(object sender, MouseEventArgs e)
        {
            ShowInfo("frame");
        }


        public void ResizeBar()
        {
            if (rangeTrackBar != null && panelScreenArea != null)
            {
                int totalHeight = panelScreenArea.Height; // Get total height of the panel

                int bottomHeight = (int)(totalHeight * (rangeTrackBar.LowerValue / 100.0));
                int topHeight = (int)(totalHeight * ((100 - rangeTrackBar.UpperValue) / 100.0));

                pnlBarBottom.Height = bottomHeight;
                pnlBarTop.Height = topHeight;

                // Set visibility based on height
                pnlBarBottom.BackColor = bottomHeight == 0 ? Color.Transparent : FS2SettingsManager.barColor;
                pnlBarTop.BackColor = topHeight == 0 ? Color.Transparent : FS2SettingsManager.barColor;
            }
        }

        private void panelScreenArea_Paint(object sender, PaintEventArgs e)
        {

            if (drawGuides)
            {
                RenderGuides(e, panelScreenArea, guideColor);
            }

            //arrow
            if (FS2SettingsManager.drawArrows || drawnArrows.Count > 0)
            {
                RenderArrows(e);
            }

            //frame
            if (drawFrame || isDrawing || drawnRectangles.Count > 0)
            {
                RenderFrame(e);
                DrawFrameCurrent(e);
            }

            //numbers
            if (FS2SettingsManager.drawNumber || drawnTexts.Count > 0)
            {
                RenderNumbers(e);
            }

        }

        private void mitOpenFolder_Click(object sender, EventArgs e)
        {
            string appExeDir = Directory.GetCurrentDirectory();

            //check directory for files
            bool exists = Directory.Exists(appExeDir + "\\" + SUBPATH);

            // create if not exists
            if (!exists)
                Directory.CreateDirectory(appExeDir + "\\" + SUBPATH);

            //path to open
            string PathToDir = appExeDir + "\\" + SUBPATH;

            //open dir
            Process.Start("explorer.exe", PathToDir);
        }


        private void MenuItemUpdate()
        {
            mitSize01.Text = resWorked[0, 0].ToString() + "x" + resWorked[1, 0].ToString();
            mitSize02.Text = resWorked[0, 1].ToString() + "x" + resWorked[1, 1].ToString();
            mitSize03.Text = resWorked[0, 2].ToString() + "x" + resWorked[1, 2].ToString();
            mitSize04.Text = resWorked[0, 3].ToString() + "x" + resWorked[1, 3].ToString();
        }


        private void AdjustClientSize(int widthIndex, int heightIndex)
        {
            // Retrieve dimensions
            int clientW = Convert.ToInt32(resWorked[0, widthIndex]);
            int clientH = Convert.ToInt32(resWorked[1, heightIndex]);

            // Apply scaling factor
            int scaledClientW = (int)(clientW * scalingFactor) + frameSize * 2;
            int scaledClientH = (int)(clientH * scalingFactor) + frameSize * 2;

            // Set client size
            this.ClientSize = new Size(scaledClientW, scaledClientH);

            // Refresh the form
            this.Refresh();

            ShowInfo("start");
        }

        public void FormResizer(int Width, int Height)
        {
            // Apply scaling factor
            int scaledClientW = Convert.ToInt32(Width * scalingFactor) + frameSize * 2;
            int scaledClientH = Convert.ToInt32(Height * scalingFactor) + frameSize * 2;

            // Set client size
            this.ClientSize = new Size((int)(scaledClientW), (int)(scaledClientH));
        }

        private void mitSize01_Click(object sender, EventArgs e)
        {
            AdjustClientSize(0, 0);
            currentRes = 0;
        }

        private void mitSize02_Click(object sender, EventArgs e)
        {
            AdjustClientSize(1, 1);
            currentRes = 1;
        }

        private void mitSize03_Click(object sender, EventArgs e)
        {
            AdjustClientSize(2, 2);
            currentRes = 2;
        }

        private void mitSize04_Click(object sender, EventArgs e)
        {
            AdjustClientSize(3, 3);
            currentRes = 3;
        }

        private void btnNextRes_Click(object sender, EventArgs e)
        {
            currentRes++;

            int res = currentRes;

            if (res > 3)
            {
                res = 0;
                currentRes = 0;
            }

            AdjustClientSize(res, res);
        }

        private void FS2MainForm_Move(object sender, EventArgs e)
        {
            // Get the actual screen based on the panel's left edge
            Screen currentScreen = GetScreenByPanelPosition(this.Left + frameSize);
            Rectangle screenBounds = currentScreen.WorkingArea; // Get usable screen area

            // Get panel's actual position inside form
            int panelLeft = this.Left + frameSize;
            int panelTop = this.Top + frameSize;
            int panelRight = panelLeft + panelScreenArea.Width;
            int panelBottom = panelTop + panelScreenArea.Height;

            // Only snap if we are close to the screen edges, not if already unsnapped
            if (Math.Abs(panelLeft - screenBounds.Left) <= snapMargin)
            {
                this.Left = screenBounds.Left - frameSize; // Snap to LEFT edge
            }
            else if (Math.Abs(panelRight - screenBounds.Right) <= snapMargin)
            {
                this.Left = screenBounds.Right - panelScreenArea.Width - frameSize; // Snap to RIGHT edge
            }
            else
            {
                // Allow more flexibility here if unsnapping
                // Do not apply snapping logic if the form is not close enough to the edge
                // You could allow the user to unsnap freely without being dragged back
                if (this.Left < screenBounds.Left + snapMargin || this.Left > screenBounds.Right - snapMargin)
                {
                    // Allow more flexibility for moving away
                    // No snapping behavior here
                }
            }

            // Snap to TOP edge of the CURRENT monitor
            if (Math.Abs(panelTop - screenBounds.Top) <= snapMargin)
            {
                this.Top = screenBounds.Top - frameSize; // Snap to TOP edge
            }
            else if (Math.Abs(panelBottom - screenBounds.Bottom) <= snapMargin)
            {
                this.Top = screenBounds.Bottom - panelScreenArea.Height - frameSize; // Snap to BOTTOM edge
            }
        }


        private Screen GetScreenByPanelPosition(int panelLeft)
        {
            foreach (var screen in Screen.AllScreens)
            {
                // Check if the panel's left edge falls within this screen
                if (panelLeft >= screen.Bounds.Left && panelLeft < screen.Bounds.Right)
                {
                    return screen; // Found the correct screen!
                }
            }

            return Screen.PrimaryScreen; // Fallback to primary screen
        }

        private void mitClear_Click(object sender, EventArgs e)
        {
            drawnRectangles.Clear();
            drawnArrows.Clear();
            drawnTexts.Clear();

            currentRectangle = new Rectangle(startPoint, new Size(0, 0));

            numbering = 1; // Reset numbering

            // Ensure the form is in focus
            this.Activate();

            panelScreenArea.Invalidate();
            panelScreenArea.Update();

            this.Refresh();

            ShowInfo("clear");
        }

        private void mitFulscreen_Click(object sender, EventArgs e)
        {
            Bitmap captureBitmap = CaptureCurrentMonitorScreenshot(this);
            Clipboard.SetImage(captureBitmap);

            // Save file if needed
            if (saveToFile)
            {
                SetFileName();
                SaveToFile(captureBitmap);
                ShowInfo("fullscreen");
            }

        }
    }
}
