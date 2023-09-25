using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleTaskList.Helper
{
    public static class RegexHelper
    {
        public static bool IsValidDateFormat(string input, string format)
        {
            return Regex.IsMatch(input, @"^\d{4}-\d{2}-\d{2}$") && DateTime.TryParseExact(input, format, null, DateTimeStyles.None, out _);
        }
    }
}
