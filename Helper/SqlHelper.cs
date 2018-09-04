using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Common.Helper;
using System.Linq;

namespace Common.Helper
{
    public partial class SqlHelper : SingleTon<SqlHelper>
    {
        public string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        public string connStr2 = ConfigurationManager.ConnectionStrings["ConnString2"].ConnectionString;

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="str"></param>
        public void ExcuteNon(string str)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand com = new SqlCommand(str, conn);
                com.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// 防注入
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dict"></param>
        public void ExcuteNon(string str, Dictionary<string, string> dict, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            if (transaction == null)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlCom = new SqlCommand(str, conn);
                    foreach (var item in dict)
                    {
                        sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    conn.Open();
                    sqlCom.Connection = conn;
                    sqlCom.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                var conn = connection;

                SqlCommand sqlCom = new SqlCommand(str, conn)
                {
                    Transaction = transaction
                };
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                //conn.Open();
                //sqlCom.Connection = conn;
                sqlCom.ExecuteNonQuery();
                //conn.Close();


            }
        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public int ExcuteNonQuery(string str, Dictionary<string, string> dict, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var r = 0;
            if (transaction == null)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlCom = new SqlCommand(str, conn);
                    foreach (var item in dict)
                    {
                        sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    conn.Open();
                    sqlCom.Connection = conn;
                    r = sqlCom.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                var conn = connection;

                SqlCommand sqlCom = new SqlCommand(str, conn);
                sqlCom.Transaction = transaction;
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                //conn.Open();
                //sqlCom.Connection = conn;
                r = sqlCom.ExecuteNonQuery();
                //conn.Close();

            }
            return r;
        }

        /// <summary>
        /// 执行sql语句，返回结果
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public string ExecuteScalar(string sqlStr)
        {
            string r = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand sqlCom = new SqlCommand(sqlStr, conn);
                var t = sqlCom.ExecuteScalar();
                if (t != null)
                    r = t.ToString();
                else
                    r = null;
                conn.Close();
            }
            return r;
        }

        public string ExecuteScalar(string sqlStr, Dictionary<string, string> dict, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            string r = "";
            if (transaction == null)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlCom = new SqlCommand(sqlStr, conn);
                    foreach (var item in dict)
                    {
                        sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    var t = sqlCom.ExecuteScalar();
                    if (t != null)
                        r = t.ToString();
                    else
                        r = null;
                    conn.Close();
                }
            }
            else
            {
                var conn = connection;
                //using (conn)
                //{
                //connection.Open();
                SqlCommand sqlCom = new SqlCommand(sqlStr, conn);
                sqlCom.Transaction = transaction;
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                var t = sqlCom.ExecuteScalar();
                if (t != null)
                    r = t.ToString();
                else
                    r = null;
                //conn.Close();
                //}
            }
            return r;
        }

