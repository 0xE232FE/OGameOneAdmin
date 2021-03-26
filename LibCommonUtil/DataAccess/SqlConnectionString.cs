using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace LibCommonUtil
{
    public class SqlConnectionString
    {
        /// <summary>
        /// To create SqlConnectorString with default connection. We need to add the ConnectionString into 
        /// the Config file of our application. Please see an example of App.Config or Web.Config files.
        /// <example of App.Config or Web.Config files>
        ///     <?xml version="1.0" encoding="utf-8" ?>
        ///     <configuration>
        ///         <connectionStrings>
        ///             <add name="SQLDefaultDB" providerName="System.Data.SqlClient" connectionString="Data Source=(local)\SQLEXPRESS;Initial Catalog=SQLDevDB; Integrated Security=SSPI;"/>
        ///             <add name="SQLSecondDB" providerName="System.Data.SqlClient" connectionString="Data Source=TESTSVR;Initial Catalog=SQLTestDB;User Id=sa;Password=password;"/>
        ///             <add name="SQLMDCube" providerName="System.Data.SqlClient" connectionString="Data Source=MDCubeSVR;Initial Catalog=MDCubeDB;Persist Security Info=True;User ID=sa;Password=password"/>
        ///         </connectionStrings>        
        ///         <appSettings>
        ///             <add key ="defaultDatabase" value ="SQLDefaultDB"/> <!-- change "SQLDefaultDB" to "SQLSecondDB" to connect to TESTSVR server -->
        ///             <add key ="secondDatabase" value ="SQLSecondDB"/>
        ///             <add key ="defaultMDCube" value ="SQLMDCube"/>
        ///         </appSettings>
        ///     </configuration>
        /// </example of App.Config or Web.Config files>
        /// </summary>        
        
        private const char CONNSTRING_DELIMIT = ';';
        private string _sConnectionString = null;
        private NameValueCollection _ConnStrCollection = null;

        #region -----Contructors------------------------

        /// <summary>
        /// Create a default connectionString.
        /// </summary>
        public SqlConnectionString()
        {
            _sConnectionString = ReturnDefaultConnectionString();
            _ConnStrCollection = ParseConnectionString(_sConnectionString);
            UpdateConnectionString();
        }

        /// <summary>
        /// Create a connectionString object by a specific database key in App.Config file.
        /// </summary>
        /// <param name="sDataBaseKey">other database key in Section appSetting (e.g: 'secondDatabase')</param>
        public SqlConnectionString(string sDataBaseKey)
        {
            _sConnectionString = ReturnSpecificConnectionString(sDataBaseKey);
            _ConnStrCollection = ParseConnectionString(_sConnectionString);
            UpdateConnectionString();
        }

        /// <summary>
        /// Create a connectionString object by specific ServerName and DatabaseName.
        /// </summary>
        /// <param name="dbServerName">Database Server name</param>
        /// <param name="dbName">Database name</param>
        public SqlConnectionString(string dbServerName, string dbName)
        {
            if (string.IsNullOrEmpty(dbServerName)) throw new ArgumentException("Server name is null or empty");
            if (string.IsNullOrEmpty(dbName)) throw new ArgumentException("Database name is null or empty");

            _ConnStrCollection = CreateConnStrCollection(dbServerName, dbName); 
            UpdateConnectionString();
        }

        /// <summary>
        /// Create a connectionString object by specific ServerName, DatabaseName, UserID and Password.
        /// </summary>
        /// <param name="dbServerName">Database Server name</param>
        /// <param name="dbName">Database name</param>
        /// <param name="userID">Username</param>
        /// <param name="password">Password</param>
        public SqlConnectionString(string dbServerName, string dbName, string userID, string password)
        {
            if (string.IsNullOrEmpty(dbServerName)) throw new ArgumentException("Server name is null or empty");
            if (string.IsNullOrEmpty(dbName)) throw new ArgumentException("Database Name is null or empty");
            if (string.IsNullOrEmpty(userID)) throw new ArgumentException("UserID is null or empty");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password is null or empty");

            _ConnStrCollection = CreateConnStrCollection(dbServerName, dbName, userID, password);
            UpdateConnectionString();
        }
        #endregion

        #region -----Public methods and Properties------

        /// <summary>
        /// Save current connection string into configuration file in connectionString linking with 'defaultDatabase' key.
        /// </summary>
        /// <param name="sAppConfigFile">The path to the configuration file associated with the executable file.</param>
        public void Save(string sAppConfigFile)
        {
            Save(sAppConfigFile, "");
        }

        /// <summary>
        /// Save current connection string into configuration file.
        /// </summary>
        /// <param name="sAppConfigFile">The path to the configuration file associated with the executable file.</param>
        /// <param name="sDatabaseKey">The database key linking with appropriate connectionString.</param>
        public void Save(string sAppConfigFile, string sDatabaseKey)
        {
            string defaultDatabase = "";

            if (string.IsNullOrEmpty(sDatabaseKey))
                defaultDatabase = ConfigurationManager.AppSettings["defaultDatabase"];
            else
                defaultDatabase = ConfigurationManager.AppSettings[sDatabaseKey];

            if (!string.IsNullOrEmpty(defaultDatabase))
            {
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(sAppConfigFile);
                    config.ConnectionStrings.ConnectionStrings[defaultDatabase].ConnectionString = this.ToString();
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("connectionStrings");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Return current ConnectionString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this._sConnectionString;
        }

        /// <summary>
        /// Gets/Sets the name of database server.
        /// </summary>
        public string DBServerName
        {
            get { return _ConnStrCollection["Data Source"]; }
            set { _ConnStrCollection["Data Source"] = value; UpdateConnectionString(); }
        }

        /// <summary>
        /// Gets/Sets the name of database.
        /// </summary>
        public string DBName
        {
            get { return _ConnStrCollection["Initial Catalog"]; }
            set { _ConnStrCollection["Initial Catalog"] = value; UpdateConnectionString(); }
        }

        /// <summary>
        /// Gets/Sets user ID.
        /// </summary>
        public string UserID 
        {
            get { return _ConnStrCollection["User ID"]; }
            set { _ConnStrCollection["User ID"] = value; UpdateConnectionString(); }
        }

        /// <summary>
        /// Gets/set password.
        /// </summary>
        public string Password 
        {
            get { return _ConnStrCollection["Password"]; }
            set { _ConnStrCollection["Password"] = value; UpdateConnectionString(); }
        }

        /// <summary>
        /// Gets connection mode (Windows Authentication or SQL Server Authentication).
        /// </summary>
        public bool Integrated
        {
            get 
            {
                if ((_ConnStrCollection["Integrated Security"].ToUpper() == "SSPI") || (_ConnStrCollection["Integrated Security"].ToUpper() == "TRUE"))
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region -----Private methods--------------------
        private string ReturnSpecificConnectionString(string sDatabaseKey)
        {
            string sConnectionStringName = ConfigurationManager.AppSettings[sDatabaseKey];
            if (string.IsNullOrEmpty(sConnectionStringName)) throw new Exception("Section 'appSetting' does not contain '" + sDatabaseKey + "' key");

            string sConnectionString = ConfigurationManager.ConnectionStrings[sConnectionStringName].ConnectionString;
            if (string.IsNullOrEmpty(sConnectionString))
                throw new Exception("Config file does not contain connectionStrings section");

            return Encryption.DecryptString(sConnectionString);
        }

        private string ReturnDefaultConnectionString()
        {
            string defaultDatabase = ConfigurationManager.AppSettings["defaultDatabase"];
            if (string.IsNullOrEmpty(defaultDatabase)) throw new Exception("Section 'appSetting' does not contain 'defaultDatabase' key");

            string sConnectionString = ConfigurationManager.ConnectionStrings[defaultDatabase].ConnectionString;
            if (string.IsNullOrEmpty(sConnectionString))
                throw new Exception("Config file does not contain connectionStrings section");

            return Encryption.DecryptString(sConnectionString);
        }

        /// <summary>
        /// Parse a connection string into an array of pair key/value
        /// </summary>
        /// <param name="strConn">A connection string</param>
        /// <returns>NameValueCollection</returns>
        private NameValueCollection ParseConnectionString(string strConn)
        {
            NameValueCollection strConnCollection = new NameValueCollection();
             
            if (!string.IsNullOrEmpty(strConn))
            {
                string[] sTemp = strConn.Split(';');
                string[] sKeyValue;
                foreach (string s in sTemp)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        sKeyValue = s.Split('=');
                        strConnCollection.Add(sKeyValue[0].Trim(), sKeyValue[1].Trim());
                    }
                }
            }

            return strConnCollection;
        }

        /// <summary>
        /// Create an array of pair key/value for connection string by database server name and database name
        /// </summary>
        /// <param name="dbServerName">Database server name</param>
        /// <param name="dbName">Database name</param>
        /// <returns></returns>
        private NameValueCollection CreateConnStrCollection(string dbServerName, string dbName)
        {
            NameValueCollection strConnCollection = new NameValueCollection();
            strConnCollection.Add("Data Source", dbServerName);
            strConnCollection.Add("Initial Catalog", dbName);
            strConnCollection.Add("Integrated Security", "SSPI");

            return strConnCollection;
        }

        /// <summary>
        /// Create an array of pair key/value for connection string by database server name, database name, user name and password.
        /// </summary>
        /// <param name="dbServerName">Database server name</param>
        /// <param name="dbName">Database name</param>
        /// <param name="userID">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        private NameValueCollection CreateConnStrCollection(string dbServerName, string dbName, string userID, string password)
        {
            NameValueCollection strConnCollection = new NameValueCollection();
            strConnCollection.Add("Data Source", dbServerName);
            strConnCollection.Add("Initial Catalog", dbName);
            strConnCollection.Add("User ID", userID);
            strConnCollection.Add("Password", password);

            return strConnCollection;
        }

        /// <summary>
        /// Update connection string base on array of pair key/value.
        /// </summary>
        private void UpdateConnectionString()
        {
            string sTempConnStr = "";
            if ((_ConnStrCollection != null) && (_ConnStrCollection.Count > 0))
            {
                for (int i = 0; i < _ConnStrCollection.Count; i++)
                {
                    sTempConnStr += _ConnStrCollection.GetKey(i) + "=" + _ConnStrCollection.Get(i) + CONNSTRING_DELIMIT;
                }
            }
            if (!string.IsNullOrEmpty(sTempConnStr)) _sConnectionString = sTempConnStr;
        }

        #endregion
    }
}
