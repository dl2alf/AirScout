using System;
using System.Collections.Generic;
using ZstdNet;

namespace AirScout.PlaneFeeds.Plugin.PublicBinCraft
{
    // air/ground status, matches airground_t in readsb.h
    public enum AirGround : byte
    {
        Invalid = 0,
        Ground = 1,
        Airborne = 2,
        Uncertain = 3
    }

    // First "element" of a binCraft response - metadata about the whole reply.
    // Written by readsb's apiReq() in api.c into a zeroed struct binCraft-sized slot.
    public class BinCraftHeader
    {
        public DateTime Now;
        public uint ElementSize;
        public uint AircraftWithPositionCount;
        public short South, West, North, East;
        public uint MessageCount;
        public uint ResultCount;
        public uint BinCraftVersion;
        public double MessageRate;
        public uint Flags;
    }

    // One aircraft record - mirrors struct binCraft in readsb's aircraft.h (packed, little-endian).
    // Scale factors below are the inverse of those applied by toBinCraft() in aircraft.c.
    public class BinCraftAircraft
    {
        public string Hex;
        public double? Seen;      // seconds since last message of any kind

        public double? Lat;
        public double? Lon;

        public double? BaroRate;  // ft/min
        public double? GeomRate;  // ft/min
        public double? BaroAlt;   // ft
        public double? GeomAlt;   // ft

        public string Squawk;
        public double? Gs;        // knots
        public double? Track;     // degrees
        public double? MagHeading;
        public double? TrueHeading;

        public byte Category;
        public AirGround AirGround;

        public string Callsign;
        public string TypeCode;
        public string Registration;

        public double? SeenPos;   // seconds since last valid position
    }

    public static class PublicBinCraftParser
    {
        // byte offsets into each 112-byte struct binCraft record (no TRACKS_UUID build)
        private const int OFF_HEX = 0;
        private const int OFF_SEEN = 4;
        private const int OFF_LON = 8;
        private const int OFF_LAT = 12;
        private const int OFF_BARO_RATE = 16;
        private const int OFF_GEOM_RATE = 18;
        private const int OFF_BARO_ALT = 20;
        private const int OFF_GEOM_ALT = 22;
        private const int OFF_SQUAWK = 32;
        private const int OFF_GS = 34;
        private const int OFF_TRACK = 40;
        private const int OFF_MAG_HEADING = 44;
        private const int OFF_TRUE_HEADING = 46;

        private const int OFF_CATEGORY = 64;
        private const int OFF_AIRGROUND_BYTE = 68; // low nibble = airground, high nibble = nav_altitude_src
        private const int OFF_VALID_BYTE0 = 73;    // nic_baro,alert,spi,callsign_valid,baro_alt_valid,geom_alt_valid,position_valid,gs_valid
        private const int OFF_VALID_BYTE1 = 74;    // ias_valid,tas_valid,mach_valid,track_valid,track_rate_valid,roll_valid,mag_heading_valid,true_heading_valid
        private const int OFF_VALID_BYTE2 = 75;    // baro_rate_valid,geom_rate_valid,...
        private const int OFF_VALID_BYTE3 = 76;    // gva_valid,sda_valid,squawk_valid,...

        private const int OFF_CALLSIGN = 78;
        private const int LEN_CALLSIGN = 8;
        private const int OFF_TYPECODE = 88;
        private const int LEN_TYPECODE = 4;
        private const int OFF_REGISTRATION = 92;
        private const int LEN_REGISTRATION = 12;
        private const int OFF_SEEN_POS = 108;

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static bool BitSet(byte b, int bit) => ((b >> bit) & 1) != 0;

        public static byte[] Decompress(byte[] data)
        {
            NativeLoader.LoadLibzstd();
            using (var decompressor = new Decompressor())
            {
                return decompressor.Unwrap(data);
            }
        }

        public static BinCraftHeader ParseHeader(byte[] data)
        {
            var h = new BinCraftHeader();
            long nowMs = BinaryUtils.ReadInt64LE(data, 0);
            h.Now = UnixEpoch.AddMilliseconds(nowMs);
            h.ElementSize = BinaryUtils.ReadUInt32LE(data, 8);
            h.AircraftWithPositionCount = BinaryUtils.ReadUInt32LE(data, 12);
            // offset 16: index (paging, unused here)
            h.South = BinaryUtils.ReadInt16LE(data, 20);
            h.West = BinaryUtils.ReadInt16LE(data, 22);
            h.North = BinaryUtils.ReadInt16LE(data, 24);
            h.East = BinaryUtils.ReadInt16LE(data, 26);
            h.MessageCount = BinaryUtils.ReadUInt32LE(data, 28);
            h.ResultCount = BinaryUtils.ReadUInt32LE(data, 32);
            // offset 36: dummy/reserved
            h.BinCraftVersion = BinaryUtils.ReadUInt32LE(data, 40);
            h.MessageRate = BinaryUtils.ReadUInt32LE(data, 44) / 10.0;
            h.Flags = BinaryUtils.ReadUInt32LE(data, 48);
            return h;
        }