        /// <summary>
        /// 执行sql得List,防注入
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public List<T> ExecuteGetDt<T>(string sqlStr, Dictionary<string, string> dict) where T : class, new()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand sqlCom = new SqlCommand(sqlStr, conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }

                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                da.Fill(dt);
            }
            return dt.ConvertToList<T>().ToList();
        }

        /// <summary>
        /// 事务执行sql
        /// </summary>
        /// <param name="sqls">几组sql一起</param>
        /// <param name="transactionName">事务名称</param>
        /// <param name="dicts">几组参数一起，要与sqls对应</param>
        /// <returns>事务执行是否成功</returns>
        public bool ExecuteInTransaction(List<string> sqls, string transactionName, List<Dictionary<string, string>> dicts)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlTransaction transaction = conn.BeginTransaction(transactionName);//-txy

                try
                {
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        SqlCommand com = new SqlCommand
                        {
                            Connection = conn,
                            Transaction = transaction,
                            CommandText = sqls[i]
                        };
                        foreach (var item in dicts[i])
                        {
                            com.Parameters.AddWithValue(item.Key, item.Value);
                        }
                        com.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断某个字段的某个值在表中是否存在,传0就不跟自己比较了
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExists(string tableName, string fieldName, string value, int id)
        {
            string str = "select * from " + tableName + " where isDeleted='False' and id!=" + id + " and " + fieldName + " = @value";
            var dict = new Dictionary<string, string>();
            dict.Add("@value", value);
            var r = ExecuteScalar(str, dict);
            if (r == null)
                return false;
            return true;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExists2(string tableName, string fieldName, string value, string
            primaryKey, string keyValue)
        {
            //string str = "select * from " + tableName + " where  id!=" + id + " and " + fieldName + " = @value";
            string str = $"select * from {tableName} where {primaryKey}!=@keyValue and {fieldName}=@value";
            var dict = new Dictionary<string, string>();
            dict.Add("@value", value);
            dict.Add("@keyValue", keyValue);
            var r = ExecuteScalar(str, dict);
            if (r == null)
                return false;
            return true;
        }

        public List<T> GetListFromDb<T>() where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string selectAll = "select * from " + tableName;
                SqlDataAdapter da = new SqlDataAdapter(selectAll, conn);
                DataTable table = new DataTable();
                da.Fill(table);
                var list = table.ConvertToList<T>();
                return list;
            }
        }

        /// <summary>
        /// 根据id直接从数据库获取数据,防注入了
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="listName">比如userInfo 下划线自带</param>
        /// <param name="id">就是id</param>
        /// <returns></returns>
        public T GetById<T>(int id) where T : class, new()
        {
            string selectAll = $"select * from {typeof(T).Name} where id =@id ";
            var dict = new Dictionary<string, string>();
            dict.Add("@id", id.ToString());
            var list = ExecuteGetDt<T>(selectAll, dict);
            if (list.Count == 0)
                return null;
            return list.First();

            //return list.FirstOrDefault();
        }

        /// <summary>
        /// 查询参数放入字典，进行查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public List<T> GetByParams<T>(string tableName, Dictionary<string, string> dict) where T : class, new()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + tableName + " where ");

                foreach (var item in dict)
                {
                    sb.Append(item.Key + " = '" + item.Value + "' and ");
                }
                sb.Append(" 1 = 1");
                SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), conn);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public List<T> GetByCondition<T>(string condition) where T : class, new()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var tableName = typeof(T).Name;
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + tableName + " where " + condition);
                SqlDataAdapter da = new SqlDataAdapter(sb.ToString(), conn);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }

        /// <summary>
        /// 根据id集合来查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetByIds<T>(string ids) where T : class, new()
        {
            return GetByCondition<T>(" id in (" + ids + ")");
        }

        /// <summary>
        /// 根据id集合来查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetByIds<T>(int[] ids) where T : class, new()
        {
            var str = string.Join(",", ids);
            return GetByCondition<T>(" id in (" + str + ")");
        }

        /// <summary>
        /// 根据条件查询,防注入版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public List<T> GetByCondition<T>(string condition, Dictionary<string, string> dict) where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from " + tableName + " where " + condition);

                SqlCommand sqlCom = new SqlCommand(sb.ToString(), conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }

                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }

        /// <summary>
        /// 条件复杂的时候，查询出所有符合的id,已经倒序了(如果需要按多个字段来排序的话，就自己写sql吧)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<T> GetDistinctCount<T>(string condition, Dictionary<string, string> dict) where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select distinct id from " + tableName + " where " + condition + " order by id desc");

                SqlCommand sqlCom = new SqlCommand(sb.ToString(), conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }

                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }
        /// <summary>
        /// 条件查询类似orderTryJoin这种一对多（当然也支持一对一）的视图。排序，分页    select自己写,一对多的话，select要distinct 主键
        /// </summary>
        /// <returns></returns>
        public List<T> GetViewPaging<T>(string select, string condition, int index, int size, string orderStr, Dictionary<string, string> dict) where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(select + " where " + condition);

                int x = (index - 1) * size;
                string str = "select distinct o.* from " + tableName + " as o,( ";
                str += "select top " + size + " * from (" + sb.ToString() + ") r where id not in (select top " + x + " id from (" + sb.ToString() + ") r " + orderStr + ") " + orderStr + "";
                str += " ) as r where r.id=o.id " + orderStr;//-txy 有点问题 员工列表排序时，不过别的也出现过
                SqlCommand sqlCom = new SqlCommand(str, conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }

        public List<T> GetViewPaging<T>(string condition, int index, int size, string orderStr, Dictionary<string, string> dict) where T : class, new()
        {
            var tableName = typeof(T).Name;
            var select = $"select * from {tableName} ";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(select + " where " + condition);

                int x = (index - 1) * size;
                string str = "select distinct o.* from " + tableName + " as o,( ";
                str += "select top " + size + " * from (" + sb.ToString() + ") r where id not in (select top " + x + " id from (" + sb.ToString() + ") r " + orderStr + ") " + orderStr + "";
                str += " ) as r where r.id=o.id " + orderStr;//-txy 有点问题 员工列表排序时，不过别的也出现过
                SqlCommand sqlCom = new SqlCommand(str, conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }
        /// <summary>
        /// 视图的sql字符串传进来，可以在代码中修改，不需要修改数据库的视图。但如果数据库有的话，dbToClass就方便很多。而且数据库有个视图也没什么大不了的
        /// </summary>
        public List<T> GetViewPagingOwn<T>(string select, string condition, int index, int size, string orderStr, string viewStr, Dictionary<string, string> dict) where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(select + " where " + condition);

                int x = (index - 1) * size;
                string str = "select distinct o.* from (" + viewStr + ") as o,( ";
                str += "select top " + size + " * from (" + sb.ToString() + ") r where id not in (select top " + x + " id from (" + sb.ToString() + ") r " + orderStr + ") " + orderStr + "";
                str += " ) as r where r.id=o.id " + orderStr;//-txy 有点问题 员工列表排序时，不过别的也出现过
                SqlCommand sqlCom = new SqlCommand(str, conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }

        /// <summary>
        /// 视图一对多分页查询。mainkey：相同的字段，fields：mainkey+查询字段
        /// </summary>
        /// <returns></returns>
        public List<T> GetMutiView<T>(string mainKey, string fields, string condition, int index, int size, string orderStr, Dictionary<string, string> dict) where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"select distinct {fields} from {tableName} where {condition}");

                int x = (index - 1) * size;
                string str = $"select distinct o.* from {tableName} as o,( select top {size} * from ({sb}) r where {mainKey} not in (select top {x} {mainKey} from ({sb}) r {orderStr}) {orderStr} ) as r where r.{mainKey}=o.{mainKey} {orderStr}";
                SqlCommand sqlCom = new SqlCommand(str, conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }

        public List<T> GetMutiViewCount<T>(string mainKey, string condition, Dictionary<string, string> dict) where T : class, new()
        {
            var tableName = typeof(T).Name;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"select distinct {mainKey} from {tableName } where {condition} ");

                SqlCommand sqlCom = new SqlCommand(sb.ToString(), conn);
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }

                SqlDataAdapter da = new SqlDataAdapter(sqlCom);
                DataTable table = new DataTable();
                da.Fill(table);
                return table.ConvertToList<T>();
            }
        }

    }

    public partial class SqlHelper : SingleTon<SqlHelper>
    {
        public List<string> GetTables()
        {
            var tableNames = new List<string>();
            DataTable table;
            using (SqlConnection conn = new SqlConnection(connStr2))
            {
                conn.Open();
                table = conn.GetSchema(SqlClientMetaDataCollectionNames.Tables);
                conn.Close();
            }
            foreach (DataRow dr in table.Rows)
            {
                tableNames.Add(dr[2].ToString());
            }
            tableNames = tableNames.OrderBy(p => p.ToString()).ToList();
            return tableNames;
        }

        public string ExecuteScalar2(string sqlStr, Dictionary<string, string> dict, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            string r = "";
            if (transaction == null)
            {
                using (SqlConnection conn = new SqlConnection(connStr2))
                {
                    conn.Open();
                    SqlCommand sqlCom = new SqlCommand(sqlStr, conn);
                    foreach (var item in dict)
                    {
                        sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    var t = sqlCom.ExecuteScalar();
                    if (t != null)
                        r = t.ToString();
                    else
                        r = null;
                    conn.Close();
                }
            }
            else
            {
                var conn = connection;
                SqlCommand sqlCom = new SqlCommand(sqlStr, conn);
                sqlCom.Transaction = transaction;
                foreach (var item in dict)
                {
                    sqlCom.Parameters.AddWithValue(item.Key, item.Value);
                }
                var t = sqlCom.ExecuteScalar();
                if (t != null)
                    r = t.ToString();
                else
                    r = null;
            }
            return r;
        }

        public bool IsTableExist(string tableName)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@name", tableName);
            var str = "select count(1) from sys.objects where name =@name";
            var r = Convert.ToInt32(ExecuteScalar(str, dict));
            if (r > 0)
                return true;
            return false;
        }

        public bool IsTableExist2(string tableName)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("@name", tableName);
            var str = "select count(1) from sys.objects where name =@name";
            var r = Convert.ToInt32(ExecuteScalar2(str, dict));
            if (r > 0)
                return true;
            return false;
        }
    }
}
