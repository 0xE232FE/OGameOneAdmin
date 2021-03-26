using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGameOneAdmin.Object
{
    /// <summary>
    /// Class containing key and value of some entity.
    /// </summary>
    public class ItemObject
    {
        private string key;
        private object valueOfKey;

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="key">Key of object.</param>
        /// <param name="valueOfKey">Value of object.</param>
        public ItemObject(string key, object valueOfKey)
        {
            this.key = key;
            this.valueOfKey = valueOfKey;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ItemObject()
        {
            key = string.Empty;
            valueOfKey = string.Empty;
        }

        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        public override string ToString()
        {
            return key;
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object"></see>.
        ///</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Gets or sets Key of object.
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// Gets or sets Value of object.
        /// </summary>
        public object ValueOfKey
        {
            get { return valueOfKey; }
            set { valueOfKey = value; }
        }
    }
}
