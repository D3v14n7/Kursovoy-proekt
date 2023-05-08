using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhoneLib
{
    public class Phone
    {
        public static bool PhoneCorrectness(string Phone)
        {
            Regex Numbers = new Regex(@"^\d{11}$");
            if (Numbers.IsMatch(Phone))
                return true;
            else
                return false;
        }
    }
}
