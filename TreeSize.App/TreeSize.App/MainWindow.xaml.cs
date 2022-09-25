using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TreeSize.Core;

namespace TreeSize.App
{
    public partial class MainWindow
    {

        private ObservableCollection<TreeFolderItemViewModel> TreeFolderItemViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            TreeFolderItemViewModel = new ObservableCollection<TreeFolderItemViewModel>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TreeFolderItemViewModel.Clear();
            var folderPath = selectedDrive.SelectedItem.ToString();

            var treeFolderItemViewModel = new TreeFolderItemViewModel()
            {
                Name = System.IO.Path.GetFileName(folderPath),
                FullName = folderPath,
                IsFolder = true,
                Source = TreeFolderItemViewModel,
            };
            TreeFolderItemViewModel.Add(treeFolderItemViewModel);

            treeFolderItemViewModel.IsExpanded = true;
            Tree.ItemsSource = TreeFolderItemViewModel;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var listViewItem = (ListViewItem)sender;
            if (listViewItem.DataContext is TreeFolderItemViewModel folderItem)
            {
                folderItem.IsExpanded = !folderItem.IsExpanded;
            }
        }
    }
   
}
