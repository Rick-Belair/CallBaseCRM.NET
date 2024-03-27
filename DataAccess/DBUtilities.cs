using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DataAccess
{
    public class DBUtilities
    {
        protected string GetProviderName(string AppSettigsKeyName)
        {
            ConnectionStringSettingsCollection ConnectionStrings = ConfigurationManager.ConnectionStrings;
            return ConnectionStrings[AppSettigsKeyName].ProviderName;
        }//GetProviderName(string AppSettigsKeyName)

        protected string GetCurrentConnectionString()
        {
            string CURRENT_CONNECTION_STRING = ConfigurationManager.AppSettings.Get("CURRENT_CONNECTION_STRING");
            return CURRENT_CONNECTION_STRING;
        }//GetCurrentConnectionString()

        protected string GetConnectionString(string AppSettigsKeyName)
        {
            ConnectionStringSettingsCollection ConnectionStrings = ConfigurationManager.ConnectionStrings;
            return ConnectionStrings[AppSettigsKeyName].ConnectionString;
        }//GetConnectionString(string AppSettigsKeyName)

        public DbParameter CreateParameter(string Name, DbType DataType, object Value)
        {
            try
            {
                DbProviderFactory Factory = DbProviderFactories.GetFactory(GetProviderName(GetCurrentConnectionString()));
                DbParameter Parameter = Factory.CreateParameter();
                Parameter.ParameterName = Name;
                Parameter.DbType = DataType;
                    Parameter.Value = Value;

                return Parameter;
            }
            catch (DbException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occur", ex);
            }

        }//CreateParameter(string Name, DbType DataType, object Value)

    }//class

}//namespace
