using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityLibrary.AES;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int fastPowMod(int num,int power,int mod)
        {
            if (power == 0)
                return 1;
            if (power == 1)
                return num;

            long half = fastPowMod(num, power / 2,mod)%mod;
            if (power %2 == 0)
                return (int)((half % mod) * (half % mod) % mod);
            else
                return (int)(((half % mod) * (half % mod) * (num % mod)) % mod);
        }
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            int C = fastPowMod(M,e,n);
       
            return C;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q;
            int eular = (p - 1) * (q - 1);
            int d = GetMultiplicativeInverse(e, eular);
            int M = fastPowMod(C,d,n);

            return M;
        }
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
