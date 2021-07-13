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
    /// Represents a user's profile.
    /// </summary>
    public class TamaProfile
    {
        int birthMonth = 1;
        int birthDay = 1;
        string name;
        string greeting;

        /// <summary>
        /// Gets or sets the birth month of the user.
        /// </summary>
        public int BirthMonth
        {
            get
            {
                return birthMonth;
            }
            set
            {
                if (value < 1 || value > 12) throw new ArgumentOutOfRangeException(nameof(value), "Birth month must be between 1 and 12 inclusive.");
                birthMonth = value;
            }
        }

        /// <summary>
        /// Gets or sets the birth day of the user.
        /// </summary>
        public int BirthDay
        {
            get
            {
                return birthDay;
            }
            set
            {
                if (value < 1 || value > 31) throw new ArgumentOutOfRangeException(nameof(value), "Birth day must be between 1 and 31 inclusive.");
                birthDay = value;
            }
        }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != null && value.Length > 8) throw new ArgumentException("Name cannot be longer than 8 characters.");
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the user's greeting.
        /// </summary>
        public string Greeting
        {
            get
            {
                return greeting;
            }
            set
            {
                if (value != null && value.Length > 24) throw new ArgumentException("Greeting cannot be longer than 24 characters.");
                greeting = value;
            }
        }

        // Used only for deserialization
        internal int NameLength { get; set; }
        internal int GreetingLength { get; set; }
    }
}
