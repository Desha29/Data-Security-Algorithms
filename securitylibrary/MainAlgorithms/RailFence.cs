using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            plainText = plainText.ToUpper().Replace(" ", "");
            cipherText = cipherText.ToUpper().Replace(" ", "");
            int key = 0;
            for ( key = 2; key <= plainText.Length; key++) // Start from key=2 (minimum valid rails)
            {
                string testCipher = Encrypt(plainText, key);
                if (testCipher == cipherText)
                {
                    break;
                }
            }
            return key; // Correct key found

            //throw new Exception("Key not found"); // In case no valid key is found
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            string plainText = "";

            Queue<char> planQ = new Queue<char>(cipherText);

            int length = cipherText.Length;

            int width = (length + key - 1) / key;
            int depth = key;
            char[,] railMat = new char[depth, width];


            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (planQ.Count != 0)
                    {
                        railMat[i, j] = planQ.Dequeue();
                    }
                    else
                        railMat[i, j] = '\0';
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    if (railMat[j, i] != '\0')
                    {
                        plainText += railMat[j, i];
                    }
                }
            }

            return plainText.ToLower();

        }

        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();

            string cipherText = "";

            plainText = plainText.ToUpper().Replace("J", "I").Replace(" ", "");

            Queue<char> planQ = new Queue<char>(plainText);

            int length = plainText.Length;

            int width = (length + key -1) / key;
            int depth = key;
            char[,] railMat = new char[depth, width];
            //List<List<char>> rail = new List<List<char>>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    if (planQ.Count != 0)
                    {
                        railMat[j, i] = planQ.Dequeue();
                    }
                    else
                        railMat[j, i] = '\0';
                }
            }

            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (railMat[i, j] != '\0')
                    {
                        cipherText += railMat[i, j];
                    }
                }
            }
            return cipherText;
        }
    }
}
