using System.Collections.ObjectModel;

namespace TreeSize.Core
{
    public class CurrentDirectory
    {
        public string DirectoryPath { get; set; }
        public event Action<string> SizeHandler;
        public static event Action<ObservableCollection<CurrentDirectory>>? ContentHandler;
        public string Name { get; set; }
        public ObservableCollection<CurrentDirectory> Content { get; set; }

        public CurrentDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath)) throw new ArgumentException("Invalid path");
            DirectoryPath = directoryPath;
            Name = Path.GetFileName(directoryPath);
        }

        private IEnumerable<string> GetAllDirectoryFiles()
        {
            Stack<string> dirs = new Stack<string>(20);
            IEnumerable<string> result = Enumerable.Empty<string>();
            if (!Directory.Exists(DirectoryPath))
            {
                throw new ArgumentException("Invalid directory path");
            }
            dirs.Push(DirectoryPath);

            while (dirs.Count > 0)
            {
                string currentDirectory = dirs.Pop();
                string[] subDirectores;
                IEnumerable<string> subDirectoresFiles;

                try
                {
                    subDirectores = Directory.GetDirectories(currentDirectory);
                    subDirectoresFiles = Directory.EnumerateFiles(currentDirectory);
                }
                catch (UnauthorizedAccessException e)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                foreach (string str in subDirectores)
                    dirs.Push(str);
                result = result.Concat(subDirectoresFiles);
            }           
            return result;
        }
        public enum Get
        {
            files,
            directories,
        }

        public IEnumerable<string> GetDirectoryContent(Get type)
        {
            if (File.Exists(DirectoryPath)) return Enumerable.Empty<string>();
            if (!Directory.Exists(DirectoryPath))
            {
                return Enumerable.Empty<string>();
                throw new ArgumentException("Invalid directory path");
            }
            try
            {
                if (type == Get.directories) return Directory.EnumerateDirectories(DirectoryPath);
                return Directory.EnumerateFiles(DirectoryPath, "*.*");
                               
            }
            catch (UnauthorizedAccessException e)
            {
                return Enumerable.Empty<string>();
            }
            catch (DirectoryNotFoundException)
            {
                return Enumerable.Empty<string>();
            }

        }
        public void GetDirectorySize()
        {
            Thread thread = new Thread(() =>
            {
                int count = 0;
                long size = 0;
                if (File.Exists(DirectoryPath))
                {
                    SizeHandler.Invoke(BytesToString(new FileInfo(DirectoryPath).Length));
                    return;
                }
                foreach (var item in new CurrentDirectory(DirectoryPath).GetAllDirectoryFiles())
                {
                    size += new FileInfo(item).Length;
                    count++;
                    if (count % 100 == 0) SizeHandler.Invoke(BytesToString(size));
                }
                SizeHandler.Invoke(BytesToString(size));
            });
            thread.Start();   
        }
        static public string GetFileSize(string fileSize)
        {
            if (!File.Exists(fileSize)) throw new ArgumentException("Invalid path");
            return BytesToString(new FileInfo(fileSize).Length);
        }

        static string BytesToString(long byteCount) // метод для вывода информации о размере файла
        {
            string[] suf = { "Byt", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }
    }

}