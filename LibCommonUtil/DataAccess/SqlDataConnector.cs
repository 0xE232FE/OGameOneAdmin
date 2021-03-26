using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Xml;
using System.Collections;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;

namespace LibCommonUtil
{
    public class SqlDataConnector : IDisposable
    {
        /// <summary>
        /// To create SqlDataConnector with default connection. We need to add the ConnectionString into 
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

        private string _sConnectionString = "";
        private SqlConnection _oConnection = null;
        private SqlTransaction _oTransaction = null;
        private bool _bRollBacked = false;
        private bool _bControlTransaction = false;

        #region -------Constructors--------

        /// <summary>
        /// Creates a default SqlDataConnector.
        /// </summary>
        public SqlDataConnector()
        {
            try
            {
                _sConnectionString = new SqlConnectionString().ToString();
                _oConnection = new SqlConnection(_sConnectionString);
                _oConnection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Creates a SqlDataConnector by specific database key stored in 'appSettings' of App.Config or Web.Config file.
        /// </summary>
        /// <param name="sDatabaseKey">other database key (e.g: 'secondDatabase')</param>
        public SqlDataConnector(string sDatabaseKey)
        {
            try
            {
                _sConnectionString = new SqlConnectionString(sDatabaseKey).ToString();
                _oConnection = new SqlConnection(_sConnectionString);
                _oConnection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Creates a SqlDataConnector by a specific SqlConnectionString.
        /// </summary>
        /// <param name="sConnectionString">ConnectionString</param>
        public SqlDataConnector(SqlConnectionString oConnectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(oConnectionString.ToString())) throw new ArgumentException("ConnectionString is null or empty.");
                _sConnectionString = oConnectionString.ToString();
                _oConnection = new SqlConnection(_sConnectionString);
                _oConnection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region -------Destructor and Releases all resources----------
        /// <summary>
        /// Destructor.
        /// </summary>
        ~SqlDataConnector()
        {
        }

        /// <summary>
        /// Releases all resources used by System.Data.Common.DbTransaction and System.ComponentModel.Component 
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_oTransaction != null)
                    _oTransaction.Dispose();

                if (_oConnection != null)
                {
                    if (_oConnection.State != ConnectionState.Closed)
                        _oConnection.Close();
                    _oConnection.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region -------Properties----------
        /// <summary>
        /// Gets the current connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _sConnectionString;
            }
        }

        /// <summary>
        /// Gets the rollback status of current transaction.
        /// </summary>
        public bool IsRollbacked
        {
            get { return _bRollBacked; }
        }

        /// <summary>
        /// Gets status to see whether transaction to be controlled or not.
        /// </summary>
        public bool IsTransControlled
        {
            get { return _bControlTransaction; }
        }

        /// <summary>
        /// Gets current connection object.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _oConnection; }
        }

        /// <summary>
        /// Gets current transaction object.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _oTransaction; }
        }
        #endregion

        #region -------Public Methods------


        #region ---Connection and Transaction handling------

