using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace cr2reader
{
    class Program
    {
        static void Main(string[] args)
        {

            string dirPath = "/Users/stufox/Documents/test/cr2";
            string filePattern = "*.CR2";
            const ushort takenDateTagId = 306;
            List<string> files = new List<string>(Directory.EnumerateFiles(dirPath,filePattern));
            foreach (var file in files)
            {
                System.Console.WriteLine($"Processing file {file}");
            using (BinaryReader reader = new BinaryReader(File.Open(file,FileMode.Open,FileAccess.Read)))
            {
                var header = new CR2Header();
                header.byteOrder = reader.ReadBytes(2);
                header.tiffMagicWord = reader.ReadUInt16();
                header.tiffHeaderOffset = reader.ReadUInt32();
                header.cr2MagicWord = reader.ReadBytes(2);
                header.cr2Version = reader.ReadBytes(2);
                header.rawIFDOffset = reader.ReadUInt32();


                string dateTagPattern = @"\d{4}:\d{2}:\d{2} \d{2}:\d{2}:\d{2}";
                var numEntries = reader.ReadUInt16();

                var entries = new List<IFDEntry>();
                for (int i=1;i<=numEntries;i++)
                {
                    var entry = new IFDEntry();
                    entry.tagId = reader.ReadUInt16();
                    entry.tagType = reader.ReadUInt16();
                    entry.numberOfValue = reader.ReadUInt32();
                    entry.valuePointer = reader.ReadUInt32();
                    entries.Add(entry);

                    }
                    //need to add validation here to test for case if the tag isn't populated
                    foreach (var entry in entries)
                    {
                        switch(entry.tagId)
                        {
                            case takenDateTagId:
                                reader.BaseStream.Seek(entry.valuePointer,SeekOrigin.Begin);
                                var bytes = reader.ReadBytes((int)entry.numberOfValue);
                                System.Console.WriteLine($"Tag id {entry.tagId} has contents of {Encoding.ASCII.GetString(bytes)}");
                                if (Regex.IsMatch(Encoding.ASCII.GetString(bytes),dateTagPattern))
                                    System.Console.WriteLine("matched your regex");
                                break;
                            default:
                                break;

                        }
                    }
                }

            } 
            
        }
    }
}
