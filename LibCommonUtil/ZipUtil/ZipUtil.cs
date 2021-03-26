using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text;
// Third Party Libraries
using ICSharpCode.SharpZipLib.Zip;

namespace LibCommonUtil
{
    sealed public class ZipUtil
    {
        /***********************************************************************************************************/

        #region ------- Public declaration -------
        public enum CompressionType
        {
            GZip,
            BZip2,
            Zip
        }
        public static CompressionType CompressionProvider = CompressionType.GZip;
        #endregion ------- Public declaration -------

        #region ------- Private methods -------
        private static Stream OutputStream(Stream inputStream)
        {
            switch (CompressionProvider)
            {
                case CompressionType.BZip2:
                    return new ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream(inputStream);
                case CompressionType.GZip:
                    return new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(inputStream);
                case CompressionType.Zip:
                    return new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(inputStream);
                default:
                    return new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(inputStream);

            }
        }
        private static Stream InputStream(Stream inputStream)
        {
            switch (CompressionProvider)
            {
                case CompressionType.BZip2:
                    return new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(inputStream);
                case CompressionType.GZip:
                    return new ICSharpCode.SharpZipLib.GZip.GZipInputStream(inputStream);
                case CompressionType.Zip:
                    return new ICSharpCode.SharpZipLib.Zip.ZipInputStream(inputStream);
                default:
                    return new ICSharpCode.SharpZipLib.GZip.GZipInputStream(inputStream);
            }
        }
        #endregion ------- Private methods -------

