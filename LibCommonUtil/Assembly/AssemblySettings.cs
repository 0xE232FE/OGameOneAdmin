using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace LibCommonUtil
{
    sealed public class AssemblySettings
    {
        /***********************************************************************************************************/


        #region ----- Private Variable ------


        private IDictionary settings;


        #endregion ----- Private Variable ------


        /***********************************************************************************************************/


        #region -----Constructor ------


        /// <summary>
        /// Constructor.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public AssemblySettings()
            : this(Assembly.GetCallingAssembly())
        { }


        /// <summary>
        /// Overload Constructor. Specified assembly.
        /// </summary>
        /// <param name="asm"></param>
        public AssemblySettings(Assembly asm)
        {
            settings = GetConfig(asm);
        }


        #endregion -----Constructor ------


        /***********************************************************************************************************/


        #region ----- Public Property ------


        public string this[string key]
        {
            get
            {
                string settingValue = null;

                if (settings != null)
                {
                    settingValue = settings[key] as string;
                }

                return (settingValue == null ? "" : settingValue);
            }
            set
            {
                string settingValue = null;

                if (settings != null)
                {
                    settingValue = settings[key] as string;
                }

                if (settingValue != null)
                {
                    settings[key] = value;
                }
            }
        }


        #endregion ----- Public Property ------


        /***********************************************************************************************************/


        #region ----- Public Static Methods ------


        /// <summary>
        /// Open and parse configuration file for calling assembly,
        /// returning collection to caller for future use outside of this class.
        /// </summary>
        /// <returns></returns>
        public static IDictionary GetConfig()
        {
            return GetConfig(Assembly.GetCallingAssembly());
        }


        /// <summary>
        /// Open and parse configuration file for specified assembly,
        /// returning collection to caller for future use outside of this class.
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static IDictionary GetConfig(Assembly asm)
        {
            try
            {
                string cfgFile = asm.CodeBase + ".config";
                const string nodeName = "assemblysettings";

                XmlDocument doc = new XmlDocument();
                doc.Load(new XmlTextReader(cfgFile));

                XmlNodeList nodes = doc.GetElementsByTagName(nodeName);

                foreach (XmlNode node in nodes)
                {
                    if (node.LocalName == nodeName)
                    {
                        DictionarySectionHandler handler = new DictionarySectionHandler();
                        return (IDictionary)handler.Create(null, null, node);
                    }
                }
            }
            catch
            { }

            return (null);
        }


        #endregion ----- Public Static Methods ------


        /***********************************************************************************************************/
    }
}
