using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ConsoleDemoApp;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Linq;

static void Main(string[] args)
{
    // Parse command-line arguments
    if (args.Length < 2)
    {
        Console.WriteLine("Usage: FileAnalyzer.exe <inputDirectory> <outputCSVFile> [--subdirectories]");
        return;
    }

    string inputDirectory = args[0];
    string outputCSVFile = args[1];
    bool includeSubdirectories = args.Length > 2 && args[2] == "--subdirectories";

    // Get a list of all files in the directory and its subdirectories
    string[] files = Directory.GetFiles(inputDirectory, "*", includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

    // Initialize a list to store file information
    List<FilePatInf> fileInfoList = new List<FilePatInf>();


    foreach (string file in files)
    {
        byte[] fileSignature = new byte[4];

        using (FileStream fileStm = new FileStream(file, FileMode.Open, FileAccess.Read))
        {
            fileStm.Read(fileSignature, 0, 4);
        }

        string fileSignatureHex = BitConverter.ToString(fileSignature).Replace("-", "");

        if (fileSignatureHex == "FFD8")
        {
            // JPG file signature
            AddFileInfo(fileInfoList, file, "JPG");
        }
        else if (fileSignatureHex == "25504446")
        {
            // PDF file signature
            AddFileInfo(fileInfoList, file, "PDF");
        }
    }


    static void AddFileInfo(List<FilePatInf> fileInfoList, string filePath, string fileType)
    {
        // Calculate MD5 hash of the file content
        string md5Hash = CalculateMD5(filePath);

        fileInfoList.Add(new FilePatInf
        {
            FilePath = filePath,
            FileType = fileType,
            MD5Hash = md5Hash
        });
    }

    static string CalculateMD5(string filePath)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hashBytes = md5.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

    static void WriteToCSV(string outputCSVFile, List<FilePatInf> fileInfoList)
    {
        using (StreamWriter sw = new StreamWriter(outputCSVFile))
        {
            sw.WriteLine("FilePath,FileType,MD5Hash");

            foreach (var fileInfo in fileInfoList)
            {
                sw.WriteLine($"{fileInfo.FilePath},{fileInfo.FileType},{fileInfo.MD5Hash}");
            }
        }
    }


//    create a command line application in c# that takes inputs and a flag 
//a. A directory that contains the files to be analyzed
//b.A path for the output file including file name and extension
//c.A flag to determine whether or not to include subdirectories contained, and all subsequently embedded subdirectories within the input directory a above
//Process each of the files in the directory and subdirectories if the flag is present
//Determines using a file signature if a given file is PDF or a JPG
//a.JPG files start with 0xFFD8
//b.PDF files start with 0x25504446
//For each file that is a PDF or a JPG, creates an entry in the output CSV containing the following information
//a.the full path of the file
//b.the actual file type
//c.the MD5 hash of the file content
}








Console.WriteLine("Hello, World!");
