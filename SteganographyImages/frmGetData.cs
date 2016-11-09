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
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Diagnostics;

using Ext.System.Core;

namespace SteganographyImages {
    public partial class frmGetData : Form {

        public static int flags { get; private set; }

        public static byte[] requestData() {
            using(var frm = new frmGetData()) {
                frm.ShowDialog();
                return frm._data;
            }
        }

        private byte[] _data = null;
        private Encoding _encoding = null;
        private Bitmap _image = null;
        private FileInfo _file = null;

        public frmGetData() {
            InitializeComponent();
            txtEncdoing.Text = "UTF-8";
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            try {
                _encoding = Encoding.GetEncoding(txtEncdoing.Text);
                lblLength.Text = _encoding.GetByteCount(txtText.Text).ToString();
            } catch(Exception) {
                _encoding = null;
            }
        }

        private void btnOkText_Click(object sender, EventArgs e) {
            try {
                flags = (int)ImagesProccessor.DataAdapter.Flags.Text;
                _data = _encoding?.GetBytes(txtText.Text) ?? null;
                if(chbEncryption.Checked) {
                    _data = EncryptData(_data, chbCompression.Checked);
                    flags |= (int)ImagesProccessor.DataAdapter.Flags.AES;
                }
                this.Close();
            } catch(Exception ex) {
                MessageBox.Show(string.Format("{0}: {1}", ex.GetType().ToString(), ex.Message));
            }
        }

        private void btnCancelText_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            if(_image == null)
                return;
            using(MemoryStream mem = new MemoryStream()) {
                _image.Save(mem, ImageFormat.Jpeg);
                flags = (int)ImagesProccessor.DataAdapter.Flags.Image;
                var buf = mem.ToArray();
                if(chbEncryption.Checked) {
                    buf = EncryptData(buf, chbCompression.Checked);
                    flags |= (int)ImagesProccessor.DataAdapter.Flags.AES;
                }
                _data = buf;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void UpdateImageInfo(FileInfo info) {
            txtInfo.Text = string.Format("{0}; {1}; {2}x{3}; {4}", info.Name, info.Length.ToByteMetricString(false),
                _image.Width, _image.Height, Image.GetPixelFormatSize(_image.PixelFormat));
        }

        private void btnBrowse_Click(object sender, EventArgs e) {
            try {
                using(var ofd = new OpenFileDialog()) {
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.Multiselect = false;
                    ofd.Filter = "Images|*.jpg;*.bmp|All files|*";
                    if(ofd.ShowDialog() == DialogResult.Cancel)
                        return;
                    _image = new Bitmap(ofd.FileName);
                    UpdateImageInfo(new FileInfo(ofd.FileName));
                    pictureBox1.Image = _image;
                }
            } catch(Exception) {
                _image = null;
            }
        }

        private void txtText_TextChanged(object sender, EventArgs e) {
            try {
                lblLength.Text = _encoding?.GetByteCount(txtText.Text).ToByteMetricString(false) ?? "0 B";
            } catch(Exception) {

            }
        }

        private void button1_Click_1(object sender, EventArgs e) {
            try {
                using(var ofd = new OpenFileDialog()) {
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.Multiselect = false;
                    ofd.Filter = "All files|*";
                    if(ofd.ShowDialog() == DialogResult.Cancel)
                        return;
                    _file = new FileInfo(ofd.FileName);
                    string hash = "N/A";
                    try {
                        using(var fs = _file.OpenRead()) {
                            using(var md5 = MD5.Create()) {
                                hash = string.Join("-", md5.ComputeHash(fs).Select(x => x.ToString("X02")));
                            }
                        }
                    } finally {

                    }
                    txtFileInfo.Text = string.Format("{0}; {1}; MD5: {2}", _file.Name, _file.Length.ToByteMetricString(), hash);
                }
            } catch(Exception) {
                _image = null;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e) {
            using(var fs = _file.OpenRead()) {
                flags = (int)ImagesProccessor.DataAdapter.Flags.Binary;
                var buf = new byte[fs.Length];
                fs.Read(buf, 0, buf.Length);
                if(chbEncryption.Checked) {
                    buf = EncryptData(buf, chbCompression.Checked);
                    flags |= (int)ImagesProccessor.DataAdapter.Flags.AES;
                }
                _data = buf;
            }
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            panel1.Enabled = chbEncryption.Checked;
        }

        private void EncryptOnce(byte[] data, string key, byte[] result, bool firstKey) {
            using(Rijndael aes = Rijndael.Create()) {
                aes.BlockSize = 256;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                if(!firstKey)
                    aes.Padding = PaddingMode.None;
                else
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
                using(var encryptor = aes.CreateEncryptor()) {
                    var offset = 0;
                    for(; offset < data.Length - 32; offset += 32) {
                        encryptor.TransformBlock(data, offset, 32, result, offset);
                    }
                    var buf = encryptor.TransformFinalBlock(data, offset, data.Length - offset);
                    Array.Copy(buf, 0, result, offset, buf.Length);
                }
            }
        }

        private byte[] Compress(byte[] data) {
            using(MemoryStream mem = new MemoryStream()) {
                using(GZipStream gzip = new GZipStream(mem, CompressionLevel.Optimal)) {
                    gzip.Write(data, 0, data.Length);
                }
                return mem.ToArray();
            }
        }

        private byte[] EncryptData(byte[] data, bool compression) {
            if(compression)
                data = Compress(data);
            int len = data.Length;
            len += 32;
            len += 32 - len % 32;
            var keys = txtKeys.Text.Split('\r', '\n').Where(x => x.Length > 0).ToArray();
            if(keys.Length == 0) {
                chbEncryption.Checked = false;
                return null;
            }
            byte[] buf = new byte[len];
            byte[] res = new byte[len];
            byte[] newData = new byte[data.Length + 32];
            using(RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider()) {
                rand.GetNonZeroBytes(newData);
                Array.Copy(data, 0, newData, 32, data.Length);
            }
            EncryptOnce(newData, keys[0], res, true);
            //
            //Trace.WriteLine(string.Format("Encrypting ({0}): {1}", keys[0], string.Join("-", res.Select(x => x.ToString("X02")))));
            //
            for(int i = 1; i < keys.Length; i++) {
                EncryptOnce(res, keys[i], buf, false);
                //
                //Trace.WriteLine(string.Format("Encrypting ({0}): {1}", keys[i], string.Join("-", buf.Select(x => x.ToString("X02")))));
                //
                var swap = res;
                res = buf;
                buf = swap;
            }
            //Trace.WriteLine("After encryption: " + string.Join("-", res.Select(x => x.ToString("X02"))));
            return res;
        }

    }
}