        /// <summary>
        /// Opens new connection or re-opens current connection with current connection string.
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                if (_oConnection == null)
                    if (string.IsNullOrEmpty(_sConnectionString))
                        throw new ArgumentException("Can not open connection because the ConnectionString is null or empty.");
                    else
                    {
                        _oConnection = new SqlConnection(_sConnectionString);
                        _oConnection.Open();
                    }
                else
                    if ((_oConnection.State == ConnectionState.Closed) || (_oConnection.State == ConnectionState.Broken))
                        _oConnection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Closes current connection.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if ((_oConnection != null) && (_oConnection.State != ConnectionState.Closed))
                    _oConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Opens a new transaction for current connection.
        /// </summary>
        public void OpenTransaction()
        {
            try
            {
                if (_oConnection == null) throw new Exception("Connection does not exist or does not open.");
                _oTransaction = _oConnection.BeginTransaction();
                _bRollBacked = false;
                _bControlTransaction = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Opens a new transaction for current connection with a isolation level.
        /// </summary>
        /// <param name="iso">The isolation level under which the transaction should run</param>
        public void OpenTransaction(IsolationLevel iso)
        {
            try
            {
                if (_oConnection == null) throw new Exception("Connection does not exist or does not open.");
                _oTransaction = _oConnection.BeginTransaction(iso);
                _bRollBacked = false;
                _bControlTransaction = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Opens a new transaction for current connection with a name of transaction.
        /// </summary>
        /// <param name="sTransName">Name of transaction</param>
        public void OpenTransaction(string sTransName)
        {
            try
            {
                if (_oConnection == null) throw new Exception("Connection does not exist or does not open.");
                _oTransaction = _oConnection.BeginTransaction(sTransName);
                _bRollBacked = false;
                _bControlTransaction = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Opens a new transaction for current connection with a isolation level and name of transaction.
        /// </summary>
        /// <param name="iso">The isolation level under which the transaction should run</param>
        /// <param name="sTransName">Name of transaction</param>
        public void OpenTransaction(IsolationLevel iso, string sTransName)
        {
            try
            {
                if (_oConnection == null) throw new Exception("Connection does not exist or does not open.");
                _oTransaction = _oConnection.BeginTransaction(iso, sTransName);
                _bRollBacked = false;
                _bControlTransaction = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Commits and closes the current database transaction.
        /// </summary>
        public void CloseTransaction()
        {
            try
            {
                if (_oTransaction == null) return;
                //- Transaction was rollbacked or commited or it is no longer valid, therefore nothing to commit.
                if ((_oTransaction != null) && (_oTransaction.Connection == null)) return;
                _oTransaction.Commit();
                _oTransaction = null;
                _bControlTransaction = false;
            }
            catch (Exception e)
            {
                try
                {
                    _oTransaction.Rollback();
                    _bRollBacked = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                throw e;
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void RollBackTransaction()
        {
            try
            {
                if (_oTransaction == null) return;
                if ((_oTransaction != null) && (_oTransaction.Connection == null)) throw new Exception("Transaction was rollbacked or commited or it is no longer valid.");
                _oTransaction.Rollback();
                _bRollBacked = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Rolls back a transaction with specific name.
        /// </summary>
        /// <param name="sTransName">The name of transaction to roll back</param>
        public void RollBackTransaction(string sTransName)
        {
            try
            {
                if (_oTransaction == null) return;
                if ((_oTransaction != null) && (_oTransaction.Connection == null)) throw new Exception("Transaction was rollbacked or commited or it is no longer valid.");
                _oTransaction.Rollback(sTransName);
                _bRollBacked = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region ---Complementary Functions-
        /// <summary>
        /// Verifies current database exists.
        /// </summary>
        /// <returns>returns True if database is existing otherwise returns False</returns>
        public bool VerifyDataBase()
        {
            if ((_oConnection != null) && (!String.IsNullOrEmpty(_oConnection.Database)))
                return true;
            else
                return false;
        }

        public SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
        {
            SqlParameter param = new SqlParameter(paramName, dbType);

            if (objValue == null)
            {
                param.IsNullable = true;
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = objValue;
            }

            return param;
        }
        #endregion

        #region ---Using Queries-----------

        /// <summary>
        /// Execute a query that takes no parameters and returns a Dataset.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <returns>A Dataset containing the resultset generated by the query</returns>
        public DataSet ExecDataSetbyQuery(string sQuery)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteDataset(_oConnection, CommandType.Text, sQuery);
                else
                    return SqlHelper.ExecuteDataset(_oTransaction, CommandType.Text, sQuery);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes parameters and returns a Dataset
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <param name="cmdParameters">Array of parameters</param>
        /// <returns>A Dataset containing the resultset generated by the query</returns>
        public DataSet ExecDataSetbyQuery(string sQuery, params SqlParameter[] cmdParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteDataset(_oConnection, CommandType.Text, sQuery, cmdParameters);
                else
                    return SqlHelper.ExecuteDataset(_oTransaction, CommandType.Text, sQuery, cmdParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes no parameters and returns no resultset.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <returns>A number of rows effected by query string</returns>
        public int ExecNonQuerybyQuery(string sQuery)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteNonQuery(_oConnection, CommandType.Text, sQuery);
                else
                    return SqlHelper.ExecuteNonQuery(_oTransaction, CommandType.Text, sQuery);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes parameters and returns no resultset.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <param name="cmdParameters">Array of parameters</param>
        /// <returns>A number of rows effected by query string</returns>
        public int ExecNonQuerybyQuery(string sQuery, params SqlParameter[] cmdParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteNonQuery(_oConnection, CommandType.Text, sQuery, cmdParameters);
                else
                    return SqlHelper.ExecuteNonQuery(_oTransaction, CommandType.Text, sQuery, cmdParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes no parameters and returns a DataReader.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <returns>A SqlDataReader containing the resultset generated by the query</returns>
        public SqlDataReader ExecDataReaderbyQuery(string sQuery)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteReader(_oConnection, CommandType.Text, sQuery);

                else
                    return SqlHelper.ExecuteReader(_oTransaction, CommandType.Text, sQuery);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes parameters and returns a DataReader.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <param name="cmdParameters">Array of parameters</param>
        /// <returns>A SqlDataReader containing the resultset generated by the query</returns>
        public SqlDataReader ExecDataReaderbyQuery(string sQuery, params SqlParameter[] cmdParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteReader(_oConnection, CommandType.Text, sQuery, cmdParameters);
                else
                    return SqlHelper.ExecuteReader(_oTransaction, CommandType.Text, sQuery, cmdParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes no parameters and returns a 1x1 resultset. 
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the query</returns>
        public object ExecScalarbyQuery(string sQuery)
        {
            try
            {
                object oScalar = new object();

                if (_oTransaction == null)
                    oScalar = SqlHelper.ExecuteScalar(_oConnection, CommandType.Text, sQuery);
                else
                    oScalar = SqlHelper.ExecuteScalar(_oTransaction, CommandType.Text, sQuery);

                if (oScalar == null || Convert.IsDBNull(oScalar))
                    return null;
                else
                    return oScalar;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes parameters and returns a 1x1 resultset.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <param name="cmdParameters">Array of parameters</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the query</returns>
        public object ExecScalarbyQuery(string sQuery, params SqlParameter[] cmdParameters)
        {
            try
            {
                object oScalar = new object();

                if (_oTransaction == null)
                    oScalar = SqlHelper.ExecuteScalar(_oConnection, CommandType.Text, sQuery, cmdParameters);
                else
                    oScalar = SqlHelper.ExecuteScalar(_oTransaction, CommandType.Text, sQuery, cmdParameters);

                if (oScalar == null || Convert.IsDBNull(oScalar))
                    return null;
                else
                    return oScalar;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes no parameters and returns a XmlReader.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <returns>XmlReader object</returns>
        public XmlReader ExecXmlReaderbyQuery(string sQuery)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteXmlReader(_oConnection, CommandType.Text, sQuery);
                else
                    return SqlHelper.ExecuteXmlReader(_oTransaction, CommandType.Text, sQuery);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a query that takes parameters and returns a XmlReader.
        /// </summary>
        /// <param name="sQuery">Query string</param>
        /// <param name="cmdParameters">Array of parameters</param>
        /// <returns>XmlReader object</returns>
        public XmlReader ExecXmlReaderbyQuery(string sQuery, params SqlParameter[] cmdParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteXmlReader(_oConnection, CommandType.Text, sQuery, cmdParameters);
                else
                    return SqlHelper.ExecuteXmlReader(_oTransaction, CommandType.Text, sQuery, cmdParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region ---Using Stored Procedure--

        /// <summary>
        /// Execute a stored procedure that takes object parameters and returns a DataSet.
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>DataSet</returns>
        public DataSet ExecDataSetbySP(string sSPName, params object[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteDataset(_oConnection, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteDataset(_oTransaction, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes parameters and returns a DataSet.
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of SqlParameters to be assigned as the input values of the stored procedure</param>
        /// <returns>DataSet</returns>
        public DataSet ExecDataSetbySP(string sSPName, params SqlParameter[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteDataset(_oConnection, CommandType.StoredProcedure, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteDataset(_oTransaction, CommandType.StoredProcedure, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes object parameters and returns no Results.
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>A number of rows effected by stored procedure</returns>
        public int ExecNonQuerybySP(string sSPName, params object[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteNonQuery(_oConnection, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteNonQuery(_oTransaction, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes parameters and returns no Results.
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of SqlParameters to be assigned as the input values of the stored procedure</param>
        /// <returns>A number of rows effected by stored procedure</returns>
        public int ExecNonQuerybySP(string sSPName, params SqlParameter[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteNonQuery(_oConnection, CommandType.StoredProcedure, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteNonQuery(_oTransaction, CommandType.StoredProcedure, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes object parameters and returns a SqlDataReader
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>SqlDataReader object</returns>
        public SqlDataReader ExecDataReaderbySP(string sSPName, params object[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteReader(_oConnection, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteReader(_oTransaction, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes parameters and returns a SqlDataReader
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of SqlParameters to be assigned as the input values of the stored procedure</param>
        /// <returns>SqlDataReader object</returns>
        public SqlDataReader ExecDataReaderbySP(string sSPName, params SqlParameter[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteReader(_oConnection, CommandType.StoredProcedure, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteReader(_oTransaction, CommandType.StoredProcedure, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes object parameters and returns a 1x1 resultset
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the stored procedure</returns>
        public object ExecScalarbySP(string sSPName, params object[] aParameters)
        {
            try
            {
                object oScalar = new object();

                if (_oTransaction == null)
                    oScalar = SqlHelper.ExecuteScalar(_oConnection, sSPName, aParameters);
                else
                    oScalar = SqlHelper.ExecuteScalar(_oTransaction, sSPName, aParameters);

                if (oScalar == null || Convert.IsDBNull(oScalar))
                    return null;
                else
                    return oScalar;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes parameters and returns a 1x1 resultset
        /// </summary>
        /// <param name="sSPName">The name of the stored procedure</param>
        /// <param name="aParameters">An array of SqlParameters to be assigned as the input values of the stored procedure</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the stored procedure</returns>
        public object ExecScalarbySP(string sSPName, params SqlParameter[] aParameters)
        {
            try
            {
                object oScalar = new object();

                if (_oTransaction == null)
                    oScalar = SqlHelper.ExecuteScalar(_oConnection, CommandType.StoredProcedure, sSPName, aParameters);
                else
                    oScalar = SqlHelper.ExecuteScalar(_oTransaction, CommandType.StoredProcedure, sSPName, aParameters);

                if (oScalar == null || Convert.IsDBNull(oScalar))
                    return null;
                else
                    return oScalar;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes object parameters and returns a XmlReader.
        /// </summary>
        /// <param name="sSPName">The name of stored procedure</param>
        /// <param name="aParameters">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>XmlReader object</returns>
        public XmlReader ExecXmlReaderbySP(string sSPName, params object[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteXmlReader(_oConnection, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteXmlReader(_oTransaction, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute a stored procedure that takes parameters and returns a XmlReader.
        /// </summary>
        /// <param name="sSPName">The name of stored procedure</param>
        /// <param name="aParameters">An array of SqlParameters to be assigned as the input values of the stored procedure</param>
        /// <returns>XmlReader object</returns>
        public XmlReader ExecXmlReaderbySP(string sSPName, params SqlParameter[] aParameters)
        {
            try
            {
                if (_oTransaction == null)
                    return SqlHelper.ExecuteXmlReader(_oConnection, CommandType.StoredProcedure, sSPName, aParameters);
                else
                    return SqlHelper.ExecuteXmlReader(_oTransaction, CommandType.StoredProcedure, sSPName, aParameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #endregion
    }
}
