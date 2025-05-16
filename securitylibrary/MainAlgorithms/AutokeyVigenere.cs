using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToUpper();
            key = key.ToUpper();

            String keyStream = key;
            if (keyStream.Length < plainText.Length)
            {
                for (int i = 0; keyStream.Length < plainText.Length; i++)
                {

                    keyStream += plainText[i];

                }
            }
            StringBuilder cipherText = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
            {

                int encryptedChar = ((plainText[i] - 'A') + (keyStream[i] - 'A')) % 26;
                cipherText.Append((char)(encryptedChar + 'A'));
            }

            return cipherText.ToString();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            string plain_Text = "";

            for (int i = 0; i < cipherText.Length; i++)
            {
                int decryptedChar = ((cipherText[i] - 'A') - (key[i] - 'A') + 26) % 26;// 26 3shan lw -ve 
                char decrypted_Letter = (char)(decryptedChar + 'A');
                plain_Text += decrypted_Letter;

                if (key.Length < cipherText.Length)
                    key += decrypted_Letter;
            }
            return plain_Text;
        }

        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            string key = "";

            for (int i = 0; i < plainText.Length; i++)
            {

                int keyChar = ((cipherText[i] - 'A') - (plainText[i] - 'A') + 26) % 26;// 26 3shan lw -ve 
                key += (char)(keyChar + 'A');
            }

            /*
             lab 2
            key =hello
            plaintext=computer
            keystream=hellocom
            ciphertext=jsxaivsd
             */
            for (int i = 1; i < key.Length; i++)//key = hellocom but it = hello
            {
                string possibleKey = key.Substring(0, i);//if i = 1 (key = h only and .....)
                string generatedCipher = Encrypt(plainText, possibleKey);
                if (generatedCipher == cipherText)
                    return possibleKey;
            }

            return key;
        }
    }
}
