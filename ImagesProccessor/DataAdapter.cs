using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImagesProccessor {
    public static class DataAdapter {
        public static readonly uint Mark = 0xDEADBEEF;
        public static readonly int HeaderLength = 7;
        public enum Flags {
            Text    = 0x0001,
            Binary  = 0x0002,
            Image   = 0x0004,
            GZIP    = 0x0100, // do I need it ?
            AES     = 0x0200
        };

        public static void WriteHeader(Stream str, int dataLength, int flags) {
            str.WriteByte((byte)dataLength);
            var buf = BitConverter.GetBytes(Mark);
            str.Write(buf, 0, 4);
            buf = BitConverter.GetBytes(flags);
            str.Write(buf, 0, 2);
        }

        public static void WriteText(Stream str, string Text) {
            var buf = Encoding.UTF8.GetBytes(Text);
            var buf2 = BitConverter.GetBytes(buf.Length);
            str.Write(buf2, 0, buf2.Length);
            str.Write(buf, 0, buf.Length);
            str.WriteByte(0);
        }

        public static byte[] GetDataFromText(string Text, int dataLenegth, short flags) {
            using(MemoryStream mem = new MemoryStream()) {
                WriteHeader(mem, dataLenegth, flags);
                WriteText(mem, Text);
                return mem.ToArray();
            }
        }

        public static byte[] GetDataWithHeader(byte[] data, int dataLenegth, short flags) {
            using(MemoryStream mem = new MemoryStream()) {
                WriteHeader(mem, dataLenegth, flags);
                var len = BitConverter.GetBytes(data.Length);
                mem.Write(len, 0, len.Length);
                mem.Write(data, 0, data.Length);
                mem.WriteByte(0);
                return mem.ToArray();
            }
        }

    }
}
