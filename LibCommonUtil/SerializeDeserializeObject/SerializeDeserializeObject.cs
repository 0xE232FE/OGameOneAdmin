using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LibCommonUtil
{
    sealed public class SerializeDeserializeObject
    {
        /***********************************************************************************************************/


        #region ------ Public Static Methods ------


        /// <summary>
        /// Convert a UTF8 Byte Array to a complete String.
        /// </summary>
        /// <param name="characters">UTF8 Byte Array to be converted to String</param>
        /// <returns>String converted from UTF8 Byte Array</returns>
        public static string UTF8ByteArrayToString(byte[] byteArray)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(byteArray);
            return (constructedString);
        }


        /// <summary>
        /// Converts the String to UTF8 Byte Array
        /// </summary>
        /// <param name="string">Text to be converted to UTF8 Byte Array</param>
        /// <returns>UTF8 Byte Array converted from String</returns>
        public static byte[] StringToUTF8ByteArray(string text)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(text);
            return byteArray;
        }


        /// <summary>
        /// Serialize an Object into Xml String.
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="obj">Object to be serialized</param>
        /// <returns>Xml string serialized from Object</returns>
        public static string SerializeObject<T>(T obj)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlSerializer.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlTextWriter.Close();
                return UTF8ByteArrayToString(memoryStream.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Reconstruct an Object from an XML string
        /// </summary>
        /// /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="string">Xml String to be deserialized</param>
        /// <returns>Object deserialized from Xml string</returns>
        public static T DeserializeObject<T>(string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (T)xmlSerializer.Deserialize(memoryStream);
        }


        public static void SerializeObjectToFile<T>(string fileName, bool deleteFile, T obj)
        {
            FileStream fs = null;
            if (deleteFile && File.Exists(fileName))
                File.Delete(fileName);
            try
            {
                fs = new FileStream(fileName, FileMode.Create);
                XmlSerializer xmlS = new XmlSerializer(typeof(T));
                xmlS.Serialize(fs, obj);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        public static T DeserializeObjectFromFile<T>(string fileName)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer xmlS = new XmlSerializer(typeof(T));
                return (T)xmlS.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/
    }
}
