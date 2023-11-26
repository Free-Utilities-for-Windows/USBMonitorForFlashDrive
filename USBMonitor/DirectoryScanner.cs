using USBMonitor.Models;
using File = System.IO.File;

namespace USBMonitor;

public class DirectoryScanner
{
    public MyFolder ScanDirectory(string path)
    {
        MyFolder rootFolder = new MyFolder { Name = Path.GetFileName(path), FullPath = path };

        try
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                if (HasAccessToFolder(directory))
                {
                    rootFolder.Children.Add(ScanDirectory(directory));
                }
            }

            foreach (string file in Directory.GetFiles(path))
            {
                rootFolder.Files.Add(new MyFile { Name = Path.GetFileName(file), FullPath = file });
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Access denied to directory.");
        }

        return rootFolder;
    }

    private MyFolder CreateFolder(DirectoryInfo dirInfo, MyFolder parent, Drive drive)
    {
        MyFolder folder = new MyFolder
        {
            Id = dirInfo.FullName.GetHashCode(),
            Name = dirInfo.Name,
            FullPath = dirInfo.FullName,
            Drive = drive,
            Parent = parent,
            Children = new List<MyFolder>()
        };

        try
        {
            foreach (DirectoryInfo childDirInfo in dirInfo.GetDirectories())
            {
                MyFolder childFolder = CreateFolder(childDirInfo, folder, drive);
                folder.Children.Add(childFolder);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access denied to directory: {ex.Message}");
        }

        return folder;
    }

    public void PrintFolderStructure(MyFolder folder, string indent = "")
    {
        Console.WriteLine($"{indent}{folder.Name}");

        foreach (MyFolder childFolder in folder.Children)
        {
            PrintFolderStructure(childFolder, indent + "  ");
        }
    }
    
    private bool HasAccessToFolder(string folderPath)
    {
        try
        {
            Directory.GetDirectories(folderPath);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    public List<MyFolder> FlattenFolderStructure(MyFolder rootFolder)
    {
        List<MyFolder> folders = new List<MyFolder>();
        FlattenFolderStructure(rootFolder, folders);
        return folders;
    }

    private void FlattenFolderStructure(MyFolder folder, List<MyFolder> folders)
    {
        folders.Add(folder);

        foreach (MyFolder childFolder in folder.Children)
        {
            FlattenFolderStructure(childFolder, folders);
        }
    }
}