        #region ------- Helper methods -------
        public static byte[] CompressBytes(byte[] bytesToCompress)
        {
            MemoryStream ms = new MemoryStream();
            Stream s = OutputStream(ms);
            s.Write(bytesToCompress, 0, bytesToCompress.Length);
            s.Close();
            return ms.ToArray();

            //MemoryStream ms = new MemoryStream();
            //ZipOutputStream s = new ZipOutputStream(ms);
            //ZipEntry zentry = new ZipEntry("ZippedFile");
            //s.PutNextEntry(zentry);
            //s.Write(bytesToCompress, 0, bytesToCompress.Length);
            //s.Finish();
            //s.Close();
            //return ms.ToArray();
        }
        public static byte[] CompressToByte(string stringToCompress)
        {
            byte[] bytData = Encoding.Default.GetBytes(stringToCompress);
            return CompressBytes(bytData);
        }
        public static byte[] DeCompressBytes(byte[] bytesToDecompress)
        {
            byte[] writeData = new byte[4096];
            Stream s2 = InputStream(new MemoryStream(bytesToDecompress));
            MemoryStream outStream = new MemoryStream();
            while (true)
            {
                int size = s2.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    outStream.Write(writeData, 0, size);
                }
                else
                {
                    break;
                }
            }
            s2.Close();
            byte[] outArr = outStream.ToArray();
            outStream.Close();
            return outArr;
        }
        /// <summary>
        /// Copy the contents of one <see cref="Stream"/> to another.
        /// </summary>
        /// <param name="source">The stream to source data from.</param>
        /// <param name="destination">The stream to write data to.</param>
        /// <param name="buffer">The buffer to use during copying.</param>
        public static void Stream_Copy(Stream source, Stream destination, byte[] buffer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            // Ensure a reasonable size of buffer is used without being prohibitive.
            if (buffer.Length < 128)
            {
                throw new ArgumentException("Buffer is too small", "buffer");
            }

            bool copying = true;

            while (copying)
            {
                int bytesRead = source.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    destination.Write(buffer, 0, bytesRead);
                }
                else
                {
                    destination.Flush();
                    copying = false;
                }
            }
        }
        #endregion ------- Helper methods -------

        #region ------ Public Static Methods ------
        public static string StringCompress(int intOption, string stringToCompress)
        {
            string strOut = string.Empty;
            byte[] compressedData = CompressToByte(stringToCompress);
            switch (intOption)
            {
                case 0:
                    strOut = System.Text.Encoding.Default.GetString(compressedData);
                    break;
                case 1:
                    strOut = Convert.ToBase64String(compressedData);
                    break;
                default:
                    strOut = Convert.ToBase64String(compressedData);
                    break;
            }
            return strOut;
        }

        public static string StringDecompress(int intOption, string stringToDecompress)
        {
            string outString = string.Empty;
            if (!string.IsNullOrEmpty(stringToDecompress))
            {
                try
                {
                    byte[] inArr = null;
                    switch (intOption)
                    {
                        case 0:
                            inArr = System.Text.Encoding.Default.GetBytes(stringToDecompress.Trim());
                            break;
                        case 1:
                            inArr = Convert.FromBase64String(stringToDecompress.Trim());
                            break;
                        default:
                            inArr = Convert.FromBase64String(stringToDecompress.Trim());
                            break;
                    }
                    //byte[] inArr = Convert.FromBase64String(stringToDecompress.Trim());
                    //byte[] inArr = System.Text.Encoding.Default.GetBytes(stringToDecompress.Trim());
                    byte[] outArr = DeCompressBytes(inArr);
                    outString = System.Text.Encoding.Default.GetString(outArr, 0, outArr.Length);
                }
                catch (NullReferenceException nEx)
                {
                    return nEx.Message;
                }
            }
            return outString;
        }

        public static byte[] CompressBytes_7Zip(byte[] bytesToCompress)
        {
            MemoryStream memoryStream = new MemoryStream();
            GZipStream zipperStream = new GZipStream(memoryStream, CompressionMode.Compress, true);

            using (zipperStream)
            {
                zipperStream.Write(bytesToCompress, 0, bytesToCompress.Length);

                byte[] compressedBytes1 = memoryStream.ToArray();
                byte[] compressedBytes = SevenZip.Compression.LZMA.SevenZipHelper.Compress(compressedBytes1);

                zipperStream.Close();
                memoryStream.Close();

                return compressedBytes;
            }
        }

        public static byte[] DecompressBytes_7Zip(byte[] bytesToDecompress)
        {
            MemoryStream memoryStream = new MemoryStream(SevenZip.Compression.LZMA.SevenZipHelper.Decompress(bytesToDecompress));

            MemoryStream decompressedStream = new MemoryStream();
            int totalRead = 0;
            int blockSize = 4096;
            byte[] tempBuffer = new byte[blockSize];
            using (GZipStream gzStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                while (true)
                {
                    int bytesRead = gzStream.Read(tempBuffer, 0, blockSize);
                    if (bytesRead == 0)
                        break;
                    decompressedStream.Write(tempBuffer, 0, bytesRead);
                    totalRead += bytesRead;
                }

                memoryStream.Close();
                byte[] decompressedBytes = decompressedStream.ToArray();
                gzStream.Close();
                decompressedStream.Close();

                return decompressedBytes;
            }
        }

        public static MemoryStream CompressStream(MemoryStream source)
        {
            if (source == null) return null;

            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = null;

            try
            {
                byte[] buffer = new byte[source.Length];
                if (source.CanSeek)
                    source.Position = 0;
                source.Read(buffer, 0, buffer.Length);
                compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
                compressedzipStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                compressedzipStream.Close();
            }
            return ms;
        }

        public static MemoryStream DeCompressStream(Stream source)
        {
            if (source == null) return null;

            if (source.CanSeek)
                source.Position = 0;

            GZipStream gs = new GZipStream(source, CompressionMode.Decompress);
            BinaryReader reader = new BinaryReader(gs);
            MemoryStream result = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(result);
            int bytesRead;
            byte[] buffer = new byte[2];

            try
            {
                do
                {
                    bytesRead = reader.Read(buffer, 0, buffer.Length);
                    writer.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);

                writer.Flush();
                result.Position = 0;
            }
            finally
            {
                //writer.Close();
                //reader.Close();
                //gs.Close();
            }

            return result;
        }

        public static void ZipFile(string sourcePath,
                                   string sourceName,
                                   string destinationPath,
                                   string destinationName,
                                   string comment,
                                   string password)
        {
            // Create Archive folder if does not exist
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Create zip file
            ZipFile zip = ICSharpCode.SharpZipLib.Zip.ZipFile.Create(destinationPath + destinationName);

            if ((password != null) || (password != String.Empty))
            {
                // Set up embedded security password
                zip.Password = password;
            }

            // Initialize the file so that it can accept updates
            zip.BeginUpdate();

            // Add the xml file to the zip file
            zip.Add(sourcePath + sourceName);

            zip.SetComment(comment);

            // Commit the update
            zip.CommitUpdate();

            // Close the zip file
            zip.Close();
        }


        public static void UnZipFile(string zipFileName,
                                     string targetDirectory,
                                     string password,
                                     bool deleteZipFile)
        {
            FastZip fastZip = new FastZip();

            if ((password != null) || (password != String.Empty))
            {
                fastZip.Password = password;
            }

            fastZip.ExtractZip(zipFileName, targetDirectory, "");

            if (deleteZipFile)
            {
                File.Delete(zipFileName);
            }
        }


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/
    }
}
