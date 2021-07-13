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
    class LsbBitReader
    {
        BinaryReader br;
        byte byteBuf;
        int bufBitsRemaining;

        public LsbBitReader(BinaryReader br)
        {
            if (br == null) throw new ArgumentNullException(nameof(br));
            this.br = br;
        }

        public uint Read(int numBits)
        {
            if (numBits < 0 || numBits > 32)
                throw new ArgumentOutOfRangeException(nameof(numBits), "Number of bits must be between 0 and 32 inclusive.");

            uint value = 0;
            int bitsRead = 0;
            while (bitsRead < numBits)
            {
                if (bufBitsRemaining == 0)
                {
                    byteBuf = br.ReadByte();
                    bufBitsRemaining = 8;
                }

                int bitsToRead = Math.Min(bufBitsRemaining, numBits - bitsRead);
                value |= (uint)byteBuf << bitsRead;
                bitsRead += bitsToRead;
                byteBuf >>= bitsToRead;
                bufBitsRemaining -= bitsToRead;
            }
            return value & ((1u << numBits) - 1);
        }

        public void Reset()
        {
            bufBitsRemaining = 0;
        }
    }
}
