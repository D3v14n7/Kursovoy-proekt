using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordLib
{
    public class Password
    {
        public static string PasswordComplexity(string password)
        {
            int scoreReliability = 0;
            Regex lenght = new Regex(@".{8,}");
            if (lenght.IsMatch(password))
            {
                scoreReliability++;
            }
            Regex letterSmall = new Regex(@"(?=.*[a-z])");
            if (letterSmall.IsMatch(password))
            {
                scoreReliability++;
            }
            Regex letterBig = new Regex(@"(?=.*[A-Z])");
            if (letterBig.IsMatch(password))
            {
                scoreReliability++;
            }
            Regex number = new Regex(@"(?=.*[0-9])");
            if (number.IsMatch(password))
            {
                scoreReliability++;
            }
            Regex symbol = new Regex(@"(?=.*[@#$%^!*])");
            if (symbol.IsMatch(password))
            {
                scoreReliability++;
            }

            if (scoreReliability < 2)
                return "Ненадежный";
            else if ((scoreReliability < 4) && (scoreReliability > 1))
                return "Простой";
            else if (scoreReliability == 4)
                return "Хороший";
            else
                return "Сложный";
        }
    }
}
