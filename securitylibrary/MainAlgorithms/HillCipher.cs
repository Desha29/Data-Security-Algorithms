using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {

        public static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static int ModularInverse(int a, int m)
        {
            a = a % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                {
                    return x;
                }
            }
            return -1; // Return -1 if modular inverse doesn't exist
        }

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {

            for (int a = 0; a < 26; a++)
            {
                for (int b = 0; b < 26; b++)
                {
                    for (int c = 0; c < 26; c++)
                    {
                        for (int d = 0; d < 26; d++)
                        {
                            // Create the key matrix
                            List<int> keyMatrix = new List<int> { a, b, c, d };

                            // Encrypt the plaintext using the current key matrix
                            List<int> encryptedText = Encrypt(plainText, keyMatrix);

                            // Check if the encrypted text matches the ciphertext
                            if (encryptedText.SequenceEqual(cipherText))
                            {
                                // Return the key matrix as a list
                                return new List<int> { a, b, c, d };
                            }
                        }
                    }
                }
            }
            throw new InvalidAnlysisException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int key_dimention = (int)Math.Sqrt(key.Count);
            int[,] keyMatrix = new int[key_dimention, key_dimention];
            int index = 0;
            for (int row = 0; row < key_dimention; row++)
            {
                for (int col = 0; col < key_dimention; col++)
                {
                    keyMatrix[row, col] = key[index];
                    index++;
                }
            }


            List<int> plaintext = new List<int>();

            int det = 0;
            if (key_dimention == 2)
            {
                det = ((keyMatrix[0, 0] * keyMatrix[1, 1]) - (keyMatrix[0, 1] * keyMatrix[1, 0])) % 26;

                if (det < 0)
                {
                    det += 26;
                }

                if (det == 0 || GCD(det, 26) != 1)
                {
                    throw new InvalidAnlysisException();
                }


                int modular_inverse = ModularInverse(det, 26);

                if (modular_inverse == -1)
                {
                    throw new InvalidAnlysisException();
                }

                int[,] inverseMatrix = new int[2, 2];
                inverseMatrix[0, 0] = (modular_inverse * keyMatrix[1, 1]) % 26;
                inverseMatrix[0, 1] = (modular_inverse * -keyMatrix[0, 1]) % 26;
                inverseMatrix[1, 0] = (modular_inverse * -keyMatrix[1, 0]) % 26;
                inverseMatrix[1, 1] = (modular_inverse * keyMatrix[0, 0]) % 26;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (inverseMatrix[i, j] < 0) inverseMatrix[i, j] += 26;
                    }
                }


                List<int> inverseKey = new List<int>();
                for (int i = 0; i < key_dimention; i++)
                {
                    for (int j = 0; j < key_dimention; j++)
                    {
                        inverseKey.Add(inverseMatrix[i, j]);
                    }
                }
                plaintext = Encrypt(cipherText, inverseKey);

            }
            else if (key_dimention == 3)
            {
                int a = keyMatrix[0, 0], b = keyMatrix[0, 1], c = keyMatrix[0, 2],
                    d = keyMatrix[1, 0], e = keyMatrix[1, 1], f = keyMatrix[1, 2],
                    g = keyMatrix[2, 0], h = keyMatrix[2, 1], i = keyMatrix[2, 2];

                det = (a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g)) % 26;

                if (det < 0)
                {
                    det += 26;
                }

                if (det == 0 || GCD(det, 26) != 1)
                {
                    throw new InvalidAnlysisException();
                }


                int modular_inverse = ModularInverse(det, 26);


                if (modular_inverse == -1)
                {
                    throw new InvalidAnlysisException();
                }
                int[,] inverseKeyMatrix = new int[3, 3];
                inverseKeyMatrix[0, 0] = (modular_inverse * (e * i - f * h)) % 26;
                inverseKeyMatrix[0, 1] = (modular_inverse * -(b * i - c * h)) % 26;
                inverseKeyMatrix[0, 2] = (modular_inverse * (b * f - c * e)) % 26;

                inverseKeyMatrix[1, 0] = (modular_inverse * -(d * i - f * g)) % 26;
                inverseKeyMatrix[1, 1] = (modular_inverse * (a * i - c * g)) % 26;
                inverseKeyMatrix[1, 2] = (modular_inverse * -(a * f - c * d)) % 26;

                inverseKeyMatrix[2, 0] = (modular_inverse * (d * h - e * g)) % 26;
                inverseKeyMatrix[2, 1] = (modular_inverse * -(a * h - b * g)) % 26;
                inverseKeyMatrix[2, 2] = (modular_inverse * (a * e - b * d)) % 26;

                // Normalize negative values
                for (i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (inverseKeyMatrix[i, j] < 0)
                            inverseKeyMatrix[i, j] += 26;
                    }
                }

                //int[,] transposeKeyMatrix = new int[key_dimention, key_dimention];
                //for (i = 0; i < key_dimention; i++)
                //{
                //    for (int j = 0; j < key_dimention; j++)
                //    {
                //        transposeKeyMatrix[j, i] = inverseKeyMatrix[i, j]; // Swap rows and columns
                //    }
                //}
                List<int> inverseKey = new List<int>();
                for (i = 0; i < key_dimention; i++)
                {
                    for (int j = 0; j < key_dimention; j++)
                    {
                        inverseKey.Add(inverseKeyMatrix[i, j]);
                    }
                }

                plaintext = Encrypt(cipherText, inverseKey);

            }
            else
            {
                //very complex :);
            }

            return plaintext;

        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int keyLength = key.Count;
            int key_dimention = (int)Math.Sqrt(keyLength);
            List<int> ciphertext = new List<int>();

            if (key_dimention * key_dimention != keyLength)
            {
                throw new InvalidAnlysisException();
            }


            int[,] keyMatrix = new int[key_dimention, key_dimention];
            int index = 0;

            for (int row = 0; row < key_dimention; row++)
            {
                for (int col = 0; col < key_dimention; col++)
                {
                    keyMatrix[row, col] = key[index];
                    index++;
                }
            }
            for (int i = 0; i < plainText.Count; i += key_dimention)
            {
                for (int j = 0; j < key_dimention; j++)
                {
                    int row = 0;
                    for (int k = 0; k < key_dimention; k++)
                    {
                        row += keyMatrix[j, k] * plainText[i + k];
                    }
                    row = row % 26;
                    ciphertext.Add(row);
                }
            }

            return ciphertext;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {

            // Extract three 3x1 plaintext and ciphertext pairs
            int[,] plainMatrix = new int[3, 3];
            int[,] cipherMatrix = new int[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    plainMatrix[i, j] = plainText[j * 3 + i];
                    cipherMatrix[i, j] = cipherText[j * 3 + i];
                }
            }

            // Step 1: Calculate the determinant of the plaintext matrix
            int det = CalculateDeterminant(plainMatrix) % 26;
            if (det < 0) det += 26; // Ensure the determinant is positive

            // Step 2: Check if the determinant is invertible modulo 26
            if (GCD(det, 26) != 1)
            {
                throw new InvalidAnlysisException();
            }


            // Step 3: Find the modular inverse of the determinant
            int detInverse = ModularInverse(det, 26);

            if (detInverse == -1)
            {
                throw new InvalidAnlysisException();
            }

            // Step 4: Compute the adjoint matrix of the plaintext matrix
            int[,] adjointMatrix = CalculateAdjointMatrix(plainMatrix);



            // Step 5: Compute the inverse of the plaintext matrix
            int[,] inversePlainMatrix = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    inversePlainMatrix[i, j] = (adjointMatrix[i, j] * detInverse) % 26;
                    if (inversePlainMatrix[i, j] < 0)
                    {
                        inversePlainMatrix[i, j] += 26;
                    }
                }
            }

            // Step 6: Multiply the ciphertext matrix by the inverse plaintext matrix to get the key matrix
            int[,] keyMatrix = MultiplyMatrices(cipherMatrix, inversePlainMatrix);

            // Step 7: Return the key matrix as a list
            return new List<int> {
                            keyMatrix[0, 0], keyMatrix[0, 1], keyMatrix[0, 2],
                            keyMatrix[1, 0], keyMatrix[1, 1], keyMatrix[1, 2],
                            keyMatrix[2, 0], keyMatrix[2, 1], keyMatrix[2, 2]
                           };
        }

        // Helper function to calculate the determinant of a 3x3 matrix
        private int CalculateDeterminant(int[,] matrix)
        {
            int det = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1])
                    - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])
                    + matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
            return det;
        }

        // Helper function to calculate the adjoint matrix of a 3x3 matrix
        public int[,] CalculateAdjointMatrix(int[,] matrix)
        {
            int[,] adjoint = new int[3, 3];

            adjoint[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) % 26;
            adjoint[0, 1] = (-(matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])) % 26;
            adjoint[0, 2] = (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]) % 26;

            adjoint[1, 0] = (-(matrix[0, 1] * matrix[2, 2] - matrix[0, 2] * matrix[2, 1])) % 26;
            adjoint[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) % 26;
            adjoint[1, 2] = (-(matrix[0, 0] * matrix[2, 1] - matrix[0, 1] * matrix[2, 0])) % 26;

            adjoint[2, 0] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) % 26;
            adjoint[2, 1] = (-(matrix[0, 0] * matrix[1, 2] - matrix[0, 2] * matrix[1, 0])) % 26;
            adjoint[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) % 26;

            // Ensure all values are positive
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (adjoint[i, j] < 0)
                    {
                        adjoint[i, j] += 26;
                    }
                }
            }

            return adjoint;
        }

        // Helper function to multiply two 3x3 matrices
        public int[,] MultiplyMatrices(int[,] matrix1, int[,] matrix2)
        {

            int[,] result = new int[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[j, k];
                    }
                    result[i, j] = result[i, j] % 26;
                    if (result[i, j] < 0)
                    {
                        result[i, j] += 26;
                    }
                }
            }

            return result;
        }
    }
}
