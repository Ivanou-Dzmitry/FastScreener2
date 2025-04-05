using System.Drawing.Drawing2D;
using System.Drawing;
using static FastScreener2.FS2SettingsManager;
using System.Diagnostics;
using System.Numerics;

namespace FastScreener2
{
    internal class FSUtils
    {
        //for arrow
        public struct Line
        {
            public Point startPoint { get; set; }
            public Point endPoint { get; set; }
            public Color lineColor { get; set; }
            public float lineWidth { get; set; }

            public Line(Point start, Point end, Color color, float width)
            {
                startPoint = start;
                endPoint = end;
                lineColor = color;
                lineWidth = width;
            }
        }

        //for number
        public class TextItem
        {
            public string Text { get; set; }
            public Point Position { get; set; }

            public TextItem(string text, Point position)
            {
                Text = text;
                Position = position;
            }
        }


        private Point startPoint;
        private bool dragging = false;
        public static List<Line> drawnArrows = new List<Line>();
        public static List<Rectangle> drawnRectangles = new List<Rectangle>();
        public static List<TextItem> drawnTexts = new List<TextItem>();

        //for DPI
        public static Image ScaleImage(Image originalImage, float scaleFactor)
        {
            if (originalImage == null) return null;

            // Store original dimensions for possible reset
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;

            // Calculate the new width and height based on the scale factor
            int newWidth = (int)(originalWidth * scaleFactor);
            int newHeight = (int)(originalHeight * scaleFactor);

            // If scale factor is 1, reset to original size
            if (scaleFactor == 1)
            {
                newWidth = originalWidth;
                newHeight = originalHeight;
            }

            // Ensure the new width and height are not less than 1 (to avoid creating a zero-sized image)
            newWidth = Math.Max(1, newWidth);
            newHeight = Math.Max(1, newHeight);

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }



