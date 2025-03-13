using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastScreener2
{
    public class BlurOutlineLabel : Label
    {
        public Color OutlineColor { get; set; } = Color.Black;
        public float OutlineWidth { get; set; } = 4f; // More width = more blur
        public int BlurAmount { get; set; } = 6; // Adjust for blur intensity

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                path.AddString(Text, Font.FontFamily, (int)Font.Style, e.Graphics.DpiY * Font.Size / 72, ClientRectangle, format);

                // Fake blur by drawing multiple slightly offset outlines
                for (int i = -BlurAmount; i <= BlurAmount; i += 2)
                {
                    for (int j = -BlurAmount; j <= BlurAmount; j += 2)
                    {
                        using (Pen blurPen = new Pen(Color.FromArgb(50, OutlineColor), OutlineWidth))
                        {
                            e.Graphics.DrawPath(blurPen, path);
                        }
                    }
                }

                // Draw sharp outline
                using (Pen outlinePen = new Pen(OutlineColor, OutlineWidth))
                {
                    e.Graphics.DrawPath(outlinePen, path);
                }

                // Fill text
                using (SolidBrush textBrush = new SolidBrush(ForeColor))
                {
                    e.Graphics.FillPath(textBrush, path);
                }
            }
        }
    }

}
