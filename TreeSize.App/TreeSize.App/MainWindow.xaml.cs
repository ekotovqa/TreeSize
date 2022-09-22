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
        readonly  MainViewModel _mainViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FoldersTreeViewModel folderTreeViweModel = new FoldersTreeViewModel(selectedDrive.SelectedItem.ToString());
            folderTreeViweModel.GetContent();
            _mainViewModel.FoldersTreeViewModels = folderTreeViweModel.Content;
            _mainViewModel.FilesTreeViewModels = folderTreeViweModel.Files;
        }
         
        private void fileTree_Expanded(object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)e.OriginalSource;
            var node = (FoldersTreeViewModel)treeViewItem.Header;
            node.LoadChildren();
        }
    }
   
}
