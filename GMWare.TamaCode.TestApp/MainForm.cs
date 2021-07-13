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
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMWare.TamaCode.TestApp
{
    public partial class MainForm : Form
    {
        QRCodeGenerator qrGen = new QRCodeGenerator();
        Random random = new Random();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            raisedTamaCheckbox_CheckedChanged(this, EventArgs.Empty);
            meetupTimeComboBox.SelectedIndex = 0;

            //MessageBox.Show(this, "This program has minimal range checking for values and may " +
            //    "generate invalid data that could crash your Tamagotchi Pix. The author of this " +
            //    "program takes no responsibility for data loss or damage resulting from the use " +
            //    "of this program.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void goToSetupButton_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = setupTabPage;
        }

        private void raisedTamaCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = raisedTamaCheckbox.Checked;
            raisedTamaNumericUpDown.Enabled = isChecked;
            raisedAgeLabel.Enabled = isChecked;
            raisedAgeNumericUpDown.Enabled = isChecked;
        }

        private void encodeButton_Click(object sender, EventArgs e)
        {
            try
            {
                BaseTamaCode tamaCode = null;

                if (tabControl.SelectedTab == setupTabPage)
                {
                    MessageBox.Show(this, "Please select the tab corresponding to the type of code you want to generate.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (tabControl.SelectedTab == downloadTabPage)
                {
                    tamaCode = new TamaCodeDownload { Item = (short)downloadItemNumericUpDown.Value };
                }
                else
                {
                    if (!uint.TryParse(deviceIdTextbox.Text, System.Globalization.NumberStyles.HexNumber, null, out uint deviceId))
                    {
                        MessageBox.Show(this, "Could not parse device ID. Please make sure it is a valid hex number.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var stats = new TamaStats
                    {
                        Tama = (int)tamaNumericUpDown.Value,
                        Age = (int)ageNumericUpDown.Value,
                        Accessory = (int)accessoryNumericUpDown.Value,
                        AccessoryVariant = (int)accessoryVariantNumericUpDown.Value,
                        RaisedTama = raisedTamaCheckbox.Checked ? (int)raisedTamaNumericUpDown.Value : 255,
                        RaisedAge = raisedTamaCheckbox.Checked ? (int)raisedAgeNumericUpDown.Value : 0
                    };

                    var profile = new TamaProfile
                    {
                        BirthMonth = (int)birthMonthNumericUpDown.Value,
                        BirthDay = (int)birthDayNumericUpDown.Value,
                        Name = nameTextBox.Text.ToUpper(),
                        Greeting = greetingTextBox.Text.ToUpper()
                    };


                    if (tabControl.SelectedTab == profileTabPage)
                    {
                        tamaCode = new TamaCodeProfile(deviceId, stats, profile);
                    }
                    else if (tabControl.SelectedTab == playdateTabPage)
                    {
                        tamaCode = new TamaCodePlaydate(deviceId, stats, profile);
                    }
                    else if (tabControl.SelectedTab == meetupTabPage)
                    {
                        tamaCode = new TamaCodeMeetup(deviceId, stats, profile)
                        {
                            IsParticularUser = isParticularUserCheckBox.Checked,
                            Time = (TamaMeetupTimes)meetupTimeComboBox.SelectedIndex,
                            Item = (short)meetupItemNumericUpDown.Value
                        };
                    }
                    else if (tabControl.SelectedTab == giftTabPage)
                    {
                        tamaCode = new TamaCodeGift(deviceId, stats, profile)
                        {
                            Item = (short)giftItemNumericUpDown.Value
                        };
                    }
                }

                if (tamaCode == null)
                {
                    MessageBox.Show(this, "Please select a valid tab.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    byte[] data = TamaCodeEncoder.Encode(tamaCode);
                    codeHexTextBox.Text = BitConverter.ToString(data).Replace("-", string.Empty);
                    UpdateQRCode(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not generate Tama Code: " + ex.Message,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void UpdateQRCode(byte[] data)
        {
            var qrData = qrGen.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            var qr = new QRCode(qrData);
            qrPicturebox.Image = qr.GetGraphic(10);
        }

        private void randomDeviceIdButton_Click(object sender, EventArgs e)
        {
            // Generate four random hex digits
            // Originally created by hashing something
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; ++i)
            {
                if (random.Next(2) == 1)
                {
                    sb.Append((char)('0' + random.Next(10)));
                }
                else
                {
                    sb.Append((char)('a' + random.Next(6)));
                }
            }

            // Reinterpret string as uint
            string randString = sb.ToString();
            byte[] strBytes = Encoding.ASCII.GetBytes(randString);
            uint strVal = BitConverter.ToUInt32(strBytes, 0);
            deviceIdTextbox.Text = $"{strVal:x8}";
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] encoded = HexStringToBytes(codeHexTextBox.Text);
                BaseTamaCode tamaCode = TamaCodeEncoder.Decode(encoded);
                UpdateQRCode(encoded);

                TabPage selectedTab = null;
                if (tamaCode is TamaCodeDownload tcd)
                {
                    selectedTab = downloadTabPage;
                    downloadItemNumericUpDown.Value = tcd.Item;
                }
                else if (tamaCode is FriendTamaCode ftc)
                {
                    var stats = ftc.Stats;
                    var profile = ftc.Profile;

                    tamaNumericUpDown.Value = stats.Tama;
                    ageNumericUpDown.Value = stats.Age;
                    deviceIdTextbox.Text = ftc.DeviceId.ToString("x8");

                    accessoryNumericUpDown.Value = stats.Accessory;
                    accessoryVariantNumericUpDown.Value = stats.AccessoryVariant;
                    if (stats.RaisedTama != 255)
                    {
                        raisedTamaCheckbox.Checked = true;
                        raisedTamaNumericUpDown.Value = stats.RaisedTama;
                        raisedAgeNumericUpDown.Value = stats.RaisedAge;
                    }
                    else
                    {
                        raisedTamaCheckbox.Checked = false;
                    }

                    birthMonthNumericUpDown.Value = profile.BirthMonth;
                    birthDayNumericUpDown.Value = profile.BirthDay;
                    nameTextBox.Text = profile.Name;
                    greetingTextBox.Text = profile.Greeting;

                    switch (tamaCode.CodeType)
                    {
                        case TamaCodeType.Profile:
                            selectedTab = profileTabPage;
                            break;
                        case TamaCodeType.Playdate:
                            selectedTab = playdateTabPage;
                            break;
                        case TamaCodeType.Meetup:
                            selectedTab = meetupTabPage;
                            var meetupCode = tamaCode as TamaCodeMeetup;
                            isParticularUserCheckBox.Checked = meetupCode.IsParticularUser;
                            meetupTimeComboBox.SelectedIndex = (int)meetupCode.Time;
                            meetupItemNumericUpDown.Value = meetupCode.Item;
                            break;
                        case TamaCodeType.Gift:
                            selectedTab = giftTabPage;
                            var giftCode = tamaCode as TamaCodeGift;
                            giftItemNumericUpDown.Value = giftCode.Item;
                            break;
                        case TamaCodeType.Camera:
                        case TamaCodeType.BackItem:
                        case TamaCodeType.BackTamatomo:
                            MessageBox.Show(this, $"{tamaCode.CodeType} code is not supported.",
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }

                if (selectedTab == null)
                {
                    MessageBox.Show(this, $"Unknown Tama Code type.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    tabControl.SelectedTab = selectedTab;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error decoding Tama Code: " + ex.Message,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static byte[] HexStringToBytes(string str)
        {
            if (str.Length % 2 != 0) throw new ArgumentException("String does not have even number of characters.", nameof(str));
            byte[] data = new byte[str.Length / 2];
            for (int i = 0; i < data.Length; ++i)
            {
                byte b = Convert.ToByte(str.Substring(i * 2, 2), 16);
                data[i] = b;
            }
            return data;
        }

        private void qrPicturebox_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var fs = saveFileDialog.OpenFile())
                    {
                        qrPicturebox.Image.Save(fs, ImageFormat.Png);
                    }
                    MessageBox.Show(this, "Tama Code saved.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error saving Tama Code: " + ex.Message,
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
