using System.Data.SqlTypes;
using System.Drawing.Imaging;
using System.Resources;
using static FastScreener2.FSUtils;
using static FastScreener2.FS2SettingsManager;
using System.Diagnostics;

namespace FastScreener2
{
    public partial class FS2MainForm : Form
    {
        //alpha color to remove 
        private Color ALPHA_KEY_COLOR = Color.FromArgb(255, 1, 0, 1);

        // Variables for dragging
        // private Point formStartLocation; // Store form position when dragging starts

        //for scaling
        public static float scalingFactor;

        // Create the Keyboard Hook
        KeyboardHook keyboardHook = new KeyboardHook();

        // Create the Mouse Hook
        MouseHook mouseHook = new MouseHook();

        private int frameSize = 32;

        public static int clickInArrowCount = 0;
        public static int clickInFrameCount = 0;

        static Point relativePoint; //first click point
        public static Rectangle currentRectangle;
        private Point startPoint;
        private bool isDrawing;
        private bool isLineDrawing;
        public static int numbering = 1; //for numbers


        //for file
        private string stringURL = "";

        public static FS2MainForm Instance { get; private set; }

        public FS2MainForm()
        {
            InitializeComponent();

            Instance = this;  // Store the reference when the form is created

            //set transparent form
            this.BackColor = ALPHA_KEY_COLOR;
            this.TransparencyKey = ALPHA_KEY_COLOR;

            Rectangle virtScreenRect = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            foreach (Screen screen in Screen.AllScreens)
                virtScreenRect = Rectangle.Union(virtScreenRect, screen.Bounds);

            //Get virtual screen size
            FS2SettingsManager.virtScreenWidth = virtScreenRect.Width;
            FS2SettingsManager.virtScreenHeight = virtScreenRect.Height;

            FS2SettingsManager.Load();

            FormResizer(FS2SettingsManager.startResW, FS2SettingsManager.startResH);

            /*            //set client size
                        clientWidth = this.ClientSize.Width;
                        clientHeight = this.ClientSize.Height; //set height*/

            //load UI values Checked true/false
            mitArrow.Checked = FS2SettingsManager.drawArrows;
            //btnArrowType.Enabled = FS2SettingsManager.drawArrows;
            chbArrow.Checked = FS2SettingsManager.drawArrows;
            ArrowPicUpdater(FS2SettingsManager.arrowType);

            //frame
            mitFrame.Checked = FS2SettingsManager.drawFrame;
            chbFrame.Checked = FS2SettingsManager.drawFrame;
            FramePicUpdater(FS2SettingsManager.frameType);

            //guides
            mitGuidlines.Checked = FS2SettingsManager.drawGuides;
            chbGuides.Checked = FS2SettingsManager.drawGuides;
            //btnGuides.Enabled = FS2SettingsManager.drawGuides;

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
            // utils.AttachDragEvents(panelDragRightB);
            //utils.AttachDragEvents(panelDragRightT);
            utils.AttachDragEvents(panelDragTopR);
            utils.AttachDragEvents(panelDragTopL);

            //get scaling
            scalingFactor = GetScalingFactor(this);

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

            //scale buttons
            ScaleButtonImage(btnScreen, scalingFactor);
            ScaleButtonImage(buttonMainMenu, scalingFactor);
            ScaleButtonImage(btnArrowType, scalingFactor);
            ScaleButtonImage(chbNumbers, scalingFactor);
            ScaleButtonImage(chbGuides, scalingFactor);
            ScaleButtonImage(chbFrame, scalingFactor);

            ShowInfo("start");


            MenuItemUpdate();
        }

        public void FormResizer(int Width, int Height)
        {
            Width = Width + frameSize * 2;
            Height = Height + frameSize * 2;

            // Set client size
            this.ClientSize = new Size((int)(Width), (int)(Height));
        }

        private void PanelSize()
        {
            panelBottom.Height = frameSize;
            panelDragTop.Height = frameSize;

            panelDragLeft.Width = frameSize;
            panelRight.Width = frameSize;

            buttonCloseForm.Width = frameSize;
            buttonCloseForm.Height = frameSize;
        }


        public float GetScalingFactor(Form form)
        {
            using (Graphics g = form.CreateGraphics())
            {
                float dpiX = g.DpiX;
                return dpiX / 96f; // Assuming default DPI is 96
            }
        }


