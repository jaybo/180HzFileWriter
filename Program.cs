﻿using System;
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
            int width = 320;
            int height = 240;

            VideoFileWriter writer = new VideoFileWriter();
            
            writer.Open("test180.avi", width, height, 180, VideoCodec.MPEG4);
            
            RectangleF rectText = new RectangleF(100,100,100,100); 
            RectangleF rectBlock = new RectangleF(0, 200, 100, 100); 
            
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            SolidBrush brushWhite = new SolidBrush(Color.White);
            SolidBrush brushBlack = new SolidBrush(Color.Black);

            for (int i = 0; i < 180*10; i++)
            {
                bmp.SetPixel(i % width, i % height, Color.Blue);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Black);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawString(i.ToString(), new Font("Tahoma", 12), Brushes.White, rectText);

                    g.FillRectangle(((i & 1) == 0) ? brushWhite : brushBlack, rectBlock);
                    g.Flush();
                }
                writer.WriteVideoFrame(bmp);
            }
            writer.Close();

        }
    }
}
