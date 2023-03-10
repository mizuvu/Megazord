using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Zord.Extensions
{
    public static class DataTableHelper
    {
        /// <summary>
        /// Convert DataTable to some Type
        /// </summary>
        public static IEnumerable<T> ConvertToType<T>(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                    }
                }

                yield return obj;
            }
        }

        /// <summary>
        /// Load list values to DataTable
        /// </summary>
        public static DataTable Load<T>(IList<T> values)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable("table", "table");

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in values)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
