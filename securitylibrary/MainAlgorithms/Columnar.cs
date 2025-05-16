using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            List<int> key = new List<int>();
            Dictionary<int, int> keyDictionary = new Dictionary<int, int>();
            SortedDictionary<int, int> mainDictionary = new SortedDictionary<int, int>();
            int keySize = 2;
            bool keyFound = false;
            while(!keyFound)
            {
                int numCols = keySize;
                int numRows = (int)Math.Ceiling((double)plainText.Length / keySize);
                int charIndex = 0;
                string[,] matrix = new string[numRows, numCols];
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        if (charIndex < plainText.Length)
                        {
                            matrix[row, col] = plainText[charIndex].ToString();
                            charIndex++;
                        }
                        else
                        {

                            matrix[row, col] = "";

                        }
                    }
                }

                List<string> cipherChunks = new List<string>();
                //dividing the cipher into columns

                for (int col = 0; col < keySize; col++)
                {
                    string chunk = "";
                    for (int row = 0; row < numRows; row++)
                    {
                        chunk += matrix[row, col];
                    }

                    //adding the chunk if it matches
                    cipherChunks.Add(chunk);
                }

                string copyOfCipher = cipherText;
                keyFound = true;
                mainDictionary = new SortedDictionary<int, int>();
                foreach (string chunk in cipherChunks)
                {

                    int x = copyOfCipher.IndexOf(chunk);

                    //checking if its found
                    if (x != -1)
                    {
                        mainDictionary.Add(x, cipherChunks.IndexOf(chunk) + 1);
                        copyOfCipher.Replace(chunk, "x");

                    }
                    else
                    {
                        keyFound = false;
                    }

                }

                keySize++;
            } ;

            for (int chunk = 0; chunk < mainDictionary.Count; chunk++)
            {
                
                keyDictionary.Add(mainDictionary.ElementAt(chunk).Value, chunk + 1);
            }

            for (int i = 1; i < keyDictionary.Count + 1; i++)
            {
                key.Add(keyDictionary[i]);
            }
           
            return key;


        }
        public string Decrypt(string cipherText, List<int> key)
        {
            int columns = key.Count;
            int rows = (int)Math.Ceiling((double)cipherText.Length / columns);

            char[,] matrix = new char[rows, columns];
            int index = 0;

            // Create a sorted key index array
            int[] sortedKeyIndices = new int[columns];
            for (int i = 0; i < columns; i++)
            {
                sortedKeyIndices[key[i] - 1] = i;
            }

            // Fill the matrix column-wise
            for (int col = 0; col < columns; col++)
            {
                int sortedIndex = sortedKeyIndices[col];
                for (int row = 0; row < rows && index < cipherText.Length; row++)
                {
                    matrix[row, sortedIndex] = cipherText[index++];
                }
            }

            // Read the matrix row-wise
            StringBuilder plainText = new StringBuilder(cipherText.Length);
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (matrix[row, col] != '\0')
                    {
                        plainText.Append(matrix[row, col]);
                    }
                }
            }

            return plainText.ToString().TrimEnd('x');
        }


        public string Encrypt(string plainText, List<int> key)
        {
            plainText = plainText.Replace(" ", "");
            int columns = key.Count;

            int rows = (int)Math.Ceiling((double)plainText.Length / columns);
            char[,] matrix = new char[rows, columns];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (index < plainText.Length)
                    {
                        matrix[i, j] = plainText[index++];
                    }
                    else
                    {
                        matrix[i, j] = 'x';
                    }
                }
            }
            StringBuilder cipherText = new StringBuilder();
            List<int> sortedKey = key.Select((x, i) => new KeyValuePair<int, int>(x, i)).OrderBy(x => x.Key).Select(x => x.Value).ToList();
            foreach (int i in sortedKey)
            {
                for (int j = 0; j < rows; j++)
                {
                    cipherText.Append(matrix[j, i]);
                }
            }
            return cipherText.ToString();
            
        }
    }
}
