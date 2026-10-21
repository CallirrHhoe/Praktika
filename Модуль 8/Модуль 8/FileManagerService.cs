using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace FileManagerApp
{
    public class Manager
    {
        public IEnumerable<Item> GetContents(string path, string sortBy, bool asc)
        {
            var items = new List<Item>();
            try
            {
                var dir = new DirectoryInfo(path);
                foreach (var d in dir.GetDirectories())
                {
                    items.Add(new Item { Name = d.Name, FullPath = d.FullName, Type = "Папка", Size = 0, FormattedSize = "<ПАПКА>", LastModified = d.LastWriteTime, IsDirectory = true });
                }
                foreach (var f in dir.GetFiles())
                {
                    items.Add(new Item { Name = f.Name, FullPath = f.FullName, Type = f.Extension, Size = f.Length, FormattedSize = FormatSize(f.Length), LastModified = f.LastWriteTime, IsDirectory = false });
                }
            }
            catch (UnauthorizedAccessException) { }
            return Sort(items, sortBy, asc);
        }
        private string FormatSize(long bytes)
        {
            string[] suffixes = { "б", "КБ", "МБ", "ГБ", "ТБ" };
            int i = 0;
            double d = bytes;
            while (d >= 1024 && i < suffixes.Length - 1)
            {
                d /= 1024;
                i++;
            }
            return $"{d:F1} {suffixes[i]}";
        }
        public IEnumerable<Item> Sort(List<Item> items, string sortBy, bool asc)
        {
            var q = items.OrderBy(x => !x.IsDirectory);
            if (sortBy == "Size")
            {
                return asc ? q.ThenBy(x => x.Size) : q.ThenByDescending(x => x.Size);
            }
            if (sortBy == "Date")
            {
                return asc ? q.ThenBy(x => x.LastModified) : q.ThenByDescending(x => x.LastModified);
            }
            return asc ? q.ThenBy(x => x.Name) : q.ThenByDescending(x => x.Name);
        }
        public void Copy(string src, string dstDir, bool isDir)
        {
            if (isDir)
            {
                string target = Path.Combine(dstDir, Path.GetFileName(src));
                CopyDir(src, target);
            }
            else
            {
                File.Copy(src, Path.Combine(dstDir, Path.GetFileName(src)), true);
            }
        }
        private void CopyDir(string src, string dst)
        {
            Directory.CreateDirectory(dst);
            foreach (var f in Directory.GetFiles(src))
            {
                File.Copy(f, Path.Combine(dst, Path.GetFileName(f)), true);
            }
            foreach (var d in Directory.GetDirectories(src))
            {
                CopyDir(d, Path.Combine(dst, Path.GetFileName(d)));
            }
        }
        public void Move(string src, string dstDir, bool isDir)
        {
            string target = Path.Combine(dstDir, Path.GetFileName(src));
            if (isDir)
            {
                Directory.Move(src, target);
            }
            else
            {
                if (File.Exists(target)) File.Delete(target);
                File.Move(src, target);
            }
        }
        public void Delete(string path, bool isDir)
        {
            if (isDir) Directory.Delete(path, true);
            else File.Delete(path);
        }
    }
}