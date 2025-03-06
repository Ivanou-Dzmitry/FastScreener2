using System.Drawing.Imaging;
using System.Resources;
using static FastScreener2.FSUtils;

namespace FastScreener2
{
    public partial class FS2MainForm : Form
    {
        //alpha color to remove 
        private Color ALPHA_KEY_COLOR = Color.FromArgb(255, 1, 0, 1);

        // Variables for dragging
        private bool dragging = false;
        private Point formStartLocation; // Store form position when dragging starts

        //for scaling
        public static float scalingFactor;

        // Create the Keyboard Hook
        KeyboardHook keyboardHook = new KeyboardHook();

        // Create the Mouse Hook
        MouseHook mouseHook = new MouseHook();

        private int frameSize = 32;

        public static int clickCount = 0;

        static Point relativePoint;
        private Rectangle currentRectangle;
        private Point startPoint;
        private bool isDrawing;
        private bool isLineDrawing;
        public static int numbering = 1; //for numbers


        public FS2MainForm()
        {
            InitializeComponent();

            //set transparent form
            this.BackColor = ALPHA_KEY_COLOR;
            this.TransparencyKey = ALPHA_KEY_COLOR;

            FS2SettingsManager.Load();

            //load UI values Checked true/false
            mitArrow.Checked = FS2SettingsManager.drawArrows;
            //btnArrowType.Enabled = FS2SettingsManager.drawArrows;
            chbArrow.Checked = FS2SettingsManager.drawArrows;
            ArrowPicUpdater(FS2SettingsManager.arrowType);

            //frame
            mitFrame.Checked = FS2SettingsManager.drawFrame;
            chbFrame.Checked = FS2SettingsManager.drawFrame;

            //guides
            mitGuidlines.Checked = FS2SettingsManager.drawGuides;
            chbGuides.Checked = FS2SettingsManager.drawGuides;
            //btnGuides.Enabled = FS2SettingsManager.drawGuides;

            //number
            mitNumber.Checked = FS2SettingsManager.drawNumber;
            chbNumbers.Checked = FS2SettingsManager.drawNumber;

            //file
            mitSaveFile.Checked = FS2SettingsManager.saveToFile;

            //center panel
            panelScreenArea.BackColor = Color.Transparent;

            // Attach the same event handlers to all 4 panels
            AttachDragEvents(panelDragBottomL);
            AttachDragEvents(panelDragBottomR);
            AttachDragEvents(panelDragTop);
            AttachDragEvents(panelDragLeft);
            AttachDragEvents(panelDragRightB);
            AttachDragEvents(panelDragRightT);

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
        }

        public void UpdateDebugLabel(string text)
        {
            labelDebug.Text = text;
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

        private void AttachDragEvents(Panel panel)
        {
            panel.MouseDown += Panel_MouseDown;
            panel.MouseMove += Panel_MouseMove;
            panel.MouseUp += Panel_MouseUp;
        }

        // Mouse Down: Start dragging
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            // Get the click position relative to the screen
            startPoint = ((Control)sender).PointToScreen(e.Location);
        }

        // Mouse Move: Move the Form
        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                // Get the new mouse position relative to the screen
                Point newPoint = ((Control)sender).PointToScreen(e.Location);

                // Calculate how much the mouse moved
                int offsetX = newPoint.X - startPoint.X;
                int offsetY = newPoint.Y - startPoint.Y;

                // Update the form's position
                this.Location = new Point(this.Left + offsetX, this.Top + offsetY);

                // Update startPoint for smooth movement
                startPoint = newPoint;

