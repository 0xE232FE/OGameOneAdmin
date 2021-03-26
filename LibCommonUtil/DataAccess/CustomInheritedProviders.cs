using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Specialized;

namespace LibCommonUtil.CustomInheritedProviders
{
    public class SqlMembershipProvider : System.Web.Security.SqlMembershipProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            // Update the private connection string field in the base class.
            string connectionString = GetConnectionString();
            // Set private property of Membership provider.
            FieldInfo connectionStringField = GetType().BaseType.GetField("_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic);
            connectionStringField.SetValue(this, connectionString);
        }

        public static string GetConnectionString()
        {
            string defaultDatabase = ConfigurationManager.AppSettings["defaultDatabase"];
            if (string.IsNullOrEmpty(defaultDatabase)) throw new Exception("Section 'appSetting' does not contain 'defaultDatabase' key");

            string sConnectionString = ConfigurationManager.ConnectionStrings[defaultDatabase].ConnectionString;
            if (string.IsNullOrEmpty(sConnectionString))
                throw new Exception("Config file does not contain connectionStrings section");

            return Encryption.DecryptString(sConnectionString);
        }
    }

    public class SqlRoleProvider : System.Web.Security.SqlRoleProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            // Update the private connection string field in the base class.
            string connectionString;
            try
            {
                connectionString = GetConnectionString();
            }
            catch (Exception)
            {
                throw;
            }

            // Set private property of Membership provider.
            FieldInfo connectionStringField = GetType().BaseType.GetField("_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic);
            connectionStringField.SetValue(this, connectionString);
        }

        public static string GetConnectionString()
        {
            string defaultDatabase = ConfigurationManager.AppSettings["defaultDatabase"];
            if (string.IsNullOrEmpty(defaultDatabase)) throw new Exception("Section 'appSetting' does not contain 'defaultDatabase' key");

            string sConnectionString = ConfigurationManager.ConnectionStrings[defaultDatabase].ConnectionString;
            if (string.IsNullOrEmpty(sConnectionString))
                throw new Exception("Config file does not contain connectionStrings section");

            return Encryption.DecryptString(sConnectionString);
        }
    }
}
