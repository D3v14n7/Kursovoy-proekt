using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIOLib
{
    public class FIO
    {
        public static bool NameСorrectness(string FIO)
        {
            Regex Word = new Regex(@"^[А-Я][а-я]*$");
            if (Word.IsMatch(FIO))
                return true;
            else
                return false;
        }
    }
}
