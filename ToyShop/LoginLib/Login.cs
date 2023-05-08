using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoginLib
{
    public class Login
    {
        public static bool LoginCorrectness(string Login)
        {
            Regex Text = new Regex(@".{4,20}$");
            if (Text.IsMatch(Login))
                return true;
            else
                return false;
        }
    }
}
