using System;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;
using Biblioteca.Services;
using System.Threading;

namespace Projeto.Services
{
    public class BitmapSourceService
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private static BitmapSource Bitmap2BitmapSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;

            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        public static BitmapSource FromSvgByteArray(byte[] bytes, int rasterWidth = 1000) => Bitmap2BitmapSource(BitmapService.FromSvgByteArray(bytes, rasterWidth));
        public static Task<BitmapSource> FromSvgByteArrayAsync(byte[] bytes, int rasterWidth = 1000) => Task.Run(() => FromSvgByteArray(bytes, rasterWidth));
        public static Task<BitmapSource> FromSvgByteArrayAsync(byte[] bytes, CancellationToken ct, int rasterWidth = 1000) => Task.Run(() => FromSvgByteArray(bytes, rasterWidth), ct);
    }
}
