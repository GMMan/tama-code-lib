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
using System.Text;

namespace GMWare.TamaCode
{
    /// <summary>
    /// Represents the type of Tama Code.
    /// </summary>
    public enum TamaCodeType : byte
    {
        /// <summary>
        /// Profile card.
        /// </summary>
        Profile,
        /// <summary>
        /// Playdate invitation.
        /// </summary>
        Playdate,
        /// <summary>
        /// Character for explore mode, unused.
        /// </summary>
        Camera,
        /// <summary>
        /// Unknown.
        /// </summary>
        BackItem,
        /// <summary>
        /// Unknown.
        /// </summary>
        BackTamatomo,
        /// <summary>
        /// Meetup invitation.
        /// </summary>
        Meetup,
        /// <summary>
        /// Item gift.
        /// </summary>
        Gift,
        /// <summary>
        /// Item download not associated with an originating user.
        /// </summary>
        Download,
        /// <summary>
        /// Party invitation
        /// </summary>
        Party,

        MaxType
    }
}
