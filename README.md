# USBMonitorForFlashDrive

USB Monitor is a tool that scans a USB drive when it's inserted, exports the directory structure to an Excel file, and copies the files to a specific location.

## Features

- Scans a USB drive upon insertion
- Exports the directory structure to an Excel file
- Copies the files to a specific location

## Usage

1. Open a command prompt as administrator.
2. Navigate to the directory containing the utility.
3. Run the utility as a command-line argument. For example:

    ```
    .\USBMonitor.exe
    ```
4. Insert a USB drive.
5. The application will automatically scan the USB drive, export the directory structure to an Excel file, and copy the files to a specific location.

## Note

The application requires access to the directories it scans. If it encounters a directory it cannot access, it will skip that directory and continue with the next one.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Author

Bohdan Harabadzhyu

## License

[MIT](https://choosealicense.com/licenses/mit/)
