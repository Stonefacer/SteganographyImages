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
using System.Security.Cryptography;

using Ext.System.Drawing;
using Ext.System.Core;
using Ext.System.Core.Progress;

using ImagesProccessor;

namespace SteganographyImages {
    public partial class Form1 : Form {

        private ImagesProccessor.Processor _imgProcessor;
        private FixedSizeArea _areaSource = null;
        private FixedSizeArea _areaResult = null;
        private byte[] _data = null;
        private int _dataFlags = 0;
        private Bitmap _sourceImage;
        private Bitmap _imageBuffer;

        public Form1() {
            InitializeComponent();
            _imgProcessor = new ImagesProccessor.Processor();
            _imgProcessor.DataLength = numericUpDown1.Value.ToInt();

            _areaSource = new FixedSizeArea(picSorce.Width, picSorce.Height);
            _areaSource.MergeControl(picSorce);

            _areaResult = new FixedSizeArea(picResult.Width, picResult.Height);
            _areaResult.MergeControl(picResult);
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void panel1_Resize(object sender, EventArgs e) {
            picResult.Width = picSorce.Width = panel1.Width / 2;
        }

        private async void btnLoad_Click(object sender, EventArgs e) {
            try {
                string fileName = "";
                using(OpenFileDialog ofd = new OpenFileDialog()) {
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.Multiselect = false;
                    ofd.ReadOnlyChecked = true;
                    ofd.Filter = "Images|*.jpg;*.png;*.jpeg;*.JPG;*.JPEG;*.bmp;*.BMP;*.PNG;*.TIFF;*.tiff;*.tif;*.TIF|All files|*";
                    if(ofd.ShowDialog() == DialogResult.Cancel)
                        return;
                    fileName = ofd.FileName;
                }
                var hash = "N/A";
                using(var md5 = MD5.Create()) {
                    try {
                        using(FileStream fs = new FileStream(fileName, FileMode.Open)) {
                            hash = string.Join("-", md5.ComputeHash(fs).Select(x => x.ToString("X02")));
                        }
                    } finally {

                    }
                }
                var info = new FileInfo(fileName);
                using(FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    _sourceImage = new Bitmap(fs);
                _imgProcessor.LoadImage(_sourceImage);
                GUIWait();
                _imgProcessor.Progress.ProgressChanged += Progress_ProgressChanged;
                await ReadData();
                _imgProcessor.Progress.ProgressChanged -= Progress_ProgressChanged;
                txtInfo.Text = string.Format("{0}; {1} bit/pixel; {2} x {3}; {4}; MD5: {5}", info.Name,
                    Image.GetPixelFormatSize(_sourceImage.PixelFormat), _sourceImage.Width, _sourceImage.Height, info.Length.ToByteMetricString(), hash);
                if(_data != null) {
                    lblSize.Text = _data.Length.ToByteMetricString(false);
                    numericUpDown1.Value = _imgProcessor.DataLength;
                }
                _areaSource.SetImage(_sourceImage);
                _areaSource.Redraw();
                _imageBuffer = new Bitmap(_sourceImage);
                _areaResult.SetImage(_imageBuffer);
                _areaResult.Redraw();
                UpdateMaxData();
            } catch(Exception ex) {
                MessageBox.Show(string.Format("{0}: {1}", ex.GetType().ToString(), ex.Message));
            } finally {
                if(_imgProcessor?.ImageLoaded ?? false) {
                    GUIReady();
                }
                btnShowData.Enabled = _data != null;
            }
        }

        private Task ReadData() {
            return Task.Factory.StartNew(() => _data = _imgProcessor.TryReadData(out _dataFlags));
        }

        private void GUIReady() {
            GUIStateChange(true);
        }

        private void GUIWait() {
            GUIStateChange(false);
        }

        private void GUIStateChange(bool ready) {
            btnLoad.Enabled = ready;
            btnResetData.Enabled = ready;
            btnSave.Enabled = ready;
            numericUpDown1.Enabled = ready;
            btnInsertData.Enabled = ready;
            btnRandData.Enabled = ready;
            picResult.Enabled = ready;
            picSorce.Enabled = ready;
            btnOptimize.Enabled = ready;
            if(ready)
                lblProgress.Text = "Ready";
        }

        private void UpdateMaxData() {
            lblMaxData.Text = _imgProcessor.MaxData.ToByteMetricString(false);
        }

        private async void button1_Click(object sender, EventArgs e) {
            GUIWait();
            _imgProcessor.Progress.ProgressChanged += Progress_ProgressChanged;
            await _imgProcessor.ResetDataAsync();
            _imgProcessor.Progress.ProgressChanged -= Progress_ProgressChanged;
            _areaResult.SetImage(_imgProcessor.Result);
            _areaResult.Redraw();
            GUIReady();
        }

        private void Progress_ProgressChanged(object sender, EventArgs args) {
            this.Invoke((Action)(() => { lblProgress.Text = string.Format("{0} %", ((IProgressExt)sender).Percents); }));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            _imgProcessor.DataLength = numericUpDown1.Value.ToInt();
            UpdateMaxData();
        }

        private void button1_Click_1(object sender, EventArgs e) {
            using(SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.AddExtension = true;
                sfd.CheckPathExists = true;
                sfd.Filter = "PNG|*.png;|JPEG|*.jpg";
                if(sfd.ShowDialog() == DialogResult.Cancel)
                    return;
                using(FileStream fs = new FileStream(sfd.FileName, FileMode.Create)) {
                    _imgProcessor.Result.Save(fs, ImageFormat.Png);
                }
            }
        }

        private async void button1_Click_2(object sender, EventArgs e) {
            var data = frmGetData.requestData();
            if(data == null)
                return;
            data = DataAdapter.GetDataWithHeader(data, _imgProcessor.DataLength, (short)frmGetData.flags);//DataAdapter.GetDataFromText(txtText.Text, _imgProcessor.DataLength, 0);
            lblSize.Text = data.Length.ToByteMetricString(false);
            GUIWait();
            _imgProcessor.Progress.ProgressChanged += Progress_ProgressChanged;
            await _imgProcessor.SaveDataAsync(data);
            _imgProcessor.Progress.ProgressChanged -= Progress_ProgressChanged;
            GUIReady();
            _areaResult.SetImage(_imgProcessor.Result);
            _areaResult.Redraw();
        }

        private void btnShowData_Click(object sender, EventArgs e) {
            DataViewer.ShowModal(_data, _dataFlags);
        }

        private async void btnRandData_Click(object sender, EventArgs e) {
            GUIWait();
            _imgProcessor.Progress.ProgressChanged += Progress_ProgressChanged;
            await _imgProcessor.RandDataAsync();
            _imgProcessor.Progress.ProgressChanged -= Progress_ProgressChanged;
            _areaResult.SetImage(_imgProcessor.Result);
            _areaResult.Redraw();
            GUIReady();
        }

        private void btnOptimize_Click(object sender, EventArgs e) {
            var res = frmImageOptimizer.Optimize(_imgProcessor.Result == null?_imageBuffer: (Bitmap)_imgProcessor.Result);
            if(res != null) {
                _imgProcessor.LoadImage(res);
                _areaResult.SetImage(res);
            }
        }
    }
}
