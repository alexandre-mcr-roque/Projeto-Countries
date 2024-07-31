using Svg;
using System.Drawing;

namespace Biblioteca.Services
{
    /*
     * Utilized to facilitate the creation of bitmaps
     */
    public class BitmapService
    {
        public static Bitmap FromPngByteArray(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new Bitmap(stream);
            }
        }

        public static Task<Bitmap> FromPngByteArrayAsync(byte[] bytes) => Task.Run(() => FromPngByteArray(bytes));
        public static Task<Bitmap> FromPngByteArrayAsync(byte[] bytes, CancellationToken ct) => Task.Run(() => FromPngByteArray(bytes), ct);

        public static Bitmap FromSvgByteArray(byte[] bytes, int rasterWidth = 1000)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var document = SvgDocument.Open<SvgDocument>(stream);
                return document.Draw(rasterWidth, 0);
            }
        }

        public static Task<Bitmap> FromSvgByteArrayAsync(byte[] bytes, int rasterWidth = 1000) => Task.Run(() => FromSvgByteArray(bytes, rasterWidth));
        public static Task<Bitmap> FromSvgByteArrayAsync(byte[] bytes, CancellationToken ct, int rasterWidth = 1000) => Task.Run(() => FromSvgByteArray(bytes, rasterWidth), ct);
    }
}
