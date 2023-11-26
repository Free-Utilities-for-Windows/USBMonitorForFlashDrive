using OfficeOpenXml;
using USBMonitor.Models;

namespace USBMonitor;

public class FolderExporter
{
    public byte[] ExportToExcel(List<MyFolder> folders)
    {
        using (MemoryStream stream = new MemoryStream())
        using (ExcelPackage package = new ExcelPackage(stream))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Folders and Files");

            int row = 1;
            foreach (MyFolder folder in folders)
            {
                worksheet.Cells[row, 1].Value = folder.FullPath;
                row++;

                foreach (MyFile file in folder.Files)
                {
                    worksheet.Cells[row, 1].Value = file.FullPath;
                    row++;
                }
            }

            package.Save();
            return stream.ToArray();
        }
    }

    public string SaveToExcel(List<MyFolder> folders, string deviceName)
    {
        try
        {
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "USBScanner");
            Console.WriteLine($"Creating directory: {defaultPath}");
            Directory.CreateDirectory(defaultPath);

            string deviceFolderName = $"{deviceName}_{DateTime.Now:yyyyMMdd_HHmmss}";
            string deviceFolderPath = Path.Combine(defaultPath, deviceFolderName);
            Console.WriteLine($"Creating device directory: {deviceFolderPath}");
            Directory.CreateDirectory(deviceFolderPath);

            string filePath = Path.Combine(deviceFolderPath, "output.xlsx");
            Console.WriteLine($"Saving Excel file to: {filePath}");

            byte[] excelData = ExportToExcel(folders);
            File.WriteAllBytes(filePath, excelData);
            
            return deviceFolderPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving to Excel: {ex.Message}");
            throw;
        }
    }
    
    public (int textFilesCount, int imageFilesCount) CopyFiles(List<MyFolder> folders, string destinationPath)
    {
        string[] textExtensions = new[] { ".doc", ".docx", ".txt" };
        string[] imageExtensions = new[] { ".jpg", ".png", ".jpeg" };

        int textFilesCount = 0;
        int imageFilesCount = 0;

        foreach (MyFolder folder in folders)
        {
            foreach (MyFile file in folder.Files)
            {
                string fileExtension = Path.GetExtension(file.FullPath).ToLower();

                if (textExtensions.Contains(fileExtension))
                {
                    string destinationFilePath = Path.Combine(destinationPath, Path.GetFileName(file.FullPath));
                    File.Copy(file.FullPath, destinationFilePath, true);
                    textFilesCount++;
                }
                else if (imageExtensions.Contains(fileExtension))
                {
                    string destinationFilePath = Path.Combine(destinationPath, Path.GetFileName(file.FullPath));
                    File.Copy(file.FullPath, destinationFilePath, true);
                    imageFilesCount++;
                }
            }
        }

        return (textFilesCount, imageFilesCount);
    }
}