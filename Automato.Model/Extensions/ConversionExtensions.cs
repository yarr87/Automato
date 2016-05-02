using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Extensions
{
    public static class ConversionExtensions
    {
        public static decimal? ToDecimal(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            decimal num;

            if (decimal.TryParse(str, out num))
            {
                return num;
            }

            return null;
        }

        public static int ToInt(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            int num;

            if (int.TryParse(str, out num))
            {
                return num;
            }

            return 0;
        }
    }
}
