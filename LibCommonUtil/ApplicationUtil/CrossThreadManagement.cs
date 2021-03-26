using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace LibCommonUtil
{
    sealed public class CrossThreadManagement
    {
        /***********************************************************************************************************/


        #region ----- Delegate ------


        delegate void SetControlValueCallback(Control control, string argument, object value);


        #endregion ----- Delegate ------


        /***********************************************************************************************************/


        #region ------ Public Static Methods ------


        public static void SetControlValue(Control control, string argument, object value)
        {
            if (control.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlValue);
                control.Invoke(d, new object[] { control, argument, value });
            }
            else
            {
                Type type = control.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties();
                MethodInfo[] methodInfos = type.GetMethods();

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.Name.ToUpper() == argument.ToUpper())
                    {
                        propertyInfo.SetValue(control, value, null);
                    }
                }

                foreach (MethodInfo methodInfo in methodInfos)
                {
                    if (methodInfo.Name.ToUpper() == argument.ToUpper())
                    {
                        methodInfo.Invoke(control, new object[] { value + Environment.NewLine });
                    }
                }
            }
        }


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/
    }
}
