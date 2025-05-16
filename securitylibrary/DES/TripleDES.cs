using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        public string Decrypt(string cipherText, List<string> key)
        {
            //  throw new NotImplementedException();
            DES dES = new DES();
            string plaintext = dES.Decrypt(cipherText, key[0]);
            plaintext = dES.Encrypt(plaintext, key[1]);
            plaintext = dES.Decrypt(plaintext, key[0]);

            return plaintext;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            //throw new NotImplementedException();
            DES dES = new DES();
            string ciphertext = dES.Encrypt(plainText, key[0]);
            ciphertext = dES.Decrypt(ciphertext, key[1]);
            ciphertext = dES.Encrypt(ciphertext, key[0]);

            return ciphertext;
        }

        public List<string> Analyse(string plainText,string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}

