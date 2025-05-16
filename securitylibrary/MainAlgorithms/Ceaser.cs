using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {

            string encryptedText = "";
            foreach (char p in plainText)
            {
                if (char.IsLetter(p))
                {
                    char offset = char.IsUpper(p) ? 'A' : 'a';
                    encryptedText += (char)((((p + key) - offset) % 26) + offset);
                }
                else
                {
                    encryptedText += p;
                }
            }
            return encryptedText;

           // throw new NotImplementedException();

        }

        public string Decrypt(string cipherText, int key)
        {
            //string decryptedText = "";
            //foreach (char c in cipherText)
            //{
            //    if (char.IsLetter(c))
            //    {
            //        char offset = char.IsUpper(c) ? 'A' : 'a';
            //        decryptedText += (char)((((c - key) - offset + 26) % 26) + offset);
            //    }
            //    else
            //    {
            //        decryptedText += c;
            //    }
            //}
            //return decryptedText;
            return Encrypt(cipherText, 26 - key);
            // throw new NotImplementedException();
        }

        public int Analyse(string plainText, string cipherText)
        {
            for (int i = 0; i < 26; i++)
            {
                string temp = Encrypt(plainText, i);
                string normalText = new string(temp.Where(c => char.IsLetter(c)).ToArray()).ToLower();
                string normalCipher = new string(cipherText.Where(c => char.IsLetter(c)).ToArray()).ToLower();
                if (normalText == normalCipher)
                {
                    return i;
                }
            }
            return -1;
            // throw new NotImplementedException();
        }
    }
}
