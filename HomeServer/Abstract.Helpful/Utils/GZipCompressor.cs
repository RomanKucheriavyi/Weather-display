using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Text;
using Abstract.Helpful.Lib.Logging;

namespace Abstract.Helpful.Lib.Utils
{
    public static class GZipCompressor
    {
        [Pure]
        [Safe]
        public static string CompressOrDefault(string text, Encoding encoding)
        {
            if (text.IsNullOrEmpty())
                return default;

            try
            {
                var buffer = encoding.GetBytes(text);
                var ms = new MemoryStream();
                using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }

                ms.Position = 0;
                var compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);

                var array = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, array, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, array, 0, 4);
                return Convert.ToBase64String(array);
            }
            catch (Exception exception)
            {
                StaticLogger.Log(exception.ToPrettyDevelopersString());
                return default;
            }
        }

        [Pure]
        [Safe]
        public static string DecompressOrDefault(string compressedText, Encoding encoding)
        {
            if (compressedText.IsNullOrEmpty())
                return default;

            try
            {
                var base64String = Convert.FromBase64String(compressedText);
                using (var ms = new MemoryStream())
                {
                    var msgLength = BitConverter.ToInt32(base64String, 0);
                    ms.Write(base64String, 4, base64String.Length - 4);

                    var buffer = new byte[msgLength];

                    ms.Position = 0;
                    using (var zip = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        zip.Read(buffer, 0, buffer.Length);
                    }

                    return encoding.GetString(buffer);
                }
            }
            catch (Exception exception)
            {
                StaticLogger.Log(exception.ToPrettyDevelopersString());
                return default;
            }
        }
    }
}