                labelDebug.Text = this.Location.ToString();
            }
        }

        private void SwapPanelsIfNeeded()
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

        // Mouse Up: Stop dragging
        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;

            // Check if form is moved into negative coordinates
            SwapPanelsIfNeeded();
        }


        private void CaptureScreen()
        {
            // bitmap size
            int bitmapWidth = panelScreenArea.Width;
            int bitmapHeight = panelScreenArea.Height;

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

            SetScaledBitmapToClipboard(captureBitmap, scalingFactor);

            labelDebug.Text = panelScreenArea.Location.Y + "/" + panelScreenArea.Location.X;
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
            clickCount++;

            ArrowPicUpdater(clickCount);

            ScaleButtonImage(btnArrowType, scalingFactor);
        }

        private void ArrowPicUpdater(int number)
        {

            if (clickCount > 4)
            {
                clickCount = 1;
                number = 1;
            }

            switch (number)
            {
                case 1:
                    btnArrowType.Image = FS2Resources.arrow_type01_icon;
                    FS2SettingsManager.arrowType = 1; clickCount = 1;
                    FS2SettingsManager.SetSetting("arrow_type", "1");
                    FS2SettingsManager.Save();
                    break;
                case 2:
                    btnArrowType.Image = FS2Resources.arrow_type02_icon;
                    FS2SettingsManager.arrowType = 2; clickCount = 2;
                    FS2SettingsManager.SetSetting("arrow_type", "2");
                    FS2SettingsManager.Save();
                    break;
                case 3:
                    btnArrowType.Image = FS2Resources.arrow_type03_icon;
                    FS2SettingsManager.arrowType = 3; clickCount = 3;
                    FS2SettingsManager.SetSetting("arrow_type", "3");
                    FS2SettingsManager.Save();
                    break;
                case 4:
                    btnArrowType.Image = FS2Resources.arrow_type04_icon;
                    FS2SettingsManager.arrowType = 4; clickCount = 4;
                    FS2SettingsManager.SetSetting("arrow_type", "4");
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
            FS2SettingsForm settingsForm = new FS2SettingsForm();

            settingsForm.ShowDialog();
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



        //hook mouse
        private void mouseHook_MMB(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            // important point
            relativePoint = panelScreenArea.PointToClient(Cursor.Position);

            //draw Frame
            if (FS2SettingsManager.drawFrame)
            {
                startPoint = new Point(relativePoint.X, relativePoint.Y);
                currentRectangle = new Rectangle(startPoint, new Size(0, 0));
                isDrawing = true;
            }

            isLineDrawing = true;

            //draw Arrow
            if (FS2SettingsManager.drawArrows && isLineDrawing)
            {
                // DrawArrow(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), relativePoint, arrowColor);
            }

            //draw Number
            if (FS2SettingsManager.drawNumber)
            {
                // DrawNumber(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), relativePoint, numberColor, numbering.ToString());
                numbering++;
            }
        }

        // Mouse Middle Button Up (End drawing)
        private void mouseHook_MouseUp(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            isDrawing = false;
            isLineDrawing = false;

            // Calculate the final rectangle
            int width = relativePoint.X - startPoint.X;
            int height = relativePoint.Y - startPoint.Y;

            // Create and add the rectangle
            Rectangle newRectangle = new Rectangle(
                    Math.Min(startPoint.X, relativePoint.X),
                    Math.Min(startPoint.Y, relativePoint.Y),
                    Math.Abs(width),
                    Math.Abs(height)
                );

            if (FS2SettingsManager.drawFrame)
            {
                //drawnRectangles.Add(newRectangle);
                //DrawFrame(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), relativePoint, frameColor);
            }

            //avoid draw line on mouse UP
            if (FS2SettingsManager.drawArrows && isLineDrawing)
            {
                //DrawArrow(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), relativePoint, arrowColor);
            }

        }

        private void mouseHook_MouseMove(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            isLineDrawing = false;

            if (isDrawing)
            {
                // important point
                relativePoint = panelScreenArea.PointToClient(Cursor.Position);

                int width = relativePoint.X - startPoint.X;
                int height = relativePoint.Y - startPoint.Y;

                currentRectangle = new Rectangle(startPoint.X, startPoint.Y, width, height);

                //DrawFrameCurrent(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), relativePoint, frameColor);

                if (FS2SettingsManager.drawFrame)
                {
                    //DrawFrame(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), relativePoint, frameColor);
                    panelScreenArea.Invalidate();
                }

            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FS2SettingsForm settingsForm = new FS2SettingsForm();

            settingsForm.ShowDialog();
        }

        private void buttonCloseForm_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Brown;
        }

        private void buttonCloseForm_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.SlateGray;
        }


    }
}