        public void ShowInfo(string type)
        {
            string leftTopPos = "Pos X:" + this.Location.X.ToString() + ", Y:" + this.Location.Y.ToString();
            string screenArea = "Size W:" + panelScreenArea.Width.ToString() + ", H:" + panelScreenArea.Height.ToString();

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
                saveFile = "to file and clipboard";
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
                labelDebug.Text = name + " | " + scale;
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


        private void CaptureScreen()
        {
            // bitmap size
            int bitmapWidth = panelScreenArea.Width;
            int bitmapHeight = panelScreenArea.Height;

            panelScreenArea.BorderStyle = BorderStyle.None;

            bool guideIsOn = false;

            //paint rect
            PaintEventArgs paintRect = new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle);


            if (drawGuides)
            {
                guideIsOn = true;
                RenderGuides(paintRect, panelScreenArea, ALPHA_KEY_COLOR);
            }

            //Creating a new Bitmap object
            Bitmap captureBitmap = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb);

            //Creating a Rectangle object which will capture our Current Screen
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;

            //Creating a New Graphics Object
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Position of screenshot
            int posY = this.Location.Y + 32; //set size
            int posX = this.Location.X + 32;

            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(posX, posY, 0, 0, captureRectangle.Size);
            captureGraphics.Dispose();


