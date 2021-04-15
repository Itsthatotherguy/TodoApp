using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Utilities.Helpers
{
    public static class SortingManager
    {
        public static int SortText(string a, string b)
        {
            return String.Compare(a, b, StringComparison.OrdinalIgnoreCase);
        }

        public static int SortDate(DateTime? a, DateTime? b)
        {
            if (a == null || b == null)
            {
                return 0;
            }

            return DateTime.Compare((DateTime)a, (DateTime)b);
        }

        public static int SortBool(bool a, bool b)
        {
            return String.Compare(a.ToString(), b.ToString(), StringComparison.Ordinal);
        }
    }
}
