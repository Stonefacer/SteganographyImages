using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

using Ext.System.Drawing;
using System.IO.Compression;

namespace SteganographyImages {
    public partial class DataViewer : Form {

        public static void ShowModal(byte[] data, int flags) {
            using(var frm = new DataViewer(data, flags)) {
                frm.ShowDialog();
            }
        }

        private byte[] _data = null;
        private int _flags = 0;
        private Bitmap _image = null;
        private FixedSizeArea _area = null;
        private string _hash = null;

        public DataViewer(byte[] data, int flags) {
            InitializeComponent();
            _area = new FixedSizeArea(pictureBox1.Width, pictureBox1.Height);
            _area.MergeControl(pictureBox1);
            _data = data;
            _flags = flags;
            lblFlags.Text = string.Format("0x{0:X04}", flags);
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void DataViewer_Load(object sender, EventArgs e) {
            TryUpdateText();
        }

        private void TryUpdateText() {
            try {
                var enc = Encoding.GetEncoding(txtEncoding.Text);
                var buf = _data;
                if(chbEncryption.Checked) {
                    buf = new byte[_data.Length];
                    Array.Copy(_data, buf, _data.Length);
                    buf = Decrypt(buf, chbCompression.Checked);
                }
                txtText.Text = enc.GetString(chbNoNulls.Checked ? buf.Select(x=>x==0?(byte)1: x).ToArray() : buf);
            } catch(Exception ex) {
                txtText.Text = string.Format("{0}: {1}\r\n{2}", ex.GetType().ToString(), ex.Message, ex.StackTrace);
            } finally {

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            TryUpdateText();
        }

        private void txtText_TextChanged(object sender, EventArgs e) {

        }

        private void LoadImage() {
            if(_image == null) {
                try {
                    var buf = _data;
                    if(chbEncryption.Checked) {
                        buf = new byte[_data.Length];
                        Array.Copy(_data, buf, _data.Length);
                        buf = Decrypt(buf, chbCompression.Checked);
                    }
                    using(var mem = new MemoryStream(buf)) {
                        _image = new Bitmap(mem);
                        _area.SetImage(_image);
                        _area.Redraw();
                    }
                } catch(Exception ex) {
                    MessageBox.Show(string.Format("{0}: {1}", ex.GetType().ToString(), ex.Message));
                }
            }
        }

        private void LoadBinaryData() {
            if(_hash == null || chbEncryption.Checked) {
                var buf = _data;
                if(chbEncryption.Checked) {
                    buf = new byte[_data.Length];
                    Array.Copy(_data, buf, _data.Length);
                    buf = Decrypt(buf, chbCompression.Checked);
                }
                using(var md5 = MD5.Create()) {
                    _hash = string.Join("-", md5.ComputeHash(buf).Select(x => x.ToString("X02")));
                }
                lblMD5.Text = _hash;
            }
        }

        private void UpdateArea() {
            switch(tabViewer.SelectedIndex) {
                case 0:
                    TryUpdateText();
                    break;
                case 1:
                    LoadImage();
                    break;
                case 2:
                    LoadBinaryData();
                    break;
            }
        }

        private void tabViewer_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateArea();
        }

        private void btnSaveFile_Click(object sender, EventArgs e) {
            using(SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.AddExtension = false;
                sfd.Filter = "All files|*";
                if(sfd.ShowDialog() != DialogResult.Cancel) {
                    using(var fs = new FileStream(sfd.FileName, FileMode.Create)) {
                        var buf = _data;
                        if(chbEncryption.Checked) {
                            buf = new byte[_data.Length];
                            Array.Copy(_data, buf, _data.Length);
                            buf = Decrypt(buf, chbCompression.Checked);
                        }
                        fs.Write(buf, 0, buf.Length);
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            panel1.Enabled = chbEncryption.Checked;
            _hash = null;
            _image = null;
        }

        private void DecryptOnce(byte[] data, string key, byte[] result) {
            using(Rijndael aes = Rijndael.Create()) {
                aes.BlockSize = 256;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;
                using(SHA512 sha = SHA512.Create()) {
                    var buf = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
                    var keyBinary = new byte[32];
                    var iv = new byte[32];
                    Array.Copy(buf, keyBinary, 32);
                    Array.Copy(buf, 32, iv, 0, 32);
                    aes.Key = keyBinary;
                    aes.IV = iv;
                }
                using(var decryptor = aes.CreateDecryptor()) {
                    var offset = 0;
                    for(; offset < data.Length - 32; offset += 32) {
                        decryptor.TransformBlock(data, offset, 32, result, offset);
                    }
                    var buf = decryptor.TransformFinalBlock(data, offset, data.Length - offset);
                    Array.Copy(buf, 0, result, offset, buf.Length);
                }
            }
        }

        private byte[] Decompress(byte[] data) {
            using(MemoryStream mem = new MemoryStream(data)) {
                using(GZipStream gzip = new GZipStream(mem, CompressionMode.Decompress)) {
                    using(MemoryStream res = new MemoryStream()) {
                        gzip.CopyTo(res);
                        return res.ToArray();
                    }
                }
            }
        }

        private byte[] DecryptFinal(byte[] data, string key) {
            using(Rijndael aes = Rijndael.Create()) {
                aes.BlockSize = 256;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.ISO10126;
                using(SHA512 sha = SHA512.Create()) {
                    var buf = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
                    var keyBinary = new byte[32];
                    var iv = new byte[32];
                    Array.Copy(buf, keyBinary, 32);
                    Array.Copy(buf, 32, iv, 0, 32);
                    aes.Key = keyBinary;
                    aes.IV = iv;
                }
                using(var decryptor = aes.CreateDecryptor()) {
                    //return decryptor.TransformFinalBlock(data, 0, data.Length);
                    var listData = new List<byte>(data.Length * 2);
                    var buf = new byte[32];
                    var offset = 0;
                    for(; offset < data.Length - 32; offset += 32) {
                        decryptor.TransformBlock(data, offset, 32, buf, 0);
                        listData.AddRange(buf);
                    }
                    buf = decryptor.TransformFinalBlock(data, offset, data.Length - offset);
                    listData.AddRange(buf);
                    return listData.Skip(32).ToArray();
                }
            }
        }

        private byte[] Decrypt(byte[] data, bool compression) {
            if(data.Length % 32 != 0)
                throw new InvalidDataException("incorrect data length");
            //
            //Trace.WriteLine("Before decryption: " + string.Join("-", data.Select(x => x.ToString("X02"))));
            //
            var keys = txtKeys.Text.Split('\r', '\n').Where(x => x.Length > 0).ToArray();
            if(keys.Length == 0) {
                chbEncryption.Checked = false;
                return null;
            }
            byte[] buf = new byte[data.Length];
            for(int i = keys.Length - 1; i > 0; i--) {
                DecryptOnce(data, keys[i], buf);
                //
                //Trace.WriteLine(string.Format("Decrypting ({0}): {1}", keys[i], string.Join("-", buf.Select(x => x.ToString("X02")))));
                //
                var swap = buf;
                buf = data;
                data = swap;
            }
            var res = DecryptFinal(data, keys[0]);
            var resData = new byte[res.Length - 32];
            Array.Copy(res, 32, resData, 0, resData.Length);
            if(compression)
                resData = Decompress(resData);
            return resData;
        }

        private void chbNoNulls_CheckedChanged(object sender, EventArgs e) {
            TryUpdateText();
        }
    }
}
