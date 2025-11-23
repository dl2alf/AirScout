using System;
using System.Collections.Generic;
using System.IO;
using ZstdNet;

public static class BinCraftParser
{
    public static Dictionary<string, object> Parse(byte[] data, bool zstdCompressed = false)
    {
        if (zstdCompressed)
        {
            NativeLoader.LoadLibzstd();
            using (var decompressor = new Decompressor())
            {
                data = decompressor.Unwrap(data);
            }
        }

        var result = new Dictionary<string, object>();

        uint nowLow = BinaryUtils.ReadUInt32LE(data, 0);
        uint nowHigh = BinaryUtils.ReadUInt32LE(data, 4);
        int stride = (int)BinaryUtils.ReadUInt32LE(data, 8);
        uint globalAcCount = BinaryUtils.ReadUInt32LE(data, 12);
        uint globeIndex = BinaryUtils.ReadUInt32LE(data, 16);

        result["now"] = nowLow / 1000.0 + nowHigh * 4294967.296;
        result["stride"] = stride;
        result["global_ac_count_withpos"] = globalAcCount;
        result["globeIndex"] = globeIndex;

        result["south"] = BinaryUtils.ReadInt16LE(data, 20);
        result["west"] = BinaryUtils.ReadInt16LE(data, 22);
        result["north"] = BinaryUtils.ReadInt16LE(data, 24);
        result["east"] = BinaryUtils.ReadInt16LE(data, 26);

        var aircraftList = new List<Dictionary<string, object>>();

        for (int offset = stride; offset + stride <= data.Length; offset += stride)
        {
            var ac = new Dictionary<string, object>();

            // Correction example:
            int icao = BinaryUtils.ReadInt32LE(data, offset + 0);   // offset 0
            ac["hex"] = (icao & 0xFFFFFF).ToString("X6");

            ac["versionType"] = BinaryUtils.ReadUInt16LE(data, offset + 4);  // if present
            ac["flags"] = BinaryUtils.ReadUInt16LE(data, offset + 6);        // if present

            ac["lon"] = BinaryUtils.ReadInt32LE(data, offset + 8) / 1e6;
            ac["lat"] = BinaryUtils.ReadInt32LE(data, offset + 12) / 1e6;
            ac["alt_baro"] = BinaryUtils.ReadInt16LE(data, offset + 20) * 25;
            ac["alt_geom"] = BinaryUtils.ReadInt16LE(data, offset + 22) * 25;

            ac["track"] = BinaryUtils.ReadInt16LE(data, offset + 24) / 90.0;
            ac["gs"] = BinaryUtils.ReadInt16LE(data, offset + 26) / 10.0;
            ac["seen_pos"] = BinaryUtils.ReadUInt16LE(data, offset + 54) / 10.0;

            aircraftList.Add(ac);
        }

        result["aircraft"] = aircraftList;
        return result;
    }

    private static string GetType(int t)
    {
        switch (t)
        {
            case 0: return "adsb_icao";
            case 1: return "adsb_icao_nt";
            case 2: return "adsr_icao";
            case 3: return "tisb_icao";
            case 4: return "adsc";
            case 5: return "mlat";
            case 6: return "other";
            case 7: return "mode_s";
            case 8: return "adsb_other";
            case 9: return "adsr_other";
            case 10: return "tisb_trackfile";
            case 11: return "tisb_other";
            case 12: return "mode_ac";
            default: return "unknown";
        }
    }

}
