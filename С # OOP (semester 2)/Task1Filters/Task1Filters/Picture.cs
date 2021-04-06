using System;
using System.IO;

namespace Task1Filters
{
    public struct RGB
    {
        public byte rgbRed;
        public byte rgbGreen;
        public byte rgbBlue;
    }

    public class Picture
    {
        private struct BITMAPFILEHEADER
        {
            public ushort Type;
            public uint Size;
            public ushort Reserved1;
            public ushort Reserved2;
            public uint OffBits;
        }
        private struct BITMAPINFOHEADER
        {
            public uint Size;
            public uint Width;
            public uint Height;
            public ushort Planes;
            public ushort BitCount;
            public uint Compression;
            public uint SizeImage;
            public uint XPelsPerMeter;
            public uint YPelsPerMeter;
            public uint ClrUsed;
            public uint ClrImportant;
        }

        private BITMAPFILEHEADER bitFile;
        private BITMAPINFOHEADER bitInfo;
        private int vect;
        public RGB[,] Pixels { get; set; }
        public RGB[,] NewPixels { get; set; }
        public void Reading(FileStream input)
        {
            BinaryReader reader = new BinaryReader(input);
            bitFile.Type = reader.ReadUInt16();
            bitFile.Size = reader.ReadUInt32();
            bitFile.Reserved1 = reader.ReadUInt16();
            bitFile.Reserved2 = reader.ReadUInt16();
            bitFile.OffBits = reader.ReadUInt32();

            bitInfo.Size = reader.ReadUInt32();
            bitInfo.Width = reader.ReadUInt32();
            bitInfo.Height = reader.ReadUInt32();
            bitInfo.Planes = reader.ReadUInt16();
            bitInfo.BitCount = reader.ReadUInt16();
            bitInfo.Compression = reader.ReadUInt32();
            bitInfo.SizeImage = reader.ReadUInt32();
            bitInfo.XPelsPerMeter = reader.ReadUInt32();
            bitInfo.YPelsPerMeter = reader.ReadUInt32();
            bitInfo.ClrUsed = reader.ReadUInt32();
            bitInfo.ClrImportant = reader.ReadUInt32();
            if (bitInfo.BitCount != 24 && bitInfo.BitCount != 32)
            {
                throw new Exception("wrong picture format, try again");
            }
            vect = (int)(4 - (bitInfo.Width * (bitInfo.BitCount / 8)) % 4) & 3;
            Pixels = new RGB[bitInfo.Height, bitInfo.Width];
            for (var i = 0; i < bitInfo.Height; i++)
            {
                for (var j = 0; j < bitInfo.Width; j++)
                {
                    Pixels[i, j].rgbRed = (byte)reader.ReadByte();
                    Pixels[i, j].rgbGreen = (byte)reader.ReadByte();
                    Pixels[i, j].rgbBlue = (byte)reader.ReadByte();

                    if (bitInfo.BitCount == 32)
                    {
                        reader.ReadByte();
                    }
                }
                for (var k = 0; k < vect; k++)
                {
                    reader.ReadByte();
                }
            }
        }

        public void Writing(FileStream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(bitFile.Type);
            writer.Write(bitFile.Size);
            writer.Write(bitFile.Reserved1);
            writer.Write(bitFile.Reserved2);
            writer.Write(bitFile.OffBits);
            writer.Write(bitInfo.Size);
            writer.Write(bitInfo.Width);
            writer.Write(bitInfo.Height);
            writer.Write(bitInfo.Planes);
            writer.Write(bitInfo.BitCount);
            writer.Write(bitInfo.Compression);
            writer.Write(bitInfo.SizeImage);
            writer.Write(bitInfo.XPelsPerMeter);
            writer.Write(bitInfo.YPelsPerMeter);
            writer.Write(bitInfo.ClrUsed);
            writer.Write(bitInfo.ClrImportant);

            for (var i = 0; i < bitInfo.Height; i++)
            {
                for (var j = 0; j < bitInfo.Width; j++)
                {
                    writer.Write(NewPixels[i, j].rgbRed);
                    writer.Write(NewPixels[i, j].rgbGreen);
                    writer.Write(NewPixels[i, j].rgbBlue);
                    if (bitInfo.BitCount == 32)
                    {
                        writer.Write(0);
                    }
                }
                for (var k = 0; k < vect; k++)
                {
                    writer.Write(0);
                }

            }
        }

        public uint ReturnHeight()
        {
            return bitInfo.Height;
        }

        public uint ReturnWidth()
        {
            return bitInfo.Width;
        }
    }
}
