using System.Collections.ObjectModel;

namespace TreeSize.Core
{
    public class CurrentDirectory
    {
        public string DirectoryPath { get; set; }
        public static event Action<long> SizeHandler;
        public static event Action<ObservableCollection<CurrentDirectory>>? ContentHandler;
        public string Name { get; set; }
        public ObservableCollection<CurrentDirectory> Content { get; set; }

        public CurrentDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath)) throw new ArgumentException("Invalid path");
            DirectoryPath = directoryPath;
            Name = Path.GetFileName(directoryPath);
            //Content = new ObservableCollection<CurrentDirectory>();

            //new Task(() =>
            //{
            //    foreach (var subDirectory in GetDirectoryContent())
            //    {
            //        Content.Add(new CurrentDirectory(subDirectory));
            //        ContentHandler?.Invoke(Content);
            //    }
                
            //}).Start();
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

        public IEnumerable<string> GetDirectoryContent()
        {
            if (File.Exists(DirectoryPath)) return Enumerable.Empty<string>();
            if (!Directory.Exists(DirectoryPath))
            {
                return Enumerable.Empty<string>();
                throw new ArgumentException("Invalid directory path");
            }
            try
            {
                return Directory.EnumerateDirectories(DirectoryPath).Union(Directory.EnumerateFiles(DirectoryPath, "*.*"));               
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
        static public void GetItemSize(string directoryPath)
        {
            Thread thread = new Thread(() =>
            {
                int count = 0;
                long size = 0;
                if (File.Exists(directoryPath))
                {
                    SizeHandler.Invoke(new FileInfo(directoryPath).Length);
                    return;
                }
                foreach (var item in new CurrentDirectory(directoryPath).GetAllDirectoryFiles())
                {
                    size += new FileInfo(item).Length;
                    count++;
                    if (count % 100 == 0) SizeHandler.Invoke(size);
                }
                SizeHandler.Invoke(size);
            });
            thread.Start();
            
        }
    }
}