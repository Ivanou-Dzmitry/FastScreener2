using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using static FastScreener2.FS2SettingsManager;
using static FastScreener2.FSUtils;
using static FastScreener2.MouseHook;


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
        //Line currentLine = new Line();
        private Point startPoint;
        private bool startPointSet = false;
        private Point currentPoint;

        //fix parasit move
        private Point? _lastStableMousePoint = null;
        private const int movementThreshold = 5; // pixels

        private bool isFrameDrawing;
        private bool isLineDrawing;
        //private int lineDirection = 0;

        private int dynamicArrowType;
        private int dynamicFrameType;

        public static int numbering = 1; //for numbers

        private Button? lastPressedButton; // Allow null assignment - Store the last pressed button

        public static bool isReseted = false;

        //for file
        private string stringURL = "";

        private string fileName = "";

        public static FS2MainForm Instance { get; private set; }
        private const int WM_DPICHANGED = 0x02E0;
        private float currentDpi = 96f; // Default DPI
        private Dictionary<Control, float> originalFontSizes = new();

        public static string drawnTextString = string.Empty;
        private bool isTextDialogOpen = false;
        private PointF textPoint;

        public static bool isAppActive;
        private Point previousValidPoint = Point.Empty;

        [DllImport("user32.dll")]
        private static extern int GetDpiForWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        //undo sys
        private static Stack<UndoItem> undoStack = new Stack<UndoItem>();

        private int arT, frT, guT, watP = 0; //for cycles

        string appName = "FastScreener";
        public static string version;

        public enum DrawType
        {
            Arrow,
            Rectangle,
            Text,
            String
        }

        public class UndoItem
        {
            public DrawType Type;
            public object Data;
        }

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
            //Debug.WriteLine(scalingFactor);

            //this.PerformAutoScale();

            //font resize
            FixControlFont(labelDebug, scalingFactor);
            FixControlFont(txtbName, scalingFactor);

            //MessageBox.Show($"{buttonMainMenu.Width}");

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
            //FramePicUpdater(FS2SettingsManager.frameType);

            //guides
            mitGuidlines.Checked = FS2SettingsManager.drawGuides;
            chbGuides.Checked = FS2SettingsManager.drawGuides;

            //number
            mitNumber.Checked = FS2SettingsManager.drawNumber;
            chbNumbers.Checked = FS2SettingsManager.drawNumber;

            //file
            mitSaveFile.Checked = FS2SettingsManager.saveToFile;
            chbSave.Checked = FS2SettingsManager.saveToFile;

            //text
            mitText.Checked = FS2SettingsManager.drawText;
            chbText.Checked = FS2SettingsManager.drawText;

            mitWatermark.Checked = FS2SettingsManager.drawWatermark;
            chbWatermark.Checked = FS2SettingsManager.drawWatermark;

            if (watermarkPath != string.Empty && drawWatermark)
            {
                try
                {
                    watermarkImage = Image.FromFile(watermarkPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to load watermark image:\n{ex.Message}",
                        "Error Loading Image",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }

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

            

            string versionFull = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion ?? "unknown";

            version = versionFull.Split('+')[0];

            ShowInfo("start");

            //upd version in file
            string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string helpFilePath = Path.Combine(projectRoot, "fs2_help.txt");
            //Debug.WriteLine(helpFilePath);
            UpdateHelpFileVersion(helpFilePath);

            MenuItemUpdate();

            this.KeyPreview = true;
            contextMenuMain.Focus();
         
            IconsSizeUpdate();

            //label
            labelDebug.Visible = showInfoLabel;
            mitShowInfo.Checked = showInfoLabel;

            PanelColor();

            Image checkmarkImage = FS2Resources.Checkmark;
            contextMenuMain.Renderer = new CustomCheckRenderer(checkmarkImage);

            CenterLabelInPanel();

            LoadFileNameHistory();

            //hack lenght
            string wideSpace = "\u2003\u2003\u2003\u2003\u2003\u2003";
            mitWatermark.Text = "Watermark" + wideSpace;
        }


        public void IconsSizeUpdate()
        {
            //buttons
            SetControlImage(btnSettings, "settings_icon");
            SetControlImage(buttonMinimizeForm, "minimize_icon");
            SetControlImage(buttonCloseForm, "close_icon");
            SetControlImage(btnNextRes, "res_cycle_icon");
            SetControlImage(btnScreen, "screen_icon");
            SetControlImage(buttonMainMenu, "menu_icon");
            //SetControlImage(, "menu_icon");


            //checkboxes
            SetControlImage(chbSave, "save_icon");
            SetControlImage(chbNumbers, "number_icon");

            //SetControlImage(chbArrow, "arrow_icon");
            ApplyArrowType(arrowType);

            //frame
            if (frameType == 1)
            {
                SetFrameType(1);
            }
            else
            {
                SetFrameType(2);
            }
            
            SetControlImage(chbGuides, "guides_icon");
            SetControlImage(chbText, "text_icon");
            SetControlImage(chbWatermark, "watermark_icon");

            SetControlImage(txtbName, "");

            //Debug.WriteLine($"{txtbName.Width}");

            //Debug.WriteLine("Icons updated");

            NameFieldPos(); //call 1
        }

        private void SetControlImage(Control control, string resourceName)
        {
            // Determine the size of the icon based on scaling factor
            int iconSize = scalingFactor switch
            {
                1 => 24,
                1.5f => 32,
                2 => 48,
                _ => 16 // Default to 16px if scalingFactor is unexpected
            };

            int controlW = scalingFactor switch
            {
                1 => 31,
                1.5f => 46,
                2 => 61,
                _ => 31 // Default to 16px if scalingFactor is unexpected
            };

            int controlH = scalingFactor switch
            {
                1 => 35,
                1.5f => 52,
                2 => 69,
                _ => 31 // Default to 16px if scalingFactor is unexpected
            };

            
            if( control.Name != "txtbName")
            {
                control.Width = controlW;
                control.Height = controlH;
            }

            if (control.Name == "txtbName")
            {
                if (scalingFactor == 1)
                    control.Width = 347;
                else if (scalingFactor == 1.5)
                    control.Width = 520;
                else if (scalingFactor == 2)
                    control.Width = 693;
            }

            //Debug.WriteLine($"Icon size: {iconSize}"); // Debugging line to check size

            //MessageBox.Show($"{iconSize}");

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


        private void PanelColor()
        {
            panelDragTop.BackColor = panelColor;
            panelDragLeft.BackColor = panelColor;

            panelDragBottomL.BackColor = panelColor;
            panelDragBottomR.BackColor = panelColor;

            panelDragTopR.BackColor = panelColor;
            panelDragTopL.BackColor = panelColor;
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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            using (Graphics g = this.CreateGraphics())
            {
                currentDpi = g.DpiX;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DPICHANGED)
            {
                int newDpi = (int)(m.WParam.ToInt64() & 0xFFFF);
                var suggestedRect = Marshal.PtrToStructure<RECT>(m.LParam);

                // Move and resize the window to suggested bounds
                this.Bounds = Rectangle.FromLTRB(
                    suggestedRect.left,
                    suggestedRect.top,
                    suggestedRect.right,
                    suggestedRect.bottom
                );

                // Scale the entire form based on DPI change
                //float scaleFactor = newDpi / currentDpi;

                //this.Scale(new SizeF(scaleFactor, scaleFactor)); //dubl
                currentDpi = newDpi;
                
                scalingFactor = GetScalingFactor(this);
                
                //scale icons
                IconsSizeUpdate();                
                
                //new frame size
                frameSize = Convert.ToInt32(32 * scalingFactor);
                
                //resize panels
                PanelSize();
                
                //font resize
                float dpiScale = newDpi / 96f;
                FixControlFont(labelDebug, dpiScale);
                FixControlFont(txtbName, dpiScale);

                this.PerformAutoScale();

                //recalc position Call-2
                NameFieldPos();

                panelScreenArea.Invalidate();
            }

            base.WndProc(ref m);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private void FixControlFont(Control ctrl, float dpiScale)
        {

            if (ctrl == null || ctrl.Font == null) return;
            
            //font size
            if(dpiScale == 1.0)
            {
                ctrl.Font = new Font(ctrl.Font.FontFamily, 6.5F, ctrl.Font.Style);
            }
            else if(dpiScale == 1.5)
            {
                ctrl.Font = new Font(ctrl.Font.FontFamily, 8.25F, ctrl.Font.Style);
            }
            else if (dpiScale == 2.0)
            {
                ctrl.Font = new Font(ctrl.Font.FontFamily, 11.0F, ctrl.Font.Style);
            }
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

            if (scalingFactor == 1 || !dpiScaleMulti)
            {
                screenArea = "Size W:" + panelW + ", H:" + panelH;
            }
            else
            {
                screenArea = $"Size W: {panelW} ({panelWS}), H: {panelH} ({panelHS})";
            }

            string fullName = $"{appName} {version}";
            string scale = "Scale: " + scalingFactor;

            string frameSize = "";

            if (rangeTrackBar != null)
            {
                frameSize = "Bar bottom: " + pnlBarBottom.Height + ", top: " + pnlBarTop.Height;
            }

            string saveFile = "";

            string undoActions = undoStack.Count.ToString();

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
                labelDebug.Text = leftTopPos + " | " + screenArea + " | Elements: " + undoActions + " | " + scale;
            }

            if (type == "start")
            {
                labelDebug.Text = fullName + " | " + screenArea + " | " + scale;
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

            if (type == "frame_size")
            {
                int width = relativePoint.X - startPoint.X;
                int height = relativePoint.Y - startPoint.Y;
                labelDebug.Text = $"Frame (WxH): {width}x{height}";
            }

            CenterLabelInPanel();
        }

        public void SwapPanelsIfNeeded()
        {
            var screen = Screen.FromControl(this);
            var screenBounds = screen.Bounds;
            var formBounds = this.Bounds;

            bool isOutsideLeft = formBounds.Left < screenBounds.Left;
            bool isOutsideTop = formBounds.Top < screenBounds.Top;

            // Horizontal swap only if form is crossing the left screen edge
            if (isOutsideLeft)
            {
                panelDragLeft.Dock = DockStyle.Right;
                panelRight.Dock = DockStyle.Left;
            }
            else
            {
                panelDragLeft.Dock = DockStyle.Left;
                panelRight.Dock = DockStyle.Right;
            }

            // Vertical swap only if form is crossing the top screen edge
            if (isOutsideTop)
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

        //!important Main Screen
        private void CaptureScreen()
        {

            float scale = 1;

            if (dpiScaleMulti == true)
            {
                scale = scalingFactor; //scaling factor
            }

            //Debug.WriteLine($"Scale: {scale}, frameSize: {frameSize}");


            int bitmapWidth = panelScreenArea.Width;
            int bitmapHeight = panelScreenArea.Height;

            panelScreenArea.BorderStyle = BorderStyle.None;

            bool guideIsOn = drawGuides;
            
            if (guideIsOn)
            {
                DrawGuideStatus(); //off
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

                SetScaledBitmapToClipboard(captureBitmap, scale);
            }

            panelScreenArea.BorderStyle = BorderStyle.FixedSingle;

            if (clearAfterScreen)
                AfterScreenRoutine();

            if (guideIsOn)
            {
                DrawGuideStatus(); //on
            }

            LogScreenshot(DateTime.Now.ToString("yyyy-MM-dd"), bitmapWidth, bitmapHeight, fileName);

            if (txtbName.Text != string.Empty)
            {
                SaveFileNameToHistory(txtbName.Text);
            }

            ShowInfo("capture");
        }

        private void AfterScreenRoutine()
        {
            // Clear objects to free memory
            panelScreenArea.Invalidate();
            drawnRectangles.Clear();
            currentRectangle = new Rectangle(0, 0, 0, 0);
            drawnArrows.Clear();
            drawnTexts.Clear();

            undoStack.Clear();
            UpdateUndoMenu(); // Optional: updates mitUndo.Enabled state

            numbering = 1; // Reset numbering
        }

        private void SaveToFile(Bitmap captureBitmap)
        {

            float scale = 1;
            if (dpiScaleMulti == true)
            {
                scale = scalingFactor; //scaling factor
            }

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
                int newWidth = (int)(captureBitmap.Width / scale);
                int newHeight = (int)(captureBitmap.Height / scale);

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

            if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            {
                mitMax_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }

            if (keyData == (Keys.Control | Keys.Z))
            {
                UndoAction();
                return true; // Mark as handled
            }

            //size cycle
            if (keyData == (Keys.Control | Keys.Right))
            {
                btnNextRes_Click(this, EventArgs.Empty);
                btnNextRes.BackColor = Color.WhiteSmoke;
                lastPressedButton = btnNextRes; // Store button reference
                return true; // Mark as handled
            }

            arT = arrowType;

            //arrow cycle
            if (keyData == (Keys.Control | Keys.Up))
            {
                arT++;
                if (arT > 4)
                    arT = 1;
                ApplyArrowType(arT);
                return true; // Mark as handled
            }

            frT = frameType;

            //frame cycle
            if (keyData == (Keys.Control | Keys.Down))
            {
                frT++;
                if (frT > 2)
                    frT = 1;

                if (frT == 1)
                    SetFrameType(1);

                if (frT == 2)
                    SetFrameType(2);

                return true; // Mark as handled
            }

            guT = guidelineType;

            //guidlines cycle
            if (keyData == (Keys.Control | Keys.Left))
            {
                guT++;
                if (guT > 3)
                    guT = 1;

                guidelineType = guT;

                SetSetting("guideline_type", guidelineType.ToString());

                if (drawGuides == true)
                {
                    this.Refresh();
                }

                return true; // Mark as handled
            }


            //watermark pos cycle
            if (keyData == (Keys.Control | Keys.Home))
            {
                watP++;
                if (watP > 4)
                    watP = 1;

                UpdateWatermarkPosition(watP);

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

            if (keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                mitClear_Click(this, EventArgs.Empty);
                return true; // Mark as handled
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                mitText_Click(this, EventArgs.Empty);
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

            string res;

            // Check if the current resolution is in resWorked
            bool found = false;
            for (int i = 0; i < 4; i++)
            {
                if (resWorked[0, i] == scaledW && resWorked[1, i] == scaledH)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                res = scaledW + "," + scaledH;
            }
            else
            {
                res = resWorked[0, 0] + "," + resWorked[1, 0];
            }

            //save
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

            //tex
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
                    //btnArrowType.Image = FS2Resources.arrow_type01_icon;
                    FS2SettingsManager.arrowType = 1; clickInArrowCount = 1;
                    //SetControlImage(btnArrowType, "arrow_type01_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "1");
                    FS2SettingsManager.Save();
                    break;
                case 2:
                    //btnArrowType.Image = FS2Resources.arrow_type02_icon;
                    FS2SettingsManager.arrowType = 2; clickInArrowCount = 2;
                    //SetControlImage(btnArrowType, "arrow_type02_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "2");
                    FS2SettingsManager.Save();
                    break;
                case 3:
                    //btnArrowType.Image = FS2Resources.arrow_type03_icon;
                    FS2SettingsManager.arrowType = 3; clickInArrowCount = 3;
                    //SetControlImage(btnArrowType, "arrow_type03_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "3");
                    FS2SettingsManager.Save();
                    break;
                case 4:
                    //btnArrowType.Image = FS2Resources.arrow_type04_icon;
                    FS2SettingsManager.arrowType = 4; clickInArrowCount = 4;
                    //SetControlImage(btnArrowType, "arrow_type04_icon");
                    FS2SettingsManager.SetSetting("arrow_type", "4");
                    FS2SettingsManager.Save();
                    break;

                default:
                    break;
            }
        }

        private void mitSettings_Click(object sender, EventArgs e)
        {
            // Store the original resolution values before showing the form
            int oldWidth = resWorked[0, currentRes];
            int oldHeight = resWorked[1, currentRes];

            // Create a new instance of the Form2 class            
            formFS2Settings settingsForm = new formFS2Settings();

            settingsForm.ShowDialog();

            MenuItemUpdate();

            // Get new resolution values
            int newWidth = resWorked[0, currentRes];
            int newHeight = resWorked[1, currentRes];

            // Compare and resize if needed
            if (oldWidth != newWidth || oldHeight != newHeight)
            {
                FormResizer(newWidth, newHeight);
            }


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

            PanelColor();

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

        private void chbArrow_Click(object sender, EventArgs e)
        {
            DrawArrowStatus();
        }

        private void DrawArrowStatus()
        {
            if (drawFrame)
                DrawFrameStatus();

            if (drawNumber)
                DrawNumberStatus();

            if (drawText)
                DrawTextStatus();

            ToggleStatus(mitArrow, ref FS2SettingsManager.drawArrows, "Arrows turned ON", "Arrows turned OFF", "draw_arrows", chbArrow, false);
        }

        //FRAME
        private void mitFrame_Click(object sender, EventArgs e)
        {
            DrawFrameStatus();
        }

        private void DrawFrameStatus()
        {
            if (drawArrows)
                DrawArrowStatus();

            if (drawNumber)
                DrawNumberStatus();

            if (drawText)
                DrawTextStatus();

            ToggleStatus(mitFrame, ref FS2SettingsManager.drawFrame, "Frame turned ON", "Frame turned OFF", "draw_frame", chbFrame, false);
        }

        //GUIDES
        private void mitGuidlines_Click(object sender, EventArgs e)
        {
            DrawGuideStatus(); //1 click
        }

        private void chbGuides_Click(object sender, EventArgs e)
        {
            DrawGuideStatus(); //2 click
        }

        private void DrawGuideStatus()
        {
            //paint rect
            PaintEventArgs paintRect = new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle);

            ToggleStatus(mitGuidlines, ref FS2SettingsManager.drawGuides, "Guides turned ON", "Guides turned OFF", "draw_guidelines", chbGuides, false);

            //Debug.WriteLine("DG" + drawGuides);

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
            if (drawArrows)
                DrawArrowStatus();

            if (drawFrame)
                DrawFrameStatus();

            if (drawText)
                DrawTextStatus();

            ToggleStatus(mitNumber, ref FS2SettingsManager.drawNumber, "Numbers turned ON", "Numbers turned OFF", "draw_number", chbNumbers, false);
        }

        //SAVE
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

        //TEXT
        private void chbText_Click(object sender, EventArgs e)
        {
            DrawTextStatus();
        }

        private void mitText_Click(object sender, EventArgs e)
        {
            DrawTextStatus();
        }

        private void DrawTextStatus()
        {
            if (drawText)
            {
                ToggleStatus(mitText, ref FS2SettingsManager.drawText, "Text turned ON", "Text turned OFF", "draw_text", chbText, false);
            }
            else
            {
                if (drawArrows)
                    DrawArrowStatus();

                if (drawFrame)
                    DrawFrameStatus();

                if (drawNumber)
                    DrawNumberStatus();

                ToggleStatus(mitText, ref FS2SettingsManager.drawText, "Text turned ON", "Text turned OFF", "draw_text", chbText, false);
            }
        }

        //hook mouse MMB !Important
        private void mouseHook_MMB(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            //used panel
            Panel usedPanel = panelScreenArea;

            //get point in window
            bool inWin = usedPanel.ClientRectangle.Contains(usedPanel.PointToClient(Cursor.Position));

            isFrameDrawing = false;
            isLineDrawing = false;

            //Debug.WriteLine(inWin);

            //paint rect
            PaintEventArgs paintRect = new PaintEventArgs(panelScreenArea.CreateGraphics(), panelScreenArea.ClientRectangle);

            // important point
            if (inWin)
                relativePoint = usedPanel.PointToClient(Cursor.Position);

            //set zero start
            startPoint = new Point(0, 0);

            dynamicFrameType = 0;

            //draw free Frame t1
            if (drawFrame && frameType == 1 && inWin)
            {
                startPoint = new Point(relativePoint.X, relativePoint.Y);
                currentRectangle = new Rectangle(startPoint, new Size(0, 0));
                isFrameDrawing = true;
                dynamicFrameType = 1;
            }

            //fixed frame t2
            if (drawFrame && frameType == 2 && inWin)
            {
                startPoint = new Point(relativePoint.X, relativePoint.Y);
                AddFixedFrame();
            }

            //Debug.WriteLine($"DOWN: DT {dynamicFrameType} /FT {frameType}");

            //draw Arrow
            if (FS2SettingsManager.drawArrows && inWin)
            {
                isLineDrawing = true; //turn on line draw

                startPoint = new Point(relativePoint.X, relativePoint.Y);

                dynamicArrowType = 0;

                //set arrow on click
                SetArrow(relativePoint, FS2SettingsManager.arrowColor);
                RenderArrows(paintRect);

                //add undo arrow
                undoStack.Push(new UndoItem { Type = DrawType.Arrow, Data = drawnArrows[drawnArrows.Count - 1] });
            }

            //draw Number
            if (FS2SettingsManager.drawNumber && inWin)
            {
                AddNumber(numbering.ToString(), relativePoint);
                RenderNumbers(paintRect);
                numbering++;

                //add undo number
                undoStack.Push(new UndoItem { Type = DrawType.Text, Data = drawnTexts[drawnTexts.Count - 1] });
            }


            if (drawText && inWin)
            {
                textPoint = usedPanel.PointToClient(Cursor.Position);
            }

            //draw TEXT
            if (this.WindowState != FormWindowState.Minimized && drawText && !isTextDialogOpen && inWin)
            {
                isTextDialogOpen = true;
                isAppActive = false; //for mouse hook

                //call text diallog
                string userText = PromptForText(out textColor, out textSize, out textFont);

                if (!string.IsNullOrWhiteSpace(userText))
                {
                    drawnTextString = userText;

                    // Remove any existing text undo entries
                    RemovePreviousTextFromUndo();

                    // Add new text to undo stack
                    undoStack.Push(new UndoItem { Type = DrawType.String, Data = drawnTextString });

                    usedPanel.Invalidate(); // Force redraw
                }

                isTextDialogOpen = false;
                isAppActive = true; //for mouse hook                
            }

            ShowInfo("drag");

            //undo sys
            UpdateUndoMenu();
        }

        // Mouse Middle Button Up (End drawing)
        private void mouseHook_MouseUp(MouseHook.MSLLHOOKSTRUCT mouse)
        {
            //get point in window
            bool inWin = panelScreenArea.ClientRectangle.Contains(panelScreenArea.PointToClient(Cursor.Position));

            isFrameDrawing = false;
            isLineDrawing = false;

            int finalWidth = 0;
            int finalHeight = 0;

            // Calculate the final rectangle - Free type
            if (FS2SettingsManager.frameType == 1)
            {
                finalWidth = relativePoint.X - startPoint.X;
                finalHeight = relativePoint.Y - startPoint.Y;
            }

            // Create and add the rectangle
            Rectangle newRectangle = new Rectangle(
                    Math.Min(startPoint.X, relativePoint.X),
                    Math.Min(startPoint.Y, relativePoint.Y),
                    Math.Abs(finalWidth),
                    Math.Abs(finalHeight)
                );

            //set dynamic arrow value
            if (dynamicArrowType != 0 && FS2SettingsManager.drawArrows)
            {
                ApplyArrowType(dynamicArrowType);
                UndoAction();
                SetArrow(relativePoint, FS2SettingsManager.arrowColor);
                //add undo arrow
                undoStack.Push(new UndoItem { Type = DrawType.Arrow, Data = drawnArrows[drawnArrows.Count - 1] });
            }

            //Debug.WriteLine($"UP: DT {dynamicFrameType} /FT {frameType}");
            //Debug.WriteLine($"SIZE: W {newRectangle.Width} /H {newRectangle.Height}");

            bool smallRect = newRectangle.Width < MIN_FIXED_FRAME_W && newRectangle.Height < MIN_FIXED_FRAME_H;

            //Debug.WriteLine(smallRect);

            //for free rect
            if (drawFrame && inWin)
            {
                //switch from 2 to 1
                if (dynamicFrameType != frameType)
                {
                    UndoAction();
                    SetFrameType(frameType); //set type
                }

                if (smallRect && dynamicFrameType != 2)
                {
                    SetFrameType(2);
                    AddFixedFrame();
                    isFrameDrawing = false;
                }

                if (frameType == 1 && !smallRect)
                {
                    drawnRectangles.Add(newRectangle);
                    //undo
                    undoStack.Push(new UndoItem { Type = DrawType.Rectangle, Data = newRectangle });
                    //RenderFrame(paintRect);
                }

            }

            panelScreenArea.Invalidate();

            this.Activate();

            //undo sys
            UpdateUndoMenu();

            ShowInfo("drag");
        }

        //!Important MOVE
        private void mouseHook_MouseMove(MouseHook.MSLLHOOKSTRUCT mouse)
        {

            if (IsAppInForeground())
            {
                isAppActive = true;
            }
            else
            {
                // Your application is not in the foreground
                isAppActive = false;
            }

            //get point in window
            bool inWin = panelScreenArea.ClientRectangle.Contains(panelScreenArea.PointToClient(Cursor.Position));

            if (isFrameDrawing)
            {
                // important point
                if (inWin)
                {
                    relativePoint = panelScreenArea.PointToClient(Cursor.Position);

                    if (_lastStableMousePoint.HasValue)
                    {
                        int dx = relativePoint.X - _lastStableMousePoint.Value.X;
                        int dy = relativePoint.Y - _lastStableMousePoint.Value.Y;

                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        if (distance < movementThreshold)
                            return; // Ignore parasitic micro-movement
                    }

                    // Update stable point
                    _lastStableMousePoint = relativePoint;

                    // Set start point once movement is stable
                    if (!startPointSet)
                    {
                        startPoint = relativePoint;
                        startPointSet = true;
                        return; // Start tracking from next move
                    }

                    // Calculate width and height
                    int width = (relativePoint.X - startPoint.X);
                    int height = (relativePoint.Y - startPoint.Y);

                    //Debug.WriteLine($"W{width} /H{height} / DT {dynamicFrameType}");

                    //Debug.WriteLine($"{width} / {height} ");

                    //get small size
                    bool smallRect = Math.Abs(width) > frameWidth || Math.Abs(height) > frameHeight;

                    //switch to free
                    if (smallRect && dynamicFrameType == 2)
                    {
                        frameType = 1;
                    }

                    if (frameType == 1 && relativePoint != Point.Empty)
                    {
                        ShowInfo("frame_size");
                        currentRectangle = new Rectangle(startPoint.X, startPoint.Y, width, height);
                    }

                    panelScreenArea.Invalidate();
                }
            }

            //used panel
            Panel usedPanel = panelScreenArea;

            //line draw
            if (isLineDrawing)
            {
                if (panelScreenArea.Bounds.Contains(panelScreenArea.PointToClient(Cursor.Position)))
                {
                    currentPoint = usedPanel.PointToClient(Cursor.Position);
                    panelScreenArea.Invalidate();
                }
                else
                {
                    currentPoint = Point.Empty;
                }
            }
        }

        private void AddFixedFrame()
        {
            //scale fixed frame
            int width = (int)(frameWidth * scalingFactor);
            int height = (int)(frameHeight * scalingFactor);

            startPoint = new Point(relativePoint.X - width / 2, relativePoint.Y - height / 2);
            currentRectangle = new Rectangle(startPoint, new Size(width, height));

            drawnRectangles.Add(currentRectangle);

            //add fixed
            undoStack.Push(new UndoItem { Type = DrawType.Rectangle, Data = currentRectangle });

            dynamicFrameType = 2; //set dyn type
            isFrameDrawing = true;
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

        //REPAINT !Important
        private void panelScreenArea_Paint(object sender, PaintEventArgs e)
        {
            // Render watermark in top-left
            if (watermarkImage != null && drawWatermark)
            {
                RenderWatermark(e);
            }

            if (drawGuides)
            {
                RenderGuides(e, panelScreenArea, guideColor);
            }

            //temp line for arrow
            if (isLineDrawing)
            {
                dynamicArrowType = DrawCurrentLine(e, startPoint, currentPoint);
                //Debug.WriteLine(dynamicArrowType);
            }

            //arrow
            if (FS2SettingsManager.drawArrows || drawnArrows.Count > 0)
            {
                RenderArrows(e);
            }

            //Debug.WriteLine(drawnRectangles.Count);

            //frame
            if (drawFrame || drawnRectangles.Count > 0)
            {
                RenderFrame(e);

            }

            //drawing frame
            if (isFrameDrawing)
            {
                DrawFrameCurrent(e);
            }

            //check in win
            bool inWin = panelScreenArea.ClientRectangle.Contains(panelScreenArea.PointToClient(Cursor.Position));

            //text !string.IsNullOrEmpty(drawnTextString)
            if (drawText && inWin && relativePoint != Point.Empty)
            {
                previousValidPoint = relativePoint; // Save valid point
                RenderText(e, drawnTextString, textPoint, textFont, textColor);
            }
            else
            {
                RenderText(e, drawnTextString, previousValidPoint, textFont, textColor);
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

            //MessageBox.Show($"{scalingFactor}, Frame{frameSize}");

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

            CenterLabelInPanel();
        }

        private void FS2MainForm_Move(object sender, EventArgs e)
        {
            CornerSnapper();
            CenterLabelInPanel();
        }

        private void CornerSnapper()
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

            return Screen.PrimaryScreen ?? Screen.AllScreens.First(); // Fallback to primary screen
        }

        private void mitClear_Click(object sender, EventArgs e)
        {
            drawnRectangles.Clear();
            drawnArrows.Clear();
            drawnTexts.Clear();
            undoStack.Clear();

            drawnTextString = string.Empty;

            currentRectangle = new Rectangle(startPoint, new Size(0, 0));

            numbering = 1; // Reset numbering

            // Ensure the form is in focus
            this.Activate();

            panelScreenArea.Invalidate();
            panelScreenArea.Update();

            this.Refresh();

            ShowInfo("clear");
        }

        //full screen
        private void mitFulscreen_Click(object sender, EventArgs e)
        {

            bool guideIsOn = drawGuides;

            if (guideIsOn)
            {
                DrawGuideStatus(); //off
            }

            labelDebug.Visible = false;

            Bitmap captureBitmap = CaptureCurrentMonitorScreenshot(this, panelScreenArea);
            Clipboard.SetImage(captureBitmap);

            if (guideIsOn)
            {
                DrawGuideStatus(); //on
            }

            labelDebug.Visible = true;

            // Save file if needed
            if (saveToFile)
            {
                SetFileName();
                SaveToFile(captureBitmap);
                LogScreenshot(DateTime.Now.ToString("yyyy-MM-dd"), captureBitmap.Width, captureBitmap.Height, fileName);
                ShowInfo("fullscreen");
            }

            if (clearAfterScreen)
                AfterScreenRoutine();
        }

        private void mitShowInfo_Click(object sender, EventArgs e)
        {
            // Toggle the flag
            showInfoLabel = !showInfoLabel;

            mitShowInfo.Checked = showInfoLabel;

            // Set label visibility to match
            labelDebug.Visible = showInfoLabel;

            FS2SettingsManager.SetSetting("show_info_label", showInfoLabel.ToString().ToLower());
            FS2SettingsManager.Save();
        }

        private void mitMax_Click(object sender, EventArgs e)
        {
            ScreenHelper.MaximizeFormToCurrentMonitor(this);
            SwapPanelsIfNeeded();
            // Refresh the form
            this.Refresh();
        }


        private void chbWatermark_Click(object sender, EventArgs e)
        {
            DrawWatermarkStatus();
        }

        private void mitWatermark_Click(object sender, EventArgs e)
        {
            DrawWatermarkStatus();
        }

        private void DrawWatermarkStatus()
        {

            ToggleStatus(mitWatermark, ref FS2SettingsManager.drawWatermark, "Watermark turned ON", "Watermark turned OFF", "draw_watermark", chbWatermark, false);

            if (chbWatermark.Checked)
            {
                //Checkbox was checked user is trying to enable watermark
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                    openFileDialog.Title = "Select Watermark Image";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        watermarkPath = openFileDialog.FileName;

                        //Dispose old image if needed
                        watermarkImage?.Dispose();
                        watermarkImage = Image.FromFile(watermarkPath);

                        drawWatermark = true;
                        panelScreenArea.Invalidate(); // Refresh to show watermark

                        FS2SettingsManager.SetSetting("watermark_path", watermarkPath);
                        FS2SettingsManager.Save();
                    }
                    else
                    {
                        //User canceled uncheck and don't draw watermark
                        chbWatermark.Checked = false;
                        mitWatermark.Checked = false;
                        drawWatermark = false;
                    }
                }
            }
            else
            {
                //Checkbox was unchecked disable watermark
                drawWatermark = false;
                panelScreenArea.Invalidate(); // Refresh to remove watermark
            }
        }

        //"top-left", "top-right", "bottom-left", "bottom-right"
        private void UpdateWatermarkPosition(int position)
        {
            string posString = position switch
            {
                1 => "bottom-left",
                2 => "top-left",
                3 => "bottom-right",
                4 => "top-right",
                _ => "bottom-left" // default fallback
            };

            watermarkPosition = posString;
            panelScreenArea.Invalidate();
            FS2SettingsManager.SetSetting("watermark_position", watermarkPosition);
            FS2SettingsManager.Save();
        }

        private void mitBL_Click(object sender, EventArgs e)
        {
            UpdateWatermarkPosition(1); //bottom-left
        }

        private void mitTL_Click(object sender, EventArgs e)
        {
            UpdateWatermarkPosition(2); //top-left
        }

        private void mitBR_Click(object sender, EventArgs e)
        {
            UpdateWatermarkPosition(3); //top-right
        }

        private void mitTR_Click(object sender, EventArgs e)
        {
            UpdateWatermarkPosition(4); //bottom-left
        }

        private void SetFrameType(int type)
        {
            string iconName;
            string tooltipText;

            if (type == 1)
            {
                iconName = "frame_unlocked_icon";
                tooltipText = "Free frame";
            }
            else
            {
                iconName = "frame_locked_icon";
                tooltipText = "Fixed frame";
            }

            frameType = type;
            SetControlImage(chbFrame, iconName);
            SetSetting("frame_type", type.ToString());
            Save();
            toolTipFS.SetToolTip(chbFrame, tooltipText + ". Change type: Ctrl+Down arrow");
        }

        private void mitFreeFrame_Click(object sender, EventArgs e)
        {
            SetFrameType(1);
        }

        private void mitFixedFrame_Click(object sender, EventArgs e)
        {
            SetFrameType(2);
        }

        private void ApplyArrowType(int type)
        {
            string iconName = $"arrow_type0{type}_icon";
            //btnArrowType.Image = (Image)FS2Resources.ResourceManager.GetObject(iconName);
            FS2SettingsManager.arrowType = type;
            SetControlImage(chbArrow, iconName);
            FS2SettingsManager.SetSetting("arrow_type", type.ToString());
            FS2SettingsManager.Save();
        }

        private void mitArrowType01_Click(object sender, EventArgs e)
        {
            ApplyArrowType(1);
        }

        private void mitArrowType02_Click(object sender, EventArgs e)
        {
            ApplyArrowType(2);
        }

        private void mitArrowType03_Click(object sender, EventArgs e)
        {
            ApplyArrowType(3);
        }

        private void mitArrowType04_Click(object sender, EventArgs e)
        {
            ApplyArrowType(4);
        }

        private void chbArrow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int x = chbArrow.Left + chbArrow.Width + 1;
                cmenuArrow.Show(chbArrow, new Point(x, 0));
            }
        }

        private void chbFrame_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int x = chbFrame.Left + chbFrame.Width + 1;
                cmenuFrame.Show(chbFrame, new Point(x, 0));
            }
        }

        private void chbWatermark_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int x = chbWatermark.Left + chbWatermark.Width + 1;
                cmenuWatermark.Show(chbWatermark, new Point(x, 0));
            }
        }

        private void chbNumbers_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                using (FontDialog fontDialog = new FontDialog())
                {
                    // Try to get saved font family from settings
                    //string savedFamily = numberFontFamily;
                    if (string.IsNullOrWhiteSpace(numberFontFamily))
                    {
                        numberFontFamily = "Segoe UI"; // fallback
                    }

                    // Try to use saved font family, fallback if invalid
                    try
                    {
                        fontDialog.Font = new Font(numberFontFamily, numberFontSize);
                    }
                    catch
                    {
                        fontDialog.Font = new Font("Segoe UI", numberFontSize);
                    }

                    // Show font dialog
                    if (fontDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFamily = fontDialog.Font.FontFamily.Name;
                        SetSetting("number_font_family", selectedFamily);
                        numberFontFamily = selectedFamily;
                        panelScreenArea.Invalidate();
                    }
                }
            }
        }



        private void CenterLabelInPanel()
        {
            // Calculate the center position of the panel
            int centerX = (panelBottom.Width - labelDebug.Width) / 2;
            int centerY = (panelBottom.Height - labelDebug.Height) / 2;

            // Set the label's location to the calculated center position
            labelDebug.Location = new Point(centerX, centerY);
        }

        public bool IsAppInForeground()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            IntPtr thisWindowHandle = this.Handle;

            // Check if the foreground window is the current window
            return foregroundWindow == thisWindowHandle;
        }

        private void FS2MainForm_Resize(object sender, EventArgs e)
        {
            CenterLabelInPanel();
        }

        private void mitClearText_Click(object sender, EventArgs e)
        {
            drawnTextString = string.Empty;
            panelScreenArea.Invalidate();
        }

        private void UndoAction()
        {
            if (undoStack.Count == 0)
            {
                return;
            }

            UndoItem last = undoStack.Pop();

            switch (last.Type)
            {
                case DrawType.Arrow:
                    drawnArrows.Remove((Line)last.Data);
                    break;

                case DrawType.Rectangle:
                    drawnRectangles.Remove((Rectangle)last.Data);
                    if (drawnRectangles.Count == 0)
                    {
                        currentRectangle = new Rectangle(0, 0, 0, 0); // Or new Rectangle()
                    }
                    break;

                case DrawType.Text:
                    drawnTexts.Remove((TextItem)last.Data);
                    numbering--;
                    break;

                case DrawType.String:
                    drawnTextString = string.Empty;
                    break;
            }

            panelScreenArea.Invalidate();

            UpdateUndoMenu();

            ShowInfo("drag");
        }

        private void mitUndo_Click(object sender, EventArgs e)
        {
            UndoAction();
        }

        private void UpdateUndoMenu()
        {
            mitUndo.Enabled = undoStack.Count > 0;
        }


        private void RemovePreviousTextFromUndo()
        {
            var tempStack = new Stack<UndoItem>();

            // Move all non-text items to a temporary stack
            while (undoStack.Count > 0)
            {
                var top = undoStack.Pop();
                if (top.Type != DrawType.String)
                    tempStack.Push(top);
            }

            // Move the non-text items back to the main undo stack
            while (tempStack.Count > 0)
            {
                undoStack.Push(tempStack.Pop());
            }
        }

        private void LoadFileNameHistory()
        {
            string saved = string.Empty;

            try
            {
                saved = FS2SettingsManager.GetSetting("last_names");
            }
            catch
            {
                saved = string.Empty;
                EnsureSettingExists("last_names", "");
            }


            var names = saved.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(n => n.Trim())
                             .ToArray();

            var autoSource = new AutoCompleteStringCollection();
            autoSource.AddRange(names);

            txtbName.AutoCompleteCustomSource = autoSource;
            txtbName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtbName.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void rangeTrackBar_DoubleClick(object sender, EventArgs e)
        {
            rangeTrackBar.UpperValue = 100;
            rangeTrackBar.LowerValue = 0;
        }

    }
}
