using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using TreeSize.Core;

namespace TreeSize.App
{
    internal class MainViewModel : BaseViewModel
    {
        private bool _isSelected;
        private bool _isExpanded;
        private string _selectedDrive;
        private ObservableCollection<string> _logicalDrives;
        private ObservableCollection<FoldersTreeViewModel> _foldersTreeViewModels;
        private ObservableCollection<FilesTreeViewModel> _filesTreeViewModels;
        public FoldersTreeViewModel TreeViewModel { get; set; }

        public IList FolderContent
        {
            get
            {
                return new CompositeCollection 
                {
                    new CollectionContainer { Collection = FoldersTreeViewModels },
                    new CollectionContainer { Collection = FilesTreeViewModels }
                };
            }
            set { }
        }

        public ObservableCollection<FilesTreeViewModel> FilesTreeViewModels
        {
            get => _filesTreeViewModels;
            set
            {
                _filesTreeViewModels = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FoldersTreeViewModel> FoldersTreeViewModels
        {
            get => _foldersTreeViewModels;
            set
            {
                _foldersTreeViewModels = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDrive
        {
            get => _selectedDrive;
            set
            {
                _selectedDrive = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> LogicalDrives
        {
            get => _logicalDrives;
            set
            {
                _logicalDrives = value;
                OnPropertyChanged();
            }
        }
       
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged();

                }
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            FoldersTreeViewModels = new ObservableCollection<FoldersTreeViewModel> ();
            FilesTreeViewModels = new ObservableCollection<FilesTreeViewModel> ();
            LogicalDrives = new ObservableCollection<string>();
            foreach (var logicalDrive in Directory.GetLogicalDrives())
            {
                LogicalDrives.Add(logicalDrive);
            }
            //LogicalDrives = new ObservableCollection<string> { @"d:\efi", @"d:\11111", @"d:\openServer" };
            SelectedDrive = LogicalDrives[0];
        }
    } 
}
