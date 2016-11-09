using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

using Ext.System.Core;
using Ext.System.Drawing;
using Ext.System.Core.Progress;

using ImagesProccessor;

namespace SteganographyImages {
    public partial class frmImageOptimizer : Form {

        public static Bitmap Optimize(Bitmap image) {
            using(frmImageOptimizer frm = new frmImageOptimizer()) {
                // throw GDI++ inner error
                //using(MemoryStream ms = new MemoryStream()) {
                //    image.Save(ms, ImageFormat.Bmp);
                //    frm.SetImage(new Bitmap(ms));
                //}

                //frm.SetImage(new Bitmap(image)); locked source image while new bitmap exists

                frm.SetImage(image);
                frm.ShowDialog();
                return frm._image;
            }
        }

        private Bitmap _srcImage;
        private Bitmap _image;
        private FixedSizeArea _area;
        private ImageOptimizer _optimizer;
        private Processor _imgProcessor;
        private DefaultProgress _progress = new DefaultProgress();
        private byte[] _imgBinaryData = null;

        private frmImageOptimizer() {
            InitializeComponent();
            _area = new FixedSizeArea(pictureBox1);
            _imgProcessor = new Processor();
        }

        public void SetImage(Bitmap image) {
            _srcImage = image;
            _image = new Bitmap(_srcImage);
            _area.SetImage(_image);
        }

        private void frmImageOptimizer_Load(object sender, EventArgs e) {
            //comboBox1.SelectedIndex = 0;
            //comboBox2.
        }

        private void DisableColors() {
            numericUpDown1.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void EnableColors() {
            numericUpDown1.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            _image = null;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            //switch(cmbType.SelectedIndex) {
            //    case 0:
            //        DisableColors();
            //        break;
            //    case 1:
            //        EnableColors();
            //        break;
            //    case 2:
            //        DisableColors();
            //        break;
            //}
        }

        private void GUIChange(bool ready) {
            //panel1.Enabled = ready;
            pictureBox1.Enabled = ready;
            if(ready)
                lblProgress.Text = "Ready";
        }

        private void GUIReady() {
            GUIChange(true);
        }

        private void GUIDisable() {
            GUIChange(false);
        }

        private async void UpdateImage() {
            var diff = _optimizer.ColorsCount - numericUpDown1.Value.ToInt();
            if(diff > 0) {
                _optimizer.Progress.ProgressChanged += Progress_ProgressChanged;
                GUIDisable();
                await _optimizer.DeleteColorsAsync(diff);
                _optimizer.Progress.ProgressChanged -= Progress_ProgressChanged;
                GUIReady();
                _area.Redraw();
            } else if(diff < 0) {
                _optimizer.Progress.ProgressChanged += Progress_ProgressChanged;
                GUIDisable();
                await _optimizer.RestoreColorsAsync(-diff);
                _optimizer.Progress.ProgressChanged -= Progress_ProgressChanged;
                GUIReady();
                _area.Redraw();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            //UpdateImage();
            ChangeImageColorType();// cmbType.SelectedItem.ToString());
            UpdateDataState();
        }

        private void UpdateDataState() {
            _imgProcessor.LoadImage(_image);
            int flags = 0;
            var res = _imgProcessor.CheckDataHeader(out flags);
            lblDataState.Text = string.Format("Data state: {0} (0x{1:X04})", res ? "Present" : "Not found", flags);
        }

        private long GetBitCount() {
            switch(cmbType.SelectedIndex) {
                case 0:
                    return 8L;
                //case 1:
                //    return 24L;
                case 2:
                    return 32L;
                default:
                    return 24L;
            }
        }

        private void OptimizePNGStep0(Bitmap image) {
            Color prev = Color.White;
            double difference = 1.0 - (double)numericUpDown1.Value / 100.0;
            for(int i = 1; i < image.Height - 1; i++) {
                for(int j = 1; j < image.Width - 1; j++) {
                    var color = image.GetPixel(j, i);
                    if(ColorAndImageFactory.GetColorRangeQ(prev, color) < difference) {
                        image.SetPixel(j, i, prev);
                    } else {
                        prev = color;
                    }
                }
                _progress.OperationsDone++;
            }
        }

        private void OptimizePNGStep1(Bitmap image) {
            for(int i = 2; i < image.Height - 2; i++) {
                for(int j = 2; j < image.Width - 2; j++) {
                    image.SetPixel(j, i, ColorAndImageFactory.GetMedianFilter(
                        image.GetPixel(j - 2, i - 2), image.GetPixel(j - 1, i - 2), image.GetPixel(j, i - 2), image.GetPixel(j + 1, i - 2), image.GetPixel(j + 2, i - 2),
                        image.GetPixel(j - 2, i - 1), image.GetPixel(j - 1, i - 1), image.GetPixel(j, i - 1), image.GetPixel(j + 1, i - 1), image.GetPixel(j + 2, i - 1),
                        image.GetPixel(j - 2, i), image.GetPixel(j - 1, i), image.GetPixel(j, i), image.GetPixel(j + 1, i), image.GetPixel(j + 2, i),
                        image.GetPixel(j - 2, i + 1), image.GetPixel(j - 1, i + 1), image.GetPixel(j, i + 1), image.GetPixel(j + 1, i + 1), image.GetPixel(j + 2, i + 1),
                        image.GetPixel(j - 2, i + 2), image.GetPixel(j - 1, i + 2), image.GetPixel(j, i + 2), image.GetPixel(j + 1, i + 2), image.GetPixel(j + 2, i + 2)
                        ));
                }
                _progress.OperationsDone++;
            }
        }

        private Task OptimizePNG(Bitmap image) {
            return Task.Factory.StartNew(() => {OptimizePNGStep0(image); OptimizePNGStep1(image); });
        }

        private void BlockGUI(bool block) {
            panel1.Enabled = !block;
        }

        private async void ChangeImageColorType() {
            //string mime = string.Format("image/png", fileName.Substring(fileName.LastIndexOf('.') + 1));
            ImageCodecInfo codec = ColorAndImageFactory.GetEncoderInfo(cmbMime.SelectedItem.ToString());
            EncoderParameters encoderParameters = new EncoderParameters(2);
            EncoderParameter parameterColorDepth = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, GetBitCount());
            EncoderParameter parameterQuality = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)numericUpDown1.Value.ToInt());
            encoderParameters.Param[0] = parameterColorDepth;
            encoderParameters.Param[1] = parameterQuality;
            using(MemoryStream ms = new MemoryStream()) {
                var img = _srcImage;
                if(cmbMime.SelectedIndex == 1) {
                    img = new Bitmap(_srcImage);
                    _progress.OperationsTotal = img.Height * 2;
                    _progress.OperationsDone = 0;
                    BlockGUI(true);
                    _progress.ProgressChanged += _progress_ProgressChanged;
                    await OptimizePNG(img);
                    _progress.ProgressChanged -= _progress_ProgressChanged;
                    BlockGUI(false);
                }
                img.Save(ms, codec, encoderParameters);
                _imgBinaryData = ms.ToArray();
                lblSize.Text = _imgBinaryData.Length.ToByteMetricString(false);
                _image = new Bitmap(ms);
                //OptimizePNG();
                _area.SetImage(_image);
                _area.Redraw();
            }
        }

