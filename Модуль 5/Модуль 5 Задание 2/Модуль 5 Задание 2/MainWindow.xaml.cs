using System.IO;
using Microsoft.Win32;
namespace Wpf
{
    public class FileManager
    {
        private string _currentFilePath = string.Empty;
        public string OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _currentFilePath = openFileDialog.FileName;
                return File.ReadAllText(_currentFilePath);
            }

            return null;
        }
        public bool SaveFile(string content)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    _currentFilePath = saveFileDialog.FileName;
                }
                else
                {
                    return false; // Пользователь отменил сохранение
                }
            }
            File.WriteAllText(_currentFilePath, content);
            return true;
        }
    }
}