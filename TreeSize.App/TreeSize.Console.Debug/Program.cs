using System.Collections.ObjectModel;
using TreeSize.Core;
string directoryPath = @"d:\efi";

//c:\Users\evgen\AppData\Roaming\Microsoft\Windows\Start Menu

CurrentDirectory currentDirectory = new CurrentDirectory(directoryPath);
//CurrentDirectory.SizeHandler += ShowOnConsole;
CurrentDirectory.ContentHandler += Show;

void Show(ObservableCollection<CurrentDirectory> obj)
{
    
        Console.WriteLine(obj.Count);

}

//foreach (var subDirectory in currentDirectory.GetDirectoryContent())
//{
//    //Console.WriteLine($"{subDirectory.Replace($"{directoryPath}", "")}");
//    CurrentDirectory.GetItemSize(subDirectory);
//}
Console.WriteLine("GOGO");
Thread.Sleep(5000);
Thread.Sleep(5000);
var result = 1024;


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