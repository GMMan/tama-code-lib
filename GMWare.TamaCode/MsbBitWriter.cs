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
    // MSB aligned bit writer
    class MsbBitWriter
    {
        BinaryWriter bw;
        byte byteBuf;
        int bufBitsUsed;

        public MsbBitWriter(BinaryWriter bw)
        {
            if (bw == null) throw new ArgumentNullException(nameof(bw));
            this.bw = bw;
        }

        public void Write(uint value, int numBits)
        {
            if (numBits < 0 || numBits > 32)
                throw new ArgumentOutOfRangeException(nameof(numBits), "Number of bits must be between 0 and 32 inclusive.");

            if (numBits < 32)
            {
                // Align data to MSB
                value <<= 32 - numBits;
            }

            while (numBits > 0)
            {
                byteBuf <<= 1;
                byteBuf |= (byte)((value >> 31) & 1);
                ++bufBitsUsed;
                value <<= 1;
                --numBits;

                if (bufBitsUsed == 8)
                {
                    Flush();
                }
            }
        }

        public void Flush()
        {
            if (bufBitsUsed > 0)
            {
                // MSB align before writing
                byteBuf <<= 8 - bufBitsUsed;
                bw.Write(byteBuf);
                byteBuf = 0;
                bufBitsUsed = 0;
            }
        }
    }
}
