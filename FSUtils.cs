using System.Drawing.Drawing2D;
using System.Drawing;
using static FastScreener2.FS2SettingsManager;

namespace FastScreener2
{
    internal class FSUtils
    {
        private Point startPoint;
        private bool dragging = false;
        public static List<Line> drawnArrows = new List<Line>();

        //for DPI
        public static Image ScaleImage(Image originalImage, float scaleFactor)
        {
            int newWidth = (int)(originalImage.Width * scaleFactor);
            int newHeight = (int)(originalImage.Height * scaleFactor);

            Bitmap resizedImage = new Bitmap(originalImage, new Size(newWidth, newHeight));
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
        public static void DrawArrow(PaintEventArgs e, Point relativePoint, Color color)
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

            AddLine(startPoint, endPoint, color);

            //RenderLines(new PaintEventArgs(pnlCanvas.CreateGraphics(), pnlCanvas.ClientRectangle), ARROW_SIZE);
        }

        public static void RenderLines(PaintEventArgs e, int aSize)
        {
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

        public static void AddLine(Point startPoint, Point endPoint, Color color)
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

    }
}