        private void _progress_ProgressChanged(object sender, EventArgs args) {
            this.Invoke((Action)(() => lblProgress.Text = string.Format("{0}%", _progress.Percents)));
        }

        private void frmImageOptimizer_Shown(object sender, EventArgs e) {
            //_optimizer = new ImageOptimizer(_image, false);
            //_optimizer.Progress.ProgressChanged += Progress_ProgressChanged;
            //await _optimizer.UpdateColorsAsync();
            //_optimizer.Progress.ProgressChanged -= Progress_ProgressChanged;
            //_area.SetImage(_image);
            //numericUpDown1.Maximum = _optimizer.ColorsCount;
            //numericUpDown1.Value = numericUpDown1.Maximum;
            //numericUpDown1.Minimum = 1;
        }

        private void Progress_ProgressChanged(object sender, EventArgs args) {
            IProgressExt progress = (IProgressExt)sender;
            this.Invoke((Action)(() => { lblProgress.Text = string.Format("Loading ({0}%)", progress.Percents); }));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            if(chbLive.Checked) {
                ChangeImageColorType();
                UpdateDataState();
            }
            //UpdateImage();
        }

        private void button1_Click_1(object sender, EventArgs e) {
            using(SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.AddExtension = true;
                sfd.CheckPathExists = true;
                ImageCodecInfo codec = ColorAndImageFactory.GetEncoderInfo(cmbMime.SelectedItem.ToString());
                sfd.Filter = string.Format("Choosen format({0})|{0}", codec.FilenameExtension);
                if(sfd.ShowDialog() == DialogResult.Cancel)
                    return;
                EncoderParameters encoderParameters = new EncoderParameters(2);
                EncoderParameter parameterColorDepth = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, GetBitCount());
                EncoderParameter parameterQuality = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)numericUpDown1.Value.ToInt());
                encoderParameters.Param[0] = parameterColorDepth;
                encoderParameters.Param[1] = parameterQuality;
                using(FileStream fs = new FileStream(sfd.FileName, FileMode.Create)) {
                    fs.Write(_imgBinaryData, 0, _imgBinaryData.Length);
                }
            }
        }
    }
}
