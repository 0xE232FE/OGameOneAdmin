using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace LibCommonUtil
{
    public class PropertyComparer<T> : IComparer<T>
    {
        private enum SortType
        {
            Descending = -1,
            Ascending = 1
        }

        private SortType mySortDirecton;
        private string mySortPropertyName;


        public PropertyComparer(string sortString)
        {
            if (sortString == null)
                throw new ArgumentNullException("sortString");
            if (sortString.Length == 0)
                throw new ArgumentOutOfRangeException("sortString");

            if (sortString.ToLower().EndsWith(" desc"))
            {
                mySortPropertyName = sortString.Substring(0, sortString.Length - 5);
                mySortDirecton = SortType.Descending;
            }
            else
            {
                if (sortString.ToLower().EndsWith(" asc"))
                    mySortPropertyName = sortString.Substring(0, sortString.Length - 4);
                else
                    this.mySortPropertyName = sortString;

                this.mySortDirecton = SortType.Ascending;
            }
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            if ((x == null) && (y == null))
                return 0;
            if (x == null)
                return -(int)mySortDirecton;
            if (y == null)
                return (int)mySortDirecton;

            PropertyInfo p = x.GetType().GetProperty(mySortPropertyName);
            if (p == null)
                throw new ApplicationException();

            object vX = p.GetValue(x, null);
            object vY = p.GetValue(y, null);

            if ((vX == null) && (vY == null))
                return 0;
            if (vX == null)
                return -(int)mySortDirecton;
            if (vY == null)
                return (int)mySortDirecton;

            return (int)mySortDirecton * Comparer.DefaultInvariant.Compare(vX, vY);
        }

        #endregion
    }
}
