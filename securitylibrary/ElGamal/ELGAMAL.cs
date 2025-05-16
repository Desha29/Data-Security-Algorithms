using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            long c1 = ModPower(alpha, k, q);
            long c2 = (m * ModPower(y, k, q)) % q;

            return new List<long> { c1, c2 };
        }
        public int Decrypt(int c1, int c2, int x, int q)
        {
            long s = ModPower(c1, x, q);
            long sInverse = ModInverse(s, q);
            long m = (c2 * sInverse) % q;

            return (int)m;
        }
        private long ModPower(long baseVal, int exponent, int mod)
        {
            long result = 1;
            baseVal %= mod;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * baseVal) % mod;

                baseVal = (baseVal * baseVal) % mod;
                exponent >>= 1;
            }
            return result;
        }
        private long ModInverse(long a, int mod)
        {
            long m0 = mod, t, q;
            long x0 = 0, x1 = 1;

            if (mod == 1) return 0;

            while (a > 1)
            {
                q = a / mod;
                t = mod;

                mod = (int)(a % mod);
                a = t;

                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0) x1 += m0;

            return x1;
        }
    }
}
