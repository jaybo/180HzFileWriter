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
            int width = 256;
            int height = 256;
            int framerate = 60;

            double preamblePostamble = 0.5;    // seconds
            double movieLength = 60.0;    // seconds

            VideoFileWriter writer = new VideoFileWriter();

            string fname = "test" + framerate.ToString() + ".avi";
            writer.Open(fname, width, height, framerate, VideoCodec.MPEG4);
            
            RectangleF rectText = new RectangleF(width/2,0,width/2,height); 
            RectangleF rectBlock = new RectangleF(0,0,width/2, height); 
            
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            SolidBrush brushWhite = new SolidBrush(Color.White);
            SolidBrush brushBlack = new SolidBrush(Color.Black);

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
                    g.DrawString(i.ToString(), new Font("Tahoma", 20), Brushes.White, rectText);

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
