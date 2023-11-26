namespace USBMonitor.Models;

public class MyFolder
{
    public MyFolder()
    {
        Children = new List<MyFolder>();
        Files = new List<MyFile>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string FullPath { get; set; }
    public Drive Drive { get; set; }
    public int? ParentId { get; set; }
    public MyFolder Parent { get; set; }
    public List<MyFolder> Children { get; set; }
    public List<MyFile> Files { get; set; } = new List<MyFile>();
    public DateTime CreationTime { get; set; }
    public DateTime LastAccessTime { get; set; }
    public DateTime LastWriteTime { get; set; }
}