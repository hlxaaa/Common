using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Common.Helper
{
    public static class ListHelper
    {
        /// <summary>
        /// 转换List成为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">传入的List</param>
        public static DataTable ToDataTable<T>(this List<T> list) where T : class, new()
        {
            DataTable table = new DataTable();
            T t = new T();
            Type type = t.GetType();
            foreach (var proInfo in type.GetProperties())
            {
                DataColumn column = new DataColumn();
                column.ColumnName = proInfo.Name;
                table.Columns.Add(column);

            }
            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (var proInfo in type.GetProperties())
                {
                    var proValue = proInfo.GetValue(item, null);
                    row[proInfo.Name] = proValue;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// 转换List成为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">传入的List</param>
        public static DataTable ToList<T>(this DataTable table) where T : class, new()
        {
            T t = new T();
            Type type = t.GetType();
            foreach (DataRow row in table.Rows)
            {
                foreach (var proInfo in type.GetProperties())
                {
                    //proInfo.SetValue(,);
                }
            }
            return null;
        }

        /// <summary>  
        /// 利用反射和泛型  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static List<T> ConvertToList<T>(this DataTable dt) where T : class, new()
        {
            // 定义集合  
            List<T> ts = new List<T>();
            // 获得此模型的类型  
            Type type = typeof(T);
            //定义一个临时变量  
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性  
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性  
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量  
                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter  
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出  
                        // 取值  
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            var type_model = System.Type.GetType(pi.PropertyType.FullName, true);
                            if (type_model.Name.Contains("Nullable"))
                            {
                                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                NullableConverter nullableConverter = new NullableConverter(type_model);
                                //将convertsionType转换为nullable对的基础基元类型
                                type_model = nullableConverter.UnderlyingType;
                                object model;
                                if (type_model.BaseType.Name == "Enum")
                                    model = Enum.Parse(type_model, value.ToString());
                                else
                                    model = Convert.ChangeType(value, type_model);
                                //如果非空，则赋给对象的属性  
                                pi.SetValue(t, model, null);
                            }
                            else
                            {
                                object model;
                                if (type_model.BaseType.Name == "Enum")
                                    model = Enum.Parse(type_model, value.ToString());
                                else
                                    model = Convert.ChangeType(value, type_model);
                                //如果非空，则赋给对象的属性  
                                pi.SetValue(t, model, null);
                            }//-txy
                        }
                    }
                }
                //对象添加到泛型集合中  
                ts.Add(t);
            }
            return ts;
        }
    }
}
