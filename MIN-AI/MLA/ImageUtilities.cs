using System;
using System.Drawing;
using System.Windows.Forms;

namespace MLA
{
    public class ImageViewer
    {
        public static Bitmap CreateBitmapFromColors(Color[,] image)
        {
            Bitmap bitmap = new Bitmap(image.GetLength(1), image.GetLength(0));
            for (int i = 0; i < image.GetLength(1); i++)
            {
                for (int j = 0; j < image.GetLength(0); j++)
                {
                    bitmap.SetPixel(i, j, image[j, i]);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Load an image from file and returns a matrix of Color
        /// </summary>
        /// <param name="path">File path relative to the executable</param>
        public static Color[,] LoadImage(string path)
        {
            Bitmap img = new Bitmap(path);
            Color[,] colors = new Color[img.Height, img.Width];
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    colors[i, j] = img.GetPixel(j, i);
                }
            }
            return colors;
        }

        /// <summary>
        /// Draw a single image in a window
        /// </summary>
        /// <param name="inputImage">Color matrix representing the input image</param>
        public static void DrawImage(Color[,] inputImage)
        {
            DrawImage(CreateBitmapFromColors(inputImage));
        }

        public static void DrawImage(Bitmap bitmap)
        {
            int sw = Screen.PrimaryScreen.WorkingArea.Width;
            int sh = Screen.PrimaryScreen.WorkingArea.Height;
            Form imageWindow = new Form();
            PictureBox inputPicture = new PictureBox();
            inputPicture.Image = bitmap;
            inputPicture.Width = bitmap.Width;
            inputPicture.Height = bitmap.Height;
            inputPicture.SizeMode = PictureBoxSizeMode.Zoom;
            if (bitmap.Width > sw || bitmap.Height > sh)
            {
                int scaleFactor = (int)Math.Ceiling((float)bitmap.Width / (float)sw);
                inputPicture.Size = new Size(bitmap.Width / scaleFactor, bitmap.Height / scaleFactor);
            }
            imageWindow.Width = inputPicture.Width;
            imageWindow.Height = inputPicture.Height;
            imageWindow.Controls.Add(inputPicture);
            imageWindow.ShowDialog();
        }

        /// <summary>
        /// Draw a pair of images in a window. The images are scaled down if their size is too big.
        /// </summary>
        /// <param name="inputImage">Color matrix representing the input image</param>
        /// <param name="outputImage">Color matrix representing the output image</param>
        public static void DrawImagePair(Color[,] inputImage, Color[,] outputImage)
        {
            int sw = Screen.PrimaryScreen.WorkingArea.Width;
            int sh = Screen.PrimaryScreen.WorkingArea.Height;
            Form imageWindow = new Form();
            Bitmap inputBitmap = CreateBitmapFromColors(inputImage);
            Bitmap outputBitmap = CreateBitmapFromColors(outputImage);
            PictureBox inputPicture = new PictureBox();
            PictureBox outputPicture = new PictureBox();
            inputPicture.Width = inputBitmap.Width;
            inputPicture.Height = inputBitmap.Height;
            outputPicture.Width = outputBitmap.Width;
            outputPicture.Height = outputBitmap.Height;
            inputPicture.Image = inputBitmap;
            outputPicture.Image = outputBitmap;
            inputPicture.SizeMode = outputPicture.SizeMode = PictureBoxSizeMode.Zoom;
            if (inputBitmap.Width + outputBitmap.Width > sw || Math.Max(inputBitmap.Height, outputBitmap.Height) > sh)
            {
                int scaleFactor = (int)Math.Ceiling((float)(inputBitmap.Width + outputBitmap.Width) / (float)sw);
                inputPicture.Size = new Size(inputBitmap.Width / scaleFactor, inputBitmap.Height / scaleFactor);
                outputPicture.Size = new Size(outputBitmap.Width / scaleFactor, outputBitmap.Height / scaleFactor);
            }
            imageWindow.Width = inputPicture.Width + outputPicture.Width;
            imageWindow.Height = Math.Max(inputPicture.Height, outputPicture.Height);
            outputPicture.Location = new Point(inputPicture.Width, 0);
            imageWindow.Controls.Add(inputPicture);
            imageWindow.Controls.Add(outputPicture);
            imageWindow.ShowDialog();
        }
    }
}
