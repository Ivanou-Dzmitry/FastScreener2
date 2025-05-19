
using System.Drawing.Drawing2D;


namespace FastScreener2
{
    public class OutlineLabel : Label
    {
        public Color OutlineColor { get; set; } = Color.Black;
        public float OutlineWidth { get; set; } = 2f;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            using (Pen outlinePen = new Pen(OutlineColor, OutlineWidth) { LineJoin = LineJoin.Round })
            using (SolidBrush textBrush = new SolidBrush(ForeColor))
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                path.AddString(Text, Font.FontFamily, (int)Font.Style, e.Graphics.DpiY * Font.Size / 72, ClientRectangle, format);

                // Draw outline
                e.Graphics.DrawPath(outlinePen, path);

                // Fill text
                e.Graphics.FillPath(textBrush, path);
            }
        }
    }

}
