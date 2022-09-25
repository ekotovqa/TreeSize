using System.Collections.ObjectModel;
using TreeSize.Core;
string directoryPath = @"d:\efi";

CurrentDirectory currentDirectory = new CurrentDirectory(directoryPath);
currentDirectory.SizeHandler += ShowOnConsole;


foreach (var subDirectory in currentDirectory.GetDirectoryContent(CurrentDirectory.Get.directories))
{
    currentDirectory.GetDirectorySize();
}



void ShowOnConsole(string size)
{
    Console.WriteLine(size);
}

static string BytesToString(long byteCount) // метод для вывода информации о размере файла
{
    string[] suf = { "Byt", "KB", "MB", "GB", "TB", "PB", "EB" };
    if (byteCount == 0)
        return "0" + suf[0];
    long bytes = Math.Abs(byteCount);
    int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
    double num = Math.Round(bytes / Math.Pow(1024, place), 1);
    return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
}