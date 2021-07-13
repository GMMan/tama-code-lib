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
    /// Represents a Tama Code meetup invitation.
    /// </summary>
    public class TamaCodeMeetup : FriendTamaCode, IHasItem
    {
        /// <summary>
        /// Gets or sets whether this invitation is for a particular user.
        /// </summary>
        public bool IsParticularUser { get; set; }
        /// <summary>
        /// Gets or sets the time of the meetup.
        /// </summary>
        public TamaMeetupTimes Time { get; set; }
        /// <inheritdoc/>
        public short Item { get; set; }


        internal TamaCodeMeetup()
        {
            codeType = TamaCodeType.Meetup;
        }

        public TamaCodeMeetup(uint deviceId, TamaStats stats, TamaProfile profile) : base(deviceId, stats, profile)
        {
            codeType = TamaCodeType.Meetup;
        }

        internal override void ReadTypeSpecificData(BinaryReader br)
        {
            IsParticularUser = br.ReadBoolean();
            Time = (TamaMeetupTimes)br.ReadByte();
            Item = ReadItem(br);
        }

        internal override void WriteTypeSpecificData(BinaryWriter bw)
        {
            bw.Write(IsParticularUser);
            bw.Write((byte)Time);
            WriteItem(bw, Item);
        }
    }
}
