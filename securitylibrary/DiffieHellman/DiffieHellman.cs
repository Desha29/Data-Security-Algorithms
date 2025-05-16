using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            int Ya = ModPower(alpha, xa, q);
            int Yb = ModPower(alpha, xb, q);

            int Ka = ModPower(Yb, xa, q);
            int Kb = ModPower(Ya, xb, q);

            return new List<int> { Ka, Kb };
        }
        private int ModPower(int baseVal, int exponent, int mod)
        {
            int result = 1;
            baseVal = baseVal % mod;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1)  
                    result = (result * baseVal) % mod;

                exponent = exponent >> 1;  
                baseVal = (baseVal * baseVal) % mod;
            }
            return result;
        }
    }
}