        // Parses a raw (already decompressed) binCraft payload: header + N aircraft records.
        public static List<BinCraftAircraft> ParseAircraft(byte[] data, BinCraftHeader header)
        {
            var result = new List<BinCraftAircraft>();
            int stride = (int)header.ElementSize;
            if (stride <= 0)
                return result;

            // record 0 is the header slot - aircraft data starts at record 1
            for (int offset = stride; offset + stride <= data.Length; offset += stride)
            {
                result.Add(ParseOne(data, offset));
            }
            return result;
        }

        private static BinCraftAircraft ParseOne(byte[] data, int offset)
        {
            byte validByte0 = BinaryUtils.ReadByte(data, offset + OFF_VALID_BYTE0);
            byte validByte1 = BinaryUtils.ReadByte(data, offset + OFF_VALID_BYTE1);
            byte validByte2 = BinaryUtils.ReadByte(data, offset + OFF_VALID_BYTE2);
            byte validByte3 = BinaryUtils.ReadByte(data, offset + OFF_VALID_BYTE3);

            bool callsignValid = BitSet(validByte0, 3);
            bool baroAltValid = BitSet(validByte0, 4);
            bool geomAltValid = BitSet(validByte0, 5);
            bool positionValid = BitSet(validByte0, 6);
            bool gsValid = BitSet(validByte0, 7);

            bool trackValid = BitSet(validByte1, 3);
            bool magHeadingValid = BitSet(validByte1, 6);
            bool trueHeadingValid = BitSet(validByte1, 7);

            bool baroRateValid = BitSet(validByte2, 0);

            bool squawkValid = BitSet(validByte3, 2);

            uint hexRaw = BinaryUtils.ReadUInt32LE(data, offset + OFF_HEX);

            var ac = new BinCraftAircraft
            {
                Hex = (hexRaw & 0xFFFFFF).ToString("X6"),
                Seen = BinaryUtils.ReadInt32LE(data, offset + OFF_SEEN) / 10.0,

                Lat = positionValid ? BinaryUtils.ReadInt32LE(data, offset + OFF_LAT) / 1e6 : (double?)null,
                Lon = positionValid ? BinaryUtils.ReadInt32LE(data, offset + OFF_LON) / 1e6 : (double?)null,

                BaroRate = baroRateValid ? BinaryUtils.ReadInt16LE(data, offset + OFF_BARO_RATE) * 8.0 : (double?)null,
                BaroAlt = baroAltValid ? BinaryUtils.ReadInt16LE(data, offset + OFF_BARO_ALT) * 25.0 : (double?)null,
                GeomAlt = geomAltValid ? BinaryUtils.ReadInt16LE(data, offset + OFF_GEOM_ALT) * 25.0 : (double?)null,

                Squawk = squawkValid ? BinaryUtils.ReadUInt16LE(data, offset + OFF_SQUAWK).ToString("X4") : null,
                Gs = gsValid ? BinaryUtils.ReadInt16LE(data, offset + OFF_GS) / 10.0 : (double?)null,
                Track = trackValid ? BinaryUtils.ReadInt16LE(data, offset + OFF_TRACK) / 90.0 : (double?)null,
                MagHeading = magHeadingValid ? BinaryUtils.ReadInt16LE(data, offset + OFF_MAG_HEADING) / 90.0 : (double?)null,
                TrueHeading = trueHeadingValid ? BinaryUtils.ReadInt16LE(data, offset + OFF_TRUE_HEADING) / 90.0 : (double?)null,

                Category = BinaryUtils.ReadByte(data, offset + OFF_CATEGORY),
                AirGround = (AirGround)(BinaryUtils.ReadByte(data, offset + OFF_AIRGROUND_BYTE) & 0x0F),

                Callsign = callsignValid ? BinaryUtils.ReadAsciiString(data, offset + OFF_CALLSIGN, LEN_CALLSIGN) : null,
                TypeCode = BinaryUtils.ReadAsciiString(data, offset + OFF_TYPECODE, LEN_TYPECODE),
                Registration = BinaryUtils.ReadAsciiString(data, offset + OFF_REGISTRATION, LEN_REGISTRATION),

                SeenPos = positionValid ? BinaryUtils.ReadInt32LE(data, offset + OFF_SEEN_POS) / 10.0 : (double?)null,
            };

            return ac;
        }
    }
}
