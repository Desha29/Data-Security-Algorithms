using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();

            char[] cipher = cipherText.ToLower().ToCharArray();
            char[] plain = plainText.ToLower().ToCharArray();
            char[] alphabets = "abcdefghijklmnopqrstuvwxyz".ToLower().ToCharArray();
            Dictionary<char, char> permutation = new Dictionary<char, char>();
            //bool found ;
            //Queue<char> remain = new Queue<char>();
            List<char> exist = new List<char>();
            for (int i = 0; i < 26; i++)
            {
                permutation[alphabets[i]] = ' ';
            }
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < plain.Length; j++)
                {
                    if (alphabets[i] == plain[j])
                    {
                        permutation[alphabets[i]] = cipher[j];
                        exist.Add(cipher[j]);
                        break;
                    }
                }
            }
            for (int i = 0; i < 26; i++)
            {
                if (!exist.Contains(alphabets[i]))
                {
                    for (int j = 0; j < 26; j++)
                    {
                        if (permutation[alphabets[j]] == ' ')
                        {
                            permutation[alphabets[j]] = alphabets[i];
                            break;
                        }
                    }

                }
            }

            List<char> chars = new List<char>();
            for (int i = 0; i < 26; i++)
            {
                chars.Add(permutation[alphabets[i]]);
            }



            string key = string.Concat(chars);
            return key;


        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();

            char[] mycipher = cipherText.ToLower().ToCharArray();
            List<char> myplain = new List<char>();
            char[] alphabets = "abcdefghijklmnopqrstuvwxyz".ToLower().ToCharArray();
            char[] rowkey = key.ToLower().ToCharArray();

            Dictionary<char, char> finalkey = new Dictionary<char, char>();
            for (int i = 0; i < alphabets.Length; i++)
            {
                finalkey[rowkey[i]] = alphabets[i];
            }

            for (int i = 0; i < mycipher.Length; i++)
            {
                myplain.Add(finalkey[mycipher[i]]);
            }

            string plain;
            plain = string.Concat(myplain);
            return plain;

        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();

            char[] myplain = plainText.ToLower().ToCharArray();
            List<char> mycipher = new List<char>();
            char[] alphabets = "abcdefghijklmnopqrstuvwxyz".ToLower().ToCharArray();
            char[] rowkey = key.ToLower().ToCharArray();

            Dictionary<char, char> finalkey = new Dictionary<char, char>();
            for (int i = 0; i < alphabets.Length; i++)
            {
                finalkey[alphabets[i]] = rowkey[i];
            }

            for (int i = 0; i < myplain.Length; i++)
            {
                mycipher.Add(finalkey[myplain[i]]);
            }

            string cipher;
            cipher = string.Concat(mycipher);
            return cipher;

        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            //throw new NotImplementedException();
            char[] mycipher = cipher.ToLower().ToCharArray();
            string plain = null;
            char[] sortedLettersByFreq = "etaoinsrhldcumfpgwybvkxjqz".ToLower().ToCharArray();
            int[] cipherFrequency = new int[26];
            int[] cipherFrequencyBackup = new int[26];

            for (int x = 'a'; x <= 'z'; x++)
            {
                int counter = 0;
                for (int k = 0; k < mycipher.Length; k++)
                {
                    if (x == mycipher[k])
                    { counter++; }
                }
                cipherFrequency[((int)x - 97)] = counter;
                cipherFrequencyBackup[((int)x - 97)] = counter;
            }

            string sortedCipherFreq = null;
            Array.Sort(cipherFrequencyBackup);
            Array.Reverse(cipherFrequencyBackup);

            for (int i = 0; i < 26; i++)
            {
                for (int k = 0; k < 26; k++)
                {
                    if ((cipherFrequencyBackup[i] == cipherFrequency[k]))
                    {
                        sortedCipherFreq += (char)(k + 97);
                        break;
                    }
                }
            }
            for (int i = 0; i < mycipher.Length; i++)
            {
                for (int k = 0; k < 26; k++)
                {
                    if (mycipher[i] == sortedCipherFreq[k])
                    {
                        mycipher[i] = sortedLettersByFreq[k];
                        break;
                    }
                }
            }
            for (int i = 0; i < mycipher.Length; i++)
            {
                plain += mycipher[i];
            }
            return plain;






        }
    }
}
