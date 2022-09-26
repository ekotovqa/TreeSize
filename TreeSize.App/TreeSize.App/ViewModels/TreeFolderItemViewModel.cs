using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace TreeSize.App
{
    internal class TreeFolderItemViewModel : BaseViewModel
    {
        private string _name;
        private string _size;
        private int _filesNumber = 0;
        private int _foldersNumber = 0;
        private bool _isExpanded = false;
        private ObservableCollection<TreeFolderItemViewModel> _subFolderItems;
        public event EventHandler<string> SizeHandler;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        public int FilesNumber
        {
            get => _filesNumber;
            set
            {
                _filesNumber = value;
                OnPropertyChanged();
            }
        }

        public int FoldersNumber
        {
            get => _foldersNumber;
            set
            {
                _foldersNumber = value;
                OnPropertyChanged();
            }
        }

        public string FullName { get; set; }
        public bool IsFolder { get; set; } = true;
        public int HierarchyLevel { get; set; } = 0;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));

                if (IsFolder)
                {
                    if (IsExpanded)
                    {
                        AddSubFolderItems();
                    }
                    else
                    {
                        RemoveSubFolderItems();
                    }
                }
            }
        }

        public ObservableCollection<TreeFolderItemViewModel> Source { get; set; }
        public ObservableCollection<TreeFolderItemViewModel> SubFolderItems
        {
            get => _subFolderItems;
            set
            {
                _subFolderItems = value;
                OnPropertyChanged();
            }
        }

        public TreeFolderItemViewModel()
        {
            SubFolderItems = new ObservableCollection<TreeFolderItemViewModel>();
        }

        private void AddSubFolderItems()
        {
            var subFolderItems = GetSubFolderItems();
            var files = GetFiles();
            FilesNumber = files.Count;
            FoldersNumber = subFolderItems.Count;

            var index = Source.IndexOf(this);
            var hierarchyLevel = HierarchyLevel + 1;

            subFolderItems.ForEach(d =>
            {
                index++;
                d.HierarchyLevel = hierarchyLevel;
                Source.Insert(index, d);
                d.FoldersNumber = d.GetSubFolderItems().Count;
                d.FilesNumber = d.GetFiles().Count;
                d.SizeHandler += D_SizeHandler;
                d.GetFolderSize(d.FullName);
                SubFolderItems.Add(d);
            });

            files.ForEach(f =>
            {
                index++;
                f.HierarchyLevel = hierarchyLevel;
                Source.Insert(index, f);
                SubFolderItems.Add(f);
            });
        }

        private void D_SizeHandler(object? sender, string e)
        {
            if (sender is TreeFolderItemViewModel treeFolderItemViewModel) treeFolderItemViewModel.Size = e;
        }

        private void RemoveSubFolderItems()
        {
            for (int i = 0; i < SubFolderItems.Count; i++)
            {
                SubFolderItems[i].IsExpanded = false;
                Source.Remove(SubFolderItems[i]);
            }

            SubFolderItems.Clear();
        }

        private List<TreeFolderItemViewModel> GetSubFolderItems()
        {

            return GetContent(Type.folders).Select(f =>
                new TreeFolderItemViewModel()
                {
                    Name = Path.GetFileName(f),
                    FullName = f,
                    Size = "0 Byt",
                    Source = Source                   
                })
                .ToList();
        }

        private List<TreeFolderItemViewModel> GetFiles()
        {
            var f = GetContent(Type.files);
            return GetContent(Type.files).Select(f =>
                new TreeFolderItemViewModel()
                {
                    Name = Path.GetFileName(f),
                    FullName = f,
                    IsFolder = false,
                    Size = GetFileSize(f)
                })
                .ToList();
        }

        private enum Type
        {
            files,
            folders,
        }

        private IEnumerable<string> GetContent(Type type)
        {
            if (File.Exists(FullName)) return Enumerable.Empty<string>();
            if (!Directory.Exists(FullName))
            {
                return Enumerable.Empty<string>();
                throw new ArgumentException("Invalid directory path");
            }
            try
            {
                if (type == Type.folders) return Directory.EnumerateDirectories(FullName);
                return Directory.EnumerateFiles(FullName, "*.*");

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

        private string GetFileSize(string filePath)
        {
            if (!File.Exists(filePath)) throw new ArgumentException("Invalid path");
            return BytesToStringConvertor.BytesToString(new FileInfo(filePath).Length);
        }

        public string GetFolderSize(string folderPath)
        {
            long size = 0;
            Thread thread = new Thread(() =>
            {
                int count = 0;
                foreach (var item in GetAllDirectoryFiles(folderPath))
                {
                    size += new FileInfo(item).Length;
                    count++;
                    if (count % 100 == 0) SizeHandler.Invoke(this, BytesToStringConvertor.BytesToString(size));
                }
                SizeHandler.Invoke(this, BytesToStringConvertor.BytesToString(size));
            });
            thread.Start();
            return BytesToStringConvertor.BytesToString(size);
        }

        private IEnumerable<string> GetAllDirectoryFiles(string folderPath)
        {
            Stack<string> dirs = new Stack<string>(20);
            IEnumerable<string> result = Enumerable.Empty<string>();
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Invalid directory path");
            }
            dirs.Push(folderPath);
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
    }
}
