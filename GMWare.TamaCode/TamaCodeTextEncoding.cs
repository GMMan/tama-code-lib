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
using System.Text;

namespace GMWare.TamaCode
{
    /// <summary>
    /// Manages converting strings to and from Tama Code encoding.
    /// </summary>
    class TamaCodeTextEncoding
    {
        const string CHAR_REP = " 0123456789+-ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÃÄÆÇÉÈÊËÎÏÓÔÕÖŒÙÚÛÜŸÍÑ—~⋯,:()“”.'!?&⭕❌♡☼★🌀🎵💢⤴⤵→←₲%😄😆😣😑😵😪👾📟Ò🎁¡¿°Ì🍰✨";

        static TamaCodeTextEncoding instance;

        /// <summary>
        /// Gets an instance of <see cref="TamaCodeTextEncoding"/>.
        /// </summary>
        public static TamaCodeTextEncoding Instance
        {
            get
            {
                if (instance == null) instance = new TamaCodeTextEncoding();
                return instance;
            }
        }

        class EncodingChar
        {
            public short Index { get; set; }
            public short BitLength { get; set; }
            public int EncodedValue { get; set; }
            public char Representation { get; set; }
        }

        List<EncodingChar> encodingTable;

        private TamaCodeTextEncoding()
        {
            GenerateEncodingTable();
        }

        EncodingChar FindCharByRepresentation(char ch)
        {
            return encodingTable.Find(x => x.Representation == ch);
        }

        EncodingChar FindCharByEncodedValue(int bitLength, int encodedValue)
        {
            return encodingTable.Find(x => x.BitLength == bitLength && x.EncodedValue == encodedValue);
        }

        /// <summary>
        /// Reads Tama Code encoded text to string.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from.</param>
        /// <param name="length">The length of the string.</param>
        /// <returns>The decoded string.</returns>
        public string Decode(BinaryReader br, int length)
        {
            if (br == null) throw new ArgumentNullException(nameof(br));
            StringBuilder sb = new StringBuilder();
            MsbBitReader bitReader = new MsbBitReader(br);

            for (int i = 0; i < length; ++i)
            {
                // Original code did lookup on every bit read, but we know only
                // 6 and 7 bit encoded value are in use
                var chIndex = bitReader.Read(6);
                var encChar = FindCharByEncodedValue(6, (int)chIndex);
                if (encChar == null)
                {
                    chIndex = (chIndex << 1) | bitReader.Read(1);
                    encChar = FindCharByEncodedValue(7, (int)chIndex);
                    if (encChar == null)
                        throw new InvalidDataException("Could not decode character.");
                }

                sb.Append(encChar.Representation);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Writes string as encoded Tama Code text.
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to.</param>
        /// <param name="str">The string to write.</param>
        public void Encode(BinaryWriter bw, string str)
        {
            if (bw == null) throw new ArgumentNullException(nameof(bw));
            if (str == null) str = string.Empty;
            MsbBitWriter bitWriter = new MsbBitWriter(bw);

            foreach (var ch in str)
            {
                var encChar = FindCharByRepresentation(ch);
                if (encChar == null) throw new ArgumentException($"Character '{ch}' is unrepresentable.", nameof(str));
                bitWriter.Write((uint)encChar.EncodedValue, encChar.BitLength);
            }

            bitWriter.Flush();
        }

        void GenerateEncodingTable()
        {
            encodingTable = new List<EncodingChar>();
            for (int i = 1; i <= 0x58; ++i)
            {
                encodingTable.Add(new EncodingChar
                {
                    Index = (short)i,
                    BitLength = 7,
                    EncodedValue = 0x27 + i,
                    Representation = CHAR_REP[i - 1]
                });
            }
            for (int i = 0x59; i <= 0x6c; ++i)
            {
                encodingTable.Add(new EncodingChar
                {
                    Index = (short)i,
                    BitLength = 6,
                    EncodedValue = i - 0x59,
                    Representation = CHAR_REP[i - 1]
                });
            }
        }
    }
}
