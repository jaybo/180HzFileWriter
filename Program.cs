using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using AForge;
using AForge.Video.FFMPEG;

namespace _180HzFileWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 512;
            int height = width;
            int[] framerates = {60, 120, 180};

            double preamblePostamble = 0.5;    // seconds
            double movieLength = 60.0;    // seconds

            foreach (var framerate in framerates)
            {
                VideoFileWriter writer = new VideoFileWriter();
                VideoCodec codec = VideoCodec.WMV2;

                string fname = "test." + codec.ToString() + "." + framerate.ToString("000") + "Hz." + movieLength.ToString() + "sec." + width.ToString() + "." + height.ToString() + ".avi";
                writer.Open(fname, width, height, framerate, codec);

                RectangleF rectText = new RectangleF(width / 2, 0, width / 2, height);
                RectangleF rectBlock = new RectangleF(0, 0, width / 2, height);

                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                SolidBrush brushWhite = new SolidBrush(Color.White);
                SolidBrush brushBlack = new SolidBrush(Color.Black);
                var font = new Font("Tahoma", width <= 64 ? 8 : width <= 128 ? 12 : width <=256 ? 16 : 20);

                // Preamble
                for (int i = 0; i < (int)(framerate * preamblePostamble); i++)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Black);
                        g.Flush();
                    }
                    writer.WriteVideoFrame(bmp);
                }

                for (int i = 0; i < (int)(framerate * movieLength); i++)
                {
                    bmp.SetPixel(i % width, i % height, Color.Blue);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Black);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.DrawString(i.ToString(), font, Brushes.White, rectText);

                        g.FillRectangle(((i & 1) == 0) ? brushWhite : brushBlack, rectBlock);
                        g.Flush();
                    }
                    writer.WriteVideoFrame(bmp);
                }

                // Postamble
                for (int i = 0; i < (int)(framerate * preamblePostamble); i++)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Black);
                        g.Flush();
                    }
                    writer.WriteVideoFrame(bmp);
                }

                writer.Close();
            }
        }
    }
}
