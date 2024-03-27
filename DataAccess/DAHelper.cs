using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace DataAccess
{
    public class DAHelper: DBUtilities
    {
        private string _Provider = "";
        

        #region Constructor

        public DAHelper()
        {
            this.Provider = GetProviderName(GetCurrentConnectionString());
        }

        private string Provider
        {
            get { return _Provider; }
            set { _Provider = value; }
        }

        #endregion

        #region Connection Procedures

        private DbConnection OpenConnection()
        {
            DbProviderFactory Factory = DbProviderFactories.GetFactory(this.Provider);
            DbConnection Connection = Factory.CreateConnection();
            Connection.ConnectionString = GetConnectionString(GetCurrentConnectionString());
            Connection.Open();
            return Connection;
        }//OpenConnection()

        private void CloseConnection(ref DbConnection Connection)
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }//if the connection isnt null

        }//CloseConnection(ref DbConnection Connection)
        #endregion

        public DataSet Execute(string sqlStatement, CommandType cmdType)
        {

            DbProviderFactory Factory = DbProviderFactories.GetFactory(this.Provider);
            DbConnection Connection = Factory.CreateConnection();
            DbCommand Command = Factory.CreateCommand();

            try
            {
                Connection = OpenConnection();
                Command = CreateCommand(sqlStatement, cmdType, Connection);
                DataSet dtsData = new DataSet();
                DbDataAdapter adpAdapter = Factory.CreateDataAdapter();
                adpAdapter.InsertCommand = Command;
                adpAdapter.SelectCommand = Command;
                adpAdapter.UpdateCommand = Command;
                adpAdapter.Fill(dtsData);
                adpAdapter.Dispose();
                dtsData.Dispose();

                return dtsData;
            }
            catch (DbException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occur", ex);
            }
            finally
            {
                DisposeCommand(ref Command);
                CloseConnection(ref Connection);

            }

        }//Execute(string sqlStatement, CommandType cmdType)

        public DataSet Execute(string sqlStatement, CommandType cmdType, DbParameter[] Parameters)
        {
            DbProviderFactory Factory = DbProviderFactories.GetFactory(this.Provider);
            DbConnection Connection = Factory.CreateConnection();
            DbCommand Command = Factory.CreateCommand();

            try
            {
                Connection = OpenConnection();
                Command = CreateCommand(sqlStatement, cmdType, Parameters, Connection);
                DataSet dtsData = new DataSet();
                DbDataAdapter adpAdapter = Factory.CreateDataAdapter();
                adpAdapter.InsertCommand = Command;
                adpAdapter.SelectCommand = Command;
                adpAdapter.UpdateCommand = Command;
                adpAdapter.Fill(dtsData);
                adpAdapter.Dispose();

                return dtsData;
            }
            catch (DbException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occur", ex);
            }
            finally
            {
                DisposeCommand(ref Command);
                CloseConnection(ref Connection);
            }

        }//Execute(string sqlStatement, CommandType cmdType, DbParameter[] Parameters)

        public int ExecuteNonQuery(string sqlStatement, CommandType cmdType, DbParameter[] Parameters)
        {
            DbProviderFactory Factory = DbProviderFactories.GetFactory(this.Provider);
            DbConnection Connection = Factory.CreateConnection();
            DbCommand Command = Factory.CreateCommand();
            

            try
            {
                Connection = OpenConnection();
                Command = CreateCommand(sqlStatement, cmdType, Parameters, Connection);
                int intAffectedRow = Command.ExecuteNonQuery();
                return intAffectedRow;
            }
            catch (DbException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occur", ex);
            }
            finally
            {
                DisposeCommand(ref Command);
                CloseConnection(ref Connection);
            }
        }//ExecuteNonQuery(string sqlStatement, CommandType cmdType, DbParameter[] Parameters)

        public int ExecuteNonQuery(string sqlStatement, CommandType cmdType)
        {
            DbProviderFactory Factory = DbProviderFactories.GetFactory(this.Provider);
            DbConnection Connection = Factory.CreateConnection();
            DbCommand Command = Factory.CreateCommand();

            //tried this to see if it would allow the accents into the db, no luck 
            //CultureInfo culture;
            //culture = CultureInfo.CreateSpecificCulture("fr-fr");
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture; 
            

            try
            {
                Connection = OpenConnection();
                Command = CreateCommand(sqlStatement, cmdType, Connection);
                int intAffectedRow = Command.ExecuteNonQuery();

                return intAffectedRow;
            }
            catch (DbException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occur", ex);
            }
            finally
            {
                DisposeCommand(ref Command);
                CloseConnection(ref Connection);
            }

        }//ExecuteNonQuery(string sqlStatement, CommandType cmdType)

        #region Command Procedures
        private DbCommand CreateCommand(string sqlStatement, CommandType CmdType, DbConnection Connection)
        {
            DbCommand Command = Connection.CreateCommand();
            Command.CommandType = CmdType;
            Command.CommandText = sqlStatement;
            return Command;
        }//CreateCommand(string sqlStatement, CommandType CmdType, DbConnection Connection)

        private DbCommand CreateCommand(string sqlStatement, CommandType CmdType, DbParameter[] Parameters, DbConnection Connection)
        {
            DbCommand Command = CreateCommand(sqlStatement, CmdType, Connection);

            foreach (DbParameter Parameter in Parameters)
            {
                Command.Parameters.Add(Parameter);
            }
            return Command;

        }//CreateCommand(string sqlStatement, CommandType CmdType, DbParameter[] Parameters, DbConnection Connection)

        protected void DisposeCommand(ref DbCommand Command)
        {
            if (Command != null)
            {
                // RB - 20131022 - added parameters clear to allow the parameters to be reused for multiple queries. 
                Command.Parameters.Clear();
                Command.Dispose();
            }// if the command isnt null

        }// DisposeCommand(ref DbCommand Command)

        #endregion

        // this method will take something like "EYT,GYR,IYY,..." and will return:" 'EYI','GYR','IYY',...
        public string splitPriv(string u_grp_access)
        {
            string the_result = "";

            if (u_grp_access != null && u_grp_access.Contains(','))
            {
                string u_grp_access_tmp = u_grp_access.ToString().TrimEnd(',');

                List<string> privileges = u_grp_access_tmp.Split(',').ToList<string>();

                foreach (string priv in privileges)
                {
                    the_result = the_result + "'" + priv + "',";
                }

                the_result = the_result.ToString().TrimEnd(',');
            }
            else
                the_result = "''";
            return the_result;

        }

    }//class
}//namespace
