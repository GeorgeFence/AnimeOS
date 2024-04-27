using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using System.Drawing;

namespace AnimeOS
{
    public static class Files
    {
        public static CosmosVFS Driver;
        public static string CurrentDirectory = @"0:\";
        public static string Root = @"0:\";
        public static Disk disk;

        // directories
        // init
        public static void Initialize()
        {
            try
            {
                Driver = new CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(Driver);
            }
            // unknown exception
            catch (Exception ex)
            {
            }
        }
        // get files
        public static string[] GetFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            return files;
        }
        // get folders
        public static string[] GetFolders(string path)
        {
            string[] folders = Directory.GetDirectories(path);
            return folders;
        }
        // get volumes
        public static List<Sys.FileSystem.Listing.DirectoryEntry> GetVolumes()
        {
            return Driver.GetVolumes();
        }
        // exists
        public static bool FileExists(string file) { return File.Exists(@file); }
        public static bool FolderExists(string path) { return Directory.Exists(@path); }
        // reads
        public static string[] ReadLines(string path)
        {
            string[] data;
            data = File.ReadAllLines(path);
            return data;
        }
        public static string ReadText(string path)
        {
            string data;
            data = File.ReadAllText(path);
            return data;
        }
        public static byte[] ReadBytes(string path)
        {
            byte[] data;
            data = File.ReadAllBytes(path);
            return data;
        }
        // writes
        public static void WriteAllText(string path, string text)
        {
            File.WriteAllText(path, text);
        }
        public static void WriteAllLines(string path, string[] lines)
        {
            File.WriteAllLines(path, lines);
        }
        public static void WriteAllBytes(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }
        public static void WriteAllBytes(string path, List<byte> data)
        {
            byte[] input = new byte[data.Count];
            for (int i = 0; i < input.Length; i++) { input[i] = data[i]; }
            WriteAllBytes(path, input);
        }
        // creates
        public static bool CreateFolder(string name)
        {
            bool value = false;
            if (FolderExists(name)) { value = false; }
            else { Directory.CreateDirectory(name); value = true; }
            return value;
        }
        public static bool CreateFile(string name)
        {
            File.Create(name);
            return true;
        }
        // rename directory
        public static bool RenameFolder(string input, string newName)
        {
            bool value = false;
            if (Directory.Exists(input))
            { Directory.Move(input, newName); value = true; }
            else { value = false; }
            return value;
        }
        // rename file
        public static bool RenameFile(string input, string newName, bool overwrite)
        {
            bool value = false;
            if (FileExists(input))
            { File.Move(input, newName, overwrite); value = true; }
            else { value = false; }
            return value;
        }
        // delete directory
        public static bool DeleteFolder(string path)
        {
            if (FolderExists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                    if (!FolderExists(path)) { return true; }
                    else { return false; }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else { return false; }
        }
        // delete file
        public static bool DeleteFile(string file)
        {
            if (FileExists(file)) { File.Delete(file); return true; }
            else { return false; }
        }
        // get file info
        public static Cosmos.System.FileSystem.Listing.DirectoryEntry GetFileInfo(string file)
        {
            if (FileExists(file))
            {
                try
                {
                    Cosmos.System.FileSystem.Listing.DirectoryEntry attr = Driver.GetFile(file);
                    return attr;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static bool CopyFile(string src, string dest)
        {
            try
            {
                byte[] sourceData = ReadBytes(src);
                WriteAllBytes(dest, sourceData);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // convert bytes to megabytes
        public static double BytesToMB(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
        // convert bytes to kilobytes
        public static double BytesToKB(long bytes)
        {
            return bytes / 1024;
        }
    }
}
