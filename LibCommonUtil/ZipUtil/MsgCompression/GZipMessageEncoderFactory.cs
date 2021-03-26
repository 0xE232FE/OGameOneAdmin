using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.ServiceModel.Channels;


namespace LibCommonUtil
{
    //This class is used to create the custom encoder (GZipMessageEncoder)
    public class GZipMessageEncoderFactory : MessageEncoderFactory
    {
        private readonly MessageEncoder encoder;

        //public delegate void DelegateSendApplyChanges(int iByteSent);
        //public static event DelegateSendApplyChanges onSendApplyChanges;

        //public static void SendApplyChanges(int iByteSent)
        //{
        //    if (onSendApplyChanges != null)
        //    {
        //        onSendApplyChanges(iByteSent);
        //    }
        //}

        //The GZip encoder wraps an inner encoder
        //We require a factory to be passed in that will create this inner encoder

        public GZipMessageEncoderFactory(MessageEncoderFactory messageEncoderFactory)
        {
            if (messageEncoderFactory == null)
                throw new ArgumentNullException("messageEncoderFactory",
                                                "A valid message encoder factory must be passed to the GZipEncoder");
            encoder = new GZipMessageEncoder(messageEncoderFactory.Encoder);
        }

        //The service framework uses this property to obtain an encoder from this encoder factory
        public override MessageEncoder Encoder
        {
            get { return encoder; }
        }

        public override MessageVersion MessageVersion
        {
            get { return encoder.MessageVersion; }
        }

        //This is the actual GZip encoder

        #region Nested type: GZipMessageEncoder

        private class GZipMessageEncoder : MessageEncoder
        {
            private static string GZipContentType = "application/x-gzip";

            //This implementation wraps an inner encoder that actually converts a WCF Message
            //into textual XML, binary XML or some other format. This implementation then compresses the results.
            //The opposite happens when reading messages.
            //This member stores this inner encoder.
            private readonly MessageEncoder innerEncoder;

            //We require an inner encoder to be supplied (see comment above)
            internal GZipMessageEncoder(MessageEncoder messageEncoder)
            {
                if (messageEncoder == null)
                    throw new ArgumentNullException("messageEncoder",
                                                    "A valid message encoder must be passed to the GZipEncoder");
                innerEncoder = messageEncoder;
            }

            //public override string CharSet
            //{
            //    get { return ""; }
            //}

            public override string ContentType
            {
                get { return GZipContentType; }
            }

            public override string MediaType
            {
                get { return GZipContentType; }
            }

            //SOAP version to use - we delegate to the inner encoder for this
            public override MessageVersion MessageVersion
            {
                get { return innerEncoder.MessageVersion; }
            }

            //Helper method to compress an array of bytes
            private static ArraySegment<byte> CompressBuffer(ArraySegment<byte> buffer, BufferManager bufferManager,
                                                             int messageOffset)
            {
                //var memoryStream = new MemoryStream();
                //memoryStream.Write(buffer.Array, 0, messageOffset);

                //using (var gzStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                //{
                //    gzStream.Write(buffer.Array, messageOffset, buffer.Count);
                //}


                //byte[] compressedBytes = memoryStream.ToArray();
                //byte[] bufferedBytes = bufferManager.TakeBuffer(compressedBytes.Length);

                //Array.Copy(compressedBytes, 0, bufferedBytes, 0, compressedBytes.Length);

                //bufferManager.ReturnBuffer(buffer.Array);
                //var byteArray = new ArraySegment<byte>(bufferedBytes, messageOffset,
                //                                       bufferedBytes.Length - messageOffset);
                //return byteArray;

                using (var memoryStream = new MemoryStream())
                {
                    var zipperStream = new GZipStream(memoryStream, CompressionMode.Compress, true);

                    using (zipperStream)
                        zipperStream.Write(buffer.Array,
                           buffer.Offset, buffer.Count);

                    byte[] compressedBytes1 = memoryStream.ToArray();
                    byte[] compressedBytes = SevenZip.Compression.LZMA.SevenZipHelper.Compress(compressedBytes1);
                    byte[] bufferedBytes = bufferManager.TakeBuffer(
                       compressedBytes.Length + messageOffset);

                    Array.Copy(compressedBytes, 0, bufferedBytes, messageOffset,
                       compressedBytes.Length);

                    bufferManager.ReturnBuffer(buffer.Array);
                    var byteArray = new ArraySegment<byte>(bufferedBytes, messageOffset,
                       compressedBytes.Length);

                    return byteArray;
                }
            }

