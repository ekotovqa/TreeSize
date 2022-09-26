using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace TreeSize.App
{
    internal class MainViewModel : BaseViewModel
    {
        private string _selectedDrive;
        private ObservableCollection<string> _drives;
        public string SelectedDrive
        {
            get => _selectedDrive;
            set
            {
                _selectedDrive = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Drives
        {
            get => _drives;
            set
            {
                _drives = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Drives = new ObservableCollection<string>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady) Drives.Add(drive.Name);
            }
            SelectedDrive = Drives[0];
        }
    }
}
