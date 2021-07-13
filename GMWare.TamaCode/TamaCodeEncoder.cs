// SPDX-License-Identifier: GPL-3.0-or-later
/*
 * GMWare.TamaCode: Library for encoding and decoding Tamagotchi Pix Tama Codes
 * Copyright (C) 2021  Yukai Li
 * 
 * This file is part of GMWare.TamaCode.
 * 
 * GMWare.TamaCode is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * GMWare.TamaCode is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with GMWare.TamaCode.  If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GMWare.TamaCode
{
    /// <summary>
    /// Encodes and decodes Tama Codes.
    /// </summary>
    public static class TamaCodeEncoder
    {
        static readonly byte[] SALT = Encoding.ASCII.GetBytes("z7CM9GpFmH7Nq5XSVs48NkDha2SnuZpk");

        static Random random = new Random();

        /// <summary>
        /// Decode a Tama Code.
        /// </summary>
        /// <param name="code">The code contents as bytes.</param>
        /// <returns>The decoded Tama Code.</returns>
        public static BaseTamaCode Decode(byte[] code)
        {
            if (code.Length != 53) throw new ArgumentException("Code must be 53 bytes long.", nameof(code));
            byte[] seed = new byte[3];
            byte[] data = new byte[code.Length - seed.Length];
            Buffer.BlockCopy(code, 0, seed, 0, seed.Length);
            Buffer.BlockCopy(code, seed.Length, data, 0, data.Length);
            CryptCode(data, seed);

            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryReader br = new BinaryReader(ms);
                if (new string(br.ReadChars(3)) != "TMG") throw new ArgumentException("Code is not a Tama Code.", nameof(code));
                var codeType = (TamaCodeType)br.ReadByte();
                BaseTamaCode tamaCode = MakeTamaCodeByType(codeType);
                if (tamaCode is FriendTamaCode friendCode)
                {
                    friendCode.DeviceId = br.ReadUInt32();
                    var stats = ReadTamaStats(br);
                    friendCode.Stats = stats;
                    var profile = ReadTamaProfile(br);
                    friendCode.Profile = profile;

                    friendCode.ReadTypeSpecificData(br);

                    profile.Name = TamaCodeTextEncoding.Instance.Decode(br, profile.NameLength);
                    profile.Greeting = TamaCodeTextEncoding.Instance.Decode(br, profile.GreetingLength);
                }
                else
                {
                    tamaCode.ReadTypeSpecificData(br);
                }

                return tamaCode;
            }
        }

        /// <summary>
        /// Encodes a Tama Code.
        /// </summary>
        /// <param name="code">The Tama Code to encode.</param>
        /// <returns>The encoded byte representation of the Tama Code.</returns>
        public static byte[] Encode(BaseTamaCode code)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write("TMG".ToCharArray());
                bw.Write((byte)code.CodeType);

                if (code is FriendTamaCode friendCode)
                {
                    bw.Write(friendCode.DeviceId);
                    WriteTamaStats(bw, friendCode.Stats);
                    WriteTamaProfile(bw, friendCode.Profile);

                    friendCode.WriteTypeSpecificData(bw);

                    TamaCodeTextEncoding.Instance.Encode(bw, friendCode.Profile.Name);
                    TamaCodeTextEncoding.Instance.Encode(bw, friendCode.Profile.Greeting);
                }
                else
                {
                    code.WriteTypeSpecificData(bw);
                }

                // Pad to 50 bytes
                if (ms.Length < 50) bw.Write(new byte[50 - ms.Length]);

                data = ms.ToArray();
            }

            byte[] seed = new byte[3];
            random.NextBytes(seed);
            CryptCode(data, seed);

            byte[] encoded = new byte[seed.Length + data.Length];
            Buffer.BlockCopy(seed, 0, encoded, 0, seed.Length);
            Buffer.BlockCopy(data, 0, encoded, seed.Length, data.Length);
            return encoded;
        }

        static TamaStats ReadTamaStats(BinaryReader br)
        {
            if (br == null) throw new ArgumentNullException(nameof(br));
            var bitReader = new LsbBitReader(br);
            TamaStats ts = new TamaStats();
            ts.Tama = (int)bitReader.Read(8);
            ts.Age = (int)bitReader.Read(10);
            ts.Accessory = (int)bitReader.Read(6);
            ts.AccessoryVariant = (int)bitReader.Read(6);
            ts.RaisedTama = (int)bitReader.Read(8);
            ts.RaisedAge = (int)bitReader.Read(10);
            return ts;
        }

        static void WriteTamaStats(BinaryWriter bw, TamaStats ts)
        {
            if (bw == null) throw new ArgumentNullException(nameof(bw));
            if (ts == null) throw new ArgumentNullException(nameof(ts));
            var bitWriter = new LsbBitWriter(bw);
            bitWriter.Write((uint)ts.Tama, 8);
            bitWriter.Write((uint)ts.Age, 10);
            bitWriter.Write((uint)ts.Accessory, 6);
            bitWriter.Write((uint)ts.AccessoryVariant, 6);
            bitWriter.Write((uint)ts.RaisedTama, 8);
            bitWriter.Write((uint)ts.RaisedAge, 10);
            bitWriter.Flush();
        }

        static TamaProfile ReadTamaProfile(BinaryReader br)
        {
            if (br == null) throw new ArgumentNullException(nameof(br));
            var bitReader = new LsbBitReader(br);
            TamaProfile tp = new TamaProfile();
            tp.BirthMonth = (int)bitReader.Read(4);
            tp.BirthDay = (int)bitReader.Read(5);
            tp.NameLength = (int)bitReader.Read(4);
            tp.GreetingLength = (int)bitReader.Read(6);
            return tp;
        }

        static void WriteTamaProfile(BinaryWriter bw, TamaProfile tp)
        {
            if (bw == null) throw new ArgumentNullException(nameof(bw));
            if (tp == null) throw new ArgumentNullException(nameof(tp));
            var bitWriter = new LsbBitWriter(bw);
            bitWriter.Write((uint)tp.BirthMonth, 4);
            bitWriter.Write((uint)tp.BirthDay, 5);
            bitWriter.Write((uint)(tp.Name?.Length ?? 0), 4);
            bitWriter.Write((uint)(tp.Greeting?.Length ?? 0), 6);
            bitWriter.Flush();
        }

        static byte[] DeriveKey(byte[] seed)
        {
            if (seed == null) throw new ArgumentNullException(nameof(seed));
            if (seed.Length != 3) throw new ArgumentException("Seed must be 3 bytes in length.", nameof(seed));

            StringBuilder sb = new StringBuilder();
            using (SHA256 sha = SHA256.Create())
            {
                sha.TransformBlock(seed, 0, seed.Length, seed, 0);
                sha.TransformFinalBlock(SALT, 0, SALT.Length);
                for (int i = 0; i < 16; ++i)
                {
                    sb.AppendFormat("{0:x2}", sha.Hash[i]);
                }
            }
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        static void CryptCode(byte[] data, byte[] seed)
        {
            byte[] key = DeriveKey(seed);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] ^= key[i % key.Length];
            }
        }

        internal static BaseTamaCode MakeTamaCodeByType(TamaCodeType type)
        {
            switch (type)
            {
                case TamaCodeType.Profile:
                    return new TamaCodeProfile();
                case TamaCodeType.Playdate:
                    return new TamaCodePlaydate();
                case TamaCodeType.Camera:
                    return new TamaCodeCamera();
                case TamaCodeType.BackItem:
                    return new TamaCodeBackItem();
                case TamaCodeType.BackTamatomo:
                    return new TamaCodeBackTamatomo();
                case TamaCodeType.Meetup:
                    return new TamaCodeMeetup();
                case TamaCodeType.Gift:
                    return new TamaCodeGift();
                case TamaCodeType.Download:
                    return new TamaCodeDownload();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Unknown Tama Code type.");
            }
        }
    }
}
