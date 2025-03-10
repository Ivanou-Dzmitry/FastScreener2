using System;
using System.Drawing;
using System.Windows.Forms;

namespace FastScreener2
{
    public class RangeTrackBar : Control
    {
        private int minValue = 0;
        private int maxValue = 100;
        private int lowerValue = 25;
        private int upperValue = 75;
        private bool draggingLower = false;
        private bool draggingUpper = false;

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int trackHeight = Height / 3;
            int left = 10, right = Width - 10;
            int lowerX = left + (LowerValue - minValue) * (right - left) / (maxValue - minValue);
            int upperX = left + (UpperValue - minValue) * (right - left) / (maxValue - minValue);

            // Draw track
            g.FillRectangle(Brushes.LightGray, left, Height / 2 - trackHeight / 2, right - left, trackHeight);

            // Draw range
            g.FillRectangle(Brushes.Blue, lowerX, Height / 2 - trackHeight / 2, upperX - lowerX, trackHeight);

            // Draw thumbs
            g.FillEllipse(Brushes.White, lowerX - 5, Height / 2 - 10, 20, 20);
            g.DrawEllipse(Pens.Black, lowerX - 5, Height / 2 - 10, 20, 20);

            g.FillEllipse(Brushes.White, upperX - 5, Height / 2 - 10, 20, 20);
            g.DrawEllipse(Pens.Black, upperX - 5, Height / 2 - 10, 20, 20);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int left = 10, right = Width - 10;
            int lowerX = left + (LowerValue - minValue) * (right - left) / (maxValue - minValue);
            int upperX = left + (UpperValue - minValue) * (right - left) / (maxValue - minValue);

            if (Math.Abs(e.X - lowerX) < 10)
                draggingLower = true;
            else if (Math.Abs(e.X - upperX) < 10)
                draggingUpper = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            draggingLower = draggingUpper = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (draggingLower || draggingUpper)
            {
                int left = 10, right = Width - 10;
                int newValue = minValue + (e.X - left) * (maxValue - minValue) / (right - left);
                if (draggingLower)
                    LowerValue = newValue;
                else
                    UpperValue = newValue;

                Invalidate();
            }
        }
    }

}