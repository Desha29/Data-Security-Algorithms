using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        private const int AlphabetSize = 26;
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            StringBuilder key = new StringBuilder();

            for (int i = 0; i < plainText.Length; i++)
            {
                if (!char.IsLetter(plainText[i])) continue;

                int keyLetter = (cipherText[i] - plainText[i] + AlphabetSize) % AlphabetSize;
                key.Append((char)('A' + keyLetter));
            }

            return ExtractRepeatingKey(key.ToString());
        }


        private string ExtractRepeatingKey(string fullKey)
        {
            for (int length = 1; length <= fullKey.Length / 2; length++)
            {
                string subKey = fullKey.Substring(0, length);
                if (IsRepeatingPattern(fullKey, subKey))
                    return subKey;
            }
            return fullKey;
        }


        private bool IsRepeatingPattern(string fullKey, string subKey)
        {
            int subKeyLength = subKey.Length;
            for (int i = 0; i < fullKey.Length; i++)
            {
                if (fullKey[i] != subKey[i % subKeyLength])
                    return false;
            }
            return true;

        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            StringBuilder plainText = new StringBuilder();

            for (int i = 0, j = 0; i < cipherText.Length; i++)
            {
                char c = cipherText[i];
                if (!char.IsLetter(c)) continue;

                char k = key[j % key.Length];
                char p = (char)('A' + ((c - 'A' - (k - 'A') + AlphabetSize) % AlphabetSize));
                plainText.Append(p);

                j++;
            }
            return plainText.ToString();
        }


        public string Encrypt(string plainText, string key)
        {

            plainText = plainText.ToUpper();
            key = key.ToUpper();
            StringBuilder cipherText = new StringBuilder();

            for (int i = 0, j = 0; i < plainText.Length; i++)
            {
                char p = plainText[i];
                if (!char.IsLetter(p)) continue;

                char k = key[j % key.Length];
                char c = (char)('A' + ((p - 'A' + (k - 'A')) % AlphabetSize));
                cipherText.Append(c);

                j++;
            }
            return cipherText.ToString();

        }
    }
}