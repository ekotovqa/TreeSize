using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeSize.Core;

namespace TreeSize.App
{
    internal class RootDirectoryViewModel : MainViewModel
    {
        readonly CurrentDirectory _currentDirectory;
        public string Name { get; }

        public RootDirectoryViewModel(string path)
        {
            _currentDirectory = new CurrentDirectory(path);
            Name = _currentDirectory.Name;
        }

        protected void LoadChildren() { }
    }
}
