using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.Entities
{
    public abstract class BaseCollection<T> : List<T> where T : BaseObject, new()
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        /// <procedure>MapObjects</procedure>
        /// <summary>This procedure will call MapObjects with the firts table contain in
        /// the DataSet.
        /// </summary>
        /// <param name="ds">a System.Data.DataSet</param>
        /////////////////////////////////////////////////////////////////////////////////////////////
        public void MapObjects(DataSet ds)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    MapObjects(ds.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Data Collection can't be mapped!", ex);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// <procedure>MapObjects</procedure>
        /// <summary>This procedure will call the procedure MapData() of the inheritant object
        /// for each row contain in the DataTable dt then had it to the list of objects.
        /// </summary>
        /// <param name="dt">a System.Data.DataTable</param>
        ////////////////////////////////////////////////////////////////////////////////////////////
        public void MapObjects(DataTable dt)
        {
            try
            {
                Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    T obj = new T();
                    obj.MapData(dt.Rows[i]);
                    this.Add(obj);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Data Collection can't be mapped!", ex);
            }

        }


    }
}
