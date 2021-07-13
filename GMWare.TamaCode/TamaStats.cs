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
    /// Represents a user's Tama stats.
    /// </summary>
    public class TamaStats
    {
        int tama;
        int age;
        int accessory;
        int accessoryVariant;
        int raisedTama = 0xff;
        int raisedAge;

        /// <summary>
        /// Gets or sets the user's current Tama's ID.
        /// </summary>
        public int Tama
        {
            get
            {
                return tama;
            }
            set
            {
                if (value < 0 || value > 255) throw new ArgumentOutOfRangeException(nameof(value), "Tama must be between 0 and 255 inclusive.");
                tama = value;
            }
        }

        /// <summary>
        /// Gets or sets the user's current Tama's age.
        /// </summary>
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                if (value < 0 || value > 999) throw new ArgumentOutOfRangeException(nameof(value), "Age must be between 0 and 999 inclusive.");
                age = value;
            }
        }

        /// <summary>
        /// Gets or sets the accessory the user's current Tama is wearing.
        /// </summary>
        /// <remarks>Accessories only show when a Tama has been invited for a playdate or meetup.</remarks>
        public int Accessory
        {
            get
            {
                return accessory;
            }
            set
            {
                if (value < 0 || value > 63) throw new ArgumentOutOfRangeException(nameof(value), "Accessory must be between 0 and 63 inclusive.");
                accessory = value;
            }
        }

        /// <summary>
        /// Gets or sets the variant of the accessory the user's current Tama is wearing.
        /// </summary>
        public int AccessoryVariant
        {
            get
            {
                return accessoryVariant;
            }
            set
            {
                if (value < 0 || value > 63) throw new ArgumentOutOfRangeException(nameof(value), "Accessory variant must be between 0 and 63 inclusive.");
                accessoryVariant = value;
            }
        }

        /// <summary>
        /// Gets or sets the ID of a user's previously raised Tama.
        /// </summary>
        /// <remarks>The value is <c>255</c> if the user does not have another raised Tama.</remarks>
        public int RaisedTama
        {
            get
            {
                return raisedTama;
            }
            set
            {
                if (value < 0 || value > 255) throw new ArgumentOutOfRangeException(nameof(value), "Raised Tama must be between 0 and 255 inclusive.");
                raisedTama = value;
            }
        }

        /// <summary>
        /// Gets or sets the age of a user's previously raised Tama.
        /// </summary>
        public int RaisedAge
        {
            get
            {
                return raisedAge;
            }
            set
            {
                if (value < 0 || value > 999) throw new ArgumentOutOfRangeException(nameof(value), "Raised age must be between 0 and 999 inclusive.");
                raisedAge = value;
            }
        }
    }
}
