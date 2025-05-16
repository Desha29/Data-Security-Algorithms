using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int x0 = 0, x1 = 1;
            int y0 = 1, y1 = 0;
            int a = baseN, b = number;
            int q, r, m, n;
            while (b != 0)
            {
                q = a / b;
                r = a % b;
                m = x0 - q * x1;
                n = y0 - q * y1;
                a = b;
                b = r;
                x0 = x1;
                x1 = m;
                y0 = y1;
                y1 = n;
            }
            if (a != 1)
            {
                return -1; 
            }
            else
            {
                return (x0 + baseN) % baseN; 
            }
        }
    }
}
