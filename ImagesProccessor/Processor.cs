using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

using Ext.System.Core;
using Ext.System.Core.Progress;
using Ext.System.IO;

namespace ImagesProccessor {
    public class Processor {

        private static RNGCryptoServiceProvider rngCsp = null;

        private static int InsertData(int src, int data, int bitCount) {
            int Mask = 1;
            for(int i = 0; i < bitCount; i++, Mask <<= 1) {
                if((data & Mask) != 0) { // 01 11
                    src |= Mask;
                } else if((src & Mask) != 0) { // 10
                    src ^= Mask;
                }
                // 00 do nothing
            }
            return src;
        }

        private Bitmap _srcImage;
        private Bitmap _currentImage;
        private Graphics _graph;
        private DefaultProgress _progress;

        /// <summary>
        /// Count of bits in each channel
        /// </summary>
        public int DataLength { get; set; }
        public Image Result { get; protected set; }
        public int MaxData
        {
            get
            {
                if(_srcImage == null)
                    return 0;
                return (Image.GetPixelFormatSize(_srcImage.PixelFormat) / 8 * _srcImage.Width * _srcImage.Height / 8) * DataLength;
            }
        }
        public bool ImageLoaded
        {
            get
            {
                return _srcImage != null;
            }
        }

        public readonly IProgressExt Progress;

        public Processor() {
            _progress = new DefaultProgress();
            Progress = _progress;
        }

        public void LoadImage(Bitmap img) {
            if(Image.GetPixelFormatSize(img.PixelFormat) < 24)
                throw new Exception("RGB image required!");
            _srcImage = img;
            _currentImage?.Dispose();
            _currentImage = new Bitmap(img);
            _graph?.Dispose();
            _graph = Graphics.FromImage(_currentImage);
            Result = _currentImage;
        }

