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
    /// Represents a Tama Code.
    /// </summary>
    public abstract class BaseTamaCode
    {
        protected TamaCodeType codeType;

        /// <summary>
        /// Gets the code type.
        /// </summary>
        public TamaCodeType CodeType => codeType;

        /// <summary>
        /// Deserializes type-specific data.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from.</param>
        internal virtual void ReadTypeSpecificData(BinaryReader br)
        { }

        /// <summary>
        /// Serializes type-specific data.
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to.</param>
        internal virtual void WriteTypeSpecificData(BinaryWriter bw)
        { }

        /// <summary>
        /// Deserializes an encoded item.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from.</param>
        /// <returns>The item ID.</returns>
        protected short ReadItem(BinaryReader br)
        {
            short item = br.ReadInt16();
            br.ReadByte(); // Unused?
            return item;
        }

        /// <summary>
        /// Serializes an encoded item.
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to.</param>
        /// <param name="item">The item ID.</param>
        protected void WriteItem(BinaryWriter bw, short item)
        {
            bw.Write(item);
            bw.Write((byte)0);
        }
    }
}
