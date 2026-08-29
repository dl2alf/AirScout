using System;
using System.Collections.Generic;

namespace AirScout.PlaneFeeds.Plugin.PublicBinCraft
{
    public static class BinaryUtils
    {
        public static int ReadInt32LE(byte[] buffer, int offset) => BitConverter.ToInt32(LittleEndianBytes(buffer, offset, 4), 0);
        public static uint ReadUInt32LE(byte[] buffer, int offset) => BitConverter.ToUInt32(LittleEndianBytes(buffer, offset, 4), 0);
        public static long ReadInt64LE(byte[] buffer, int offset) => BitConverter.ToInt64(LittleEndianBytes(buffer, offset, 8), 0);
        public static short ReadInt16LE(byte[] buffer, int offset) => BitConverter.ToInt16(LittleEndianBytes(buffer, offset, 2), 0);
        public static ushort ReadUInt16LE(byte[] buffer, int offset) => BitConverter.ToUInt16(LittleEndianBytes(buffer, offset, 2), 0);
        public static byte ReadByte(byte[] buffer, int offset) => buffer[offset];

        private static byte[] LittleEndianBytes(byte[] buffer, int offset, int count)
        {
            var slice = new byte[count];
            Buffer.BlockCopy(buffer, offset, slice, 0, count);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(slice);
            return slice;
        }

        // Reads a fixed-length, NUL-padded ASCII field (callsign/typeCode/registration in struct binCraft)
        public static string ReadAsciiString(byte[] buffer, int start, int length)
        {
            var chars = new List<char>();
            for (int i = start; i < start + length && buffer[i] != 0; i++)
            {
                if (buffer[i] >= 32 && buffer[i] < 127)
                    chars.Add((char)buffer[i]);
            }
            return new string(chars.ToArray()).Trim();
        }
    }
}
