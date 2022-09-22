using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TreeSize.Core;

namespace TreeSize.App
{
    public class FoldersTreeViewModel : BaseViewModel
    {       
        public string FolderName { get; set; }
        public string InitialPath { get; set; }
        public ObservableCollection<FoldersTreeViewModel> Content { get; set; }
        public ObservableCollection<FilesTreeViewModel> Files { get; set; }

        public IList Children
        {
            get
            {
                return new CompositeCollection()
                {
                new CollectionContainer() { Collection = Content },
                new CollectionContainer() { Collection = Files }
                };
            }
        }

        public FoldersTreeViewModel(string path)
        {
            InitialPath = path;
            Content = new ObservableCollection<FoldersTreeViewModel>();
            Files = new ObservableCollection<FilesTreeViewModel>();
            
        }

        public ObservableCollection<FoldersTreeViewModel> GetContent()
        {
            CurrentDirectory currentDirectory = new CurrentDirectory(InitialPath);


            foreach (var file in currentDirectory.GetDirectoryContent(CurrentDirectory.Get.files))
            {
                FilesTreeViewModel fileModel = new FilesTreeViewModel(file);
                Files.Add(fileModel);
            }

            FolderName = currentDirectory.Name;
            foreach (var firstLevelItem in currentDirectory.GetDirectoryContent(CurrentDirectory.Get.directories))
            {
                FoldersTreeViewModel contentForTree = new FoldersTreeViewModel(firstLevelItem);
                Content.Add(contentForTree);

                CurrentDirectory subDirectory = new CurrentDirectory(contentForTree.InitialPath);
                contentForTree.FolderName = subDirectory.Name;
                foreach (var secondLevelItem in subDirectory.GetDirectoryContent(CurrentDirectory.Get.directories))
                {
                    FoldersTreeViewModel contentForTreeSecondLevel = new FoldersTreeViewModel(secondLevelItem);
                    contentForTree.Content.Add(contentForTreeSecondLevel);
                    CurrentDirectory subsubDirectory = new CurrentDirectory(contentForTreeSecondLevel.InitialPath);
                    contentForTreeSecondLevel.FolderName = subsubDirectory.Name;
                }
            }
            return Content;
        }

        public ObservableCollection<FoldersTreeViewModel> LoadChildren()
        {
            Content.Clear();
            return this.GetContent();
        }
    }
}
