using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeSize.Core;

namespace TreeSize.App
{
    public class FolderTreeViweModel //: BaseViewModel
    {
        
        public string Name { get; set; }
        public string InitialPath { get; set; }
        public ObservableCollection<FolderTreeViweModel> Content { get; set; }


        public FolderTreeViweModel(string path)
        {
            InitialPath = path;
            Content = new ObservableCollection<FolderTreeViweModel>();
        }

        public ObservableCollection<FolderTreeViweModel> GetContent()
        {
            CurrentDirectory currentDirectory = new CurrentDirectory(InitialPath);
            Name = currentDirectory.Name;

            foreach (var firstLevelItem in currentDirectory.GetDirectoryContent())
            {
                FolderTreeViweModel contentForTree = new FolderTreeViweModel(firstLevelItem);
                Content.Add(contentForTree);

                CurrentDirectory subDirectory = new CurrentDirectory(contentForTree.InitialPath);
                contentForTree.Name = subDirectory.Name;
                foreach (var secondLevelItem in subDirectory.GetDirectoryContent())
                {
                    FolderTreeViweModel contentForTreeSecondLevel = new FolderTreeViweModel(secondLevelItem);
                    contentForTree.Content.Add(contentForTreeSecondLevel);
                    CurrentDirectory subsubDirectory = new CurrentDirectory(contentForTreeSecondLevel.InitialPath);
                    contentForTreeSecondLevel.Name = subsubDirectory.Name;
                }
            }
            return Content;
        }

        public ObservableCollection<FolderTreeViweModel> LoadChildren()
        {
            Content.Clear();
            return this.GetContent();
        }
    }
}