            //Saving the Image File (I am here Saving it in My E drive).
            if (saveToFile == true)
            {
                string appExeDir = Directory.GetCurrentDirectory();

                //check directory for files
                bool exists = Directory.Exists(appExeDir + "\\" + SUBPATH);

                // create if not exists
                if (!exists)
                    Directory.CreateDirectory(appExeDir + "\\" + SUBPATH);

                //datatime for random_name
                string currentTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

                string fileName = "";

                if (txtbName.Text != "")
                {
                    fileName = txtbName.Text + txtbNumber.Text + ".png";
                }
                else
                {
                    fileName = currentTime + "_screenshot.png";
                }

                //full path to file
                stringURL = appExeDir + "\\" + SUBPATH + "\\" + fileName;

                try
                {
                    captureBitmap.Save(stringURL, ImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show("Can't save screenshot to file! Path: " + stringURL, "FastScreener Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }



            SetScaledBitmapToClipboard(captureBitmap, scalingFactor);

            //labelDebug.Text = panelScreenArea.Location.Y + "/" + panelScreenArea.Location.X;

            panelScreenArea.BorderStyle = BorderStyle.FixedSingle;

            //dispose objects
            captureBitmap.Dispose();
            captureGraphics.Dispose();

            //rectangle data clear
            panelScreenArea.Invalidate();
            drawnRectangles.Clear();
            currentRectangle = new Rectangle(startPoint, new Size(0, 0));

            //claer arrows array
            drawnArrows.Clear();

            //text clear
            drawnTexts.Clear();

            ShowInfo("capture");

            //return nubering to start
            numbering = 1;

            //turn on grid again
            if (guideIsOn == true)
            {
                this.Refresh();
                RenderGuides(paintRect, panelScreenArea, guideColor);
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
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            keyboardHook.KeyDown -= new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            keyboardHook.Uninstall();

            mouseHook.MiddleButtonDown -= new MouseHook.MouseHookCallback(mouseHook_MMB);
            mouseHook.MiddleButtonUp -= new MouseHook.MouseHookCallback(mouseHook_MouseUp);
            mouseHook.MouseMove -= new MouseHook.MouseHookCallback(mouseHook_MouseMove);

            mouseHook.Uninstall();

            string res = panelScreenArea.Width + "," + panelScreenArea.Height;
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

            ScaleButtonImage(btnArrowType, scalingFactor);
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
                    FS2SettingsManager.SetSetting("arrow_type", "1");
                    FS2SettingsManager.Save();
                    break;
                case 2:
                    btnArrowType.Image = FS2Resources.arrow_type02_icon;
                    FS2SettingsManager.arrowType = 2; clickInArrowCount = 2;
                    FS2SettingsManager.SetSetting("arrow_type", "2");
                    FS2SettingsManager.Save();
                    break;
                case 3:
                    btnArrowType.Image = FS2Resources.arrow_type03_icon;
                    FS2SettingsManager.arrowType = 3; clickInArrowCount = 3;
                    FS2SettingsManager.SetSetting("arrow_type", "3");
                    FS2SettingsManager.Save();
                    break;
                case 4:
                    btnArrowType.Image = FS2Resources.arrow_type04_icon;
                    FS2SettingsManager.arrowType = 4; clickInArrowCount = 4;
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
            ScaleButtonImage(btnFrameType, scalingFactor);
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
                    FS2SettingsManager.frameType = 1; clickInFrameCount = 1;
                    FS2SettingsManager.SetSetting("frame_type", "1");
                    FS2SettingsManager.Save();
                    break;
                case 2:
                    btnFrameType.Image = FS2Resources.frame_locked_icon;
                    FS2SettingsManager.frameType = 2; clickInFrameCount = 2;
                    FS2SettingsManager.SetSetting("frame_type", "2");
                    FS2SettingsManager.Save();
                    break;
                default:
                    break;
            }


        }


        //for DPI
        private void ScaleButtonImage(Control targetControl, float scalingFactor)
        {
            if (targetControl is Button button && button.Image != null)
            {
                button.Image = FSUtils.ScaleImage(button.Image, scalingFactor);
            }

            if (targetControl is CheckBox chb && chb.Image != null)
            {
                chb.Image = FSUtils.ScaleImage(chb.Image, scalingFactor);
            }
        }

        private void mitSettings_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Form2 class
            //FormSet toolForm = new FormSet();
            formFS2Settings settingsForm = new formFS2Settings();

            settingsForm.ShowDialog();

            MenuItemUpdate();

            if (drawGuides == true)
            {
                this.Refresh();
                //DrawGrid(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), gridColor);
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

        private void chbFrame_CheckedChanged(object sender, EventArgs e)
        {

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

            //draw Frame
            if (FS2SettingsManager.drawFrame && FS2SettingsManager.frameType == 1)
            {
                startPoint = new Point(relativePoint.X, relativePoint.Y);
                currentRectangle = new Rectangle(startPoint, new Size(0, 0));
                isDrawing = true;
            }

            if (FS2SettingsManager.drawFrame && FS2SettingsManager.frameType == 2)
            {

                int width = FS2SettingsManager.frameWidth;
                int height = FS2SettingsManager.frameHeight;

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

            //paint rect
            PaintEventArgs paintRect = new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle);


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



            if (drawnArrows.Count > 0)
            {
                RenderArrows(paintRect);
            }


            if (FS2SettingsManager.drawFrame)
            {
                drawnRectangles.Add(newRectangle);
                RenderFrame(paintRect);
                //DrawFrame(new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle), relativePoint, FS2SettingsManager.frameColor);
            }


            if (drawnTexts.Count > 0)
            {
                RenderNumbers(paintRect);
            }

        }

        private void mouseHook_MouseMove(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            isLineDrawing = false;

            //paint rect
            PaintEventArgs paintRect = new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle);

            if (isDrawing)
            {
                // important point
                relativePoint = panelScreenArea.PointToClient(Cursor.Position);

                int width = 0;
                int height = 0;

                if (FS2SettingsManager.frameType == 1)
                {
                    width = relativePoint.X - startPoint.X;
                    height = relativePoint.Y - startPoint.Y;

                    currentRectangle = new Rectangle(startPoint.X, startPoint.Y, width, height);
                }


                if (drawnArrows.Count > 0)
                {
                    RenderArrows(paintRect);
                }

                if (FS2SettingsManager.drawFrame)
                {
                    DrawFrameCurrent(paintRect);
                    panelScreenArea.Invalidate();
                }

                if (drawnTexts.Count > 0)
                {
                    RenderNumbers(paintRect);
                }

            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            formFS2Settings settingsForm = new formFS2Settings();

            settingsForm.ShowDialog();
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


        /*        private void RangeTrackBar_ThumbMoved(object sender, EventArgs e)
                {
                    ShowInfo("frame");  // Now it works!            
                }*/


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
            if (FS2SettingsManager.drawArrows && drawnArrows.Count > 0)
            {
                RenderArrows(e);
            }

            //frame
            if (FS2SettingsManager.drawFrame && drawnRectangles.Count > 0)
            {
                RenderFrame(e);
            }

            //numbers
            if (FS2SettingsManager.drawNumber && drawnTexts.Count > 0)
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
    }
}
