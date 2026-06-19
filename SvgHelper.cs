public static class SvgHelper
{
    public static Bitmap? LoadSvgFromResources(byte[]? svgData, int width, int height)
    {
        if (svgData == null || svgData.Length == 0)
            return null;

        using (var ms = new MemoryStream(svgData))
        {
            var svgDocument = Svg.SvgDocument.Open<Svg.SvgDocument>(ms);

            // Resize the SVG document to fit the requested size
            svgDocument.Width = width;
            svgDocument.Height = height;

            // Create a Bitmap with the requested width and height
            Bitmap bitmap = new Bitmap(width, height);
            bitmap.MakeTransparent();

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                // Draw the SVG onto the Bitmap
                svgDocument.Draw(g);
            }

            return bitmap;
        }
    }
}