            //Helper method to decompress an array of bytes
            private static ArraySegment<byte> DecompressBuffer(ArraySegment<byte> buffer, BufferManager bufferManager)
            {
                var memoryStream1 = new MemoryStream(buffer.Array, buffer.Offset, buffer.Count - buffer.Offset);
                var memoryStream = new MemoryStream(SevenZip.Compression.LZMA.SevenZipHelper.Decompress(memoryStream1.ToArray()));

                var decompressedStream = new MemoryStream();
                int totalRead = 0;
                int blockSize = 1024;
                byte[] tempBuffer = bufferManager.TakeBuffer(blockSize);
                using (var gzStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    while (true)
                    {
                        int bytesRead = gzStream.Read(tempBuffer, 0, blockSize);
                        if (bytesRead == 0)
                            break;
                        decompressedStream.Write(tempBuffer, 0, bytesRead);
                        totalRead += bytesRead;
                    }
                }
                bufferManager.ReturnBuffer(tempBuffer);

                byte[] decompressedBytes = decompressedStream.ToArray();
                byte[] bufferManagerBuffer = bufferManager.TakeBuffer(decompressedBytes.Length + buffer.Offset);
                Array.Copy(buffer.Array, 0, bufferManagerBuffer, 0, buffer.Offset);
                Array.Copy(decompressedBytes, 0, bufferManagerBuffer, buffer.Offset, decompressedBytes.Length);

                var byteArray = new ArraySegment<byte>(bufferManagerBuffer, buffer.Offset, decompressedBytes.Length);
                bufferManager.ReturnBuffer(buffer.Array);

                return byteArray;
            }


            //One of the two main entry points into the encoder. Called by WCF to decode a buffered byte array into a Message.
            public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager,
                                                string contentType)
            {
                //Decompress the buffer
                ArraySegment<byte> decompressedBuffer = DecompressBuffer(buffer, bufferManager);
                //AddtoNetworkTraffic(false, buffer.Count);
                // byte[] tt = Decompress(buffer);
                // ArraySegment<byte> decompressedBuffer = new ArraySegment<byte>(tt, 0, tt.Length);
                //Use the inner encoder to decode the decompressed buffer
                Message returnMessage = innerEncoder.ReadMessage(decompressedBuffer, bufferManager);
                returnMessage.Properties.Encoder = this;
                return returnMessage;
            }

            private string GetMessageAction(string sMsg)
            {
                string sResult = "";
                int iIndex1 = sMsg.LastIndexOf("/");
                if (iIndex1 > 0)
                {
                    sResult = sMsg.Substring(iIndex1 + 1);
                }
                return sResult;
            }

            //One of the two main entry points into the encoder. Called by WCF to encode a Message into a buffered byte array.
            public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize,
                                                            BufferManager bufferManager, int messageOffset)
            {
                //Use the inner encoder to encode a Message into a buffered byte array
                ArraySegment<byte> buffer = innerEncoder.WriteMessage(message, maxMessageSize, bufferManager,
                                                                      messageOffset);
                //Compress the resulting byte array
                ArraySegment<byte> comedbuf = CompressBuffer(buffer, bufferManager, messageOffset);
                //string sMsg = GetMessageAction(message.Headers.Action);
                //if (sMsg == "ApplyChanges")
                //    SendApplyChanges(comedbuf.Count);
                //AddtoNetworkTraffic(true, comedbuf.Count);
                return comedbuf;
                // byte[] tt = Compress(buffer.Array);
                // return new ArraySegment<byte>(tt, 0, tt.Length);

            }

            public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
            {
                var gzStream = new GZipStream(stream, CompressionMode.Decompress, true);
                return innerEncoder.ReadMessage(gzStream, maxSizeOfHeaders);
            }

            public override void WriteMessage(Message message, Stream stream)
            {
                using (var gzStream = new GZipStream(stream, CompressionMode.Compress, true))
                {
                    innerEncoder.WriteMessage(message, gzStream);
                }

                // innerEncoder.WriteMessage(message, gzStream) depends on that it can flush data by flushing 
                // the stream passed in, but the implementation of GZipStream.Flush will not flush underlying
                // stream, so we need to flush here.
                stream.Flush();
            }

            private void AddtoNetworkTraffic(bool sent, int iSize)
            {
                //try
                //{
                //    String skey = "ByteReceived";
                //    if (sent)
                //        skey = "ByteSent";
                //    if (ConfigurationManager.AppSettings[skey] != null)
                //    {
                //        int lastbytes = Convert.ToInt32(ConfigurationManager.AppSettings[skey]);
                //        System.Configuration.Configuration config =
                //            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //        config.AppSettings.Settings[skey].Value = (iSize + lastbytes).ToString();
                //        config.Save(ConfigurationSaveMode.Modified);
                //        ConfigurationManager.RefreshSection("appSettings");
                //    }
                //}
                //catch
                //{

                //}


            }
        }

        #endregion
    }
}