using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TreeSize.Core;

namespace TreeSize.App
{
    internal class MainViewModel : BaseViewModel
    {

        private string _selectedDrive;
        private ObservableCollection<string> _logicalDrives;       

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

        public MainViewModel()
        {
            LogicalDrives = new ObservableCollection<string>();
            //foreach (var logicalDrive in Directory.GetLogicalDrives())
            //{
            //    LogicalDrives.Add(logicalDrive);
            //}
            LogicalDrives = new ObservableCollection<string> { @"d:\efi", @"d:\11111", @"d:\openServer" };
            SelectedDrive = LogicalDrives[0];
        }
    } 
}
