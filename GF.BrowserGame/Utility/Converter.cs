using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.BrowserGame.Utility
{
    internal static class Converter
    {
        internal static DateTime GetNoteDateTime(string sDateTime)
        {
            //Date: Sun Oct 17 14:31:35 2010
            //index  0   1   2  3  4  5  6
            char[] separators = { ' ', ':' };
            string[] values = sDateTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                DateTime dt = new DateTime(int.Parse(values[6]), Utilities.MonthToInt(values[1]), int.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5]));
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
