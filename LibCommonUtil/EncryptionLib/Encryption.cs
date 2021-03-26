using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LibCommonUtil
{
    internal class SecureLibAccess
    {
        #region ------ Private Variables ------

        private static string sPassPhrase = "$OGame - Rijndael Symmetric Cipher!";    // can be any string
        private static string sSaltValue = "$OGame - Best Online Browser Game!";      // can be any string
        private static string sHashAlgorithm = "SHA1";                                // can be "MD5"
        private static int iPasswordIterations = 5;                                   // can be any number
        private static string sInitVector = "$Xel3RedHarYBys!";                       // must be 16 bytes
        private static int iKeySize = 256;                                            // can be 192 or 128

        #endregion

        #region Private Methods

        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="sPlainText">The plain text to encode.</param>
        /// <returns>
        /// </returns>
        internal static string EncryptString(string sPlainText)
        {
            return RijndaelSymmetricCipher.Encrypt(sPlainText, sPassPhrase, sSaltValue, sHashAlgorithm, iPasswordIterations, sInitVector, iKeySize);
        }

        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="sPlainText">The plain text to encode.</param>
        /// <returns>
        /// </returns>
        internal static string EncryptString(string sPlainText, string sNewPassPhrase, string sNewSaltValue)
        {
            return RijndaelSymmetricCipher.Encrypt(sPlainText, sNewPassPhrase, sNewSaltValue, sHashAlgorithm, iPasswordIterations, sInitVector, iKeySize);
        }

        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="sPlainText">The plain text to encode.</param>
        /// <returns>
        /// </returns>
        internal static string EncryptString(string sPlainText, string sPassword)
        {
            return RijndaelSymmetricCipher.Encrypt(sPlainText, sPassPhrase + sPassword, sPassword + sSaltValue, sHashAlgorithm, iPasswordIterations, sInitVector, iKeySize);
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="sCipherText">The encoded cipher text to decrypt.</param>
        /// <returns>Decrypted string value.</returns>
        /// <remarks>
        /// </remarks>
        internal static string DecryptString(string sCipherText)
        {
            return RijndaelSymmetricCipher.Decrypt(sCipherText, sPassPhrase, sSaltValue, sHashAlgorithm, iPasswordIterations, sInitVector, iKeySize);
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="sCipherText">The encoded cipher text to decrypt.</param>
        /// <returns>Decrypted string value.</returns>
        /// <remarks>
        /// </remarks>
        internal static string DecryptString(string sCipherText, string sNewPassPhrase, string sNewSaltValue)
        {
            return RijndaelSymmetricCipher.Decrypt(sCipherText, sNewPassPhrase, sNewSaltValue, sHashAlgorithm, iPasswordIterations, sInitVector, iKeySize);
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="sCipherText">The encoded cipher text to decrypt.</param>
        /// <returns>Decrypted string value.</returns>
        /// <remarks>
        /// </remarks>
        internal static string DecryptString(string sCipherText, string sPassword)
        {
            return RijndaelSymmetricCipher.Decrypt(sCipherText, sPassPhrase + sPassword, sPassword + sSaltValue, sHashAlgorithm, iPasswordIterations, sInitVector, iKeySize);
        }


        #endregion
    }

    public class Encryption
    {
        #region Encryption Methods


        public static string EncryptString(string sCipherText)
        {
            return SecureLibAccess.EncryptString(sCipherText);
        }

        public static string EncryptString(string sCipherText, string sPassPhrase, string sSaltValue)
        {
            return SecureLibAccess.EncryptString(sCipherText, sPassPhrase, sSaltValue);
        }

        public static string EncryptString(string sCipherText, string sPassword)
        {
            return SecureLibAccess.EncryptString(sCipherText, sPassword);
        }

        public static string DecryptString(string sCipherText)
        {
            return SecureLibAccess.DecryptString(sCipherText);
        }

        public static string DecryptString(string sCipherText, string sPassPhrase, string sSaltValue)
        {
            return SecureLibAccess.DecryptString(sCipherText, sPassPhrase, sSaltValue);
        }

        public static string DecryptString(string sCipherText, string sPassword)
        {
            return SecureLibAccess.DecryptString(sCipherText, sPassword);
        }


        #endregion
    }
}