        public void AttachDragEvents(Panel panel)
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
                // Ensure sender is a Control
                if (sender is Control control)
                {
                    // Get the form that contains the control
                    Form form = control.FindForm();

                    if (form != null)
                    {
                        // Get the new mouse position relative to the screen
                        Point newPoint = control.PointToScreen(e.Location);

                        // Calculate how much the mouse moved
                        int offsetX = newPoint.X - startPoint.X;
                        int offsetY = newPoint.Y - startPoint.Y;

                        // Update the form's position
                        form.Location = new Point(form.Left + offsetX, form.Top + offsetY);

                        // Update startPoint for smooth movement
                        startPoint = newPoint;

                        FS2MainForm.Instance?.ShowInfo("drag");
                    }
                }
            }
        }


        // Mouse Up: Stop dragging
        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            FS2MainForm.Instance?.SwapPanelsIfNeeded();
        }

        //draw arrow
        public static void SetArrow(Point relativePoint, Color color)
        {

            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(relativePoint.X, relativePoint.Y);

            int scaledLenght = (int)(arrowLenght * FS2MainForm.scalingFactor);

            switch (arrowType)
            {
                case 1:
                    startPoint = new Point(relativePoint.X - scaledLenght, relativePoint.Y + scaledLenght);
                    break;
                case 2:
                    startPoint = new Point(relativePoint.X - scaledLenght, relativePoint.Y - scaledLenght);
                    break;
                case 3:
                    startPoint = new Point(relativePoint.X + scaledLenght, relativePoint.Y - scaledLenght);
                    break;
                case 4:
                    startPoint = new Point(relativePoint.X + scaledLenght, relativePoint.Y + scaledLenght);
                    break;
            }

            AddArrow(startPoint, endPoint, color);
        }

        public static void RenderArrows(PaintEventArgs e)
        {
            int aSize = ARROW_SIZE;

            foreach (var line in drawnArrows)
            {
                using (Pen linePen = new Pen(line.lineColor, line.lineWidth))
                {
                    linePen.CustomEndCap = new AdjustableArrowCap(aSize, aSize);

                    // for outline
                    var arrowPenOutline = new Pen(Color.Black, 2);
                    // for outline
                    arrowPenOutline.CustomEndCap = new AdjustableArrowCap(aSize, aSize + 1);
                    e.Graphics.DrawLine(arrowPenOutline, line.startPoint, line.endPoint);

                    e.Graphics.DrawLine(linePen, line.startPoint, line.endPoint);
                }
            }
        }

        public static void AddArrow(Point startPoint, Point endPoint, Color color)
        {
            Line newLine = new Line(
                startPoint,
                endPoint,
                color,
                1.0f // Example line width
            );

            // Add the line to the list
            drawnArrows.Add(newLine);
        }

        public static void RenderNumbers(PaintEventArgs e)
        {
            //for outline
            int outlineShift = 1;

            Font drawFont = new Font("Arial", numberFontSize, FontStyle.Bold);
            Font outlineFont = new Font("Arial", numberFontSize + outlineShift, FontStyle.Bold);

            //font
            SolidBrush drawBrush = new SolidBrush(numberColor);
            //outline
            SolidBrush outlineBrush = new SolidBrush(Color.Black);

            StringFormat drawFormat = new StringFormat();

            //align string
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            foreach (var text in drawnTexts)
            {
                //outline
                e.Graphics.DrawString(text.Text, outlineFont, outlineBrush, text.Position.X, text.Position.Y + outlineShift, drawFormat);
                
                //number
                e.Graphics.DrawString(text.Text, drawFont, drawBrush, text.Position.X, text.Position.Y, drawFormat);                
            }

            drawBrush.Dispose();
            drawFont.Dispose();
            e.Graphics.Dispose();
        }


        public static void AddNumber(string text, Point position)
        {
            TextItem newNumber = new TextItem(
                text,
                position
            );

            // Add the text to the list
            drawnTexts.Add(newNumber);
        }


        public static void DrawFrameCurrent(PaintEventArgs e)
        {
            var framePen = new Pen(frameColor, frameStrokeWidth);

            framePen.DashStyle = DashStyle.Dash;

            if (FS2MainForm.currentRectangle.Width > 0 && FS2MainForm.currentRectangle.Height > 0)
                e.Graphics.DrawRectangle(framePen, FS2MainForm.currentRectangle);

            framePen.Dispose();
        }


        //drawFrame
        public static void RenderFrame(PaintEventArgs e)
        {
            var framePen = new Pen(frameColor, frameStrokeWidth);
           
            //avoid -
            int width = Math.Abs(FS2MainForm.currentRectangle.Width);
            int height = Math.Abs(FS2MainForm.currentRectangle.Height);

            if (width > MIN_DRAWN_SIZE_FRAME && height > MIN_DRAWN_SIZE_FRAME)
            {
                foreach (var rectangle in drawnRectangles)
                {
                    e.Graphics.DrawRectangle(framePen, rectangle);
                }
            }            
        }


        public static void RenderGuides(PaintEventArgs e, Panel panel, Color color)
        {

            // Create a reusable pen instance
            using (Pen guidePen = new Pen(color))
            {

                guidePen.DashStyle = DashStyle.Dash; // Set the line style to dashed

                int initialPointVertical, initialPointHorizontal;

                if (guidlineType == 1 || guidlineType == 2)
                {
                    // Determine the grid size based on the type
                    int divisions = (guidlineType == 1) ? 3 : 4;
                    initialPointVertical = panel.Width / divisions;
                    initialPointHorizontal = panel.Height / divisions;

                    // Draw vertical lines
                    for (int i = 1; i < divisions; i++)
                    {
                        int x = initialPointVertical * i;
                        e.Graphics.DrawLine(guidePen, x, 0, x, FS2MainForm.Instance.ClientSize.Height);
                    }

                    // Draw horizontal lines
                    for (int i = 1; i < divisions; i++)
                    {
                        int y = initialPointHorizontal * i;
                        e.Graphics.DrawLine(guidePen, 0, y, FS2MainForm.Instance.ClientSize.Width, y);
                    }
                }
                else if (guidlineType == 3)
                {
                    // Custom grid dimensions
                    int topIndent = Convert.ToInt32(customGuide[0]);
                    int bottomIndent = Convert.ToInt32(customGuide[1]);
                    int leftIndent = Convert.ToInt32(customGuide[2]);
                    int rightIndent = Convert.ToInt32(customGuide[3]);

                    // Fix border dimensions
                    int canvasSizeH = panel.Height - 3;
                    int canvasSizeW = panel.Width - 3;

                    // Draw top and bottom lines
                    e.Graphics.DrawLine(guidePen, 0, topIndent, panel.Width, topIndent);
                    e.Graphics.DrawLine(guidePen, 0, canvasSizeH - bottomIndent, panel.Width, canvasSizeH - bottomIndent);

                    // Draw left and right lines
                    e.Graphics.DrawLine(guidePen, leftIndent, 0, leftIndent, panel.Height);
                    e.Graphics.DrawLine(guidePen, canvasSizeW - rightIndent, 0, canvasSizeW - rightIndent, panel.Height);
                }
            }
        }

        public static void LogScreenshot(string date, int width, int height, string fileName)
        {
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "fs_log.csv");

            try
            {
                // Check if file exists
                bool fileExists = File.Exists(logFilePath);

                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    // If the file is new, write the header first
                    if (!fileExists)
                    {
                        sw.WriteLine("Date,Width,Height,FileName");
                    }

                    // Append the log entry
                    sw.WriteLine($"{date},{width},{height},{fileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to log file: {ex.Message}", "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static double GetMaxArrowLenght(int[,] resolutions)
        {
            double maxDiagonal = 0;

            for (int i = 0; i < resolutions.GetLength(1); i++)
            {
                int width = resolutions[0, i];
                int height = resolutions[1, i];

                double diagonal = Math.Sqrt(width * width + height * height);

                if (diagonal > maxDiagonal)
                    maxDiagonal = diagonal;
            }

            return maxDiagonal * 0.5;
        }

        public static Vector2 GetHalfMaxScreenSize(int[,] resolutions)
        {
            int maxWidth = 0;
            int maxHeight = 0;

            // Iterate over the resolutions to find the max width and max height
            for (int i = 0; i < resolutions.GetLength(1); i++) // 0 -> width, 1 -> height
            {
                maxWidth = Math.Max(maxWidth, resolutions[0, i]);
                maxHeight = Math.Max(maxHeight, resolutions[1, i]);
            }

            // Return a Vector2 with half of max width and max height
            return new Vector2(maxWidth / 2f, maxHeight / 2f);
        }

        public static Vector2 GetSmallestScreenResolution()
        {
            // Get all screens connected to the system
            var screens = Screen.AllScreens;

            // Find the screen with the smallest resolution
            var smallestScreen = screens
                .OrderBy(s => s.Bounds.Width * s.Bounds.Height) // Order by area (width * height)
                .First();

            // Get the smallest width and height
            int smallestWidth = smallestScreen.Bounds.Width;
            int smallestHeight = smallestScreen.Bounds.Height;

            // Reduce the smallest width and height by 10%
            float reducedWidth = smallestWidth * 0.9f; // 90% of the smallest width
            float reducedHeight = smallestHeight * 0.9f; // 90% of the smallest height

            // Return as Vector2 (width, height)
            return new Vector2(reducedWidth, reducedHeight);
        }

    }
}
