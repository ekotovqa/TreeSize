using System.Collections.ObjectModel;
using TreeSize.Core;
string directoryPath = @"c:\";

CurrentDirectory currentDirectory = new CurrentDirectory(directoryPath);
CurrentDirectory.SizeHandler += ShowOnConsole;;


foreach (var subDirectory in currentDirectory.GetDirectoryContent(CurrentDirectory.Get.directories))
{
    CurrentDirectory.GetItemSize(subDirectory);
}



void ShowOnConsole(long size)
{
    Console.WriteLine(BytesToString(size));
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