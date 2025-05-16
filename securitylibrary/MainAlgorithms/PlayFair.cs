using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {

        private string decrypt(char firstChar, char secondChar, Dictionary<char, KeyValuePair<int, int>> keyMatrixCoordinate, char[,] keyMat)
        {
            string plainText = "";
            var pos1 = keyMatrixCoordinate[firstChar];
            var pos2 = keyMatrixCoordinate[secondChar];

            int row1 = pos1.Key, col1 = pos1.Value;
            int row2 = pos2.Key, col2 = pos2.Value;

            if (row1 == row2)
            {
                plainText += keyMat[row1, (col1 + 4) % 5];
                plainText += keyMat[row2, (col2 + 4) % 5];
            }
            else if (col1 == col2)
            {
                plainText += keyMat[(row1 + 4) % 5, col1];
                plainText += keyMat[(row2 + 4) % 5, col2];
            }
            else
            {
                plainText += keyMat[row1, col2];
                plainText += keyMat[row2, col1];
            }
            return plainText;
        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            char[,] keyMat =keyMatrix(key);
            List<char> text = new List<char>();
            Dictionary<char, KeyValuePair<int, int>> keyMatrixCoordinate = new Dictionary<char, KeyValuePair<int, int>>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    keyMatrixCoordinate.Add(keyMat[i, j], new KeyValuePair<int, int>(i, j));
                }
            }

            string plainText = "";
            for (int i = 0; i < cipherText.Length; i += 2)
            {
                plainText += decrypt(cipherText[i] , cipherText[i+1] , keyMatrixCoordinate ,keyMat);
            }

            List<char> plain = new List<char>();

            foreach (var letter in plainText)
            {
                plain.Add(letter);
            }

            if (plain.Last() == 'X' && plain.Count%2 == 0) { 
                plain.RemoveAt(plain.Count-1);
            }
            for (int i = 1; i < plain.Count - 1; i +=2)
            {
                if (plain[i]=='X' &&(plain.ElementAt(i-1) == plain.ElementAt(i + 1)))
                {
                    plain.RemoveAt(i);
                    i--;
                }
            }
            plainText ="";
            foreach (var letter in plain)
            {
                plainText += letter;
            }
            plainText = plainText.ToLower();
            return plainText;
        }

        private char[,] keyMatrix(string key)
        {
            char[,] keyMat = new char[5, 5];

            List<char> alphabet = new List<char>();

            for (int i = 0; i < 26; i++)
            {
                if (i == 9)
                {
                    continue;
                }
                alphabet.Add((char)('A' + i));
                //keys += alphabet.Last();
            }
            key = key.ToUpper().Replace("J", "I").Replace(" ", "");
            HashSet<char> keySet = new HashSet<char>();
            foreach (var letter in key)
            {
                keySet.Add(letter);
                alphabet.Remove(letter);
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (keySet.Count != 0)
                    {
                        keyMat[i, j] = keySet.ElementAt(0);
                        keySet.Remove(keySet.ElementAt(0));
                    }
                    else
                    {
                        keyMat[i, j] = alphabet.ElementAt(0);
                        alphabet.Remove(alphabet.ElementAt(0));
                    }
                    //keys += keyMat[i, j];
                }
            }
            return keyMat;
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string encryptedText = "";
            string keys = "";

            plainText = plainText.ToUpper().Replace("J", "I").Replace(" ", "");
            List<char> plain = new List<char>();

            foreach (var letter in plainText) { 
                plain.Add(letter);
            }

            for (int i = 0; i < plain.Count-1; i+=2)
            {
                if(plain.ElementAt(i)== plain.ElementAt(i + 1))
                {
                    plain.Insert(i + 1, 'X');
                }
            }
            if (plain.Count %2 != 0) {
                plain.Add('X');
            }

            char[,] keyMat = keyMatrix(key);

            Dictionary<char, KeyValuePair<int, int>> keyMatrixCoordinate = new Dictionary<char, KeyValuePair<int, int>>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    keyMatrixCoordinate.Add(keyMat[i, j], new KeyValuePair<int, int>(i, j));
                }
            }

            for (int i = 0; i < plain.Count ;)
            {
                char firstChar = plain[i];
                char secondChar = plain[i + 1];

                    encryptedText += encrypt(firstChar, secondChar, keyMatrixCoordinate, keyMat);
                    i += 2;

            }    
            return encryptedText;
        }
        private string encrypt (char firstChar ,char secondChar , Dictionary<char , KeyValuePair<int,int>> keyMatrixCoordinate , char[,] keyMat) {
            string encryptedText = "";
            var pos1 = keyMatrixCoordinate[firstChar];
            var pos2 = keyMatrixCoordinate[secondChar];

            int row1 = pos1.Key, col1 = pos1.Value;
            int row2 = pos2.Key, col2 = pos2.Value;

            if (row1 == row2)
            {
                encryptedText += keyMat[row1, (col1 + 1) % 5];
                encryptedText += keyMat[row2, (col2 + 1) % 5];
            }
            else if (col1 == col2)
            {
                encryptedText += keyMat[(row1 + 1) % 5, col1];
                encryptedText += keyMat[(row2 + 1) % 5, col2];
            }
            else
            {
                encryptedText += keyMat[row1, col2];
                encryptedText += keyMat[row2, col1];
            }
            return encryptedText;
        }

        public string Analyse(string largeCipher)
        {
            throw new NotImplementedException();
        }
    }
}
