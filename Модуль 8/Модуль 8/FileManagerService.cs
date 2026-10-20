using System;
using System.Collections.Generic;
using System.IO;
namespace FileManagerApp
{
    // Менеджер файлов
    public class Manager
    {
        // Получить содержимое папки
        public IEnumerable<Item> GetDirectoryContents(string path)
        {
            var items = new List<Item>();
            try
            {
                var dirInfo = new DirectoryInfo(path);
                foreach (var dir in dirInfo.GetDirectories())
                {
                    items.Add(new Item { Name = dir.Name, FullPath = dir.FullName, Type = "Папка", LastModified = dir.LastWriteTime, IsDirectory = true });
                }
                foreach (var file in dirInfo.GetFiles())
                {
                    items.Add(new Item { Name = file.Name, FullPath = file.FullName, Type = file.Extension, LastModified = file.LastWriteTime, IsDirectory = false });
                }
            }
            catch (UnauthorizedAccessException) { }
            return items;
        }
        // Копирование
        public void CopyItem(string sourcePath, string destPath, bool isDirectory)
        {
            if (isDirectory)
            {
                string destDir = Path.Combine(destPath, Path.GetFileName(sourcePath));
                DirectoryCopy(sourcePath, destDir, true);
            }
            else
            {
                string fileName = Path.GetFileName(sourcePath);
                File.Copy(sourcePath, Path.Combine(destPath, fileName), true);
            }
        }
        // Рекурсивное копирование папки
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            Directory.CreateDirectory(destDirName);
            foreach (FileInfo file in dir.GetFiles())
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        // Перемещение
        public void MoveItem(string sourcePath, string destPath, bool isDirectory)
        {
            string fileName = Path.GetFileName(sourcePath);
            string targetPath = Path.Combine(destPath, fileName);
            if (isDirectory) Directory.Move(sourcePath, targetPath);
            else File.Move(sourcePath, targetPath);
        }
        // Удаление
        public void DeleteItem(string path, bool isDirectory)
        {
            if (isDirectory) Directory.Delete(path, true);
            else File.Delete(path);
        }
    }
}