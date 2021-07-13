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
    public class TamaCodeBackTamatomo : FriendTamaCode
    {
        // Don't know what they are, since this code appears to be unused
        public byte A { get; set; }
        public byte B { get; set; }
        public byte C { get; set; }

        internal TamaCodeBackTamatomo()
        {
            codeType = TamaCodeType.BackTamatomo;
        }

        public TamaCodeBackTamatomo(uint deviceId, TamaStats stats, TamaProfile profile) : base(deviceId, stats, profile)
        {
            codeType = TamaCodeType.BackTamatomo;
        }

        internal override void ReadTypeSpecificData(BinaryReader br)
        {
            A = br.ReadByte();
            B = br.ReadByte();
            C = br.ReadByte();
        }

        internal override void WriteTypeSpecificData(BinaryWriter bw)
        {
            bw.Write(A);
            bw.Write(B);
            bw.Write(C);
        }
    }
}
