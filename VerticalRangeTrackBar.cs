using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace FastScreener2
{
    public class VerticalRangeTrackBar : Control
    {
        private int minValue = 0;
        private int maxValue = 100;
        private int lowerValue = 25;
        private int upperValue = 75;
        private bool draggingLower = false;
        private bool draggingUpper = false;

        private bool isDragging = false;
        private int thumbY;  // Y position of the thumb

        public event EventHandler ThumbMoved;

        // Customizable colors
        public Color TrackColor { get; set; } = Color.LightGray;
        public Color RangeColor { get; set; } = Color.Blue;
        public Color ThumbColor { get; set; } = Color.White;
        public Color ThumbBorderColor { get; set; } = Color.Black;

        public int Minimum
        {
            get => minValue;
            set { minValue = value; Invalidate(); }
        }

        public int Maximum
        {
            get => maxValue;
            set { maxValue = value; Invalidate(); }
        }

        public int LowerValue
        {
            get => lowerValue;
            set { lowerValue = Math.Max(minValue, Math.Min(value, upperValue)); Invalidate(); }
        }

        public int UpperValue
        {
            get => upperValue;
            set { upperValue = Math.Max(lowerValue, Math.Min(value, maxValue)); Invalidate(); }
        }

        // Get thumb positions in pixels (for UI adjustments)
        public int LowerThumbPosition => GetThumbY(LowerValue);
        public int UpperThumbPosition => GetThumbY(UpperValue);

        public VerticalRangeTrackBar()
        {
            this.BackColor = Color.Coral; // Transparent background
            this.DoubleBuffered = true; // Reduce flickering
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int trackWidth = Width / 3;
            int top = 10, bottom = Height - 10;
            int lowerY = GetThumbY(LowerValue);
            int upperY = GetThumbY(UpperValue);

            // Draw track
            using (SolidBrush trackBrush = new SolidBrush(TrackColor))
                g.FillRectangle(trackBrush, Width / 2 - trackWidth / 2, top, trackWidth, bottom - top);

            // Draw selected range
            using (SolidBrush rangeBrush = new SolidBrush(RangeColor))
                g.FillRectangle(rangeBrush, Width / 2 - trackWidth / 2, upperY, trackWidth, lowerY - upperY);

            // Draw thumbs
            DrawThumb(g, lowerY);
            DrawThumb(g, upperY);
        }

        private void DrawThumb(Graphics g, int y)
        {
            int thumbWidth = 20;  // Width of the box
            int thumbHeight = 10; // Height of the box

            int x = (Width / 2) - (thumbWidth / 2); // Center horizontally
            int yPos = y - (thumbHeight / 2);       // Center vertically

            // Fill the box (thumb)
            using (SolidBrush thumbBrush = new SolidBrush(ThumbColor))
                g.FillRectangle(thumbBrush, x, yPos, thumbWidth, thumbHeight);

            // Draw the border
            using (Pen borderPen = new Pen(ThumbBorderColor, 2))
                g.DrawRectangle(borderPen, x, yPos, thumbWidth, thumbHeight);
        }

        private int GetThumbY(int value)
        {
            int top = 10, bottom = Height - 10;
            return bottom - (value - minValue) * (bottom - top) / (maxValue - minValue);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int lowerY = GetThumbY(LowerValue);
            int upperY = GetThumbY(UpperValue);

            if (Math.Abs(e.Y - lowerY) < 10)
                draggingLower = true;
            else if (Math.Abs(e.Y - upperY) < 10)
                draggingUpper = true;


            if (IsThumbClicked(e.Y))  // Check if the thumb is clicked
            {
                isDragging = true;
            }
        }

        private bool IsThumbClicked(int mouseY)
        {
            return Math.Abs(mouseY - thumbY) < 10; // If within 10px of thumb
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            draggingLower = draggingUpper = false;

            isDragging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);  // Call base to ensure default behavior

            // Trigger an external event for the form
            ThumbMoved?.Invoke(this, EventArgs.Empty);

            if (draggingLower || draggingUpper)
            {
                int top = 10, bottom = Height - 10;
                int newValue = minValue + (bottom - e.Y) * (maxValue - minValue) / (bottom - top);

                int minimumGap = 8;

                // Ensure the lower thumb cannot go above the upper thumb
                if (draggingLower)
                {
                    if (newValue < UpperValue - minimumGap)  // Prevent the lower thumb from going above the upper thumb
                        LowerValue = newValue;
                }
                // Ensure the upper thumb cannot go below the lower thumb
                else if (draggingUpper)
                {
                    if (newValue > LowerValue + minimumGap)  // Prevent the upper thumb from going below the lower thumb
                        UpperValue = newValue;
                }

                Invalidate();
            }

            if (isDragging)
            {
                // Limit movement within track bounds
                thumbY = Math.Max(0, Math.Min(Height - 20, e.Y));
                Invalidate(); // Redraw the control
            }
        }

    }

}

