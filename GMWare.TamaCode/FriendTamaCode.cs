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
    /// Represents a Tama Code containing friend information.
    /// </summary>
    public abstract class FriendTamaCode : BaseTamaCode
    {
        TamaStats stats;
        TamaProfile profile;

        /// <summary>
        /// Gets or sets the device ID of the originating Tamagotchi Pix.
        /// </summary>
        public uint DeviceId { get; set; }
        /// <summary>
        /// Gets or sets the user's current Tama stats.
        /// </summary>
        public TamaStats Stats
        {
            get
            {
                return stats;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                stats = value;
            }
        }
        /// <summary>
        /// Gets or sets the user's profile.
        /// </summary>
        public TamaProfile Profile
        {
            get
            {
                return profile;
            }    
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                profile = value;
            }
        }

        protected FriendTamaCode(uint deviceId, TamaStats stats, TamaProfile profile) : this()
        {
            DeviceId = deviceId;
            this.stats = stats ?? throw new ArgumentNullException(nameof(stats));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }

        // Empty constructor for deserialization
        protected FriendTamaCode()
        { }

        // Note: serialization/deserialization for stats and profile is done in TamaCodeEncoder.
    }
}
