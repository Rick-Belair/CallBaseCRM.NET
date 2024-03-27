using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Business.Entities
{
    public abstract class BaseObject
    {
        protected static string FixUpString(string str)
        {
            if (str != null)
            {
                string s = str.Trim();
                return (s == "") ? null : s;
            }
            else
                return null;
        }

        public virtual void MapData(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    MapData(ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Data object can't be mapped!", ex);
            }
        }

        public virtual void MapData(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    MapData(dt.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Data object can't be mapped!", ex);
            }
        }

        public virtual void MapData(DataRow row)
        {
            try
            {
                //You can put common data mapping items here (e.g. create date, modified date, user, etc)
                // will not be used for now
            }
            catch (Exception ex)
            {
                throw new Exception("Data object can't be mapped!", ex);
            }
        }

        #region Get Functions


        protected static int? GetInt(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (int?)Convert.ToInt32(row[columnName]) : null;
        }

        protected static DateTime? GetDateTime(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (DateTime?)Convert.ToDateTime(row[columnName]) : null;
        }

        protected static Decimal? GetDecimal(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (Decimal?)Convert.ToDecimal(row[columnName]) : null;
        }

        protected static bool? GetBool(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (bool?)Convert.ToBoolean(row[columnName]) : null;
        }

        protected static string GetString(DataRow row, string columnName)
        {
            string s = (row[columnName] != DBNull.Value) ? ((string)row[columnName]).Trim() : null;
            return (s != "") ? s : null;
        }

        protected static double? GetDouble(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (double?)Convert.ToDouble(row[columnName]) : null;
        }

        protected static float? GetFloat(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (float?)Convert.ToSingle(row[columnName]) : null;
        }

        protected static Guid? GetGuid(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (Guid?)(row[columnName]) : null;
        }

        protected static long? GetLong(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (long?)(row[columnName]) : null;
        }
        // problem with Byte

        protected static Byte? GetByte(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (Byte?)(row[columnName]) : null;
        }
        // problem with byte
        protected static byte? Getbyte(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (byte?)(row[columnName]) : null;
        }

        #endregion
    }
}


