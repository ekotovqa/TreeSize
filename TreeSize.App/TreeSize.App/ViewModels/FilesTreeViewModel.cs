using System.IO;

namespace TreeSize.App
{
    public class FilesTreeViewModel : BaseViewModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public FilesTreeViewModel(string path)
        {
            FilePath = path;
            FileName = Path.GetFileName(FilePath);
        }
    }
}