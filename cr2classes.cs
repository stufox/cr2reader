using System;

namespace cr2reader
{
    // notes to self:
    // cr2 has 4 IFD entries 0 -3. Detail here http://lclevy.free.fr/cr2/
    // the header has two offsets - the tiff offset points to IFD #0, the CR2 offset points to IFD #3
    // all the data we want is in the tiff offset

    public class IFDEntry
    {
        public ushort tagId {get;set;}
        public ushort tagType {get;set;}
        public uint numberOfValue {get;set;}
        public uint valuePointer {get;set;}

    }
    public class CR2Header
    {
        public byte[] byteOrder {get;set;}
        public ushort tiffMagicWord {get;set;}
        public uint tiffHeaderOffset {get;set;}
        public byte[] cr2MagicWord {get;set;}
        public byte[] cr2Version {get;set;}
        public uint rawIFDOffset {get;set;}
    }
}
/* 
                IFD 
                Number of entries (2 bytes)
                Entries (12 bytes each)
                Next IFD offset (4 bytes)

                
                IFD Entries
                TagID - 2 bytes
                Tag Type - 2 bytes
                Number of values - 4 bytes (e.g. for a string, number of chars + 0 ending)
                value or pointer to value - 4 bytes

                Tag Types
                1 - unsigned int
                2 - string (ending 0)
                3 - unsigned short (2 bytes)
                4 - unsigned long (4 bytes)
                5 - unsigned rational (2 unsigned long)
                6 - signed int
                7 - byte sequence
                8 - signed short
                9 - signed long
                10 - signed rational (2 signed long)
                11 - float (4 bytes)
                12 - float (8 bytes)
                
                Tag ID 0x0110 (272DEC) - Model (e.g. Canon EOS 6D)
                0x0132 (306DEC) - DateTime
                


                */