        public void ResetData() {
            _progress.OperationsTotal = _currentImage.Width;
            _progress.OperationsDone = 0;
            for(int i = 0; i < _currentImage.Width; i++, _progress.OperationsDone++) {
                for(int j = 0; j < _currentImage.Height; j++) {
                    var pixel = _srcImage.GetPixel(i, j);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;
                    R = InsertData(R, 0, DataLength);
                    G = InsertData(G, 0, DataLength);
                    B = InsertData(B, 0, DataLength);
                    _currentImage.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }
        }

        public void RandData() {
            _progress.OperationsTotal = _currentImage.Width;
            _progress.OperationsDone = 0;
            if(rngCsp == null)
                rngCsp = new RNGCryptoServiceProvider();
            byte[] Data = new byte[3 * _currentImage.Height];
            for(int i = 0; i < _currentImage.Width; i++, _progress.OperationsDone++) {
                rngCsp.GetNonZeroBytes(Data);
                for(int j = 0; j < _currentImage.Height; j++) {
                    var pixel = _srcImage.GetPixel(i, j);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;
                    R = InsertData(R, Data[j * 3], DataLength);
                    G = InsertData(G, Data[j * 3 + 1], DataLength);
                    B = InsertData(B, Data[j * 3 + 2], DataLength);
                    _currentImage.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }
        }

        public void SaveData(byte[] data) {
            _progress.OperationsDone = 0;
            _progress.OperationsTotal = data.Length;
            int width = 0;
            int height = 0;
            using(var mem = new MemoryStream(data)) {
                mem.Seek(0, SeekOrigin.Begin);
                var bs = new BitStream(mem);
                //
                //StringBuilder sb = new StringBuilder();
                //while(mem.Position != mem.Length) {
                //    for(int i = 0; i < 8; i++) {
                //        sb.Append(bs.NextBit() == 0 ? '0' : '1');
                //        if((i + 1) % 4 == 0)
                //            sb.Append(' ');
                //    }
                //}
                //Trace.WriteLine(sb.ToString());
                //
                //bs.Seek(0, SeekOrigin.Begin);
                int bits = 0;
                while(true) {
                    var pixel = _srcImage.GetPixel(width, height);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;
                    int buf = bs.ReadInt(bits++ < 8 ? 1 : DataLength);
                    //buf = BitStream.ReverseBits(buf, 8);
                    R = InsertData(R, buf, bits < 8 ? 1 : DataLength);
                    buf = bs.ReadInt(bits++ < 8 ? 1 : DataLength);
                    //buf = BitStream.ReverseBits(buf, 8);
                    G = InsertData(G, buf, bits < 8 ? 1 : DataLength);
                    buf = bs.ReadInt(bits++ < 8 ? 1 : DataLength);
                    //buf = BitStream.ReverseBits(buf, 8);
                    B = InsertData(B, buf, bits < 8 ? 1 : DataLength);
                    _currentImage.SetPixel(width, height, Color.FromArgb(R, G, B));
                    _progress.OperationsDone = (int)bs.Position;
                    width++;
                    if(width >= _srcImage.Width) {
                        width = 0;
                        height++;
                        if(height >= _srcImage.Height)
                            throw new Exception("Not enough pixels!");
                    }
                    if(bs.Position == bs.Length)
                        break;
                }
            }
        }

        public byte[] TryReadData(out int flags) {
            // read datalen
            int dataLen = 0;
            int width = 0;
            int height = 0;
            flags = 0;
            for(int i = 0, bits = 0; i < 3; i++) {
                var pixel = _srcImage.GetPixel(width, height);
                int R = pixel.R;
                int G = pixel.G;
                int B = pixel.B;
                dataLen |= ((R & 1) << bits++);
                dataLen |= ((G & 1) << bits++);
                if(bits >= 8)
                    break;
                dataLen |= ((B & 1) << bits++);
                width++;
                if(width >= _srcImage.Width) {
                    width = 0;
                    height++;
                    if(height >= _srcImage.Height)
                        throw new Exception("Not enough pixels!");
                }
            }
            if(dataLen > 7 || dataLen < 1)
                return null;
            int mask = 1;
            _progress.OperationsTotal = _srcImage.Height;
            _progress.OperationsDone = height;
            for(int i = 1; i < dataLen; mask |= 1 << i++) ;
            using(MemoryStream mem = new MemoryStream()) {
                using(BitStream bs = new BitStream(mem)) {
                    var pixel = _srcImage.GetPixel(width, height);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;
                    bs.WriteData(B & mask, dataLen);
                    width++;
                    if(width >= _srcImage.Width) {
                        width = 0;
                        height++;
                        if(height >= _srcImage.Height)
                            throw new Exception("Not enough pixels!");
                    }
                    while(true) {
                        pixel = _srcImage.GetPixel(width, height);
                        R = pixel.R;
                        G = pixel.G;
                        B = pixel.B;
                        bs.WriteData(R & mask, dataLen);
                        bs.WriteData(G & mask, dataLen);
                        bs.WriteData(B & mask, dataLen);
                        width++;
                        if(width >= _srcImage.Width) {
                            width = 0;
                            height++;
                            _progress.OperationsDone++;
                            if(height >= _srcImage.Height)
                                break;
                        }
                    }
                } // bit stream die mf die
                mem.Seek(0, SeekOrigin.Begin);
                int mark = 0;
                mem.ReadInt(out mark);
                if(mark != (int)DataAdapter.Mark)
                    return null;
                short flagsRead = 0;
                mem.ReadShort(out flagsRead);
                flags = flagsRead;
                int dataLength = 0;
                mem.ReadInt(out dataLength);
                if(dataLength > mem.Length - mem.Position)
                    throw new InvalidDataException();
                var res = new byte[dataLength];
                mem.Read(res);
                DataLength = dataLen;
                return res;
            } // memory stream
        }

        public bool CheckDataHeader(out int flags) {
            int dataLen = 0;
            int width = 0;
            int height = 0;
            flags = 0;
            for(int i = 0, bits = 0; i < 3; i++) {
                var pixel = _srcImage.GetPixel(width, height);
                int R = pixel.R;
                int G = pixel.G;
                int B = pixel.B;
                dataLen |= ((R & 1) << bits++);
                dataLen |= ((G & 1) << bits++);
                if(bits >= 8)
                    break;
                dataLen |= ((B & 1) << bits++);
                width++;
                if(width >= _srcImage.Width) {
                    width = 0;
                    height++;
                    if(height >= _srcImage.Height)
                        throw new Exception("Not enough pixels!");
                }
            }
            if(dataLen > 7 || dataLen < 1)
                return false;
            int mask = 1;
            _progress.OperationsTotal = _srcImage.Height;
            _progress.OperationsDone = height;
            for(int i = 1; i < dataLen; mask |= 1 << i++) ;
            using(MemoryStream mem = new MemoryStream()) {
                using(BitStream bs = new BitStream(mem)) {
                    var pixel = _srcImage.GetPixel(width, height);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;
                    bs.WriteData(B & mask, dataLen);
                    width++;
                    if(width >= _srcImage.Width) {
                        width = 0;
                        height++;
                        if(height >= _srcImage.Height)
                            throw new Exception("Not enough pixels!");
                    }
                    while(bs.Length < DataAdapter.HeaderLength + 1) {
                        pixel = _srcImage.GetPixel(width, height);
                        R = pixel.R;
                        G = pixel.G;
                        B = pixel.B;
                        bs.WriteData(R & mask, dataLen);
                        bs.WriteData(G & mask, dataLen);
                        bs.WriteData(B & mask, dataLen);
                        width++;
                        if(width >= _srcImage.Width) {
                            width = 0;
                            height++;
                            _progress.OperationsDone++;
                            if(height >= _srcImage.Height)
                                break;
                        }
                    }
                } // bit stream die mf die
                mem.Seek(0, SeekOrigin.Begin);
                int mark = 0;
                mem.ReadInt(out mark);
                if(mark != (int)DataAdapter.Mark)
                    return false;
                short flagsRead = 0;
                mem.ReadShort(out flagsRead);
                flags = flagsRead;
                return true;
            } // memory stream
        }

        public Task ResetDataAsync() {
            return Task.Factory.StartNew(ResetData);
        }

        public Task SaveDataAsync(byte[] data) {
            return Task.Factory.StartNew(() => SaveData(data));
        }

        public Task RandDataAsync() {
            return Task.Factory.StartNew(RandData);
        }

    }
}
