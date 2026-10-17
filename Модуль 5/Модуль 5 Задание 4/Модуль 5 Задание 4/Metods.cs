using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
namespace Wpf
{
    public class ImageManager
    {
        public BitmapImage LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();
                return bitmap;
            }
            return null;
        }
    }
}