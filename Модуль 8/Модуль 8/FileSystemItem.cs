using System;
namespace FileManagerApp
{
    // Модель данных
    public class Item
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string Type { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsDirectory { get; set; }
    }
}