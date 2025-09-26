using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Kms.Security.Util
{
    public interface ICaptchaGenerator
    {
        (string Text, string Image) Generate();
    }

    public class CaptchaGenerator : ICaptchaGenerator
    {
        private const int Width = 200;
        private const int Height = 38;
        private readonly Font font = new Font("Arial", 24, FontStyle.Bold);
        private const string AllowedChars = "ABCDEFGHJKLMNPRSTUVWXYZ23456789"; // Removed similar characters (I, O, 1, 0)  

        public (string Text, string Image) Generate()
        {
            string captchaText = GenerateRandomText(6);

            var bitmap = GenerateCaptchaImageBitmap(captchaText);

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return (Text: captchaText, Image: "data:image/png;base64," + base64String);
            }
        }

        private string GenerateRandomText(int length)
        {
            Random random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = AllowedChars[random.Next(AllowedChars.Length)];
            }
            return new string(chars);
        }

        private Bitmap GenerateCaptchaImageBitmap(string captchaText)
        {
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; // Important for warping  

            Rectangle rect = new Rectangle(0, 0, Width, Height);
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            graphics.FillRectangle(hatchBrush, rect);

            // Character-Level Distortion and Overlap  
            Random random = new Random();
            float x = 10; // Starting X position  
            for (int i = 0; i < captchaText.Length; i++)
            {
                char c = captchaText[i];
                SizeF charSize = graphics.MeasureString(c.ToString(), font);

                // Random Transformations for each character  
                Matrix transform = new Matrix();
                transform.Translate(x, Height / 2 - charSize.Height / 2); // Center vertically  
                transform.Rotate(random.Next(-25, 25)); // More rotation  
                transform.Scale(1 + (float)random.Next(-10, 10) / 100, 1 + (float)random.Next(-10, 10) / 100); // Scale slightly  

                graphics.Transform = transform;
                Brush charBrush = new SolidBrush(Color.FromArgb(random.Next(50, 150), Color.Black)); // Semi-transparent black  
                graphics.DrawString(c.ToString(), font, charBrush, 0, 0);

                graphics.ResetTransform();
                x += charSize.Width - random.Next(5, 15); // Overlap characters  
            }

            // Wave Distortion (Warping)  
            ApplyWaveDistortion(bitmap, random.Next(3, 6), random.Next(5, 15));

            // Noise: Arcs and Dots  
            for (int i = 0; i < random.Next(10, 20); i++)
            {
                Pen arcPen = new Pen(Color.FromArgb(random.Next(50, 150), Color.DarkGray), 2);
                Rectangle arcRect = new Rectangle(random.Next(-10, Width - 10), random.Next(-10, Height - 10), random.Next(10, 30), random.Next(10, 30));
                graphics.DrawArc(arcPen, arcRect, random.Next(0, 360), random.Next(90, 270));
            }

            for (int i = 0; i < random.Next(50, 100); i++)
            {
                Brush dotBrush = new SolidBrush(Color.FromArgb(random.Next(50, 150), Color.LightGray));
                int xDot = random.Next(0, Width);
                int yDot = random.Next(0, Height);
                graphics.FillRectangle(dotBrush, xDot, yDot, 3, 3);
            }

            graphics.Dispose();
            return bitmap;
        }

        private void ApplyWaveDistortion(Bitmap bitmap, double amplitude, double frequency)
        {
            using (Bitmap tempBitmap = (Bitmap)bitmap.Clone())
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        // Distort the Y coordinate based on a sine wave  
                        int newY = (int)(y + amplitude * Math.Sin(Math.PI * frequency * x / bitmap.Width));

                        if (newY >= 0 && newY < bitmap.Height)
                        {
                            bitmap.SetPixel(x, y, tempBitmap.GetPixel(x, newY));
                        }
                        else
                        {
                            // If the distorted Y is out of bounds, set to background color or a random color  
                            bitmap.SetPixel(x, y, Color.White); // Or another color  
                        }
                    }
                }
            }
        }
    }
}
